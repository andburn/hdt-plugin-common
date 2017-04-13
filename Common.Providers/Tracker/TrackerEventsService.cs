using System;
using HDT.Plugins.Common.Data.Services;
using Hearthstone_Deck_Tracker.API;

namespace HDT.Plugins.Common.Providers.Tracker
{
	public class TrackerEventsService : IEventsService
	{
		public void OnGameEnd(Action action) => GameEvents.OnGameEnd.Add(action);

		public void OnGameStart(Action action) => GameEvents.OnGameStart.Add(action);

		public void OnInMenu(Action action) => GameEvents.OnInMenu.Add(action);
	}
}