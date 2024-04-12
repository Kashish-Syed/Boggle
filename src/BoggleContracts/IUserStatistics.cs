namespace BoggleContracts;

public interface IUserStatistics
{
    /**
     * returns the number of games played 
     */
    int TotalGamesPlayed();

    /**
     * returns the highest score a user ever scored by comparing the scores 
     * of all the games a user has played
     */
    int GetHighestScore();

    /**
     * returns the average of all games ever played by giving the total score by
     * adding the score of each game playes
     */
    int AverageScorePerGame(int totalScore);

    /**
     * comparing words from different games to get the longest word ever found by the user
     */
    string LongestWordPlayed(string word);
}