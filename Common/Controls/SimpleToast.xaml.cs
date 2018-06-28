using HDT.Plugins.Common.Utils;
using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace HDT.Plugins.Common.Controls
{
    public partial class SimpleToast : UserControl
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        private Uri _link;

        public SimpleToast()
        {
            InitializeComponent();
        }

        public SimpleToast(string title, string message, string icon, string url)
        {
            Icon = icon;
            Title = title;
            Message = message;
            if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    _link = new Uri(url);
                }
                catch (UriFormatException)
                {
                    Common.Log.Error($"SimpleToast: malformed URL {url}");
                }
            }

            InitializeComponent();
            DataContext = this;
        }

        private void Toast_OnClicked(object sender, EventArgs e)
        {
            NotificationManager.Close(this);
            if (_link != null)
                Process.Start(_link.ToString());
        }
    }
}
