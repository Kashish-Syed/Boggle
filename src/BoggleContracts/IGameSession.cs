using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleContracts;

public interface IGameSession
{
    /// <summary>
    /// A function to get the user's score.
    /// </summary>
    /// <returns></returns>
    public int GetScore();

    /// <summary>
    /// Get the winner of the game based on the highest score
    /// </summary>
    /// <returns>Winning player</returns>
    public int GetWinner();

    /// <summary>
    /// Start a new game session
    /// </summary>
    /// <returns>1 if successfully started the session and 0 otherwise</returns>

    public int StartGameSession();
}