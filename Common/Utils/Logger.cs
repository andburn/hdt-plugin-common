using System;
using System.IO;

namespace HDT.Plugins.Common.Utils
{
	/// <summary>
	/// A basic logger to log any errors in the common library.
	/// </summary>
	public class Logger
	{
		private const string _defaultFileName = "Common.log";
		private const string _defaultDirName = "HearthstoneDeckTracker";
		private readonly string _filePath;

		private static Logger _instance;

		public static Logger Instance
		{
			get
			{
				if (_instance == null)
					_instance = new Logger();
				return _instance;
			}
		}

		public bool IsEnabled { get; set; } = true;
		public bool UseTrackerLogging { get; set; } = true;

		private Logger()
		{
			_filePath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				_defaultDirName,
				_defaultFileName);
		}

		public void Log(string message)
		{
			if (IsEnabled)
			{
				if (UseTrackerLogging)
				{
					Hearthstone_Deck_Tracker.Utility.Logging.Log.Info(message);
				}
				else
				{
					try
					{
						using (var stream = new StreamWriter(_filePath))
						{
							stream.WriteLine(string.Format("[%s] %s",
								DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), message));
						}
					}
					catch (Exception e)
					{
						UseTrackerLogging = true;
						Hearthstone_Deck_Tracker.Utility.Logging.Log.Error(e);
					}
				}
			}
		}
	}
}