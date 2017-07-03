using HDT.Plugins.Common.Utils;
using IniParser;
using IniParser.Model;
using IniParser.Parser;
using System;
using System.IO;
using System.Threading;

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
				Common.Log.Error(e.Message);
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
				Common.Log.Error(e.Message);
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
				Common.Log.Debug("Settings: keys must be non-empty");
				return;
			}

			if (string.IsNullOrWhiteSpace(section))
			{
				_user.Global[key] = value;
				Common.Log.Debug($"Settings: Set Global[{key}]={value}");
			}
			else if (_user.HasSection(section))
			{
				_user[section][key] = value;
				Common.Log.Debug($"Settings: Set {section}[{key}]={value}");
			}
			else
			{
				_user.Sections.AddSection(section);
				Common.Log.Debug($"Settings: Adding section {section}");
				_user[section][key] = value;
				Common.Log.Debug($"Settings: Set {section}[{key}]={value}");
			}

			try
			{
				Lock.EnterWriteLock();
				FileParser.WriteFile(_userFile, _user);
			}
			catch (Exception e)
			{
				Common.Log.Error(e.Message);
			}
			finally
			{
				Lock.ExitWriteLock();
			}
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
			Common.Log.Debug("Settings: Restoring defaults");
			_user = new IniData();
			if (File.Exists(_userFile))
			{
				try
				{
					Lock.EnterWriteLock();
					File.Delete(_userFile);
				}
				catch (Exception e)
				{
					Common.Log.Error(e.Message);
				}
				finally
				{
					Lock.ExitWriteLock();
				}
			}
			else
				Common.Log.Debug($"Settings: User file ({_userFile}) not found");
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
				Common.Log.Debug($"Settings: Load user file from '{_userFile}'");
				try
				{
					Lock.EnterWriteLock();
					_user = FileParser.ReadFile(_userFile);
				}
				catch (Exception e)
				{
					Common.Log.Error(e.Message);
				}
				finally
				{
					Lock.ExitWriteLock();
				}
			}
			else
				Common.Log.Debug($"Settings: user file not found '{_userFile}'");
		}

		private SettingValue GetSetting(IniData data, string section, string key)
		{
			Common.Log.Debug($"Settings: Get {section}[{key}]");

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
			{
				Common.Log.Debug($"Settings: Default dir '{dir}'");
				return dir;
			}
			else
			{
				Common.Log.Debug($"Settings: Default dir '{appData}'");
				return appData;
			}
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
			Common.Log.Debug($"Settings: user file set to '{_userFile}'");
		}
	}
}