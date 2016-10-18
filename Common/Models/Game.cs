using System;
using HDT.Plugins.Common.Util;

namespace HDT.Plugins.Common.Models
{
	public class Game
	{
		public Guid Id { get; set; }
		public Deck Deck { get; set; }
		public Version DeckVersion { get; set; }
		public Region Region { get; set; }
		public GameMode Mode { get; set; }
		public GameFormat Format { get; set; }
		public GameResult Result { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public int Rank { get; set; }
		public PlayerClass PlayerClass { get; set; }
		public string PlayerName { get; set; }
		public PlayerClass OpponentClass { get; set; }
		public string OpponentName { get; set; }
		public int Turns { get; set; }
		public int Minutes { get; set; }
		public bool PlayerGotCoin { get; set; }
		public bool WasConceded { get; set; }
		public Note Note { get; set; }

		public Game()
		{
		}

		public Game(GameResult result, string player, string opponent)
			: this()
		{
			Result = result;
			PlayerName = player;
			OpponentName = opponent;
		}

		public override bool Equals(Object other)
		{
			var game = other as Game;
			if (game == null)
				return false;

			return game.Id == Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}