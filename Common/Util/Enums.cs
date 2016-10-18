namespace HDT.Plugins.Common.Util
{
	public enum PlayerClass
	{
		ALL,
		DRUID,
		MAGE,
		HUNTER,
		WARLOCK,
		WARRIOR,
		SHAMAN,
		PRIEST,
		PALADIN,
		ROGUE
	}

	public enum GameMode
	{
		ALL,
		RANKED,
		CASUAL,
		ARENA,
		BRAWL,
		FRIENDLY,
		PRACTICE,
		SPECTATOR,
		NONE
	}

	public enum GameResult
	{
		WIN,
		LOSS,
		DRAW
	}

	public enum Region
	{
		ALL = -1,
		UNKNOWN = 0,
		US = 1,
		EU = 2,
		ASIA = 3,
		CHINA = 5
	}

	public enum TimeFrame
	{
		ALL,
		TODAY,
		YESTERDAY,
		LAST_24_HOURS,
		THIS_WEEK,
		PREVIOUS_WEEK,
		LAST_7_DAYS,
		THIS_MONTH,
		PREVIOUS_MONTH,
		THIS_YEAR,
		PREVIOUS_YEAR
	}

	public enum Position
	{
		TOP,
		BOTTOM,
		LEFT,
		RIGHT
	}

	public enum GameFormat
	{
		ANY,
		WILD,
		STANDARD
	}
}