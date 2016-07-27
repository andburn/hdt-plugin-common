using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
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

		public List<Models.DeckStats> GetAllStats()
		{
			return new List<Models.DeckStats>();
			//ReloadDeckStatsList();
			//ReloadDefaultDeckStats();
			//var ds = new List<DeckStats>(DeckStatsList.Instance.DeckStats.Values);
			//ds.AddRange(DefaultDeckStats.Instance.DeckStats);
			//return ds;
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
	}
}