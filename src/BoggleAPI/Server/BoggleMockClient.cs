using BoggleContracts;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace BoggleAPI.Client
{
    public class BoggleClient : IBoggleClient
    {
        private ConcurrentBag<TcpClient> _players;

        public async Task connectPlayersAsync(IPAddress ipAddress, int port)
        {
            int numPlayers = 2;
            var ipEndPoint = new IPEndPoint(ipAddress, port);
            _players = new ConcurrentBag<TcpClient>();

            for (int i = 0; i < numPlayers; i++)
            {
                TcpClient player = new();
                await player.ConnectAsync(ipEndPoint);
                _players.Add(player);
            }
        }

        public async Task receiveMessagesAsync()
        {
            int tempCount = 1;
            try
            {
                foreach (var player in _players)
                {
                    await using NetworkStream stream = player.GetStream();

                    var messageBuffer = new byte[1_024];
                    int receivedMessage = await stream.ReadAsync(messageBuffer);

                    var message = Encoding.UTF8.GetString(messageBuffer, 0, receivedMessage);
                    Console.WriteLine($"client {tempCount} received gamecode: {message}");
                    tempCount++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"receive message async: {ex.Message}");
            }
        }
    }
}
