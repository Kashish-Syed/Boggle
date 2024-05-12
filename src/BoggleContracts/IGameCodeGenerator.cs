// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: IGameCodeGenerator.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Interface for the GameCodeGenerator.cs class
// ----------------------------------------------------------------------------------------------------

namespace BoggleContracts;

public interface IGameCodeGenerator
{
    /// <summary>
    /// Generate a 5 character game code.
    /// </summary>
    /// <returns>Generated game code.</returns>
    string GenerateGameCode();
}
