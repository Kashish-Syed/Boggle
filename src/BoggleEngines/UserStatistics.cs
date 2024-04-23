using BoggleContracts;
using System.Diagnostics.Tracing;

namespace BoggleEngines;

public class UserStatistics : IUserStatistics
{
    private int score;
    private Word word;

    public int AverageScorePerGame(int totalScore)
    {
        // totalScore/totalGamesPlayed
        throw new NotImplementedException();
    }

    public int GetHighestScore(int currentScore)
    {
        // store the score of the player in a game in highestScore (first game for now)
        // store the score of the next game 
        // compare the scores of each game and update the highestScore if greater than the previous one
        // we will update the score of each game played and compare it with the highest score to update it
        int highestScore = 0;
        if (currentScore > highestScore)
        {
            highestScore = currentScore;
        }

        return highestScore;
    }

    /// <inheritdoc />
    public string LongestWordPlayed(string word)
    {
        // Expected Input
        //{
        //    "username": "abcd"
        //    "game_sessions": 
        //    [
        //       "session_1": {
        //            "words": ["abc", "cijwi"]
        //       }
        //    ]
        //}
        // or are we fetching from database?
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public int TotalGamesPlayed()
    {
        throw new NotImplementedException();
    }
}