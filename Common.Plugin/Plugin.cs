using System;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Plugins;

namespace HDT.Plugins.Common.Plugin
{
	public class Plugin : IPlugin
	{
		public string Name => "Common";

		public string Description => "A utility plugin used by other plugins";

		public string ButtonText => "About";

		public string Author => "andburn";

		public Version Version => new Version();

		public MenuItem MenuItem => null;

		public void OnButtonPress()
		{
		}

		public void OnLoad()
		{
		}

		public void OnUnload()
		{
		}

		public void OnUpdate()
		{
		}
	}
}