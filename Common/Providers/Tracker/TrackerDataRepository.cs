using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Providers.Utils;
using HDT.Plugins.Common.Services;
using HDT.Plugins.Common.Utils;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Importing;
using Hearthstone_Deck_Tracker.Stats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using static HDT.Plugins.Common.Enums.Convert;
using API = Hearthstone_Deck_Tracker.API;
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
		private DateTime CacheCreatedAt = DateTime.MinValue;
		private List<Deck> DeckCache = null;
		private List<Game> GameCache = null;

		public List<Deck> GetAllDecks()
		{
			if (DeckCache == null || CacheNeedsRefresh())
				RefreshCache();

			Common.Log.Debug($"Tracker: GetAllDecks (${DeckCache.Count}) cached at {CacheCreatedAt}");
			return DeckCache;
		}

		public Deck GetDeck(Guid id)
		{
			var deck = GetAllDecks().SingleOrDefault(d => d.Id == id);
			Common.Log.Debug($"Tracker: GetDeck ({id}) = {deck?.Name}");
			return deck;
		}

		public List<Game> GetAllGames()
		{
			if (GameCache == null || CacheNeedsRefresh())
				RefreshCache();

			Common.Log.Debug($"Tracker: GetAllGames (${GameCache.Count}) cached at {CacheCreatedAt}");
			return GameCache;
		}

		public List<Game> GetAllGamesWithDeck(Guid id)
		{
			Common.Log.Debug($"Tracker: Getting all games with deck {id}");
			var games = new List<Game>();
			if (id == null)
				return games;

			games = GetAllGames()
				.Where(g => g.Deck != null && g.Deck.Id == id)
				.ToList();
			Common.Log.Debug($"Tracker: {games.Count} games matched");

			return games;
		}

		public int AddGames(List<Game> games)
		{
			Common.Log.Debug($"Tracker: Adding {games.Count} games");

			var count = 0;
			var newGames = new List<Game>();

			Common.Log.Debug($"Tracker: Reloading deck stats");
			Reload<DeckStatsList>();
			Reload<DefaultDeckStats>();

			var gameStats = new List<GameStats>(DeckStatsList.Instance.DeckStats.Values
				.SelectMany(x => x.Games));
			gameStats.AddRange(DefaultDeckStats.Instance.DeckStats
				.SelectMany(x => x.Games));

			// create index maps into the list on id and GameIndex
			var indexById = gameStats.ToDictionary(x => x.GameId, x => gameStats.IndexOf(x));
			var fuzzyIndex = gameStats.ToDictionary(x => new GameIndex(x), x => gameStats.IndexOf(x));

			foreach (var game in games)
			{
				// set default indices
				var index = -1;
				var gameIndex = new GameIndex(game);
				if (game.Id != null && indexById.ContainsKey(game.Id))
				{
					// look for id match first
					index = indexById[game.Id];
					Common.Log.Debug($"Tracker: Existing game found with id {game.Id}");
				}
				else if (fuzzyIndex.ContainsKey(gameIndex))
				{
					// try a fuzzy match
					index = fuzzyIndex[gameIndex];
					Common.Log.Debug($"Tracker: Game fuzzy matched ({gameIndex.StartTime})");
				}
				// if the game was matched then edit it, otherwise add new game
				if (index >= 0 && index < gameStats.Count)
				{
					var stats = gameStats[index];
					// set the deck first, so it isn't overwritten
					game.Deck = new Deck()
					{
						Name = stats.DeckName,
						Id = stats.DeckId
					};
					game.CopyTo(stats);
					count++;
					Common.Log.Debug($"Tracker: Game updated ({game.Id})");
				}
				else
				{
					newGames.Add(game);
				}
			}
			DeckStatsList.Save();
			DefaultDeckStats.Save();

			var newGamesAdded = 0;
			if (newGames.Count > 0)
			{
				newGamesAdded = AddNewGames(newGames);
			}

			return count + newGamesAdded;
		}

		private int AddNewGames(List<Game> games)
		{
			Common.Log.Debug($"Tracker: Adding {games.Count} new games");

			Common.Log.Debug($"Tracker: Refreshing deck status");
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
						Common.Log.Debug($"Tracker: Matching deck found '{match.Name}' {match.DeckId}");
						var gameStats = new GameStats();
						// copy the game info
						g.CopyTo(gameStats);
						// add the matched deck to GameStats object
						gameStats.DeckId = match.DeckId;
						gameStats.DeckName = match.Name;
						// add game to hdt stats
						DeckStatsList.Instance.DeckStats[match.DeckId].AddGameResult(gameStats);
						success = true;
						Common.Log.Debug($"Tracker: Game added to {match.Name} stats");
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
						Common.Log.Debug($"Tracker: Game added to default {klass} stats");
					}
					else
					{
						Common.Log.Debug($"Tracker: Failed to add game {g.Id}");
					}
				}
				count += success ? 1 : 0;
			}

			Common.Log.Debug($"Tracker: Saving deck stats");
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
			Common.Log.Debug($"Tracker: Found {decks.Count} decks with tag '{tag}'");

			var vDecks = new List<Deck>();
			foreach (var d in decks)
			{
				// get the newest version of the deck
				var v = d.VersionsIncludingSelf.OrderByDescending(x => x).FirstOrDefault();
				d.SelectVersion(v);
				if (d == null)
					continue;
				var vd = new Deck(d.DeckId, d.Name, d.IsArenaDeck, d.Class, d.StandardViable)
				{
					Cards = d.Cards
						.Select(x => new Card(x.Id, x.LocalizedName, x.Count, x.Background.Clone()))
						.ToList()
				};
				vDecks.Add(vd);
			}

			return vDecks;
		}

		public Deck GetOpponentDeck()
		{
			PlayerClass klass = PlayerClass.ALL;
			IEnumerable<TrackedCard> cards = new List<TrackedCard>();
			var game = API.Core.Game;

			if (game != null
				&& game.IsRunning
				&& game.CurrentGameStats != null
				&& game.CurrentGameStats.CanGetOpponentDeck)
			{
				klass = ToHeroClass(game.CurrentGameStats.OpponentHero);
				cards = game.CurrentGameStats.OpponentCards;
			}
			else
			{
				Common.Log.Debug($"Tracker: GetOpponentDeck game not running");
			}
			Common.Log.Debug($"Tracker: GetOpponentDeck {klass} ({cards.Count()})");

			return CreateDeck(klass, cards);
		}

		public string GetGameNote()
		{
			var game = API.Core.Game;
			if (game.IsRunning && game.CurrentGameStats != null)
			{
				Common.Log.Debug($"Tracker: Note get '{game.CurrentGameStats.Note}'");
				return API.Core.Game.CurrentGameStats.Note;
			}
			Common.Log.Debug($"Tracker: Note set, game not running");
			return null;
		}

		public void UpdateGameNote(string text)
		{
			if (API.Core.Game.IsRunning && API.Core.Game.CurrentGameStats != null)
			{
				API.Core.Game.CurrentGameStats.Note = text;
				Common.Log.Debug($"Tracker: Note set '{text}'");
			}
			else
			{
				Common.Log.Debug("Tracker: Update Note, game not running");
			}
		}

		public void AddDeck(Deck deck)
		{
			if (deck == null)
			{
				Common.Log.Debug("Tracker: Attempting to add a null deck");
				return;
			}
			HDTDeck d = new HDTDeck()
			{
				Name = deck.Name,
				Class = deck.Class.ToString(),
				Cards = new ObservableCollection<HDTCard>(deck.Cards.Select(c => DB.GetCardFromId(c.Id)))
			};
			Common.Log.Debug($"Tracker: Adding deck '{d.Name}' {d.Cards} ({d.Cards.Count})");

			DeckList.Instance.Decks.Add(d);
		}

		public void AddDeck(string name, string playerClass, string cards, bool archive, params string[] tags)
		{
			Common.Log.Debug($"Tracker: Adding Deck ({name}, {playerClass}, {cards.Count()}, {archive}, {tags})");
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
				API.Core.MainWindow.ArchiveDeck(deck, archive);
			}
			else
			{
				Common.Log.Debug($"Tracker: Error importing cards from string");
			}
		}

		public void DeleteAllDecksWithTag(string tag)
		{
			Common.Log.Debug($"Tracker: Deleting decks tagged '{tag}'");
			if (string.IsNullOrWhiteSpace(tag))
				return;
			var decks = DeckList.Instance.Decks.Where(d => d.Tags.Contains(tag)).ToList();
			Common.Log.Debug($"Tracker: {decks.Count} tagged decks found");
			foreach (var d in decks)
				DeckList.Instance.Decks.Remove(d);
			if (decks.Any())
				DeckList.Save();
			Common.Log.Info($"Deleted {decks.Count} tagged with '{tag}'");
		}

		public string GetGameMode()
		{
			var game = API.Core.Game;
			if (game != null && game.IsRunning && game.CurrentGameStats != null)
			{
				Common.Log.Debug($"Tracker: Mode {game.CurrentGameStats.GameMode}");
				return game.CurrentGameStats.GameMode.ToString().ToLowerInvariant();
			}
			Common.Log.Debug($"Tracker: Mode not found");
			return string.Empty;
		}

		public int GetPlayerRank()
		{
			var game = API.Core.Game;
			if (game != null
				&& game.IsRunning
				&& game.CurrentGameStats != null
				&& game.CurrentGameStats.HasRank)
			{
				Common.Log.Debug($"Tracker: Rank found {game.CurrentGameStats.Rank}");
				return game.CurrentGameStats.Rank;
			}
			Common.Log.Debug($"Tracker: Using default rank ${GameFilter.RANK_LO}");
			return GameFilter.RANK_LO;
		}

		public void InvalidateCache()
		{
			DeckCache = null;
			GameCache = null;
			CacheCreatedAt = DateTime.MinValue;
			Common.Log.Debug("Tracker: Invalidated cache");
		}

		public Guid GetActiveDeckId()
		{
			var active = DeckList.Instance.ActiveDeck;
			if (active == null)
			{
				Common.Log.Debug($"Tracker: No ActiveDeck");
				return Guid.Empty;
			}
			Common.Log.Debug($"Tracker: ActiveDeck ${active.DeckId}");
			return active.DeckId;
		}

		private Game CreateGame(GameStats stats)
		{
			Common.Log.Debug("Tracker: Creating Game from "
				+ $"({stats.GameId}, {stats.DeckId}, {stats.DeckName}, {stats.StartTime})");
			var game = new Game();
			var deck = GetDeck(stats.DeckId);
			game.CopyFrom(stats, deck);
			Common.Log.Debug("Tracker: Created Game as "
				+ $"({game.Id}, {game.Deck.Id}, {game.StartTime})");
			return game;
		}

		private Deck CreateDeck(PlayerClass klass, IEnumerable<TrackedCard> cards)
		{
			Common.Log.Debug($"Tracker: CreatingDeck {klass} {cards.Count()}");
			var deck = new Deck()
			{
				Class = klass
			};
			// add the cards to the deck
			// create a temp HDT deck too, to check if its standard
			var hdtDeck = new HDTDeck();
			foreach (var card in cards)
			{
				var c = DB.GetCardFromId(card.Id);
				c.Count = card.Count;
				hdtDeck.Cards.Add(c);
				if (c != null && c != DB.UnknownCard)
				{
					deck.Cards.Add(
						new Card(c.Id, c.LocalizedName, c.Count, c.Background.Clone()));
					Common.Log.Debug($"Tracker: Card {c.Id} x{c.Count}");
				}
				else
				{
					Common.Log.Debug($"Tracker: Card {card.Id} not found");
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
				Common.Log.Error(e);
			}
		}

		private void RefreshCache()
		{
			Common.Log.Debug($"Tracker: Refreshing Cache ${CacheCreatedAt}");
			// Reset the timestamp first (avoids recursive loop)
			CacheCreatedAt = DateTime.Now;
			// Decks
			Reload<DeckList>();
			DeckCache = DeckList.Instance.Decks
				.Where(d => d.Archived == false)
				.Select(d => new Deck(d.DeckId, d.Name, d.IsArenaDeck, d.Class, d.StandardViable))
				.OrderBy(d => d.Name)
				.ToList();
			Common.Log.Debug($"Tracker: Loaded ${DeckCache.Count} decks");
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
			Common.Log.Debug($"Tracker: Loaded ${GameCache.Count} games");
		}

		private bool CacheNeedsRefresh()
		{
			if (CacheCreatedAt == DateTime.MinValue)
				return true;
			return DateTime.Now > CacheCreatedAt.AddSeconds(CacheRefreshLimit);
		}
	}
}