using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Stats;
using System;
using static HDT.Plugins.Common.Providers.Utils.EnumConverter;

namespace HDT.Plugins.Common.Providers.Utils
{
	public static class GameExtensions
	{
		/// <summary>
		/// Copy GamesStats/Deck into a Game object
		/// Will cause existing properties to be overwritten.
		/// </summary>
		public static void CopyFrom(this Game game, GameStats stats, Deck deck)
		{
			// dont't overwrite game ids with empty
			if (stats.GameId != Guid.Empty)
				game.Id = stats.GameId;
			if (deck == null)
			{
				game.Deck = new Deck(Deck.Empty);
			}
			else
			{
				game.Deck = deck;
			}
			if (stats.PlayerDeckVersion != null)
			{
				game.DeckVersion = new Version(
					stats.PlayerDeckVersion.Major, stats.PlayerDeckVersion.Minor);
			}

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

		/// <summary>
		/// Copy Game into a GameStats object.
		/// Will cause existing properties to be overwritten.
		/// </summary>
		public static void CopyTo(this Game from, GameStats to)
		{
			// dont't overwrite game ids with empty
			if (from.Id != Guid.Empty)
				to.GameId = from.Id;
			if (from.Deck == null || from.Deck.Name == null)
			{
				to.DeckName = Deck.Empty.Name;
				to.DeckId = Deck.Empty.Id;
			}
			else
			{
				to.DeckName = from.Deck.Name;
				to.DeckId = from.Deck.Id;
			}
			if (from.DeckVersion != null)
			{
				to.PlayerDeckVersion = new SerializableVersion(
					from.DeckVersion.Major, from.DeckVersion.Minor);
			}

			to.StartTime = from.StartTime;
			// set end time based on duration
			if (from.Minutes >= 0 && from.StartTime > DateTime.MinValue)
				to.EndTime = from.StartTime.AddMinutes(from.Minutes);
			else
				to.EndTime = from.EndTime;

			to.GameMode = Convert(from.Mode);
			to.Format = Convert(from.Format);
			to.Note = from.Note?.ToString();
			to.OpponentHero = EnumStringConverter.ToTitleCase(from.OpponentClass);
			to.OpponentName = from.OpponentName;
			to.PlayerHero = EnumStringConverter.ToTitleCase(from.PlayerClass);
			to.PlayerName = from.PlayerName;
			to.Coin = from.PlayerGotCoin;
			to.Rank = from.Rank;
			to.Region = Convert(from.Region);
			to.Result = Convert(from.Result);
			to.Turns = from.Turns;
			to.WasConceded = from.WasConceded;
		}

		/// <summary>
		/// Tests for equality between a Game object and a GameStats object
		/// </summary>
		public static bool EqualTo(this Game game, GameStats stats)
		{
			return game.Id == stats.GameId &&
				game.Deck?.Name == stats.DeckName &&
				game.Deck?.Id == stats.DeckId &&
				game.Region == Convert(stats.Region) &&
				game.Mode == Convert(stats.GameMode) &&
				game.Format == Convert(stats.Format) &&
				game.Result == Convert(stats.Result) &&
				game.StartTime == stats.StartTime &&
				game.EndTime == stats.EndTime &&
				game.Rank == stats.Rank &&
				game.PlayerClass == Enums.Convert.ToHeroClass(stats.PlayerHero) &&
				game.PlayerName == stats.PlayerName &&
				game.OpponentClass == Enums.Convert.ToHeroClass(stats.OpponentHero) &&
				game.OpponentName == stats.OpponentName &&
				game.Turns == stats.Turns &&
				game.Minutes == stats.SortableDuration &&
				game.PlayerGotCoin == stats.Coin &&
				game.WasConceded == stats.WasConceded &&
				game.Note?.Text == stats.Note &&
				DeckVersionsAreEqual(game.DeckVersion, stats.PlayerDeckVersion);
		}

		private static bool DeckVersionsAreEqual(Version v, SerializableVersion sv)
		{
			if (v == null && sv == null)
				return true;
			else if (v == null || sv == null)
				return false;

			return v.Major == sv.Major && v.Minor == sv.Minor;
		}
	}
}