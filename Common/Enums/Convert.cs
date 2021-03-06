﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace HDT.Plugins.Common.Enums
{
	public static class Convert
	{
		public static PlayerClass ToHeroClass(string value)
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

	public class EnumStringConverter : IValueConverter
	{
		private static readonly Dictionary<Enum, string> _lookup = new Dictionary<Enum, string>();

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
			object result = 0;
			try
			{
				result = Enum.Parse(targetType, value.ToString(), true);
			}
			catch (Exception e)
			{
				Common.Log.Error($"Enum Converison failed ({e.Message})");
			}
			return result;
		}

		public static string ToTitleCase(Enum e)
		{
			if (_lookup.ContainsKey(e))
				return _lookup[e];

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
			Common.Log.Debug($"Convert: ToTitleCase ({e} -> {str})");
			_lookup[e] = str.ToString();
			return _lookup[e];
		}
	}
}