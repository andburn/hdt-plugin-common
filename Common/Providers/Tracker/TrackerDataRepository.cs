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
using Hearthstone_Deck_Tracker.Utility;
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

		private List<Deck> DeckCache = null;
		private List<Game> GameCache = null;

		public List<Deck> GetAllDecks()
		{
			if (DeckCache == null)
			{
				Reload<DeckList>();
				DeckCache = DeckList.Instance.Decks
					.Where(d => d.Archived == false)
					.Select(d => new Deck(d.DeckId, d.Name, d.IsArenaDeck, d.Class, d.StandardViable))
					.OrderBy(d => d.Name)
					.ToList();
			}
			return DeckCache;
		}

		public Deck GetDeck(Guid id)
		{
			return GetAllDecks().SingleOrDefault(d => d.Id == id);
		}

		public List<Game> GetAllGames()
		{
			if (GameCache == null)
			{
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
			}
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
		}

		public void UpdateGames(List<Game> games)
		{
			Reload<DeckStatsList>();
			Reload<DefaultDeckStats>();
			var ds = new List<GameStats>(DeckStatsList.Instance.DeckStats.Values.SelectMany(x => x.Games));
			ds.AddRange(DefaultDeckStats.Instance.DeckStats.SelectMany(x => x.Games));
			var dd = ds.ToDictionary(x => x.GameId);
			// make a backup
			var today = DateTime.Today.ToString("ddMMyyyy");
			BackupManager.CreateBackup($"Backup_{today}_plugin.zip");
			// copy all matching games
			foreach (var g in games)
			{
				if (dd.ContainsKey(g.Id))
				{
					g.CopyTo(dd[g.Id]);
				}
			}
			DeckStatsList.Save();
			DefaultDeckStats.Save();
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

		private Game CreateGame(GameStats stats)
		{
			var game = new Game();
			var deck = GetDeck(stats.DeckId);
			game.CopyFrom(stats, deck);
			return game;
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
	}
}