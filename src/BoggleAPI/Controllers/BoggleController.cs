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
        private readonly IGameDice _gameDice;

        private readonly IDatabaseWordInfo _wordInfo;

        public BoggleController(IDatabaseGameInfo gameInfo, IDatabasePlayerInfo playerInfo, IGameDice gameDice, IDatabaseWordInfo wordInfo)
        {
            _gameInfo = gameInfo;
            _playerInfo = playerInfo;
            _gameDice = gameDice;
            _wordInfo = wordInfo;

        }

        [HttpGet("shuffle")]
        public ActionResult<char[]> GetShuffledDice()
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
        public ActionResult<bool> CheckValidWord([FromBody] string word)
        {
            try
            {
                bool isValidWord = _wordInfo.IsValidWord(word);
                return Ok(isValidWord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("game/createGame")]
        public ActionResult<string> CreateGame()
        {
            try
            {
                string gameCode = _gameInfo.CreateGame();
                return Ok(gameCode);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("game/{gameCode}/getBoard")]
        public ActionResult<char[]> GetBoard(string gameCode)
        {
            try
            {
                char[] board = _gameInfo.GetBoard(gameCode);
                return Ok(board);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("game/{gameCode}/getWinner")]
        public ActionResult<string> GetWinner(string gameCode)
        {
            try
            {
                string winner = _gameInfo.GetWinner(gameCode);
                return Ok(winner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("game/{gameCode}/delete")]
        public ActionResult DeleteGame(string gameCode)
        {
            try
            {
                int result = _gameInfo.DeleteGame(gameCode);
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
        public IActionResult GetWordsPlayed(string gameCode, string username)
        {
            try
            {
                DataTable words = _playerInfo.GetWordsPlayed(gameCode, username);
                string json = JsonConvert.SerializeObject(words, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("game/{gameCode}/addPlayerToGame/{username}")]
        public IActionResult AddPlayerToGame(string gameCode, string username)
        {
            try
            {
                _playerInfo.AddPlayer(gameCode, username);
                return Ok("Player added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("player/{username}/add")]
        public ActionResult AddPlayer(string username,[FromBody] string password)
        {
            try
            {
                _playerInfo.AddPlayer(username, password);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("player/{username}/authenticate")]
        public ActionResult AuthenticatePlayer(string username, [FromBody] string password)
        {
            try
            {
                int userId = _playerInfo.Authenticate(username, password);
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
        public ActionResult GetUsername([FromBody] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid UserId"); 
            }

            try
            {
                string username = _playerInfo.GetUsername(userId);
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
        public ActionResult DeletePlayer(string username, [FromBody] string password)
        {
            try
            {
                _playerInfo.RemovePlayer(username, password);
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
        public IActionResult GetGames(string username)
        {
            try
            {
                DataTable games = _playerInfo.GetGames(username);
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
