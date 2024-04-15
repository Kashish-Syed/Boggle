using NUnit.Framework;
using static System.Formats.Asn1.AsnWriter;

namespace BoggleContracts;

public interface IUserStatistics
{
    /// <summary>
    /// Returns the number of games played.
    /// </summary>
    public int TotalGamesPlayed();

    /// <summary>
    /// Returns the highest score a user ever scored by comparing the scores 
    /// of all the games a user has played.
    /// </summary>
    public int GetHighestScore(int currentScore);

    /// <summary>
    /// Returns the average of all games ever played by giving the total score by
    /// adding the score of each game playes.
    /// </summary>
    public int AverageScorePerGame(int totalScore);

    /**
     * comparing words from different games to get the longest word ever found by the user
     */

    /// <summary>
    /// Compares words from different games to get the longest word ever found by the user.
    /// </summary>
    public string LongestWordPlayed(string word);
}