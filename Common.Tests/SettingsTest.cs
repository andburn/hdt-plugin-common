using HDT.Plugins.Common.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Tests
{
	[TestClass]
	public class SettingsTest
	{
		private static Settings settings;

		[TestInitialize]
		public void Init()
		{
			var ini = new string[] {
				"First = 1",
				"Second=two",
				"Third = false",
				"[Section 1]",
				"First=First Section"
			};
			settings = new Settings(string.Join("\n", ini));
			settings.RestoreDefaults();
		}

		[TestMethod]
		public void DefaultCtor_HasEmptySettings()
		{
			var s = new Settings();
			Assert.IsTrue(s.DefaultIsEmpty);
			Assert.IsTrue(s.UserIsEmpty);
		}

		[TestMethod]
		public void ArgumentCtor_ShouldHaveNonEmptyDefaultAndEmptyUser()
		{
			Assert.IsFalse(settings.DefaultIsEmpty);
			Assert.IsTrue(settings.UserIsEmpty);
		}

		[TestMethod]
		public void Get_GlobalSetting()
		{
			Assert.AreEqual(1, settings.Get("First").Int);
			Assert.AreEqual("two", settings.Get("Second"));
		}

		[TestMethod]
		public void Get_SectionSetting()
		{
			Assert.AreEqual("First Section", settings.Get("Section 1", "First"));
		}

		[TestMethod]
		public void Get_UnknownKeyInSectionHasNullValue()
		{
			Assert.IsNull(settings.Get("Section 1", "Unknown").Value);
		}

		[TestMethod]
		public void Get_UnknownKeyHasNullValue()
		{
			Assert.IsNull(settings.Get("Unknown").Value);
		}

		[TestMethod]
		public void Get_UnknownSectionHasNullValue()
		{
			Assert.IsNull(settings.Get("Non Section", "First Section").Value);
		}

		[TestMethod]
		public void Get_NullKeyHasNullValue()
		{
			Assert.IsNull(settings.Get(null).Value);
			Assert.IsNull(settings.Get("Section 1", null).Value);
		}

		[TestMethod]
		public void Get_NullSectionHasNullValue()
		{
			Assert.IsNull(settings.Get(null, "Unknown").Value);
		}

		[TestMethod]
		public void Set_DoesNotChangeDefaultValues()
		{
			Assert.AreEqual("1", settings.Get("First"));
			settings.Set("First", "2");
			Assert.AreEqual("2", settings.Get("First"));
			Assert.AreEqual("1", settings.GetDefault("First"));
		}

		[TestMethod]
		public void DefaultValueIsReturnedIfNotInUser()
		{
			settings.Set("Third", "3");
			Assert.AreEqual("1", settings.Get("First"));
		}

		[TestMethod]
		public void DecodesUnicodeStrings()
		{
			var unicode = new Settings("Name = シオヲ");
			Assert.AreEqual("\u30b7\u30AA\u30F2", unicode.Get("Name"));
		}
	}
}