// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: IBoggleClient.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Interface for BoggleClient.cs class.
// ----------------------------------------------------------------------------------------------------

using System.Net;

namespace BoggleContracts
{
    public interface IBoggleClient
    {
        /// <summary>
        /// Connects the client to the server when the client provides the Ip Adress and the
        /// port of the server they want to connect to.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        Task connectPlayersAsync(IPAddress ipAddress, int port);


        /// <summary>
        /// Function to enable the client to receive messages from the server. 
        /// </summary>
        /// <returns></returns>
        Task receiveMessagesAsync();
    }
}
