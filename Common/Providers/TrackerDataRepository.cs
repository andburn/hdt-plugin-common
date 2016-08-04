using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
using HDT.Plugins.Common.Util;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Stats;

namespace HDT.Plugins.Common.Providers
{
	internal class TrackerDataRepository : IDataRepository
	{
		private static readonly BindingFlags bindFlags =
			BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

		public List<Deck> GetAllDecks()
		{
			return DeckList.Instance.Decks
				.Where(d => d.Archived == false)
				.Select(d => new Deck(d.Name, d.IsArenaDeck))
				.OrderBy(d => d.Name)
				.ToList();
		}

		public void AddGames(List<Game> games)
		{
		}

		public List<Game> GetAllGames()
		{
			var games = new List<Game>();
			ReloadDeckStatsList();
			ReloadDefaultDeckStats();
			var ds = new List<DeckStats>(DeckStatsList.Instance.DeckStats.Values);
			ds.AddRange(DefaultDeckStats.Instance.DeckStats);
			foreach (var deck in ds)
			{
				games.AddRange(deck.Games.Select(g => CreateGame(g)));
			}
			return games;
		}

		private void ReloadDeckList()
		{
			Type type = typeof(DeckList);
			MethodInfo method = type.GetMethod("Reload", bindFlags);
			method.Invoke(null, new object[] { });
		}

		private void ReloadDefaultDeckStats()
		{
			Type type = typeof(DefaultDeckStats);
			MethodInfo method = type.GetMethod("Reload", bindFlags);
			method.Invoke(null, new object[] { });
		}

		private void ReloadDeckStatsList()
		{
			Type type = typeof(DeckStatsList);
			MethodInfo method = type.GetMethod("Reload", bindFlags);
			method.Invoke(null, new object[] { });
		}

		private Game CreateGame(GameStats stats)
		{
			var game = new Game();
			game.CopyFrom(stats);
			return game;
		}
	}
}