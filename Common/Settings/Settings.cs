using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HDT.Plugins.Common.Plugin;
using IniParser;
using IniParser.Model;
using IniParser.Parser;

namespace HDT.Plugins.Common.Settings
{	
	public class Settings
	{
		private static readonly string UserFilename = "settings.ini";
		private static readonly IniDataParser StringParser = new IniDataParser();
		private static readonly FileIniDataParser FileParser = new FileIniDataParser();

		private string _userFile;
		private IniData _default; // read only, created on construction
		private IniData _user; // read write, created on set
		private IniData _merged;

		public Settings()
		{
			_userFile = Path.Combine(PluginDirectory.Path, UserFilename);
			_default = new IniData();
			_user = new IniData();
			_merged = new IniData();
		}

		public Settings(string defaultIni) 
			: this()
		{
			_default = LoadFromString(defaultIni);
			_merged.Merge(_default);
		}

		public Settings(FileInfo defaultIniFile) 
			: this()
		{
			_default = LoadFromFile(defaultIniFile.FullName);
			_merged.Merge(_default);
		}

		public SettingValue Get(string key)
		{
			LoadUserSettings();
			_merged.Merge(_user);
			return new SettingValue(_merged.Global[key]);
		}

		public string GetDefault(string key)
		{
			return _default.Global[key];
		}

		public void Set(string key, string value)
		{
			_user.Global[key] = value;
			FileParser.WriteFile(_userFile, _user);
		}

		private void LoadUserSettings()
		{			
			if (File.Exists(_userFile))
				_user = FileParser.ReadFile(_userFile);
		}

		private IniData LoadFromString(string str)
		{
			return StringParser.Parse(str);
		}

		private IniData LoadFromFile(string file)
		{
			try
			{
				return LoadFromString(File.ReadAllText(file));
			}
			catch (Exception e)
			{
				throw e;
			}
		}		
	}
}
