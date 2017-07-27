using HDT.Plugins.Common.Models;
using System;
using System.Collections.Generic;

namespace HDT.Plugins.Common.Services
{
    public interface IDataRepository
    {
        List<Game> GetAllGames();

        List<Game> GetAllGamesWithDeck(Guid id);

        int AddGames(List<Game> games);

        List<Deck> GetAllDecks();

        List<Deck> GetAllDecksWithTag(string tag);

        void DeleteAllDecksWithTag(string tag);

        void AddDeck(Deck deck);

        void AddDeck(string name, string playerClass, string cards, bool archive, params string[] tags);

        Deck GetOpponentDeck();

        string GetGameNote();

        string GetGameMode();

		int GetPlayerRank();

        void UpdateGameNote(string text);

        void InvalidateCache();

        Guid GetActiveDeckId();
    }
}