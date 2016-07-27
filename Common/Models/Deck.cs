using System;
using System.Collections.Generic;

namespace HDT.Plugins.Common.Models
{
	public class Deck
	{
		public bool IsArena { get; set; }
		public string Name { get; set; }
		public Guid DeckId { get; set; }
		public DateTime LastPlayed { get; set; }
		public List<Card> Cards { get; set; }
		public string Class { get; set; }
		public ArenaReward ArenaReward { get; set; }
		public DeckStats DeckStats { get; set; }

		public Deck()
		{
		}

		public Deck(string name, bool arena)
		{
			Name = name;
			IsArena = arena;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}