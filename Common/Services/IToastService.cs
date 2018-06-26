using System.Windows.Controls;

namespace HDT.Plugins.Common.Services
{
    public interface IToastService
    {
        void Show(UserControl toast);
        void Hide(UserControl toast);
    }
}
