using HDT.Plugins.Common.Services;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Utility;
using System.Drawing;
using System.Threading.Tasks;
using API = Hearthstone_Deck_Tracker.API;

namespace HDT.Plugins.Common.Providers.Tracker
{
	public class TrackerClientService : IGameClientService
	{
		public string[] CurrentGameInfo()
		{
			string[] info;
			var stats = API.Core.Game?.CurrentGameStats;
			if (stats != null)
			{
				info = new string[] {
					stats.PlayerHero,
					stats.OpponentHero,
					stats.PlayerName,
					stats.OpponentName
				};
				Common.Log.Debug($"Tracker: CurrentGameInfo for {stats.GameId}");
			}
			else
			{
				Common.Log.Debug("Tracker: CurrentGameInfo is null");
				info = new string[0];
			}
			return info;
		}

		public Rectangle GameRectangle(bool dpiScale = true)
		{
			var rect = Helper.GetHearthstoneRect(dpiScale);
			Common.Log.Debug($"Tracker: Rectangle ({rect.X}, {rect.Y}, {rect.Width}, {rect.Height})");
			return rect;
		}

		public async Task<Bitmap> GameScreenshot(bool alt = true)
		{
			var rect = GameRectangle(true);
			Common.Log.Debug($"Tracker: Capturing screenshot");
			return await ScreenCapture.CaptureHearthstoneAsync(
				new Point(0, 0), rect.Width, rect.Height, altScreenCapture: alt);
		}

		public bool IsInMenu()
		{
			return API.Core.Game?.IsInMenu ?? false;
		}

		public System.Windows.Window MainWindow()
		{
			return API.Core.MainWindow;
		}
	}
}