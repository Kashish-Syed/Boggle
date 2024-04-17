using Microsoft.AspNetCore.Mvc;
using BoggleEngines;

namespace BoggleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoggleController : ControllerBase
    {
        private readonly GameDice game = new GameDice();

        private readonly Word testWord = new Word();

        public BoggleController()
        {

        }

        [HttpGet("shuffle")]
        public ActionResult<char[]> GetShuffledDice()
        {
            try
            {
                char[] result = game.ShuffleAllDice();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("isValidWord")]
        public ActionResult<bool> CheckValidWord([FromBody]string word)
        {
            try
            {
                return Ok(testWord.IsValidWord(word));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}