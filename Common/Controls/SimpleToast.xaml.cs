using System.Windows.Controls;

namespace HDT.Plugins.Common.Controls
{
    public partial class SimpleToast : UserControl
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public SimpleToast()
        {
            InitializeComponent();
        }

        public SimpleToast(string title, string message, string icon)
        {
            Icon = icon;
            Title = title;
            Message = message;

            InitializeComponent();
        }
    }
}
