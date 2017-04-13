using System;
using HDT.Plugins.Common.Data.Services;
using Hearthstone_Deck_Tracker;

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
			var propInfo = _configType.GetField(key);
			propInfo.SetValue(Config.Instance, value);
			// TODO? allow exceptions to pass up;
			// ArgumentException, TargetException,
			// MethodAccessException, TargetInvocationException
		}
	}
}