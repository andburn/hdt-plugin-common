using System.Drawing;
using System.Threading.Tasks;
using HDT.Plugins.Common.Services;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Utility;

namespace HDT.Plugins.Common.Providers
{
	public class TrackerClientService : IGameClientService
	{
		public Rectangle GameRectangle(bool dpiScale)
		{
			return Helper.GetHearthstoneRect(true);
		}

		public async Task<Bitmap> GameScreenshot(bool alt = true)
		{
			var rect = GameRectangle(true);
			return await ScreenCapture.CaptureHearthstoneAsync(
				new Point(0, 0), rect.Width, rect.Height, altScreenCapture: alt);
		}

		public bool IsInMenu()
		{
			return Core.Game?.IsInMenu ?? false;
		}
	}
}