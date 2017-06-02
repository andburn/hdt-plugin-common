using System;
using System.IO;
using System.Threading;
using HDT.Plugins.Common.Utils;
using IniParser;
using IniParser.Model;
using IniParser.Parser;

namespace HDT.Plugins.Common.Settings
{
	public class Settings
	{
		private static readonly string DefaultName = "Settings";
		private static readonly string DefaultDir = "HearthstoneDeckTracker";
		private static readonly IniDataParser StringParser = new IniDataParser();
		private static readonly FileIniDataParser FileParser = new FileIniDataParser();
		private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

		private string _userFile;
		private IniData _default; // created on object construction, read only
		private IniData _user; // created on set, read write
		private IniData _merged; // default overwritten with user

		public Settings()
		{
			Initialize();
			SetUserFile();
		}

		public Settings(string ini, string name = null, string dir = null)
		{
			Initialize();
			SetUserFile(name, dir);
			InitializeDefault(ini);
		}

		public Settings(Stream stream, string name = null, string dir = null)
		{
			Initialize();
			SetUserFile(name, dir);
			string ini = null;
			try
			{
				using (var reader = new StreamReader(stream))
				{
					ini = reader.ReadToEnd();
				}
			}
			catch (Exception e)
			{
				Logger.Instance.Log(e.Message);
			}
			InitializeDefault(ini);
		}

		public Settings(FileInfo file, string name = null, string dir = null)
		{
			Initialize();
			SetUserFile(name, dir);
			string ini = null;
			try
			{
				ini = File.ReadAllText(file.FullName);
			}
			catch (Exception e)
			{
				Logger.Instance.Log(e.Message);
			}
			InitializeDefault(ini);
		}

		public bool DefaultIsEmpty
		{
			get { return _default.IsEmpty(); }
		}

		public bool UserIsEmpty
		{
			get { return _user.IsEmpty(); }
		}

		public SettingValue Get(string section, string key)
		{
			// TODO reloading is going to make every Get relatively slow
			ReloadSettings();
			return GetSetting(_merged, section, key);
		}

		public SettingValue Get(string key)
		{
			return Get(null, key);
		}

		public SettingValue GetDefault(string section, string key)
		{
			return GetSetting(_default, section, key);
		}

		public SettingValue GetDefault(string key)
		{
			return GetDefault(null, key);
		}

		public void Set(string section, string key, string value)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				Logger.Instance.Log("Settings must have non-empty keys");
				return;
			}

			if (string.IsNullOrWhiteSpace(section))
			{
				_user.Global[key] = value;
			}
			else if (_user.HasSection(section))
			{
				_user[section][key] = value;
			}
			else
			{
				_user.Sections.AddSection(section);
				_user[section][key] = value;
			}

			FileParser.WriteFile(_userFile, _user);
		}

		public void Set(string key, string value)
		{
			Set(null, key, value);
		}

		public void Set(string section, string key, object value)
		{
			Set(section, key, value.ToString());
		}

		public void Set(string key, object value)
		{
			Set(null, key, value.ToString());
		}

		public void RestoreDefaults()
		{
			Logger.Instance.Log("Restoring default settings.");
			_user = new IniData();
			// TODO possibly backup before delete
			if (File.Exists(_userFile))
			{
				try
				{
					Lock.EnterWriteLock();
					File.Delete(_userFile);
				}
				catch (Exception e)
				{
					Logger.Instance.Log(e.Message);
				}
				finally
				{
					Lock.ExitWriteLock();
				}
			}
		}

		private void Initialize()
		{
			_default = new IniData();
			_user = new IniData();
			_merged = new IniData();
		}

		private void InitializeDefault(string ini)
		{
			_default = StringParser.Parse(ini);
			_merged.Merge(_default);
		}

		private void LoadUserSettings()
		{
			if (File.Exists(_userFile))
			{
				Logger.Instance.Log($"Loading user settings from {_userFile}");
				try
				{
					Lock.EnterWriteLock();
					_user = FileParser.ReadFile(_userFile);
				}
				catch (Exception e)
				{
					Logger.Instance.Log(e.Message);
				}
				finally
				{
					Lock.ExitWriteLock();
				}
			}
		}

		private SettingValue GetSetting(IniData data, string section, string key)
		{
			if (string.IsNullOrWhiteSpace(key))
				return SettingValue.Empty;

			if (string.IsNullOrWhiteSpace(section))
				return new SettingValue(data.Global[key]);
			else if (data.HasSection(section))
				return new SettingValue(data[section][key]);
			else
				return SettingValue.Empty;
		}

		// Reset _merged to latest settings from file
		private void ReloadSettings()
		{
			_merged.Merge(_default);
			LoadUserSettings();
			_merged.Merge(_user);
		}

		private string GetDefaultDir()
		{
			var appData = Environment.GetFolderPath(
					Environment.SpecialFolder.ApplicationData);
			var dir = Path.Combine(appData, DefaultDir);
			if (Directory.Exists(dir))
				return dir;
			else
				return appData;
		}

		private void SetUserFile(string name = null, string dir = null)
		{
			var fname = name;
			var fdir = dir;
			if (string.IsNullOrWhiteSpace(dir))
				fdir = GetDefaultDir();
			if (string.IsNullOrWhiteSpace(name))
				fname = DefaultName;
			_userFile = Path.Combine(fdir, fname + ".ini");
			Logger.Instance.Log("User setting file is " + _userFile);
		}
	}
}