using HDT.Plugins.Common.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static HDT.Plugins.Common.Util.EnumConverter;

namespace HDT.Plugins.Common.Tests.Models
{
	[TestClass]
	public class KlassKonverterTest
	{
		[TestMethod]
		public void NullOrEmptyString_Should_Be_Any()
		{
			Assert.AreEqual(PlayerClass.ALL, ConvertHeroClass(string.Empty));
			Assert.AreEqual(PlayerClass.ALL, ConvertHeroClass(null));
		}

		[TestMethod]
		public void Druid_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.DRUID, ConvertHeroClass("Druid"));
		}

		[TestMethod]
		public void Hunter_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.HUNTER, ConvertHeroClass("Hunter"));
		}

		[TestMethod]
		public void Mage_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.MAGE, ConvertHeroClass("Mage"));
		}

		[TestMethod]
		public void Paladin_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.PALADIN, ConvertHeroClass("Paladin"));
		}

		[TestMethod]
		public void Priest_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.PRIEST, ConvertHeroClass("Priest"));
		}

		[TestMethod]
		public void Rogue_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.ROGUE, ConvertHeroClass("Rogue"));
		}

		[TestMethod]
		public void Shaman_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.SHAMAN, ConvertHeroClass("Shaman"));
		}

		[TestMethod]
		public void Warlockn_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.WARLOCK, ConvertHeroClass("Warlock"));
		}

		[TestMethod]
		public void Warrior_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.WARRIOR, ConvertHeroClass("Warrior"));
		}

		[TestMethod]
		public void NonKlass_Should_Be_Any()
		{
			Assert.AreEqual(PlayerClass.ALL, ConvertHeroClass("Random String"));
		}
	}
}