using System.Globalization;
using HDT.Plugins.Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.Common.Tests.Data
{
	[TestClass]
	public class EnumStringConverterTest
	{
		private static EnumStringConverter _converter;
		private static readonly CultureInfo USCulture = new CultureInfo("en-us");

		[ClassInitialize]
		public static void Setup(TestContext context)
		{
			_converter = new EnumStringConverter();
		}

		[TestMethod]
		public void ShouldConvert_ToTitleCase()
		{
			var result = _converter.Convert(TimeFrame.PREVIOUS_MONTH, typeof(string), null, USCulture);
			Assert.AreEqual("Previous Month", result.ToString());
		}

		[TestMethod]
		public void NoUnderscores_ShouldBeSingleWord()
		{
			var result = _converter.Convert(TimeFrame.TODAY, typeof(string), null, USCulture);
			Assert.AreEqual("Today", result.ToString());
		}

		[TestMethod]
		public void Numbers_ShouldBeUntouched()
		{
			var result = _converter.Convert(TimeFrame.LAST_24_HOURS, typeof(string), null, USCulture);
			Assert.AreEqual("Last 24 Hours", result.ToString());
		}

		[TestMethod]
		public void ShouldConvert_StringEquivalentMode()
		{
			var result = _converter.ConvertBack("Brawl", typeof(GameMode), null, USCulture);
			System.Console.WriteLine(result);
			Assert.AreEqual(GameMode.BRAWL, result);
		}

		[TestMethod]
		public void ShouldReturn_StringEquivalentRegion()
		{
			var result = _converter.ConvertBack("eu", typeof(Region), null, USCulture);
			System.Console.WriteLine(result);
			Assert.AreEqual(Region.EU, result);
		}

		[TestMethod]
		public void ShouldReturn_ZeroForUnknownEnumString()
		{
			var result = _converter.ConvertBack("Sprint", typeof(GameMode), null, USCulture);
			System.Console.WriteLine(result);
			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void ShouldHandleNull()
		{
			var result = _converter.ConvertBack(null, typeof(GameResult), null, USCulture);
			System.Console.WriteLine(result);
			Assert.AreEqual(0, result);
		}
	}
}