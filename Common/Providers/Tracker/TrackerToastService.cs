using HDT.Plugins.Common.Services;
using Hearthstone_Deck_Tracker.Utility.Toasts;
using System.Windows.Controls;

namespace HDT.Plugins.Common.Providers.Tracker
{
    public class TrackerToastService : IToastService
    {
        public void Show(UserControl toast)
        {
            ToastManager.ShowCustomToast(toast);
        }

        public void Hide(UserControl toast)
        {
            ToastManager.ForceCloseToast(toast);
        }        
    }
}
