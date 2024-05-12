// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: IDatabaseWordInfo.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Interface for the DatabaseWordInfo.cs class.
// ----------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleContracts
{
    public interface IDatabaseWordInfo
    {
        /// <summary>
        /// Adds all words to database. Input file should have one word per line
        /// </summary>
        /// 
        Task AddWordsToDatabaseAsync(string filepath);

        /// <summary>
        /// Gets the ID of a word
        /// </summary>
        Task<int> GetWordIDAsync(string word);

        /// <summary>
        /// Returns true if the word is valid
        /// </summary>
        Task<bool> IsValidWordAsync(string word);
    }
}
