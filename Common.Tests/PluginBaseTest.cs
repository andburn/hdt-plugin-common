using System;
using System.Reflection;
using System.Windows.Controls;
using HDT.Plugins.Common.Plugin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Tests
{
	[TestClass]
	public class PluginBaseTest
	{
		[TestMethod]
		public void DefaultConstructor_PropertiesShouldBeCorrect()
		{
			var plugin = new ConcretePluginBase();
			Assert.IsTrue(AllPropertiesAreDefault(plugin));
		}

		[TestMethod]
		public void AssemblyConstructor_WithNullParam_PropertiesShouldBeCorrect()
		{
			var plugin = new ConcretePluginBase(null);
			Assert.IsTrue(AllPropertiesAreDefault(plugin));
		}

		[TestMethod]
		public void AssemblyConstructor_WithNoGitInfo_PropertiesShouldBeCorrect()
		{
			var plugin = new ConcretePluginBase(Assembly.GetExecutingAssembly());
			Assert.IsTrue(AllPropertiesAreDefault(plugin));
		}

		[TestMethod]
		public void AssemblyConstructor_WithNullParam_AndVersionDefined_HasCorrectVersion()
		{
			var plugin = new AttributedPluginBase(null);
			Assert.AreEqual(new Version(1, 2, 3, 4), plugin.Version);
		}

		[TestMethod]
		public void NameAndDescriptionAttributes_AreCorrect()
		{
			var plugin = new AttributedPluginBase();
			Assert.AreEqual("AttributedPlugin", plugin.Name);
			Assert.AreEqual("Plugin Description", plugin.Description);
		}

		private bool AllPropertiesAreDefault(PluginBase plugin)
		{
			return ("andburn" == plugin.Author)
				&& ("My Plugin" == plugin.Name)
				&& ("What the plugin does" == plugin.Description)
				&& ("Settings" == plugin.ButtonText)
				&& (new Version(0, 0, 0, 0) == plugin.Version);
		}
	}

	internal class ConcretePluginBase : PluginBase
	{
		public ConcretePluginBase()
			: base()
		{
		}

		public ConcretePluginBase(Assembly assembly)
			: base(assembly)
		{
		}

		public override MenuItem MenuItem
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}

	[Name("AttributedPlugin")]
	[HDT.Plugins.Common.Plugin.Description("Plugin Description")]
	[PluginVersion("1.2.3.4")]
	internal class AttributedPluginBase : PluginBase
	{
		public AttributedPluginBase()
			: base()
		{
		}

		public AttributedPluginBase(Assembly assembly)
			: base(assembly)
		{
		}

		public override MenuItem MenuItem
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}