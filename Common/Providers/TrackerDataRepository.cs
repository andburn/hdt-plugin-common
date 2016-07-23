using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
using Hearthstone_Deck_Tracker;

namespace HDT.Plugins.Common.Providers
{
	internal class TrackerDataRepository : IDataRepository
	{
		public List<Deck> GetAllDecks()
		{
			return DeckList.Instance.Decks
				.Where(d => d.Archived == false)
				.Select(d => new Deck(d.Name, d.IsArenaDeck))
				.OrderBy(d => d.Name)
				.ToList();			
		}
	}
}
