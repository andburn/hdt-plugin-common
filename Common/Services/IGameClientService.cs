using System.Drawing;
using System.Threading.Tasks;

namespace HDT.Plugins.Common.Services
{
	public interface IGameClientService
	{
		Rectangle GameRectangle(bool dpiScale);

		Task<Bitmap> GameScreenshot(bool altMethod);

		bool IsInMenu();
	}
}