using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HDT.Plugins.Common.Services;
using Hearthstone_Deck_Tracker;

namespace HDT.Plugins.Common.Providers
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
				var propInfo = _configType.GetProperty(key);
				return propInfo.GetValue(Config.Instance);
			}
			catch (NullReferenceException)
			{
				return null;
			}
		}

		public void Set(string key, object value)
		{
			var propInfo = _configType.GetProperty(key);
			propInfo.SetValue(Config.Instance, value);
			// allow exceptions to pass up;
			// ArgumentException, TargetException, 
			// MethodAccessException, TargetInvocationException
		}
	}
}
