using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BoggleContracts
{
    public interface IBoggleServer
    {
        /// <summary>
        /// starts the game server
        /// </summary>
        Tuple<IPAddress, int> StartServer();

        /// <summary>
        /// Returns the gameboard as a char array
        /// </summary>
        void StartGame();

        /// <summary>
        /// Ends the given game
        /// </summary>
        public void EndGame(object state);

        /// <summary>
        /// Sends a message to all players
        /// </summary>
        Task sendMessageToPlayersAsync(string message);
    }
}
