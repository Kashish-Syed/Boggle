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
