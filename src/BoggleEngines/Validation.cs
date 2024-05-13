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
        if (string.IsNullOrEmpty(username))
        {
            return false;
        }

        return Regex.IsMatch(username, @"^[a-zA-Z0-9_.'\s]{1,40}$");
    }


    /// <inheritdoc />
    public bool validatePassword(string password) 
    {
        if (password == null)
        {
            return false;
        }

        if (!Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{5,30}$"))
        {
            return false;
        }
        return true;
    }
}

}
