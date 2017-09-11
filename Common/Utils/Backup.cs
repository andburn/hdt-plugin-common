using Hearthstone_Deck_Tracker;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace HDT.Plugins.Common.Utils
{
	public class Backup
	{
		public const int MAX = 3;

		private static readonly string[] files = { "PlayerDecks.xml", "DeckStats.xml", "DefaultDeckStats.xml", "config.xml", "HotKeys.xml" };

		public static void CreateBackup(string filename)
		{
			if (string.IsNullOrWhiteSpace(filename))
				filename = "PluginBackup";

			try
			{
				var fn = $"{filename}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.zip";
				var dirInfo = new DirectoryInfo(Config.Instance.BackupDir);

				var backups = dirInfo.GetFiles($"{filename}_*").OrderBy(x => x.CreationTime);
				while (backups.Count() >= MAX)
				{
					var oldest = backups.OrderBy(x => x.CreationTime).First();
					Common.Log.Info("Deleting old backup: " + oldest.Name);
					oldest.Delete();
					backups = dirInfo.GetFiles($"{filename}_*").OrderBy(x => x.CreationTime);
				}

				var backupFilePath = Path.Combine(Config.Instance.BackupDir, fn);
				using (var zip = ZipFile.Open(backupFilePath, ZipArchiveMode.Create))
				{
					Common.Log.Info("Creating backup: " + fn);
					foreach (var file in files)
					{
						var path = Path.Combine(Config.Instance.DataDir, file);
						if (File.Exists(path))
							zip.CreateEntryFromFile(path, file);
					}
				}
			}
			catch (Exception ex)
			{
				Common.Log.Error(ex);
			}
		}
	}
}