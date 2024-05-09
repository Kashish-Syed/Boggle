using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleContracts
{
    public interface IDatabaseGameInfo
    {
        /// <summary>
        /// Creates a game and returns the gamecode
        /// </summary>
        Task<string> CreateGameAsync();

        /// <summary>
        /// Deletes a game by its gamecode
        /// </summary>
        Task<int> DeleteGameAsync(string gameCode);

        /// <summary>
        /// Returns the gameboard as a char array
        /// </summary>
        Task<char[]> GetBoardAsync(string gameCode);

        /// <summary>
        /// Adds a player to the game
        /// </summary>
        Task AddPlayerAsync(string gameCode, string username);

        /// <summary>
        /// Returns the player with the most points
        /// </summary>
        Task<string> GetWinnerAsync(string gameCode);
    }
}
