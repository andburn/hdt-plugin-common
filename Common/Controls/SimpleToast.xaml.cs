using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HDT.Plugins.Common.Controls
{
    public partial class SimpleToast : UserControl
    {
        public SimpleToast()
        {
            InitializeComponent();
        }

        public SimpleToast(string title, string message, string icon)
            : base()
        {
        }
    }
}
