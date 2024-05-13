// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: PlayerInfoController.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Controller for providing API endpoints for accessing and managing player information.
// ----------------------------------------------------------------------------------------------------

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
    public class PlayerInfoController : ControllerBase
    {
        private readonly IDatabasePlayerInfo _playerInfo;
        private readonly IDatabaseGameInfo _gameInfo;

        public PlayerInfoController(IDatabasePlayerInfo playerInfo, IDatabaseGameInfo gameInfo)
        {
            _gameInfo = gameInfo;
            _playerInfo = playerInfo;
        }

        /// <summary>
        /// Makes a GET API endpoint for getting the words in a certain game the player played
        /// by their username.
        /// </summary>
        /// <param name="gameCode"></param>
        /// <param name="username"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds words played by the player using their username and game code.
        /// </summary>
        /// <param name="gameCode"></param>
        /// <param name="username"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        [HttpPost("game/{gameCode}/addWordPlayed/{username}")]
        public async Task<IActionResult> AddWordPlayedAsync(string gameCode, string username, [FromBody] string word)
        {
            try
            {
                bool result = await _playerInfo.AddWordPlayedAsync(gameCode, username, word);
                if (result)
                {
                    return Ok("Word added successfully.");
                }
                else
                {
                    return BadRequest("Failed to add word.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// API POST endpoint to add players to a game using their username and the game code.
        /// </summary>
        /// <param name="gameCode"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("game/{gameCode}/addPlayerToGame/{username}")]
        public async Task<IActionResult> AddPlayerToGameAsync(string gameCode, string username)
        {
            try
            {
                await _gameInfo.AddPlayerAsync(gameCode, username);
                return Ok("Player added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// POST API endpoint to add player to a record using their username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("player/{username}/add")]
        public async Task<IActionResult> AddPlayerToRecordAsync(string username, [FromBody] string password)
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

        /// <summary>
        /// POST API endpoint that authenticates a player for login.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("player/{username}/authenticate")]
        public async Task<IActionResult> GetPlayerAuthenticationAsync(string username, [FromBody] string password)
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

        /// <summary>
        /// Gets the player username by userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("player/getUsername")]
        public async Task<IActionResult> GetPlayerUsernameAsync([FromBody] int userId)
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

        /// <summary>
        /// API Delete endpoint for removing a player.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpDelete("player/{username}/delete")]
        public async Task<IActionResult> RemovePlayerAsync(string username, [FromBody] string password)
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

        /// <summary>
        /// API GET endpoint for getting the user records by username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("player/{username}/games")]
        public async Task<IActionResult> GetGameRecordsAsync(string username)
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
