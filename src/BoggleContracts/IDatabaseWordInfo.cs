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
        void AddWordsToDatabase(string filepath);

        /// <summary>
        /// Gets the ID of a word
        /// </summary>
        int GetWordID(string word);

        /// <summary>
        /// Returns true if the word is valid
        /// </summary>
        bool IsValidWord(string word);
    }
}
