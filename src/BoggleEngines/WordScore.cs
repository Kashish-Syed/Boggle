// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: WordScore.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Engine class that contains methods to assign scores to words.
// ----------------------------------------------------------------------------------------------------


using BoggleContracts;
using System;

namespace BoggleEngines
{
    public class WordScore : IWordScore
    {
        /// <inheritdoc />
        public int CalculatePoints(int wordLength)
        {
            return wordLength switch
            {
                <= 2 => 0,
                3 or 4 => 1,
                5 => 2,
                6 => 3,
                7 => 5,
                _ => 11,
            };
        }

    }
}
