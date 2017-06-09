using System;
using HDT.Plugins.Common.Models;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Stats;
using static HDT.Plugins.Common.Providers.Utils.EnumConverter;

namespace HDT.Plugins.Common.Providers.Utils
{
	public static class GameExtensions
	{
		public static void CopyFrom(this Game game, GameStats stats, Deck deck)
		{
			game.Id = stats.GameId;
			game.Deck = deck;
			var v = stats.PlayerDeckVersion ?? new SerializableVersion();
			game.DeckVersion = new Version(v.Major, v.Minor, v.Build, v.Revision);
			game.Region = Convert(stats.Region);
			game.Mode = Convert(stats.GameMode);
			game.Format = Convert(stats.Format);
			game.Result = Convert(stats.Result);
			game.StartTime = stats.StartTime;
			game.EndTime = stats.EndTime;
			game.Rank = stats.Rank;
			game.PlayerClass = Enums.Convert.ToHeroClass(stats.PlayerHero);
			game.PlayerName = stats.PlayerName;
			game.OpponentClass = Enums.Convert.ToHeroClass(stats.OpponentHero);
			game.OpponentName = stats.OpponentName;
			game.Turns = stats.Turns;
			game.Minutes = stats.SortableDuration;
			game.PlayerGotCoin = stats.Coin;
			game.WasConceded = stats.WasConceded;
			game.Note = new Note(stats.Note);
		}

		public static void CopyTo(this Game from, GameStats to)
		{
			//from.Deck;
			//from.DeckVersion;
			to.StartTime = from.StartTime;
			to.EndTime = from.EndTime;
			to.GameMode = Convert(from.Mode);
			to.Format = Convert(from.Format);
			to.Note = from.Note.Text;
			to.OpponentHero = from.OpponentClass.ToString(); // ??? case matters
			to.OpponentName = from.OpponentName;
			to.PlayerHero = from.PlayerClass.ToString(); // ??? case matters
			to.PlayerName = from.PlayerName;
			to.Coin = from.PlayerGotCoin; // ??? correct
			to.Rank = from.Rank;
			to.Region = Convert(from.Region);
			to.Result = Convert(from.Result);
			//to.Duration = from.Seconds;
			to.Turns = from.Turns;
			to.WasConceded = from.WasConceded;
		}
	}
}