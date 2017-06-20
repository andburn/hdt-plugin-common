using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Providers.Tracker;
using HDT.Plugins.Common.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static HDT.Plugins.Common.Tests.TestHelper;

namespace HDT.Plugins.Common.Tests
{
	[TestClass]
	public class TrackerDataRepositoryTest
	{
		private static readonly string DirName = "HearthstoneDeckTracker";
		private static readonly string BackupName = "HearthstoneDeckTracker_Backup";
		private static string AppData;
		private static string DataPath;
		private static string BackupPath;
		private static IDataRepository data;
		private static Game newGameNamedDeck;
		private static Game newGameClassOnly;
		private static Game existingGameHasGameId;
		private static Game existingGameFuzzy;

		[ClassInitialize]
		public static void OneTimeSetup(TestContext context)
		{
			AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			DataPath = Path.Combine(AppData, DirName);
			BackupPath = Path.Combine(AppData, BackupName);
			if (Directory.Exists(BackupPath))
			{
				Directory.Delete(BackupPath, true);
			}
			DirectoryCopy(DataPath, BackupPath, true);
			Directory.Delete(DataPath, true);
			Directory.CreateDirectory(DataPath);
			DirectoryCopy(@".\Data", DataPath, false);

			data = new TrackerDataRepository();

			newGameNamedDeck = new Game() {
				Deck = new Deck(Guid.NewGuid(), "Beasts", false, "Hunter", true),
				Region = Enums.Region.EU,
				Result = Enums.GameResult.WIN,
				PlayerClass = Enums.PlayerClass.HUNTER
			};
			newGameClassOnly = new Game() {
				Region = Enums.Region.US,
				Result = Enums.GameResult.LOSS,
				PlayerClass = Enums.PlayerClass.WARLOCK
			};
			existingGameHasGameId = new Game() {
				Id = new Guid("35c8ea97-b251-4fc5-bf1c-b61f8ca4a694"),
				Region = Enums.Region.EU,
				Result = Enums.GameResult.LOSS,
				PlayerClass = Enums.PlayerClass.MAGE,
				OpponentClass = Enums.PlayerClass.PRIEST
			};
			existingGameFuzzy = new Game() {
				Region = Enums.Region.US,
				Result = Enums.GameResult.LOSS,
				PlayerClass = Enums.PlayerClass.HUNTER,
				OpponentName = "SomeGuy",
				OpponentClass = PlayerClass.DRUID,
				Rank = 18,
				StartTime = new DateTime(2017, 07, 21, 10, 17, 01),
			};
		}

		[ClassCleanup]
		public static void OneTimeTearDown()
		{
			Directory.Delete(DataPath, true);
			Directory.CreateDirectory(DataPath);
			DirectoryCopy(BackupPath, DataPath, true);
			Directory.Delete(BackupPath, true);
		}

		[TestInitialize]
		public void Setup()
		{
			data.InvalidateCache();
			DirectoryCopy(@".\Data", DataPath, false, true);
		}

		[TestMethod]
		public void GetAllDecks_ShouldNotIncludeArchived()
		{
			Assert.AreEqual(2, data.GetAllDecks().Count);
		}

		[TestMethod]
		public void GetAllGames_ShouldIncludeArchivedDecks()
		{
			Assert.AreEqual(7, data.GetAllGames().Count);
		}

		[TestMethod]
		public void GetAllGamesWithDeck_DeckExists()
		{
			var deck = data.GetAllDecks().Single(d => d.Name == "Beasts");
			Assert.AreEqual(2, data.GetAllGamesWithDeck(deck.Id).Count);
		}

		[TestMethod]
		public void GetAllGamesWithDeck_DeckNotExists()
		{
			Assert.AreEqual(0, data.GetAllGamesWithDeck(Guid.NewGuid()).Count);
		}

		[TestMethod]
		public void GetAllDecksWithTag_Found()
		{
			Assert.AreEqual(1, data.GetAllDecksWithTag("Poor").Count);
		}

		[TestMethod]
		public void GetAllDecksWithTag_NotFound()
		{
			Assert.AreEqual(0, data.GetAllDecksWithTag("Awesome").Count);
		}

		[TestMethod]
		public void Add_NewGame_ByDeckName()
		{
			var deck = data.GetAllDecks().Single(d => d.Name == "Beasts");
			var count = data.AddGames(new List<Game>() { newGameNamedDeck });
			Assert.AreEqual(1, count);
			data.InvalidateCache();
			Assert.AreEqual(8, data.GetAllGames().Count);
			Assert.AreEqual(3, data.GetAllGamesWithDeck(deck.Id).Count);			
		}

		[TestMethod]
		public void Add_NewGame_AsDefaultDeck()
		{
			var count = data.AddGames(new List<Game>() { newGameClassOnly });
			Assert.AreEqual(1, count);
			data.InvalidateCache();
			Assert.AreEqual(8, data.GetAllGames().Count);
			Assert.AreEqual(1,
				data.GetAllGamesWithDeck(Guid.Empty)
				.Where(g => g.PlayerClass == Enums.PlayerClass.WARLOCK)
				.ToList().Count);
		}

		[TestMethod]
		public void Add_Game_WithGameId()
		{
			var game = data.GetAllGames().Single(g => g.Id == existingGameHasGameId.Id);
			Assert.AreEqual(GameResult.WIN, game.Result);
			Assert.AreEqual(PlayerClass.DRUID, game.OpponentClass);
			Assert.AreEqual("Secrets", game.Deck.Name);

			var count = data.AddGames(new List<Game>() { existingGameHasGameId });
			Assert.AreEqual(1, count);
			data.InvalidateCache();

			game = data.GetAllGames().Single(g => g.Id == existingGameHasGameId.Id);
			Assert.AreEqual(7, data.GetAllGames().Count);
			Assert.AreEqual(GameResult.LOSS, game.Result);
			Assert.AreEqual(PlayerClass.PRIEST, game.OpponentClass);
			Assert.AreEqual("Secrets", game.Deck.Name);
		}

		[TestMethod]
		public void Add_Game_FromFuzzyIndex()
		{
			var gameId = new Guid("d76a258a-c5c6-4728-89eb-5ae73a2b6d81");
			var game = data.GetAllGames().Single(g => g.Id == gameId);
			Assert.AreEqual(GameResult.WIN, game.Result);
			Assert.AreEqual(20, game.Rank);
			Assert.AreEqual("Beasts", game.Deck.Name);

			var count = data.AddGames(new List<Game>() { existingGameFuzzy });
			Assert.AreEqual(1, count);
			data.InvalidateCache();

			game = data.GetAllGames().Single(g => g.Id == gameId);
			Assert.AreEqual(7, data.GetAllGames().Count);
			Assert.AreEqual(GameResult.LOSS, game.Result);
			Assert.AreEqual(18, game.Rank);
			Assert.AreEqual("Beasts", game.Deck.Name);
		}

		[TestMethod]
		public void DeleteDeck_UsingValidTag()
		{
			Assert.AreEqual(1, data.GetAllDecksWithTag("Favorite").Count);
			data.DeleteAllDecksWithTag("Favorite");
			Assert.AreEqual(0, data.GetAllDecksWithTag("Favorite").Count);
		}

		[TestMethod]
		public void DeleteDeck_UsingInValidTag()
		{
			data.DeleteAllDecksWithTag("Legend");
			Assert.AreEqual(2, data.GetAllDecks().Count);
		}
	}
}