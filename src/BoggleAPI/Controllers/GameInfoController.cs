using Microsoft.AspNetCore.Mvc;
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

        public GameInfoController(IDatabaseGameInfo gameInfo)
        {
            _gameInfo = gameInfo;
        }

        // returns string
        [HttpPost("game/createGame")]
        public async Task<IActionResult> MakeGameAsync()
        {
            try
            {
                string gameCode = await _gameInfo.CreateGameAsync();
                return Ok(gameCode);
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
