using System.Windows.Controls;

namespace HDT.Plugins.Common.Utils
{
	public interface IPluggable
	{
		void Load();
		void Unload();
		void Repeat();
		void ButtonPress();
		MenuItem CreateMenu();
	}
}
