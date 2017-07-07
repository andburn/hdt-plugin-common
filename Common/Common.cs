using HDT.Plugins.Common.Providers.Tracker;
using HDT.Plugins.Common.Services;
using System;

namespace HDT.Plugins.Common
{
	public static class Common
	{
		static Common()
		{
			// invalidate data caches on game and deck events
			Hearthstone_Deck_Tracker.API.GameEvents.OnGameStart
				.Add(() => TrackerDataRepository.InvalidateDataCache());
			Hearthstone_Deck_Tracker.API.GameEvents.OnGameEnd
				.Add(() => TrackerDataRepository.InvalidateDataCache());
			Hearthstone_Deck_Tracker.API.DeckManagerEvents.OnDeckCreated
				.Add(x => TrackerDataRepository.InvalidateDataCache());
			Hearthstone_Deck_Tracker.API.DeckManagerEvents.OnDeckDeleted
				.Add(x => TrackerDataRepository.InvalidateDataCache());
			Hearthstone_Deck_Tracker.API.DeckManagerEvents.OnDeckUpdated
				.Add(x => TrackerDataRepository.InvalidateDataCache());
		}

		private static ILoggingService _log;

		public static ILoggingService Log
		{
			get
			{
				if (_log == null)
					_log = new TrackerLoggingService();
				return _log;
			}
			set { _log = value; }
		}
	}
}