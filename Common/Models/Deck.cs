using System;
using System.Collections.Generic;

namespace HDT.Plugins.Common.Models
{
	public class Deck
	{
		public static readonly Deck None = new Deck();

		public Guid Id { get; set; }
		public bool IsArena { get; set; }
		public string Name { get; set; }
		public DateTime LastPlayed { get; set; }
		public List<Card> Cards { get; set; }
		public string Class { get; set; }
		public ArenaReward ArenaReward { get; set; }

		public Deck()
		{
			Id = Guid.NewGuid();
		}

		public Deck(Guid id, string name, bool arena)
		{
			Id = id;
			Name = name;
			IsArena = arena;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}