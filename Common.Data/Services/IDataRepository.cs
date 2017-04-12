using System.Collections.Generic;
using HDT.Plugins.Common.Data.Models;

namespace HDT.Plugins.Common.Data.Services
{
	public interface IDataRepository
	{
		List<Game> GetAllGames();

		void AddGames(List<Game> games);

		void UpdateGames(List<Game> games);

		List<Deck> GetAllDecks();

		List<Deck> GetAllDecksWithTag(string tag);

		void DeleteAllDecksWithTag(string tag);

		void AddDeck(Deck deck);

		void AddDeck(string name, string playerClass, string cards, bool archive, params string[] tags);

		Deck GetOpponentDeck();

		string GetGameNote();

		string GetGameMode();

		void UpdateGameNote(string text);
	}
}