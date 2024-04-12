using BoggleContracts;

namespace BoggleEngines;

public class UserStatistics : IUserStatistics
{
    private int score;
    private Word word;

    int IUserStatistics.AverageScorePerGame(int totalScore)
    {
        throw new NotImplementedException();
    }

    int IUserStatistics.GetHighestScore()
    {
        throw new NotImplementedException();
    }

    string IUserStatistics.LongestWordPlayed(string word)
    {
        throw new NotImplementedException();
    }

    int IUserStatistics.TotalGamesPlayed()
    {
        throw new NotImplementedException();
    }
}