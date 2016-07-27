using System;
using System.Collections.Generic;

namespace HDT.Plugins.Common.Models
{
	public class DeckStats
	{
		public Guid DeckId { get; set; }
		public List<GameStats> Games { get; set; }

		public DeckStats()
		{
		}

		public DeckStats(Deck deck)
		{
		}
	}
}