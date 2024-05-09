using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleContracts;

public interface IDatabasePlayerInfo
{
    /// <summary>
    /// Adds a user to the database
    /// </summary>
    /// <returns></returns>
    Task<bool> AddPlayerAsync(string username, string password);

    /// <summary>
    /// Adds a user to the database
    /// </summary>
    /// <returns></returns>
    Task<string> GetUsernameAsync(int userId);

    /// <summary>
    /// Removes a user from the database
    /// </summary>
    /// <returns></returns>
    Task<bool> RemovePlayerAsync(string username, string password);

    /// <summary>
    /// Returns userid if the password is authenticated
    /// </summary>
    /// <returns></returns>
    Task<int> AuthenticateAsync(string username, string password);

    /// <summary>
    /// Gets all gameids, scores, and whether or not they were the winner
    /// </summary>
    /// <returns></returns>
    Task<DataTable> GetGamesAsync(string username);

    /// <summary>
    /// gets all words played from the game for the specific user, as well as their point values
    /// </summary>
    /// <returns></returns>
    Task<DataTable> GetWordsPlayedAsync(string gameCode, string username);
}