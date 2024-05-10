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

        public PlayerInfoController(IDatabasePlayerInfo playerInfo)
        {
            _playerInfo = playerInfo;
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
