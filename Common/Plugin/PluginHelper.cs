using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HDT.Plugins.Common.Plugin
{
	class PluginHelper
	{
		// TODO is this even useful now its not inherited, pass in an assembly too?
		public static string GetAssemblyPath()
		{
			// Based on StackOverflow - http://stackoverflow.com/a/283917/2762059
			// Get the path of the executing plugin assembly
			string path = null;
			try
			{
				// use CodeBase over Location, more reliable
				var codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				// remove uri file protocol
				var escaped = Uri.UnescapeDataString(uri.Path);
				path = Path.GetDirectoryName(escaped);
			}
			catch (Exception)
			{
				// TODO Log error
			}
			return path;
		}

		public static Version GetGitSemVer(Assembly assembly, object obj)
		{
			if (assembly == null)
				return null;

			var nspace = obj.GetType().Namespace;
			var gitInfo = assembly.GetType(nspace + ".GitVersionInformation");
			var semVer = gitInfo?.GetField("AssemblySemVer").GetValue(null).ToString();
			Version result = null;
			Version.TryParse(semVer, out result);

			return result;
		}
	}
}
