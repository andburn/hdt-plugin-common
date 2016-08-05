using System;
using HDT.Plugins.Common.Models;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Stats;

namespace HDT.Plugins.Common.Util
{
	public static class GameExtensions
	{
		public static void CopyFrom(this Game game, GameStats stats)
		{
			game.Id = stats.GameId;
			// TODO: add other deck properties
			// FIX: add arena prop from games stats
			game.Deck = new Deck(stats.DeckId, stats.DeckName, false);
			var v = stats.PlayerDeckVersion ?? new SerializableVersion();
			game.DeckVersion = new Version(v.Major, v.Minor, v.Build, v.Revision);
			game.Region = EnumConverter.Convert(stats.Region);
			game.Mode = EnumConverter.Convert(stats.GameMode);
			game.Result = EnumConverter.Convert(stats.Result);
			game.StartTime = stats.StartTime;
			game.EndTime = stats.EndTime;
			game.Rank = stats.Rank;
			game.PlayerClass = EnumConverter.Convert<PlayerClass>(stats.PlayerHero);
			game.PlayerName = stats.PlayerName;
			game.OpponentClass = EnumConverter.Convert<PlayerClass>(stats.OpponentHero);
			game.OpponentName = stats.OpponentName;
			game.Turns = stats.Turns;
			game.Seconds = stats.SortableDuration;
			// QSTN: difference with Coin
			game.PlayerGotCoin = stats.Coin;
			game.WasConceded = stats.WasConceded;
			game.Note = new Note(stats.Note);
		}
	}
}