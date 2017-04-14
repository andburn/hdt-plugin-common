using System.Collections.Generic;

namespace HDT.Plugins.Common.Models
{
	public class ArenaReward
	{
		public int Gold { get; set; }
		public int Dust { get; set; }
		public List<Card> Cards { get; set; }
		public List<string> Packs { get; set; }
		public string PaymentMethod { get; set; }
	}
}