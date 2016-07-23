using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDT.Plugins.Common.Models
{
	public class Deck
	{
		public bool IsArena { get; set; }
		public string Name { get; set; }

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
