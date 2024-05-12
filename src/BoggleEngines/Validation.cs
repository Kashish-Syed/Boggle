// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: Validation.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Class for validating user input.
// ----------------------------------------------------------------------------------------------------

using BoggleContracts;
using System.Text.RegularExpressions;

namespace BoggleEngines
{
    public class Validation : IValidation
    {
        /// <inheritdoc />
        public bool validateUsername(string username)
        {
            if (!Regex.IsMatch(username, @"^[a-zA-Z .'/s]{1,40}$"))
            {
                return false;
            }
            return true;
        }

        /// <inheritdoc />
        public bool validatePassword(string password) 
        {
            if (!Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$"))
            {
                return false;
            }
            return true;
        }
     }
}
