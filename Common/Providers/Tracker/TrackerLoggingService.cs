using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Providers.Utils;
using HDT.Plugins.Common.Services;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System;

namespace HDT.Plugins.Common.Providers.Tracker
{
	public class TrackerLoggingService : ILoggingService
	{
		private bool _dumpToFile = false;
		private string _name;
		private Logger _dumper;

		public void SetDumpFileName(string name)
		{
			_name = name;
		}

		public void EnableDumpToFile()
		{
			_dumpToFile = true;
			_dumper = new Logger(_name);
		}

		public void DisableDumpToFile()
		{
			_dumpToFile = false;
		}

		public void Debug(object obj) => Debug(obj.ToString());

		public void Debug(string message)
		{
			if (_dumpToFile)
				_dumper?.Log(LogLevel.DEBUG, message);

			Log.Debug(message);
		}

		public void Error(object obj) => Error(obj.ToString());

		public void Error(string message)
		{
			if (_dumpToFile)
				_dumper?.Log(LogLevel.ERROR, message);

			Log.Error(message);
		}

		public void Error(Exception ex)
		{
			if (_dumpToFile)
				_dumper?.Log(LogLevel.ERROR, ex.Message);

			Log.Error(ex);
		}

		public void Info(object obj) => Info(obj.ToString());

		public void Info(string message)
		{
			if (_dumpToFile)
				_dumper?.Log(LogLevel.INFO, message);

			Log.Info(message);
		}
	}
}