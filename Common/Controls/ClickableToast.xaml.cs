using HDT.Plugins.Common.Utils;
using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace HDT.Plugins.Common.Controls
{
    public partial class ClickableToast : UserControl
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        private Uri _link;

        public ClickableToast()
        {
            InitializeComponent();
        }

        public ClickableToast(string title, string message, string icon, string url)
            : base()
        {
            Icon = icon;
            Title = title;
            Message = message;
            _link = new Uri(url);

            InitializeComponent();
        }
        
        private void Toast_OnClicked(object sender, EventArgs e)
        {
            NotificationManager.CloseAll();
            if (_link != null)
                Process.Start(_link.ToString());
        }
    }
}
