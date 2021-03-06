﻿using HDT.Plugins.Common.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.Common.Tests
{
	[TestClass]
	public class SettingValueTest
	{
		[TestMethod]
		public void DefaultConstructorHasValueEqualToEmptyString()
		{
			var val = new SettingValue();
			Assert.AreSame(string.Empty, val.Value);
		}

		[TestMethod]
		public void ValuePropShouldBeImmutable()
		{
			var val = new SettingValue("value");
			Assert.AreEqual("value", val.Value);
			var v = val.Value;
			v = "other";
			Assert.AreEqual("value", val.Value);
		}

		[TestMethod]
		public void CanImplicitlyConvertToString()
		{
			string str = new SettingValue("value");
			Assert.AreEqual("value", str);
		}

		[TestMethod]
		public void BoolValueConvertsTrue()
		{
			var val = new SettingValue("True");
			Assert.AreEqual(true, val.Bool);
			val = new SettingValue("tRuE");
			Assert.AreEqual(true, val.Bool);
		}

		[TestMethod]
		public void BoolValueConvertsFalse()
		{
			var val = new SettingValue("False");
			Assert.AreEqual(false, val.Bool);
			val = new SettingValue("fALse");
			Assert.AreEqual(false, val.Bool);
		}

		[TestMethod]
		public void BoolValueConvertsNonBoolToFalse()
		{
			var val = new SettingValue("");
			Assert.AreEqual(false, val.Bool);
			val = new SettingValue("Some Text");
			Assert.AreEqual(false, val.Bool);
		}

		[TestMethod]
		public void IntValueConvertsInteger()
		{
			var val = new SettingValue("21");
			Assert.AreEqual(21, val.Int);
			val = new SettingValue(" 22  ");
			Assert.AreEqual(22, val.Int);
		}

		[TestMethod]
		public void IntValueConvertsNonIntegerStringToZero()
		{
			var val = new SettingValue("words");
			Assert.AreEqual(0, val.Int);
			val = new SettingValue("catch22  ");
			Assert.AreEqual(0, val.Int);
		}

		[TestMethod]
		public void IntValueConvertsDouble()
		{
			var val = new SettingValue("21");
			Assert.AreEqual(21.0, val.Double);
			val = new SettingValue(" 48.39  ");
			Assert.AreEqual(48.39, val.Double);
		}

		[TestMethod]
		public void IntValueConvertsNonDoubleStringToZero()
		{
			var val = new SettingValue("words");
			Assert.AreEqual(0.0, val.Double);
			val = new SettingValue("catch22  ");
			Assert.AreEqual(0.0, val.Double);
		}

		[TestMethod]
		public void EmptyConstantIsEqualToItself()
		{
			Assert.AreSame(SettingValue.Empty, SettingValue.Empty);
			Assert.AreEqual(SettingValue.Empty, SettingValue.Empty);
		}

		[TestMethod]
		public void EmptyConstantIsEqualToNullValuedObject()
		{
			var nullValue = new SettingValue(null);
			Assert.AreEqual(SettingValue.Empty, nullValue);
			Assert.AreNotSame(SettingValue.Empty, nullValue);
		}

		[TestMethod]
		public void EmptyConstantHasNullValue()
		{
			Assert.IsNull(SettingValue.Empty.Value);
		}

		[TestMethod]
		public void EmptyConstantHasDefaultPropValues()
		{
			Assert.IsFalse(SettingValue.Empty.Bool);
			Assert.AreEqual(SettingValue.Empty.Int, 0);
			Assert.AreEqual(SettingValue.Empty.Double, 0.0);
		}
	}
}