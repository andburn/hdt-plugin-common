using HDT.Plugins.Common.Services;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Utility;
using System.Drawing;
using System.Threading.Tasks;

namespace HDT.Plugins.Common.Providers.Tracker
{
    public class TrackerClientService : IGameClientService
    {
        public string[] CurrentGameInfo()
        {
            var stats = Hearthstone_Deck_Tracker.Core.Game?.CurrentGameStats;
            if (stats != null)
                return new string[] { stats.PlayerHero, stats.OpponentHero, stats.PlayerName, stats.OpponentName };
            return new string[0];
        }

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

        public System.Windows.Window MainWindow()
        {
            return Core.MainWindow;
        }
    }
}