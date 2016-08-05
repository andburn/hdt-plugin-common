using System;
using System.Reflection;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Plugins;

namespace HDT.Plugins.Common.Plugin
{
	/// <summary>
	/// An IPlugin implementation with less boiler plate
	/// </summary>
	public abstract class PluginBase : IPlugin
	{
		private string _name;
		private string _description;
		private string _author;
		private System.Version _version;
		private string _button;
		private Assembly _pluginAssembly;

		public PluginBase()
		{
			GetAttributes();
		}

		public PluginBase(Assembly assembly)
			: this()
		{
			_pluginAssembly = assembly;
		}

		public virtual string Author
		{
			get { return _author ?? "andburn"; }
		}

		public virtual string Name
		{
			get { return _name ?? "My Plugin"; }
		}

		public virtual string Description
		{
			get { return _description ?? "What the plugin does"; }
		}

		public virtual string ButtonText
		{
			get { return _button ?? "Settings"; }
		}

		public virtual System.Version Version
		{
			get
			{
				Hearthstone_Deck_Tracker.Utility.Logging.Log.Debug($"Get() = {GetGitSemVer()} {_version} -");
				return GetGitSemVer() ?? (_version ?? new System.Version(0, 0, 0));
			}
		}

		public virtual void OnUpdate()
		{
		}

		public virtual void OnLoad()
		{
		}

		public virtual void OnUnload()
		{
		}

		public virtual void OnButtonPress()
		{
		}

		public abstract MenuItem MenuItem { get; }

		private void GetAttributes()
		{
			Attribute[] attrs = Attribute.GetCustomAttributes(this.GetType());

			foreach (var atr in attrs)
			{
				if (atr is Name)
					_name = ((Name)atr).Get();
				else if (atr is Description)
					_description = ((Description)atr).Get();
				else if (atr is Author)
					_author = ((Author)atr).Get();
				else if (atr is PluginVersion)
					_version = new System.Version(((PluginVersion)atr).Get());
				else if (atr is ButtonText)
					_button = ((ButtonText)atr).Get();
			}
		}

		private Version GetGitSemVer()
		{
			if (_pluginAssembly == null)
				return null;
			Hearthstone_Deck_Tracker.Utility.Logging.Log.Debug($"name: {_pluginAssembly.FullName}");
			var nspace = GetType().Namespace;
			Hearthstone_Deck_Tracker.Utility.Logging.Log.Debug($"nspace: {nspace}");
			var gitInfo = _pluginAssembly.GetType(nspace + ".GitVersionInformation");
			var semVer = gitInfo.GetField("AssemblySemVer").GetValue(null).ToString();
			Hearthstone_Deck_Tracker.Utility.Logging.Log.Debug($"semver: {semVer}");
			Version result = null;
			Version.TryParse(semVer, out result);
			return result;
		}
	}
}