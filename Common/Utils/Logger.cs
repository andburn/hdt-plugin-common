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

		private bool _isEnabled;

		public bool IsEnabled
		{
			get { return _isEnabled; }
			set { _isEnabled = value; }
		}

		private Logger()
		{
			try
			{
				_filePath = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
					_defaultDirName,
					_defaultFileName);
				IsEnabled = File.Exists(_filePath);
			}
			catch (Exception)
			{
				IsEnabled = false;
			}
		}

		public void Log(string message)
		{
			if (IsEnabled)
			{
				using (var stream = new StreamWriter(_filePath))
				{
					stream.WriteLine(string.Format("[%s] %s",
						DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), message));
				}
			}
		}
	}
}