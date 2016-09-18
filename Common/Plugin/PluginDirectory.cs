using System.Reflection;

namespace HDT.Plugins.Common.Plugin
{
	public static class PluginDirectory
	{
		public static string Path
		{
			get
			{
				return System.IO.Path.GetDirectoryName(
					Assembly.GetExecutingAssembly().Locati‌​on);
			}
		}
	}
}