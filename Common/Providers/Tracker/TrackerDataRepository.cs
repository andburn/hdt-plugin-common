using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Providers.Utils;
using HDT.Plugins.Common.Services;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Importing;
using Hearthstone_Deck_Tracker.Stats;
using Hearthstone_Deck_Tracker.Utility.Logging;
using static HDT.Plugins.Common.Enums.Convert;
using DB = Hearthstone_Deck_Tracker.Hearthstone.Database;
using HDTCard = Hearthstone_Deck_Tracker.Hearthstone.Card;
using HDTDeck = Hearthstone_Deck_Tracker.Hearthstone.Deck;

namespace HDT.Plugins.Common.Providers.Tracker
{
	public class TrackerDataRepository : IDataRepository
	{
		private static readonly BindingFlags bindFlags =
			BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

		private const int CacheRefreshLimit = 300; // seconds before cache needs to be refreshed
		private DateTime CacheCreatedAt;
		private List<Deck> DeckCache = null;
		private List<Game> GameCache = null;

		public List<Deck> GetAllDecks()
		{
			if (DeckCache == null || CacheNeedsRefresh())
				RefreshCache();
			return DeckCache;
		}

		public Deck GetDeck(Guid id)
		{
			return GetAllDecks().SingleOrDefault(d => d.Id == id);
		}

		public List<Game> GetAllGames()
		{
			if (GameCache == null || CacheNeedsRefresh())
				RefreshCache();
			return GameCache;
		}

		public List<Game> GetAllGamesWithDeck(Guid id)
		{
			var games = new List<Game>();
			if (id == null)
				return games;
			games = GetAllGames()
				.Where(g => g.Deck != null && g.Deck.Id == id)
				.ToList();
			return games;
		}

		public void AddGames(List<Game> games)
		{
			var newGames = new List<Game>();
			// load games from tracker
			Reload<DeckStatsList>();
			Reload<DefaultDeckStats>();
			var gameStats = new List<GameStats>(DeckStatsList.Instance.DeckStats.Values
				.SelectMany(x => x.Games));
			gameStats.AddRange(DefaultDeckStats.Instance.DeckStats
				.SelectMany(x => x.Games));
			// create index maps into the list on id and time
			var indexById = gameStats.ToDictionary(x => x.GameId, x => gameStats.IndexOf(x));
			var indexByTime = gameStats.ToDictionary(x => x.StartTime, x => gameStats.IndexOf(x));

			foreach (var game in games)
			{
				if (game.Id != null && indexById.ContainsKey(game.Id))
				{
					var idx = indexById[game.Id];
					if (idx < gameStats.Count)
					{
						game.CopyTo(gameStats[idx]);
					}
				}
				else if (game.StartTime != null && game.EndTime != null
					&& indexByTime.ContainsKey(game.StartTime))
				{
					var idx = indexByTime[game.StartTime];
					if (idx < gameStats.Count)
					{
						var g = gameStats[idx];
						if (game.EndTime == g.EndTime)
							game.CopyTo(g);
					}
				}
				else
				{
					newGames.Add(game);
				}
			}
			DeckStatsList.Save();
			DefaultDeckStats.Save();

			if (newGames.Count > 0)
			{
				AddNewGames(newGames);
			}
		}

		private int AddNewGames(List<Game> games)
		{
			Reload<DeckStatsList>();
			Reload<DefaultDeckStats>();
			Reload<DeckList>();

			var count = 0;
			foreach (var g in games)
			{
				var success = false;
				var name = g.Deck.Name.ToLowerInvariant();
				var klass = GetClass(g);

				if (!string.IsNullOrWhiteSpace(name))
				{
					// find a deck by name
					// add it if it exists (latest version)
					// if not create empty deck, and add to that
					var decks = DeckList.Instance.Decks;
					var match = decks.Where(d =>
						d.Name.ToLowerInvariant() == name
						&& d.Class.ToLower() == klass
						&& !d.Archived).SingleOrDefault();
					// if only a single match use that deck
					if (match != null)
					{
						var gameStats = new GameStats();
						g.CopyTo(gameStats);
						DeckStatsList.Instance.DeckStats[match.DeckId].AddGameResult(gameStats);
						success = true;
					}
					// multiple matches are unresolvable, fallback to default stats
				}
				if (!success)
				{
					// add to default class deck stats
					if (!string.IsNullOrEmpty(klass))
					{
						var stats = DefaultDeckStats.Instance.GetDeckStats(klass);
						var gameStats = new GameStats();
						g.CopyTo(gameStats);
						stats.AddGameResult(gameStats);
						success = true;
					}
				}
				count += success ? 1 : 0;
			}

			DeckList.Save();
			DeckStatsList.Save();
			DefaultDeckStats.Save();

			return count;
		}

		private string GetClass(Game game)
		{
			if (game.PlayerClass != PlayerClass.ALL)
				return game.PlayerClass.ToString().ToLower();
			else if (game.Deck.Class != PlayerClass.ALL)
				return game.Deck.Class.ToString().ToLower();
			else
				return string.Empty;
		}

		public List<Deck> GetAllDecksWithTag(string tag)
		{
			var decks = DeckList.Instance.Decks
				.Where(d => d.TagList.ToLowerInvariant().Contains(tag.ToLowerInvariant()))
				.ToList();
			var vDecks = new List<Deck>();
			foreach (var d in decks)
			{
				// get the newest version of the deck
				var v = d.VersionsIncludingSelf.OrderByDescending(x => x).FirstOrDefault();
				d.SelectVersion(v);
				if (d == null)
					continue;
				var vd = new Deck(d.DeckId, d.Name, d.IsArenaDeck, d.Class, d.StandardViable);
				vd.Cards = d.Cards
					.Select(x => new Card(x.Id, x.LocalizedName, x.Count, x.Background.Clone()))
					.ToList();
				vDecks.Add(vd);
			}

			return vDecks;
		}

		public Deck GetOpponentDeck()
		{
			PlayerClass klass = PlayerClass.ALL;
			IEnumerable<TrackedCard> cards = new List<TrackedCard>();

			if (Core.Game.IsRunning)
			{
				var game = Core.Game.CurrentGameStats;
				if (game != null && game.CanGetOpponentDeck)
				{
					klass = ToHeroClass(game.OpponentHero);
					cards = game.OpponentCards;
				}
			}

			return CreateDeck(klass, cards);
		}

		public string GetGameNote()
		{
			if (Core.Game.IsRunning && Core.Game.CurrentGameStats != null)
			{
				return Core.Game.CurrentGameStats.Note;
			}
			return null;
		}

		public void UpdateGameNote(string text)
		{
			if (Core.Game.IsRunning && Core.Game.CurrentGameStats != null)
			{
				Core.Game.CurrentGameStats.Note = text;
			}
		}

		public void AddDeck(Deck deck)
		{
			if (deck == null)
			{
				Log.Info("Cannot add null deck");
				return;
			}
			HDTDeck d = new HDTDeck();
			d.Name = deck.Name;
			d.Class = deck.Class.ToString();
			d.Cards = new ObservableCollection<HDTCard>(deck.Cards.Select(c => DB.GetCardFromId(c.Id)));
			DeckList.Instance.Decks.Add(d);
		}

		public void AddDeck(string name, string playerClass, string cards, bool archive, params string[] tags)
		{
			var deck = StringImporter.Import(cards);
			if (deck != null)
			{
				deck.Name = name;
				if (deck.Class != playerClass)
					deck.Class = playerClass;
				if (tags.Any())
				{
					var reloadTags = false;
					foreach (var t in tags)
					{
						if (!DeckList.Instance.AllTags.Contains(t))
						{
							DeckList.Instance.AllTags.Add(t);
							reloadTags = true;
						}
						deck.Tags.Add(t);
					}
					if (reloadTags)
					{
						DeckList.Save();
						Core.MainWindow.ReloadTags();
					}
				}
				// hacky way to update ui:
				// use MainWindow.ArchiveDeck to update
				// set deck archive to opposite of desired
				deck.Archived = !archive;
				// add and save
				DeckList.Instance.Decks.Add(deck);
				DeckList.Save();
				// now reverse 'archive' of the deck
				// this should refresh all ui elements
				Core.MainWindow.ArchiveDeck(deck, archive);
			}
		}

		public void DeleteAllDecksWithTag(string tag)
		{
			if (string.IsNullOrWhiteSpace(tag))
				return;
			var decks = DeckList.Instance.Decks.Where(d => d.Tags.Contains(tag)).ToList();
			Log.Info($"Deleting {decks.Count} tagged with '{tag}'");
			foreach (var d in decks)
				DeckList.Instance.Decks.Remove(d);
			if (decks.Any())
				DeckList.Save();
		}

		public string GetGameMode()
		{
			if (Core.Game.IsRunning && Core.Game.CurrentGameStats != null)
			{
				return Core.Game.CurrentGameStats.GameMode.ToString().ToLowerInvariant();
			}
			return string.Empty;
		}

		private Game CreateGame(GameStats stats)
		{
			var game = new Game();
			var deck = GetDeck(stats.DeckId);
			game.CopyFrom(stats, deck);
			return game;
		}

		private Deck CreateDeck(PlayerClass klass, IEnumerable<TrackedCard> cards)
		{
			var deck = new Deck();
			deck.Class = klass;
			// add the cards to the deck
			// create a temp HDT deck too, to check if its standard
			var hdtDeck = new Hearthstone_Deck_Tracker.Hearthstone.Deck();
			foreach (var card in cards)
			{
				var c = DB.GetCardFromId(card.Id);
				c.Count = card.Count;
				hdtDeck.Cards.Add(c);
				if (c != null && c != DB.UnknownCard)
				{
					deck.Cards.Add(
						new Card(c.Id, c.LocalizedName, c.Count, c.Background.Clone()));
				}
			}
			deck.IsStandard = hdtDeck.StandardViable;
			return deck;
		}

		// DeckList, DefaultDeckStats, DeckStatsList
		private void Reload<T>()
		{
			try
			{
				Type type = typeof(T);
				MethodInfo method = type.GetMethod("Reload", bindFlags);
				method.Invoke(null, new object[] { });
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void RefreshCache()
		{
			// Decks
			Reload<DeckList>();
			DeckCache = DeckList.Instance.Decks
				.Where(d => d.Archived == false)
				.Select(d => new Deck(d.DeckId, d.Name, d.IsArenaDeck, d.Class, d.StandardViable))
				.OrderBy(d => d.Name)
				.ToList();
			// Games
			var games = new List<Game>();
			Reload<DeckStatsList>();
			Reload<DefaultDeckStats>();
			var ds = new List<DeckStats>(DeckStatsList.Instance.DeckStats.Values);
			ds.AddRange(DefaultDeckStats.Instance.DeckStats);
			foreach (var deck in ds)
			{
				games.AddRange(deck.Games.Select(g => CreateGame(g)));
			}
			GameCache = games;
			// reset the timestamp
			CacheCreatedAt = DateTime.Now;
		}

		private bool CacheNeedsRefresh()
		{
			if (CacheCreatedAt == null)
				return false;
			return DateTime.Now > CacheCreatedAt.AddSeconds(CacheRefreshLimit);
		}
	}
}