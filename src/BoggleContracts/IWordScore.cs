// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: IValidation.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Interface for the WordScore.cs class.
// ----------------------------------------------------------------------------------------------------

namespace BoggleContracts
{
	public interface IWordScore
	{
		/// <summary>
		/// Calculates the score of the word based on its length.
		/// </summary>
		/// <param name="wordLength"></param>
		/// <returns></returns>
		int CalculatePoints(int wordLength);
	}
}