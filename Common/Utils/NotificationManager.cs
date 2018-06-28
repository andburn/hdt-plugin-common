using HDT.Plugins.Common.Controls;
using HDT.Plugins.Common.Services;
using System.Collections.Generic;
using System.Windows.Controls;

namespace HDT.Plugins.Common.Utils
{
    public static class NotificationManager
    {
        private static IToastService _service;

        private static readonly List<UserControl> _notifications = new List<UserControl>();

        public static void ShowToast(string title, string message, string icon = null, string url = null)
        {
            Common.Log.Debug($"NotificationManager: Toast request '{title}' '{message}'");

            if (icon == null)
                icon = IcoMoon.Notification;

            UserControl control = new SimpleToast(title, message, icon, url);
            GetService()?.Show(control);
        }

        public static void Close(UserControl control)
        {
            Common.Log.Debug("NotificationManager: Closing notification");
            GetService()?.Hide(control);
        }

        public static void SetService(IToastService service) => _service = service;

        private static IToastService GetService()
        {
            if (_service == null)
            {
                Common.Log.Error("NotificationManager: ToastService is undefined");
                return null;
            }

            return _service;
        }
    }
}
