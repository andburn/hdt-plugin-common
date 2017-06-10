using System;
using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Providers.Utils;
using Hearthstone_Deck_Tracker.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
			HDTGame = new GameStats() {
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
			PluginGame = new Game() {
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
	}
}