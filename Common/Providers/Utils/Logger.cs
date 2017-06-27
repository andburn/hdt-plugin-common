using HDT.Plugins.Common.Enums;
using System;
using System.IO;

namespace HDT.Plugins.Common.Providers.Utils
{
	/// <summary>
	/// A basic file logger
	/// </summary>
	public class Logger
	{
		private static readonly string _defaultName = "Common";
		private static readonly string _defaultDirName = "HearthstoneDeckTracker";
		private string _filePath;

		public Logger()
			: this(_defaultName)
		{
		}

		public Logger(string name)
		{
			try
			{
				_filePath = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
					_defaultDirName, $"{name}.log");
			}
			catch (Exception e)
			{
				ExceptionFallback(e, "Path creation failed");
			}
		}

		public void Log(LogLevel level, string message)
		{
			try
			{
				using (var stream = new StreamWriter(_filePath))
				{
					stream.WriteLine(string.Format("%s [%s] %s",
						level.ToString(), DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), message));
				}
			}
			catch (Exception e)
			{
				ExceptionFallback(e, message);
			}
		}

		public void Log(string message)
		{
			Log(LogLevel.INFO, message);
		}

		private void ExceptionFallback(Exception ex, string message)
		{
			Hearthstone_Deck_Tracker.Utility.Logging.Log.Error(
					$"{message} [{ex.Message}]", "Common.Logger");
		}
	}
}