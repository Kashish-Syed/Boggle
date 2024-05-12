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
        public string GameCode { get; set; }

        public int GamePort { get; set; }

        public string GameIpAddress { get; set; }
    }
}