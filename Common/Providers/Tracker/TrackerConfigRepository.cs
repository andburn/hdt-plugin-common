using System;
using HDT.Plugins.Common.Services;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace HDT.Plugins.Common.Providers.Tracker
{
	public class TrackerConfigRepository : IConfigurationRepository
	{
		private Type _configType;

		public TrackerConfigRepository()
		{
			_configType = typeof(Config);
		}

		public object Get(string key)
		{
			try
			{
				var propInfo = _configType.GetField(key);
				return propInfo.GetValue(Config.Instance);
			}
			catch (NullReferenceException)
			{
				return null;
			}
		}

		public void Set(string key, object value)
		{
			try
			{
				var propInfo = _configType.GetField(key);
				propInfo.SetValue(Config.Instance, value);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}