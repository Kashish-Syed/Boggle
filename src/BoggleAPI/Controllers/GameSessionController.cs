// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: GameSessionController.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Controller class for providing API endpoints for managing the game session server.
// ----------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using BoggleContracts;
using BoggleEngines;
using System;
using System.Data;
using System.Net;
using Newtonsoft.Json;

namespace BoggleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameSessionController : ControllerBase
    {
        private readonly IBoggleServer _boggleServer;

        public GameSessionController(IBoggleServer boggleServer)
        {
            _boggleServer = boggleServer;
        }

        /// <summary>
        /// API GET endpoint for starting the multiplayer game by starting the game timer.
        /// </summary>
        /// <returns></returns>
        [HttpGet("session/startGame")]
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

        /// <summary>
        /// API GET endpoint for ending the game by closing the TCP Listener.
        /// </summary>
        /// <returns></returns>
        [HttpGet("session/endGame")]
        public IActionResult EndMUltiplayerGame()
        {
            try
            {
                _boggleServer.EndGame(null);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
