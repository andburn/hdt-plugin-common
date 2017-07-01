using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Services;
using HDT.Plugins.Common.Utils;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HDT.Plugins.Common.Controls
{
	public class SlidePanelManager
	{
		private static List<ISlidePanel> _panels;

		static SlidePanelManager()
		{
			_panels = new List<ISlidePanel>();
		}

		public static ISlidePanel Add(ISlidePanel panel)
		{
			Common.Log.Debug($"SlidePanelManager: Adding {panel.Name}");
			_panels.Add(panel);
			return panel;
		}

		public static void CloseAll()
		{
			Common.Log.Debug($"SlidePanelManager: Closing {_panels.Count}");
			foreach (var item in _panels)
				item.Close();
		}

		public static void DetachAll()
		{
			Common.Log.Debug($"SlidePanelManager: Detaching {_panels.Count}");
			foreach (var item in _panels)
			{
				item.Close();
				item.Detach();
			}
		}

		public static void RemoveAll()
		{
			Common.Log.Debug($"SlidePanelManager: Removing {_panels.Count}");
			DetachAll();
			_panels.Clear();
		}

		public static ISlidePanel Notification(ISlidePanel panel, string title, string message,
			string icon = null, Action action = null)
		{
			panel.Position = Position.BOTTOM;
			panel.TitleVisibility = Visibility.Collapsed;
			panel.CloseButtonVisibility = Visibility.Collapsed;
			panel.IsPinned = true;
			panel.Height = 50;
			panel.SetZIndex(500);
			panel.UseAltTheme = true;

			var content = new NotificationDialog(title, message);
			content.SetUtilityButton(action, icon ?? IcoMoon.Notification);
			content.SetCloseButton(() => panel.Close(), IcoMoon.CancelCircle);

			panel.Content = content;
			panel.Attach();
			_panels.Add(panel);

			Common.Log.Debug($"SlidePanelManager: Created Notificaton ({title}, {message})");

			return panel;
		}
	}
}