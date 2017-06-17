using HDT.Plugins.Common.Enums;
using static HDT.Plugins.Common.Enums.Convert;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.Common.Tests
{
	[TestClass]
	public class HeroStringConverterTest
	{
		[TestMethod]
		public void NullOrEmptyString_Should_Be_Any()
		{
			Assert.AreEqual(PlayerClass.ALL, ToHeroClass(string.Empty));
			Assert.AreEqual(PlayerClass.ALL, ToHeroClass(null));
		}

		[TestMethod]
		public void Druid_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.DRUID, ToHeroClass("Druid"));
		}

		[TestMethod]
		public void Hunter_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.HUNTER, ToHeroClass("Hunter"));
		}

		[TestMethod]
		public void Mage_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.MAGE, ToHeroClass("Mage"));
		}

		[TestMethod]
		public void Paladin_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.PALADIN, ToHeroClass("Paladin"));
		}

		[TestMethod]
		public void Priest_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.PRIEST, ToHeroClass("Priest"));
		}

		[TestMethod]
		public void Rogue_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.ROGUE, ToHeroClass("Rogue"));
		}

		[TestMethod]
		public void Shaman_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.SHAMAN, ToHeroClass("Shaman"));
		}

		[TestMethod]
		public void Warlockn_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.WARLOCK, ToHeroClass("Warlock"));
		}

		[TestMethod]
		public void Warrior_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.WARRIOR, ToHeroClass("Warrior"));
		}

		[TestMethod]
		public void NonKlass_Should_Be_Any()
		{
			Assert.AreEqual(PlayerClass.ALL, ToHeroClass("Random String"));
		}
	}
}