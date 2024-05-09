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
    public class BoggleController : ControllerBase
    {
        private readonly IDatabaseGameInfo _gameInfo;
        private readonly IDatabasePlayerInfo _playerInfo;
        private readonly IDatabaseWordInfo _wordInfo;

        private readonly IGameDice _gameDice;

        public BoggleController(IDatabaseGameInfo gameInfo, IDatabasePlayerInfo playerInfo, IGameDice gameDice, IDatabaseWordInfo wordInfo)
        {
            _gameInfo = gameInfo;
            _playerInfo = playerInfo;
            _gameDice = gameDice;
            _wordInfo = wordInfo;

        }

        [HttpGet("shuffle")]
        public async ActionResult<char[]> GetShuffledDice()
        {
            try
            {
                char[] result = _gameDice.ShuffleAllDice();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("isValidWord")]
        public async Task<IActionResult<bool>> CheckValidWordAsync([FromBody] string word)
        {
            try
            {
                bool isValidWord = await _wordInfo.IsValidWordAsync(word);
                return Ok(isValidWord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("game/createGame")]
        public async Task<IActionResult<string>> MakeGameAsync()
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

        [HttpGet("game/{gameCode}/getBoard")]
        public async Task<IActionResult<char[]>> MakeBoardAsync(string gameCode)
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

        [HttpGet("game/{gameCode}/getWinner")]
        public Task<IActionResult<string>> FindWinnerAsync(string gameCode)
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
                if(result > 0)
                    return Ok();
                else
                    return NotFound("Game not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("game/{gameCode}/getWordsPlayed/{username}")]
        public async Task<IActionResult> GetPlayerWordsAsync(string gameCode, string username)
        {
            try
            {
                DataTable words = await _playerInfo.GetWordsPlayedAsync(gameCode, username);
                string json = JsonConvert.SerializeObject(words, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("game/{gameCode}/addPlayerToGame/{username}")]
        public async Task<IActionResult> AddPlayerToGameAsync(string gameCode, string username)
        {
            try
            {
                await _playerInfo.AddPlayerAsync(gameCode, username);
                return Ok("Player added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("player/{username}/add")]
        public Task<IActionResult> AddPlayerToRecordAsync(string username,[FromBody] string password)
        {
            try
            {
                await _playerInfo.AddPlayerAsync(username, password);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("player/{username}/authenticate")]
        public Task<IActionResult> GetPlayerAuthenticationAsync(string username, [FromBody] string password)
        {
            try
            {
                int userId = await _playerInfo.AuthenticateAsync(username, password);
                if (userId != -1)
                {
                    return Ok(new { UserId = userId });
                }
                else
                {
                    return NotFound("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("player/getUsername")]
        public Task<IActionResult> GetPlayerUsernameAsync([FromBody] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid UserId"); 
            }

            try
            {
                string username = await _playerInfo.GetUsernameAsync(userId);
                if (string.IsNullOrEmpty(username))
                {
                    return NotFound("No user found");
                }
                else
                {
                    return Ok(username);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }



        [HttpDelete("player/{username}/delete")] 
        public Task<IActionResult> RemovePlayerAsync(string username, [FromBody] string password)
        {
            try
            {
                await _playerInfo.RemovePlayerAsync(username, password);
                return Ok("Player removed successfully.");
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                    return NotFound("Player not found or wrong password.");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet("player/{username}/games")]
        public Task<IActionResult> GetGameRecordsAsync(string username)
        {
            try
            {
                DataTable games = await _playerInfo.GetGamesAsync(username);
                string json = JsonConvert.SerializeObject(games, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
