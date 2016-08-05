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
			Reload<DeckList>();
			return DeckList.Instance.Decks
				.Where(d => d.Archived == false)
				.Select(d => new Deck(d.DeckId, d.Name, d.IsArenaDeck))
				.OrderBy(d => d.Name)
				.ToList();
		}

		public void AddGames(List<Game> games)
		{
		}

		public List<Game> GetAllGames()
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
			return games;
		}

		// DeckList, DefaultDeckStats, DeckStatsList
		private void Reload<T>()
		{
			Type type = typeof(T);
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