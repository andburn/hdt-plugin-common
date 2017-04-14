using System;

namespace HDT.Plugins.Common.Services
{
	public interface IEventsService
	{
		void OnGameStart(Action action);

		void OnGameEnd(Action action);

		void OnInMenu(Action action);
	}
}