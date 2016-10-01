using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
using HDT.Plugins.Common.Util;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Stats;
using Hearthstone_Deck_Tracker.Utility;

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

			// TODO some equality so don't overwrite every game
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
		// TODO handle errors
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