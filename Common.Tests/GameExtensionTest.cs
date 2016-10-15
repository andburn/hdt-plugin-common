using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hearthstone_Deck_Tracker.Stats;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Util;

namespace Common.Tests
{
	[TestClass]
	public class GameExtensionTest
	{
		private GameStats HDTGame;
		private Game PluginGame;

		[TestInitialize()]
		public void Initialize()
		{
			HDTGame = new GameStats() {
				StartTime = new DateTime(2016, 1, 1, 12, 0, 0),
				EndTime = new DateTime(2016, 1, 1, 12, 0, 0),
				GameMode = Hearthstone_Deck_Tracker.Enums.GameMode.Brawl,
				Note = null,
				OpponentHero = "Druid",
				OpponentName = "Innkeeper",
				PlayerHero = "Mage",
				PlayerName = "Brode",
				Coin = false,
				Rank = 20,
				Region = Hearthstone_Deck_Tracker.Enums.Region.US,
				Result = Hearthstone_Deck_Tracker.Enums.GameResult.Loss,
				Turns = 10,
				WasConceded = false
			};
			PluginGame = new Game() {
				StartTime = new DateTime(2016, 2, 10, 22, 0, 0),
				EndTime = new DateTime(2016, 2, 10, 22, 10, 0),
				Id = new Guid(),
				Deck = new Deck(),
				DeckVersion = new Version(),
				Region = Region.EU,
				Mode = GameMode.BRAWL,
				Result = GameResult.WIN,
				Rank = 20,
				PlayerClass = PlayerClass.DRUID,
				PlayerName = "Innkeeper",
				OpponentClass = PlayerClass.MAGE,
				OpponentName = "Brode",
				Turns = 10,
				Minutes = 0,
				PlayerGotCoin = false,
				WasConceded = false,
				Note = new Note(),
			};
		}

		[TestMethod]
		public void CopyTest()
		{
			Assert.AreEqual(0, HDTGame.SortableDuration);
			PluginGame.CopyTo(HDTGame);
			Assert.AreEqual(10, HDTGame.SortableDuration);
		}
	}
}
