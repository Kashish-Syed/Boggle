using BoggleContracts;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace BoggleAPI.Server
{
    public class BoggleServer : IBoggleServer
    {
        private TcpListener _server;
        private bool _isRunning = false;
        private Timer _boggleTimer;
        private List<TcpClient> _players = new List<TcpClient>();

        public BoggleServer()
        {
        }

        public Tuple<string, int> StartServer()
        {
            // passing 0 so that any avilable port would be assigned
            _server = new TcpListener(IPAddress.Any, 0);

            // start the server
            _server.Start();

            // set stat of ther server to running
            _isRunning = true;

            IPEndPoint localEndPoint = _server.LocalEndpoint as IPEndPoint;
            // get the assigned port number
            int port = localEndPoint.Port;
            string ipAddress = localEndPoint.Address.ToString();

            Console.WriteLine($"Server started on port {port}");

            GetPlayersAsync();

            // give both the IpAddress and Port number of the 
            return new Tuple<string, int>(ipAddress, port);
        }

        private async Task GetPlayersAsync()
        {
            // keep track of player count
            int playerCount = 0;

            while (_isRunning && playerCount < 4)
            {
                TcpClient player = await _server.AcceptTcpClientAsync();
                _players.Add(player);
                playerCount++;
                Console.WriteLine($"player {playerCount} connected");
            }
        }

        public void StartGame()
        {
            // change game timer here
            _boggleTimer = new Timer(EndGame, null, 30000, Timeout.Infinite);
            Console.WriteLine("game timer started, you have 30 seconds");
        }

        public void EndGame(object state)
        {
            _isRunning = false;
            _server?.Stop();
            Console.WriteLine("Game ended and the server is stopped.");
        }

        // send message to all clients
        public async Task sendMessageToPlayersAsync(string message)
        {
            int tempCount = 0;
            
            if (_players.Count == 0)
            {
                Console.WriteLine("Can't send message b/c no connected clients");
            } else
            {
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message);

                foreach (var player in _players)
                {
                    await using NetworkStream stream = player.GetStream();
                    await stream.WriteAsync(messageBuffer);
                    Console.WriteLine($"message sent to {tempCount}");
                    tempCount++;
                }
            }

        }
    }
}
