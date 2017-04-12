using System;
using HDT.Plugins.Common.Data;
using HDT.Plugins.Common.Data.Models;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Stats;

namespace HDT.Plugins.Common.Util
{
	public static class GameExtensions
	{
		public static void CopyFrom(this Game game, GameStats stats, Deck deck)
		{
			game.Id = stats.GameId;
			game.Deck = deck;
			var v = stats.PlayerDeckVersion ?? new SerializableVersion();
			game.DeckVersion = new Version(v.Major, v.Minor, v.Build, v.Revision);
			game.Region = EnumConverter.Convert(stats.Region);
			game.Mode = EnumConverter.Convert(stats.GameMode);
			game.Format = EnumConverter.Convert(stats.Format);
			game.Result = EnumConverter.Convert(stats.Result);
			game.StartTime = stats.StartTime;
			game.EndTime = stats.EndTime;
			game.Rank = stats.Rank;
			game.PlayerClass = EnumConverter.Convert<PlayerClass>(stats.PlayerHero);
			game.PlayerName = stats.PlayerName;
			game.OpponentClass = EnumConverter.Convert<PlayerClass>(stats.OpponentHero);
			game.OpponentName = stats.OpponentName;
			game.Turns = stats.Turns;
			game.Minutes = stats.SortableDuration;
			game.PlayerGotCoin = stats.Coin;
			game.WasConceded = stats.WasConceded;
			game.Note = new Note(stats.Note);
		}

		// TODO check this
		public static void CopyTo(this Game from, GameStats to)
		{
			//from.Deck;
			//from.DeckVersion;
			to.StartTime = from.StartTime;
			to.EndTime = from.EndTime;
			to.GameMode = EnumConverter.Convert(from.Mode);
			to.Format = EnumConverter.Convert(from.Format);
			to.Note = from.Note.Text;
			to.OpponentHero = from.OpponentClass.ToString(); // ??? case matters
			to.OpponentName = from.OpponentName;
			to.PlayerHero = from.PlayerClass.ToString(); // ??? case matters
			to.PlayerName = from.PlayerName;
			to.Coin = from.PlayerGotCoin; // ??? correct
			to.Rank = from.Rank;
			to.Region = EnumConverter.Convert(from.Region);
			to.Result = EnumConverter.Convert(from.Result);
			//to.Duration = from.Seconds;
			to.Turns = from.Turns;
			to.WasConceded = from.WasConceded;
		}
	}
}