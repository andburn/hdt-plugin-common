using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace HDT.Plugins.Common
{
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
}