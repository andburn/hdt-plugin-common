using HDT.Plugins.Common.Services;
using HDT.Plugins.Common.Utils;
using MahApps.Metro.Controls;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static HDT.Plugins.Common.Providers.Utils.EnumConverter;
using Position = HDT.Plugins.Common.Enums.Position;

namespace HDT.Plugins.Common.Providers.Metro
{
	public class MetroSlidePanel : ISlidePanel
	{
		private Flyout _flyout;

		public ContentControl Content
		{
			get
			{
				return _flyout.Content as ContentControl;
			}
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
			get { return Convert(_flyout.Position); }
			set { _flyout.Position = Convert(value); }
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

		public MetroSlidePanel()
		{
			_flyout = new Flyout();
		}

		public MetroSlidePanel(ContentControl content)
			: base()
		{
			Content = content;
		}

		public void Attach()
		{
			try
			{
				Hearthstone_Deck_Tracker.API.Core.MainWindow.Flyouts.Items.Add(_flyout);
			}
			catch (Exception e)
			{
				Common.Log.Error(e);
			}
		}

		public void Detach()
		{
			try
			{
				Hearthstone_Deck_Tracker.API.Core.MainWindow.Flyouts.Items.Remove(_flyout);
			}
			catch (Exception e)
			{
				Common.Log.Error(e);
			}
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
				Common.Log.Debug($"MetroSlidePanel: Auto closed after {seconds}s");
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
	}
}