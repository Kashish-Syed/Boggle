namespace BoggleContracts;

public interface IUserStatistics
{
    /**
     * returns the number of games played 
     */
    public int TotalGamesPlayed();

    /**
     * returns the highest score a user ever scored by comparing the scores 
     * of all the games a user has played
     */
    public int GetHighestScore(int currentScore);

    /**
     * returns the average of all games ever played by giving the total score by
     * adding the score of each game playes
     */
    public int AverageScorePerGame(int totalScore);

    /**
     * comparing words from different games to get the longest word ever found by the user
     */
    public string LongestWordPlayed(string word);
}