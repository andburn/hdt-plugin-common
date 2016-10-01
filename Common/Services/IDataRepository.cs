using System.Collections.Generic;
using HDT.Plugins.Common.Models;

namespace HDT.Plugins.Common.Services
{
	public interface IDataRepository
	{
		List<Deck> GetAllDecks();

		List<Game> GetAllGames();

		void AddGames(List<Game> games);

		void UpdateGames(List<Game> games);
	}
}