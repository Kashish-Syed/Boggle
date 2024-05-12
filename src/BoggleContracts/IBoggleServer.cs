// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: IBoggleServer.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Interface for the BoggleServer.cs class.
// ----------------------------------------------------------------------------------------------------

using System.Net;

namespace BoggleContracts
{
    public interface IBoggleServer
    {
        /// <summary>
        /// Starts the TCP game listener (server).
        /// </summary>
        /// <param name="gameCode"></param>
        /// <returns>A tuple that contains the Ip Address and port number on which server was started.</returns>
        Tuple<IPAddress, int> StartServer(string gameCode);

        /// <summary>
        /// Starts the game and its timer.
        /// </summary>
        void StartGame();

        /// <summary>
        /// Ends the game by closing the server.
        /// </summary>
        /// <param name="state"></param>
        public void EndGame(object state);

        /// <summary>
        /// Sends message to all players
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task sendMessageToPlayersAsync(string message);
    }
}
