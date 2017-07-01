using HDT.Plugins.Common.Enums;
using System.Windows;
using System.Windows.Controls;

namespace HDT.Plugins.Common.Services
{
	public interface ISlidePanel
	{
		ContentControl Content { get; set; }

		string Name { get; set; }

		string Header { get; set; }

		double Width { get; set; }

		double Height { get; set; }

		bool IsPinned { get; set; }

		Visibility TitleVisibility { get; set; }

		Visibility CloseButtonVisibility { get; set; }

		Position Position { get; set; }

		bool UseAltTheme { get; set; }

		void Attach();

		void Detach();

		void Open();

		void Close();

		void AutoClose(int seconds);

		void SetZIndex(int index);
	}
}