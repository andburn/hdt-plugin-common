using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace HDT.Plugins.Common.Utils
{
	/// <summary>
	/// Convert a string to all upper or lower case.
	/// </summary>
	/// <remarks>based on https://stackoverflow.com/a/29841405</remarks>
	public class CaseConverter : IValueConverter
	{
		// change in xaml with:
		// <local:CaseConverter Case="Lower" x:Key="CaseConverter"/>
		public CharacterCasing Case { get; set; }

		public CaseConverter()
		{
			Case = CharacterCasing.Upper;
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var str = value as string;
			if (str != null)
			{
				switch (Case)
				{
					case CharacterCasing.Lower:
						return str.ToLower();

					case CharacterCasing.Upper:
						return str.ToUpper();

					default:
						return str;
				}
			}
			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}