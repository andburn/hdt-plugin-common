using HDT.Plugins.Common.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;

namespace HDT.Plugins.Common.Services
{
    public interface IGameClientService
    {
        Rectangle GameRectangle(bool dpiScale);

        Task<Bitmap> GameScreenshot(bool altMethod);

        bool IsInMenu();

        Window MainWindow();

        string[] CurrentGameInfo();

		void OpenDeckEditor(IEnumerable<Card> cards, params string[] tags);
    }
}