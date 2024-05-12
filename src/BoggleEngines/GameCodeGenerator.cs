// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: GameCodeGenerator.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Class for generating game code logic.
// ----------------------------------------------------------------------------------------------------

using BoggleContracts;

namespace BoggleEngines
{
    public class GameCodeGenerator : IGameCodeGenerator
    {
        /// <inheritdoc />
        public string GenerateGameCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
