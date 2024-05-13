// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: GameInfoController.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Controller class for providing API endpoints for managing the game session.
// ----------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using BoggleContracts;
using BoggleEngines;
using System;
using System.Data;
using System.Net;
using Newtonsoft.Json;


namespace BoggleAPI.Models
{
    public class GameCreationResult
    {
        /// <summary>
        /// The 5 character alphanumeric unique game code.
        /// </summary>
        public string GameCode { get; set; }

        /// <summary>
        /// The port on which the Tcp game server was started
        /// </summary>
        public int GamePort { get; set; }

        /// <summary>
        /// The Ip Address on which the game was started.
        /// </summary>
        public string GameIpAddress { get; set; }
    }
}