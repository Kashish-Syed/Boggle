// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: IValidation.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Interface for the DatabaseWordInfo.cs class.
// ----------------------------------------------------------------------------------------------------

namespace BoggleContracts
{
    public interface IValidation
    {
        /// <summary>
        /// Function to validate the user entered username so that it only contains
        /// letters, slashes, spaces, apostrophes, and periods. Restricts it to 40
        /// characters.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>True of the username is passes criteria and false otherwise.</returns>
        bool validateUsername(string username);

        /// <summary>
        /// Function to validate that the user password has at least one digit, one lowercase
        /// letter, one upper case letter, and is between 8 and 10 characters in length. 
        /// Have at least one digit ((?=.*\d)).
        /// </summary>
        /// <param name="password"></param>
        /// <returns>True if the password passes criteria and false otherwise.</returns>
        bool validatePassword(string password);
    }
}
