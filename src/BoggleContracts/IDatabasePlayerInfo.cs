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
    public void AddPlayer(string username, string password);

    /// <summary>
    /// Removes a user from the database
    /// </summary>
    /// <returns></returns>
    public void RemovePlayer(string username, string password);

    /// <summary>
    /// Returns userid if the password is authenticated
    /// </summary>
    /// <returns></returns>
    public int Authenticate(string username, string password);

    /// <summary>
    /// Gets all gameids, scores, and whether or not they were the winner
    /// </summary>
    /// <returns></returns>
    public DataTable GetGames(string username);

    /// <summary>
    /// gets all words played from the game for the specific user, as well as their point values
    /// </summary>
    /// <returns></returns>
    public DataTable GetWordsPlayed(string gameCode, string username);
}