using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace HDT.Plugins.Common.Util
{
	public class EnumStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return DependencyProperty.UnsetValue;
			try
			{
				return ToTitleCase((Enum)value);
			}
			catch (Exception)
			{
				return DependencyProperty.UnsetValue;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Enum.ToObject(targetType, value);
		}

		public string ToTitleCase(Enum e)
		{
			var str = new StringBuilder();
			var words = e.ToString().ToLower().Split('_');
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Length > 0)
				{
					str.Append(words[i].Substring(0, 1).ToUpper());
					if (words[i].Length > 1)
						str.Append(words[i].Substring(1));
					if (i < words.Length - 1) // dont add space to the end
						str.Append(" ");
				}
			}
			return str.ToString();
		}
	}

	public static class EnumConverter
	{
		public static T Convert<T>(string value)
		{
			if (typeof(T) == typeof(PlayerClass))
				return (T)(object)ConvertHeroClass(value);
			throw new EnumConversionException(typeof(T) + " is an unsupoorted type");
		}

		public static Region Convert(Hearthstone_Deck_Tracker.Enums.Region region)
		{
			switch (region)
			{
				case Hearthstone_Deck_Tracker.Enums.Region.US:
					return Region.US;

				case Hearthstone_Deck_Tracker.Enums.Region.EU:
					return Region.EU;

				case Hearthstone_Deck_Tracker.Enums.Region.ASIA:
					return Region.ASIA;

				case Hearthstone_Deck_Tracker.Enums.Region.CHINA:
					return Region.CHINA;

				case Hearthstone_Deck_Tracker.Enums.Region.UNKNOWN:
				default:
					return Region.UNKNOWN;
			}
		}

		public static GameMode Convert(Hearthstone_Deck_Tracker.Enums.GameMode mode)
		{
			switch (mode)
			{
				case Hearthstone_Deck_Tracker.Enums.GameMode.All:
					return GameMode.ALL;

				case Hearthstone_Deck_Tracker.Enums.GameMode.Ranked:
					return GameMode.RANKED;

				case Hearthstone_Deck_Tracker.Enums.GameMode.Casual:
					return GameMode.CASUAL;

				case Hearthstone_Deck_Tracker.Enums.GameMode.Arena:
					return GameMode.ARENA;

				case Hearthstone_Deck_Tracker.Enums.GameMode.Brawl:
					return GameMode.BRAWL;

				case Hearthstone_Deck_Tracker.Enums.GameMode.Friendly:
					return GameMode.FRIENDLY;

				case Hearthstone_Deck_Tracker.Enums.GameMode.Practice:
					return GameMode.PRACTICE;

				case Hearthstone_Deck_Tracker.Enums.GameMode.Spectator:
					return GameMode.SPECTATOR;

				default:
				case Hearthstone_Deck_Tracker.Enums.GameMode.None:
					return GameMode.NONE;
			}
		}

		public static GameResult Convert(Hearthstone_Deck_Tracker.Enums.GameResult result)
		{
			switch (result)
			{
				case Hearthstone_Deck_Tracker.Enums.GameResult.Win:
					return GameResult.WIN;

				case Hearthstone_Deck_Tracker.Enums.GameResult.Loss:
					return GameResult.LOSS;

				case Hearthstone_Deck_Tracker.Enums.GameResult.Draw:
				case Hearthstone_Deck_Tracker.Enums.GameResult.None:
				default:
					return GameResult.DRAW;
			}
		}

		private static PlayerClass ConvertHeroClass(string value)
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

	public class EnumConversionException : Exception
	{
		public EnumConversionException()
			: base()
		{
		}

		public EnumConversionException(string message)
			: base(message)
		{
		}
	}
}