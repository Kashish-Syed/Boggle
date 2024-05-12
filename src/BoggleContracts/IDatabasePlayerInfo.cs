// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: IDatabasePlayerInfo.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Inetrface for the DatabasePlayerInfo.cs class.
// ----------------------------------------------------------------------------------------------------

using System.Data;

namespace BoggleContracts;

public interface IDatabasePlayerInfo
{
    /// <summary>
    /// Adds the user to the database
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns>True if user was addedd sucessfully</returns>
    Task<bool> AddPlayerAsync(string username, string password);

    /// <summary>
    /// Gets the user by their username.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<string?> GetUsernameAsync(int userId);

    /// <summary>
    /// Removes the user from the database.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<bool> RemovePlayerAsync(string username, string password);

    /// <summary>
    /// Returns UserId if the password is authenticated.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<int> AuthenticateAsync(string username, string password);

    /// <summary>
    /// Gets all gameids, scores, and whether or not they were the winner.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<DataTable> GetGamesAsync(string username);

    /// <summary>
    /// Gets all words played from the game for the specific user, as well as their point values.
    /// </summary>
    /// <param name="gameCode"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<DataTable> GetWordsPlayedAsync(string gameCode, string username);

    /// <summary>
    /// Adds a record that the player played a word in the given game.
    /// </summary>
    /// <param name="gameCode"></param>
    /// <param name="username"></param>
    /// <param name="word"></param>
    /// <returns></returns>
    Task<bool> AddWordPlayedAsync(string gameCode, string username, string word);

    /// <summary>
    /// Gets the id of the username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<int> GetPlayerIdAsync(string username);
}