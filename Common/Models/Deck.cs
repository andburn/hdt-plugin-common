using System;
using System.Collections.Generic;
using System.Linq;
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
			Cards = new List<Card>();
		}

		public Deck(Deck deck)
			: this()
		{
			if (deck == null)
				return;
			Id = deck.Id;
			IsArena = deck.IsArena;
			Name = deck.Name;
			Class = deck.Class;
			LastPlayed = deck.LastPlayed;
			Cards = deck.Cards;
			IsStandard = deck.IsStandard;
			ArenaReward = ArenaReward;
		}

		public Deck(PlayerClass klass, bool standard)
			: this()
		{
			Class = klass;
			IsStandard = standard;
		}

		public Deck(Guid id, string name, bool arena, string klass, bool standard)
		{
			Id = id;
			Name = name;
			IsArena = arena;
			Class = EnumConverter.ConvertHeroClass(klass);
			IsStandard = standard;
		}

		/// <summary>
		/// Uses the Jaccard index to give a indication of similarity
		/// between the two decks. </summary>
		/// <returns>Returns a float between 0 and 1 inclusive </returns>
		public virtual float Similarity(Deck deck)
		{
			if (deck == null)
				return 0;

			var lenA = Cards.Sum(x => x.Count);
			var lenB = deck.Cards.Sum(x => x.Count);
			var lenAnB = 0;

			if (lenA == 0 && lenB == 0)
				return 1;

			foreach (var i in Cards)
			{
				foreach (var j in deck.Cards)
				{
					if (i.Equals(j))
					{
						lenAnB += Math.Min(i.Count, j.Count);
					}
				}
			}

			return (float)Math.Round((float)lenAnB / (lenA + lenB - lenAnB), 2);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}