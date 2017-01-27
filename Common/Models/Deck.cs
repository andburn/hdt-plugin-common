using System;
using System.Collections.Generic;
using HDT.Plugins.Common.Util;

namespace HDT.Plugins.Common.Models
{
	public class Deck
	{
		public static readonly Deck None = new Deck() { Name = "No Deck" };

		public Guid Id { get; set; }
		public bool IsArena { get; set; }
		public string Name { get; set; }
		public DateTime LastPlayed { get; set; }
		public List<Card> Cards { get; set; }
		public PlayerClass Class { get; set; }
		public bool IsStandard { get; set; }
		public ArenaReward ArenaReward { get; set; }

		public Deck()
		{
			Id = Guid.NewGuid();
		}

		public Deck(Guid id, string name, bool arena, string klass, bool standard)
		{
			Id = id;
			Name = name;
			IsArena = arena;
			Class = EnumConverter.ConvertHeroClass(klass);
			IsStandard = standard;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}