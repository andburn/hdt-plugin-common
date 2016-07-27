using System.Collections.Generic;

namespace HDT.Plugins.Common.Util
{
	public static class IcoMoon
	{
		private const string DEFAULT_ICON = "\uea0e"; // blocked

		// TODO generate this from Reference.html
		private static readonly Dictionary<string, string> _table = new Dictionary<string, string>() {
			{"pie-chart", "\ue99a"},
			{"download3", "\ue9c7"},
			{"upload3", "\ue9c8"},
			{"cog", "\ue994"},
			{"info", "\uea0c" },
			{"notification", "\uea08" },
			{"cancel-circle", "\uea0d" }
		};

		public static string Get(string name)
		{
			if (!string.IsNullOrWhiteSpace(name) && _table.ContainsKey(name))
				return _table[name];
			else
				return DEFAULT_ICON;
		}
	}
}