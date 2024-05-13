// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: IDatabaseWordInfo.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Interface for the DatabaseWordInfo.cs class.
// ----------------------------------------------------------------------------------------------------

namespace BoggleContracts
{
    public interface IDatabaseWordInfo
    {
        /// <summary>
        /// Adds all words to database. Input file should have one word per line
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        Task AddWordsToDatabaseAsync(string filepath);

        /// <summary>
        /// Gets the ID of a word.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        Task<int> GetWordIDAsync(string word);

        /// <summary>
        /// Returns true if the word is valid.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        Task<bool> IsValidWordAsync(string word);
    }
}
