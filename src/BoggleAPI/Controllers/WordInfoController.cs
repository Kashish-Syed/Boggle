// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: WordInfoController.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Controller for providing API endpoints for managing words. 
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
    public class WordInfoController : ControllerBase
    {
        private readonly IDatabaseWordInfo _wordInfo;

        public WordInfoController(IDatabaseWordInfo wordInfo)
        {
            _wordInfo = wordInfo;
        }

        
        /// <summary>
        /// API Endpoint to check if the word is valid based on the word list
        /// in the database.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        [HttpPost("isValidWord")]
        public async Task<IActionResult> CheckValidWordAsync([FromBody] string word)
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
    }

}
