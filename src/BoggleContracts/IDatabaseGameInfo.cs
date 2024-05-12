// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: IDatabaseGameInfo.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Interface for the DatabaseGameInfo.cs class.
// ----------------------------------------------------------------------------------------------------

namespace BoggleContracts
{
    public interface IDatabaseGameInfo
    {
        /// <summary>
        /// Creates a game and returns the gamecode.
        /// </summary>
        /// <returns></returns>
        Task<string> CreateGameAsync();

        /// <summary>
        /// Deletes a game by its gamecode.
        /// </summary>
        /// <param name="gameCode"></param>
        /// <returns></returns>
        Task<int> DeleteGameAsync(string gameCode);

        /// <summary>
        /// Returns the gameboard as a char array.
        /// </summary>
        /// <param name="gameCode"></param>
        /// <returns></returns>
        Task<char[]?> GetBoardAsync(string gameCode);

        /// <summary>
        /// Adds a player to the game.
        /// </summary>
        /// <param name="gameCode"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task AddPlayerAsync(string gameCode, string username);

        /// <summary>
        /// Returns the player with the most points.
        /// </summary>
        /// <param name="gameCode"></param>
        /// <returns></returns>
        Task<string?> GetWinnerAsync(string gameCode);
    }
}
