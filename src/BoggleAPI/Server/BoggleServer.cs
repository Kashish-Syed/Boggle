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

            GetPlayers();

            // give both the IpAddress and Port number of the 
            return new Tuple<string, int>(ipAddress, port);
        }

        private async Task GetPlayers()
        {
            // keep track of player count
            int playerCount = 0;

            while (_isRunning && playerCount <= 4)
            {
                TcpClient player = await _server.AcceptTcpClientAsync();
                playerCount++;
            }
        }

        public void StartGame()
        {
            _boggleTimer = new Timer(EndGame, null, 30000, Timeout.Infinite);
            Console.WriteLine("game timer started, you have 30 seconds");
        }

        public void EndGame(object state)
        {
            _isRunning = false;
            _server?.Stop();
            Console.WriteLine("Game ended and the server is stopped.");
        }

        // could add pause game as well here
    }
}
