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
        string CreateGame();

        /// <summary>
        /// Deletes a game by its gamecode
        /// </summary>
        int DeleteGame(string gameCode);

        /// <summary>
        /// Returns the gameboard as a char array
        /// </summary>
        char[] GetBoard(string gameCode);

        /// <summary>
        /// Adds a player to the game
        /// </summary>
        void AddPlayer(string gameCode, string username);

        /// <summary>
        /// Returns the player with the most points
        /// </summary>
        string GetWinner(string gameCode);
    }
}
