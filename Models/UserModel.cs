using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// The user model
    /// Other potential things to add:
    /// LongestWord
    /// Score
    /// GameSession
    /// Scores
    /// Games etc
    /// </summary>
    public class User
    {
        /// <summary>
        /// User Guid
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// The user's username
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// The list of words the user entered
        /// </summary>
        public List<string>? Words { get; set; }
    }
}
