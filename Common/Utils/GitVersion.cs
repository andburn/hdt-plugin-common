using System;
using System.Reflection;

namespace HDT.Plugins.Common.Utils
{
	public class GitVersion
	{
		public static Version Get(Assembly assembly, object obj)
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