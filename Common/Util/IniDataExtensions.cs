using IniParser.Model;

namespace HDT.Plugins.Common.Util
{
	public static class IniDataExtensions
	{
		public static bool IsEmpty(this IniData data)
		{
			return data.Sections.Count <= 0 && data.Global.Count <= 0;
		}

		public static bool HasSection(this IniData data, string section)
		{
			return data.Sections.ContainsSection(section);
		}
	}
}