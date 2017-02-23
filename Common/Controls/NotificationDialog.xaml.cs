using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using HDT.Plugins.Common.Util;

namespace HDT.Plugins.Common.Controls
{
	public partial class NotificationDialog : UserControl
	{
		private Regex regex = new Regex(@"(?<pre>[^\[]*)\[(?<text>[^\]\(]+)\]\((?<url>[^\)]+)\)\s*(?<post>.*)", RegexOptions.Compiled);

		public NotificationDialog(string title, string message)
		{
			InitializeComponent();

			TitleText.Text = title;

			var match = regex.Match(message);
			if (match.Success)
			{
				MessageText.Inlines.Clear();
				MessageText.Inlines.Add(match.Groups["pre"].Value);
				Hyperlink hyperLink = new Hyperlink() {
					Foreground = System.Windows.Media.Brushes.White,
					NavigateUri = new Uri(match.Groups["url"].Value)
				};
				hyperLink.Inlines.Add(match.Groups["text"].Value);
				hyperLink.RequestNavigate += HyperLink_RequestNavigate;
				MessageText.Inlines.Add(hyperLink);
				MessageText.Inlines.Add(" " + match.Groups["post"].Value);
			}
			else
			{
				MessageText.Text = message;
			}
		}

		private void SetButton(Button button, Action action, string icon)
		{
			button.Content = icon;
			button.IsEnabled = true;
			if (action != null)
				button.Click += (s, e) => { action.Invoke(); };
			else
				button.IsEnabled = false;
			button.UpdateLayout();
		}

		public void SetUtilityButton(Action action, string icon)
		{
			SetButton(UtilityButton, action, icon);
		}

		public void SetCloseButton(Action action, string icon)
		{
			SetButton(CloseButton, action, icon);
		}

		private void HyperLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(e.Uri.ToString());
		}
	}
}