﻿using Microsoft.AspNetCore.Mvc;
using BoggleContracts;
using BoggleEngines;
using System;
using System.Data;
using Newtonsoft.Json;

namespace BoggleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameInfoController : ControllerBase
    {
        private readonly IDatabaseGameInfo _gameInfo;
        private readonly IBoggleServer _boggleServer;

        // would need to refactor this and add it somewhere else later
        private class GameCreationResult
        {
            public string GameCode { get; set; }
            public int GamePort { get; set; }
            public string GameIpAddress { get; set; }
        }

        public GameInfoController(IDatabaseGameInfo gameInfo, IBoggleServer boggleServer)
        {
            _gameInfo = gameInfo;
            _boggleServer = boggleServer;
        }

        // returns string
        [HttpPost("game/createGame")]
        public async Task<IActionResult> MakeGameAsync()
        {
            try
            {
                string gameCode = "11111";
                //string gameCode = await _gameInfo.CreateGameAsync();
                Tuple<string, int> gameServerInfo = _boggleServer.StartServer();

                var result = new GameCreationResult
                {
                    GameCode = gameCode,
                    GamePort = gameServerInfo.Item2,
                    GameIpAddress = gameServerInfo.Item1,
                };
                // send game code to all clients
                await _boggleServer.sendMessageToPlayersAsync(gameCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("game/startGame")]
        public IActionResult StartMultiplayerGame()
        {
            try
            {
                _boggleServer.StartGame();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // returns char[]
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

        // returns string
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
