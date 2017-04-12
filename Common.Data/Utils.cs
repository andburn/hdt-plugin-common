namespace HDT.Plugins.Common.Data
{
	public static class Utils
	{
		public static PlayerClass ConvertHeroClass(string value)
		{
			switch (value?.ToLowerInvariant().Trim())
			{
				case "druid":
					return PlayerClass.DRUID;

				case "hunter":
					return PlayerClass.HUNTER;

				case "mage":
					return PlayerClass.MAGE;

				case "paladin":
					return PlayerClass.PALADIN;

				case "priest":
					return PlayerClass.PRIEST;

				case "rogue":
					return PlayerClass.ROGUE;

				case "shaman":
					return PlayerClass.SHAMAN;

				case "warlock":
					return PlayerClass.WARLOCK;

				case "warrior":
					return PlayerClass.WARRIOR;

				default:
					return PlayerClass.ALL;
			}
		}
	}
}