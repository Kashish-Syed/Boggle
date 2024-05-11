using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BoggleContracts
{
    public interface IBoggleClient
    {
        /// <summary>
        /// connects to the server
        /// </summary>
        Task connectPlayersAsync(IPAddress ipAddress, int port);


        /// <summary>
        /// receives messages
        /// </summary>
        Task receiveMessagesAsync();
    }
}