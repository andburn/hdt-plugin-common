using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Utility;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

		public void OpenDeckEditor(IEnumerable<Card> cards, params string[] tags)
		{
			if (cards == null || cards.Count() <= 0)
			{
				Common.Log.Debug("Tracker: OpenDeckEditor cards are empty");
				return;
			}

			var deck = new Hearthstone_Deck_Tracker.Hearthstone.Deck();
			string klass = null;

			foreach (var c in cards)
			{
				var card = Hearthstone_Deck_Tracker.Hearthstone.Database.GetCardFromId(c.Id);
				card.Count = c.Count;
				deck.Cards.Add(card);
				// get class from any class cards
				var cardKlass = card.PlayerClass;
				if (klass == null && cardKlass != null
						&& card.PlayerClass.ToLowerInvariant() != "neutral")
				{
					klass = card.PlayerClass;
					Common.Log.Debug($"Tracker: Class card {klass} found");
				}
			}

			if (!string.IsNullOrWhiteSpace(klass))
			{
				deck.Class = klass;
			}
			else
			{
				Common.Log.Info($"NewDeck: Class not found");
				return;
			}

			if (tags.Length > 0)
				deck.Tags.AddRange(tags);

			API.Core.MainWindow.ShowDeckEditorFlyout(deck, true);
			API.Core.MainWindow.Show();
		}
	}
}