using System;
using HDT.Plugins.Common.Util;

namespace HDT.Plugins.Common.Models
{
	public class GameStats
	{
		public Guid DeckId { get; set; }
		public Region Region { get; set; }
		public GameMode GameMode { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public string DeckName { get; set; }
		public string PlayerDeckVersionString { get; set; }
		public string PlayerHero { get; set; }
		public string RegionString { get; set; }
		public string RankString { get; set; }
		public string GotCoin { get; set; }
		public string OpponentHero { get; set; }
		public string OpponentName { get; set; }
		public int Turns { get; set; }
		public int SortableDuration { get; set; }
		public GameResult Result { get; set; }
		public bool WasConceded { get; set; }
		public Guid GameId { get; set; }
		public string Note { get; set; }

		public GameStats()
		{
		}

		public GameStats(GameResult result, string player, string opponent)
		{
		}
	}
}