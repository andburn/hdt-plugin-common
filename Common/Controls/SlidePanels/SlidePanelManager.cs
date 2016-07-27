using System;
using System.Collections.Generic;
using System.Windows;
using HDT.Plugins.Common.Util;

namespace HDT.Plugins.Common.Controls.SlidePanels
{
	public class SlidePanelManager
	{
		private static List<SlidePanel> _panels;

		static SlidePanelManager()
		{
			_panels = new List<SlidePanel>();
		}

		public static SlidePanel Add()
		{
			return null;
		}

		// TODO any use for this
		public static void CloseAll()
		{
			foreach (var item in _panels)
				item.Close();
		}

		public static void DetachAll()
		{
			foreach (var item in _panels)
			{
				item.Close();
				item.Detach();
			}
		}

		public static SlidePanel Notification(string title, string message,
			string icon = "notification", Action action = null)
		{
			var panel = new SlidePanel();

			panel.Position = Position.BOTTOM;
			panel.TitleVisibility = Visibility.Collapsed;
			panel.CloseButtonVisibility = Visibility.Collapsed;
			panel.IsPinned = true;
			panel.Height = 50;
			panel.SetZIndex(500);
			panel.UseAltTheme = true;

			var content = new NotificationDialog(title, message);
			content.SetUtilityButton(action, icon);
			content.SetCloseButton(() => panel.Close(), "cancel-circle");

			panel.Content = content;
			panel.Attach();
			_panels.Add(panel);

			return panel;
		}
	}
}