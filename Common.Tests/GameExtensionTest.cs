using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Providers.Utils;
using Hearthstone_Deck_Tracker.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HDT.Plugins.Common.Tests
{
	[TestClass]
	public class GameExtensionTest
	{
		private GameStats HDTGame;
		private Game PluginGame;
		private Deck Deck;

		[TestInitialize()]
		public void Initialize()
		{
			Deck = new Deck(new Guid(), "A Deck", false, "Mage", true);
			HDTGame = new GameStats()
			{
				GameId = new Guid("2dcabbf1-b4b2-4083-9ade-3b2c722fe269"),
				DeckName = Deck.Name,
				DeckId = Deck.Id,
				StartTime = new DateTime(2016, 2, 10, 22, 0, 0),
				EndTime = new DateTime(2016, 2, 10, 22, 10, 0),
				GameMode = Hearthstone_Deck_Tracker.Enums.GameMode.Brawl,
				Note = "eSports",
				OpponentHero = "Druid",
				OpponentName = "Innkeeper",
				PlayerHero = "Mage",
				PlayerName = "Brode",
				Coin = false,
				Rank = 20,
				Region = Hearthstone_Deck_Tracker.Enums.Region.EU,
				Result = Hearthstone_Deck_Tracker.Enums.GameResult.Win,
				Turns = 15,
				WasConceded = false
			};
			PluginGame = new Game()
			{
				StartTime = new DateTime(2016, 2, 10, 22, 0, 0),
				EndTime = new DateTime(2016, 2, 10, 22, 10, 0),
				Id = new Guid("2dcabbf1-b4b2-4083-9ade-3b2c722fe269"),
				Deck = Deck,
				DeckVersion = new Version(1, 1, 0),
				Region = Region.EU,
				Mode = GameMode.BRAWL,
				Result = GameResult.WIN,
				Rank = 20,
				PlayerClass = PlayerClass.MAGE,
				PlayerName = "Brode",
				OpponentClass = PlayerClass.DRUID,
				OpponentName = "Innkeeper",
				Turns = 15,
				Minutes = 10,
				PlayerGotCoin = false,
				WasConceded = false,
				Note = new Note("eSports")
			};
		}

		[TestMethod]
		public void EqualToTest()
		{
			Assert.IsTrue(PluginGame.EqualTo(HDTGame));
		}

		[TestMethod]
		public void EqualTo_DefaultCtors()
		{
			Assert.IsTrue((new Game()).EqualTo(new GameStats()));
		}

		[TestMethod]
		public void CopyToTest()
		{
			var stats = new GameStats();
			PluginGame.CopyTo(stats);
			Assert.IsTrue(PluginGame.EqualTo(stats));
		}

		[TestMethod]
		public void CopyFromTest()
		{
			var game = new Game();
			game.CopyFrom(HDTGame, Deck);
			Assert.IsTrue(game.EqualTo(HDTGame));
		}

		[TestMethod]
		public void CopyTo_GameDefaultCtor()
		{
			var stats = new GameStats();
			var game = new Game();
			game.CopyTo(stats);
			Assert.IsTrue(game.EqualTo(stats));
		}

		[TestMethod]
		public void CopyFrom_GameStatsDefaultCtor()
		{
			var stats = new GameStats();
			var game = new Game();
			game.CopyFrom(stats, null);
			Assert.IsTrue(game.EqualTo(stats));
		}

		[TestMethod]
		public void CopyTo_HeroCopiedCorrectly()
		{
			var stats = new GameStats();
			var game = new Game()
			{
				PlayerClass = PlayerClass.MAGE,
				OpponentClass = PlayerClass.DRUID
			};
			game.CopyTo(stats);
			Assert.AreEqual("Mage", stats.PlayerHero);
			Assert.AreEqual("Druid", stats.OpponentHero);
		}

		[TestMethod]
		public void CopyTo_FormatCopiedCorrectly()
		{
			var stats = new GameStats();
			var game = new Game()
			{
				Mode = GameMode.RANKED,
				Format = GameFormat.STANDARD
			};
			game.CopyTo(stats);
			Assert.AreEqual(
				Hearthstone_Deck_Tracker.Enums.Format.Standard,
				stats.Format);
			stats.GameMode = Hearthstone_Deck_Tracker.Enums.GameMode.Arena;
			Assert.IsNull(stats.Format);
		}

		[TestMethod]
		public void CopyTo_ModeCopiedCorrectly()
		{
			var stats = new GameStats();
			var game = new Game();

			game.Mode = GameMode.RANKED;
			game.CopyTo(stats);
			Assert.AreEqual(
				Hearthstone_Deck_Tracker.Enums.GameMode.Ranked,
				stats.GameMode);

			game.Mode = GameMode.ALL;
			game.CopyTo(stats);
			Assert.AreEqual(
				Hearthstone_Deck_Tracker.Enums.GameMode.All,
				stats.GameMode);
		}

		[TestMethod]
		public void CopyTo_UseStartTimeAndDurationForNullEndTimes()
		{
			var stats = new GameStats();
			var game = new Game()
			{
				StartTime = new DateTime(2010, 5, 19, 10, 03, 05),
				Minutes = 17
			};
			Assert.AreEqual(DateTime.MinValue, game.EndTime);

			game.CopyTo(stats);
			Assert.AreEqual(new DateTime(2010, 5, 19, 10, 20, 05),
				stats.EndTime);
		}
	}
}