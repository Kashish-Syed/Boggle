// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: BoggleController.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Provides API endpoint for game setup.
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
    public class BoggleController : ControllerBase
    {
        private readonly IGameDice _gameDice;

        public BoggleController(IGameDice gameDice)
        {
            _gameDice = gameDice;
        }

        /// <summary>
        /// API Endpoint for shuffling the dice.
        /// </summary>
        /// <returns></returns>
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
    }
}
