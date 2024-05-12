// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: GameInfoController.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Controller class for providing API endpoints for managing the game session.
// ----------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using BoggleContracts;
using BoggleEngines;
using BoggleAPI.Models;
using System;
using System.Data;
using System.Net;
using Newtonsoft.Json;

namespace BoggleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameInfoController : ControllerBase
    {
        private readonly IDatabaseGameInfo _gameInfo;
        private readonly IBoggleServer _boggleServer;
        private readonly IBoggleClient _boggleClient;

        public GameInfoController(IDatabaseGameInfo gameInfo, IBoggleServer boggleServer, IBoggleClient boggleClient)
        {
            _gameInfo = gameInfo;
            _boggleServer = boggleServer;
            _boggleClient = boggleClient;
        }

        /// <summary>
        /// API Endpoint for creating a new game.
        /// </summary>
        /// <returns></returns>
        [HttpPost("game/createGame")]
        public async Task<IActionResult> MakeGameAsync()
        {
            try
            {
                string gameCode = await _gameInfo.CreateGameAsync();
                Tuple<IPAddress, int> gameServerInfo = _boggleServer.StartServer(gameCode);

                await Task.Delay(1000);

                var result = new GameCreationResult
                {
                    GameCode = gameCode,
                    GamePort = gameServerInfo.Item2,
                    GameIpAddress = gameServerInfo.Item1.ToString(),
                };

                await Task.Delay(2000);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// API endpoint for creating a new game board.
        /// </summary>
        /// <param name="gameCode"></param>
        /// <returns></returns>
        [HttpGet("game/{gameCode}/getBoard")]
        public async Task<IActionResult> MakeBoardAsync(string gameCode)
        {
            try
            {
                char[] board = await _gameInfo.GetBoardAsync(gameCode);
                return Ok(board);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// API endpoint for getting a winner of game identified by the game code.
        /// </summary>
        /// <param name="gameCode"></param>
        /// <returns></returns>
        [HttpGet("game/{gameCode}/getWinner")]
        public async Task<IActionResult> FindWinnerAsync(string gameCode)
        {
            try
            {
                string winner = await _gameInfo.GetWinnerAsync(gameCode);
                return Ok(winner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// API endpoint for deleting a game record.
        /// </summary>
        /// <param name="gameCode"></param>
        /// <returns></returns>
        [HttpDelete("game/{gameCode}/delete")]
        public async Task<IActionResult> RemoveGameAsync(string gameCode)
        {
            try
            {
                int result = await _gameInfo.DeleteGameAsync(gameCode);
                if (result > 0)
                    return Ok();
                else
                    return NotFound("Game not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
