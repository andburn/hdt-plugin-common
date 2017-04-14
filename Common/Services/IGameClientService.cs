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
	}
}