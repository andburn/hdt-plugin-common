using System;
using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using Hearthstone_Deck_Tracker.Stats;

namespace HDT.Plugins.Common.Providers.Utils
{
	/// <summary>
	/// Used to create a fuzzy index for the creation of keys for HashTables/Dictionaries.
	/// When a rough comparison between Game and GameStats is required.
	/// </summary>
	public class GameIndex
	{
		public string OpponentName { get; set; }
		public PlayerClass OpponentClass { get; set; }
		public DateTime StartTime { get; set; }

		public GameIndex(GameStats game)
		{
			OpponentName = game.OpponentName;
			OpponentClass = Enums.Convert.ToHeroClass(game.OpponentHero);
			var time = new DateTime(
				game.StartTime.Year, game.StartTime.Month, game.StartTime.Day,
				game.StartTime.Hour, game.StartTime.Minute, game.StartTime.Second);
			StartTime = time.ToUniversalTime();
		}

		public GameIndex(Game game)
		{
			OpponentName = game.OpponentName;
			OpponentClass = game.OpponentClass;
			var time = new DateTime(
				game.StartTime.Year, game.StartTime.Month, game.StartTime.Day,
				game.StartTime.Hour, game.StartTime.Minute, game.StartTime.Second);
			StartTime = time.ToUniversalTime();
		}

		public override bool Equals(object obj)
		{
			var g = obj as GameIndex;
			if (g == null)
				return false;

			return OpponentName.Equals(g.OpponentName)
				&& OpponentClass.Equals(g.OpponentClass)
				&& StartTime.Year == g.StartTime.Year
				&& StartTime.Month == g.StartTime.Month
				&& StartTime.Day == g.StartTime.Day
				&& StartTime.Hour == g.StartTime.Hour
				&& StartTime.Minute == g.StartTime.Minute
				&& StartTime.Second == g.StartTime.Second;
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		public override string ToString()
		{
			return $"{OpponentName}_{OpponentClass.ToString()}.{StartTime.Ticks}";
		}
	}
}