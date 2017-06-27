using HDT.Plugins.Common.Providers.Tracker;
using HDT.Plugins.Common.Services;

namespace HDT.Plugins.Common
{
	public static class Common
	{
		private static ILoggingService _log;

		public static ILoggingService Log
		{
			get
			{
				if (_log == null)
					_log = new TrackerLoggingService();
				return _log;
			}
			set { _log = value; }
		}
	}
}