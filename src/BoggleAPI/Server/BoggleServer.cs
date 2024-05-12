// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: BoggleServer.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Class to handle the Boggle server for multiplayer.
// ----------------------------------------------------------------------------------------------------

using BoggleContracts;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BoggleAPI.Server
{
    public class BoggleServer : IBoggleServer
    {
        private TcpListener _server;
        private bool _isRunning = false;
        private bool _receivingPlayers = false;
        private CancellationTokenSource _cancellationTokenSource;
        private Timer _boggleTimer;
        private ConcurrentBag<TcpClient> _players = new ConcurrentBag<TcpClient>();

        /// <inheritdoc />
        public Tuple<IPAddress, int> StartServer(string gameCode)
        {
            // passing 0 so that any available port would be assigned
            _server = new TcpListener(IPAddress.Any, 1337);
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                // start the server
                _server.Start();

                // set state of the server to running
                _isRunning = true;
                _receivingPlayers = true;

                IPEndPoint localEndPoint = _server.LocalEndpoint as IPEndPoint;
                // get the assigned port number
                int port = localEndPoint.Port;

                // get local IP address 
                IPAddress localIP = GetLocalIPAddress();

                Console.WriteLine($"Server started on IP {localIP} and port {port}");

                GetPlayersAsync(gameCode, _cancellationTokenSource.Token);

                // return both the IpAddress and Port number of the server
                return new Tuple<IPAddress, int>(localIP, port);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"server start: {ex.Message}");
                return new Tuple<IPAddress, int>(IPAddress.Any, 1337);
            }
        }

        private IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        /// <inheritdoc />
        public void StartGame()
        {
            // change game timer here
            _receivingPlayers = false;
            _cancellationTokenSource.Cancel();

            _boggleTimer = new Timer(EndGame, null, 10000, Timeout.Infinite);
            Console.WriteLine("Game timer started, you have 30 seconds");
        }

        /// <inheritdoc />
        public void EndGame(object state)
        {
            try
            {
                _isRunning = false;
                if (_players.Count != 0)
                {
                    foreach (var player in _players)
                    {
                        if (player.Connected)
                        {
                            NetworkStream stream = player.GetStream();
                            stream.Close();
                            player.Close();
                        }
                    }
                }
                _server?.Stop();
                _server?.Dispose();
                
                _players = new ConcurrentBag<TcpClient>();
                Console.WriteLine("Game ended and the server is stopped.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EndGame: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task sendMessageToPlayersAsync(string message)
        {
            int tempCount = 1;

            if (_players.Count == 0)
            {
                Console.WriteLine("Can't send message because no connected clients");
            }
            else
            {
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message);

                foreach (var player in _players)
                {
                    if (player.Connected)
                    {
                        await using (NetworkStream stream = player.GetStream())
                        {
                            await stream.WriteAsync(messageBuffer);
                            Console.WriteLine($"Message sent to player {tempCount}");
                            tempCount++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Start accepting players on the sever
        /// </summary>
        /// <param name="gameCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task GetPlayersAsync(string gameCode, CancellationToken cancellationToken)
        {
            // keep track of player count
            int playerCount = 1;

            try
            {
                // max other players that can join is 3
                while (_isRunning && _receivingPlayers && playerCount < 4)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine("game started, no more players can join");
                        break;
                    }
                    TcpClient player = await _server.AcceptTcpClientAsync(cancellationToken);
                    _players.Add(player);
                    // send the gameCode to players as soon as they connect
                    Console.WriteLine($"player {playerCount} connected");
                    sendMessageToPlayerAsync(player, gameCode, playerCount);
                    playerCount++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetPlayersAsync: {ex.Message}");
            }
        }

        /// <summary>
        /// Function to send a message to a single player.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="message"></param>
        /// <param name="playerCount"></param>
        /// <returns></returns>
        private async Task sendMessageToPlayerAsync(TcpClient player, string message, int playerCount)
        {
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);

            if (player.Connected)
            {
                await using (NetworkStream stream = player.GetStream())
                {
                    await stream.WriteAsync(messageBuffer);
                    Console.WriteLine($"Message sent to player {playerCount}");
                }
            }
        }
    }
}
