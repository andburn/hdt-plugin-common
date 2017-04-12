using System;

namespace HDT.Plugins.Common.Data.Services
{
	public interface IEventsService
	{
		void OnGameStart(Action action);

		void OnGameEnd(Action action);

		void OnInMenu(Action action);
	}
}