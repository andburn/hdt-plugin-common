using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HDT.Plugins.Common.Util;
using MahApps.Metro.Controls;

namespace HDT.Plugins.Common.Controls.SlidePanels
{
	public class SlidePanel
	{
		private Flyout _flyout;

		public ContentControl Content
		{
			set
			{
				_flyout.Content = value;
				_flyout.UpdateLayout();
			}
		}

		public string Name
		{
			get { return _flyout.Name; }
			set { _flyout.Name = value; }
		}

		public string Header
		{
			get { return _flyout.Header; }
			set { _flyout.Header = value; }
		}

		public double Width
		{
			get { return _flyout.Width; }
			set { _flyout.Width = value; }
		}

		public double Height
		{
			get { return _flyout.Height; }
			set { _flyout.Height = value; }
		}

		public bool IsPinned
		{
			get { return _flyout.IsPinned; }
			set { _flyout.IsPinned = value; }
		}

		public Visibility TitleVisibility
		{
			get { return _flyout.TitleVisibility; }
			set { _flyout.TitleVisibility = value; }
		}

		public Visibility CloseButtonVisibility
		{
			get { return _flyout.CloseButtonVisibility; }
			set { _flyout.CloseButtonVisibility = value; }
		}

		public Position Position
		{
			get { return ConvertPosition(_flyout.Position); }
			set { _flyout.Position = ConvertPosition(value); }
		}

		public bool UseAltTheme
		{
			get
			{
				return _flyout.Theme == FlyoutTheme.Accent;
			}
			set
			{
				if (value)
					_flyout.Theme = FlyoutTheme.Accent;
				else
					_flyout.UpdateDefaultStyle();
			}
		}

		public SlidePanel()
		{
			_flyout = new Flyout();
		}

		public SlidePanel(ContentControl content)
			: base()
		{
			Content = content;
		}

		public void Attach()
		{
			Hearthstone_Deck_Tracker.API.Core.MainWindow.Flyouts.Items.Add(_flyout);
		}

		public void Detach()
		{
			Hearthstone_Deck_Tracker.API.Core.MainWindow.Flyouts.Items.Remove(_flyout);
		}

		public void Open()
		{
			_flyout.IsOpen = true;
		}

		public void Close()
		{
			_flyout.IsOpen = false;
		}

		private async Task AutoCloseAsync(int seconds = 0)
		{
			Open();
			if (seconds > 0)
			{
				await Task.Delay(seconds * 1000);
				Close();
			}
		}

		public void AutoClose(int seconds = 0)
		{
			AutoCloseAsync(seconds).Forget();
		}

		public void SetZIndex(int idx)
		{
			Panel.SetZIndex(_flyout, idx);
		}

		private Position ConvertPosition(MahApps.Metro.Controls.Position position)
		{
			switch (position)
			{
				case MahApps.Metro.Controls.Position.Left:
					return Position.LEFT;

				case MahApps.Metro.Controls.Position.Right:
					return Position.RIGHT;

				case MahApps.Metro.Controls.Position.Top:
					return Position.TOP;

				case MahApps.Metro.Controls.Position.Bottom:
				default:
					return Position.BOTTOM;
			}
		}

		private MahApps.Metro.Controls.Position ConvertPosition(Position position)
		{
			switch (position)
			{
				case Position.TOP:
					return MahApps.Metro.Controls.Position.Top;

				case Position.LEFT:
					return MahApps.Metro.Controls.Position.Left;

				case Position.RIGHT:
					return MahApps.Metro.Controls.Position.Right;

				case Position.BOTTOM:
				default:
					return MahApps.Metro.Controls.Position.Bottom;
			}
		}
	}
}