using Microsoft.AspNetCore.Mvc;
using BoggleContracts;
using BoggleEngines;

namespace BoggleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoggleController : ControllerBase
    {
        private readonly IGameDice _game;
        private readonly IWord _word;

        public BoggleController(IGameDice game, IWord word)
        {
            _game = game;
            _word = word;
        }

        [HttpGet("shuffle")]
        public ActionResult<char[]> GetShuffledDice()
        {
            try
            {
                char[] result = _game.ShuffleAllDice();
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
                return Ok(_word.IsInputMatch(word));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}