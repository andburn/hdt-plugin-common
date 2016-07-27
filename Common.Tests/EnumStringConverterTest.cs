using HDT.Plugins.Common.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Tests
{
	[TestClass]
	public class EnumStringConverterTest
	{
		private static EnumStringConverter _converter;

		[ClassInitialize]
		public static void Setup(TestContext context)
		{
			_converter = new EnumStringConverter();
		}

		[TestMethod]
		public void ShouldConvert_ToTitleCase()
		{
			var result = _converter.Convert(TimeFrame.PREVIOUS_MONTH, typeof(string), null, new System.Globalization.CultureInfo("en-us"));
			Assert.AreEqual("Previous Month", result.ToString());
		}

		[TestMethod]
		public void NoUnderscores_ShouldBeSingleWord()
		{
			var result = _converter.Convert(TimeFrame.TODAY, typeof(string), null, new System.Globalization.CultureInfo("en-us"));
			Assert.AreEqual("Today", result.ToString());
		}

		[TestMethod]
		public void Numbers_ShouldBeUntouched()
		{
			var result = _converter.Convert(TimeFrame.LAST_24_HOURS, typeof(string), null, new System.Globalization.CultureInfo("en-us"));
			Assert.AreEqual("Last 24 Hours", result.ToString());
		}
	}
}