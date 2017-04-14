using System;
using System.Collections.Generic;
using System.Windows;
using HDT.Plugins.Common.Data.Enums;
using HDT.Plugins.Common.Data.Services;
using HDT.Plugins.Common.Utils;

namespace HDT.Plugins.Common.Controls
{
	public class SlidePanelManager
	{
		private static List<ISlidePanel> _panels;

		static SlidePanelManager()
		{
			_panels = new List<ISlidePanel>();
		}

		public static ISlidePanel Add()
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

		// TODO passing the panel in seems a bit weird, look at ninject solutions
		public static ISlidePanel Notification(ISlidePanel panel, string title, string message,
			string icon = IcoMoon.Notification, Action action = null)
		{
			panel.Position = Position.BOTTOM;
			panel.TitleVisibility = Visibility.Collapsed;
			panel.CloseButtonVisibility = Visibility.Collapsed;
			panel.IsPinned = true;
			panel.Height = 50;
			panel.SetZIndex(500);
			panel.UseAltTheme = true;

			var content = new NotificationDialog(title, message);
			content.SetUtilityButton(action, icon);
			content.SetCloseButton(() => panel.Close(), IcoMoon.CancelCircle);

			panel.Content = content;
			panel.Attach();
			_panels.Add(panel);

			return panel;
		}
	}
}