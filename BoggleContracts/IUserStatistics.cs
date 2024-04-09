namespace Boggle;

internal interface IUserStatistics
{
    /**
     * returns the number of games played 
     */
    private int TotalGamesPlayed();

    /**
     * returns the highest score a user ever scored by comparing the scores 
     * of all the games a user has played
     */
    private int GetHighestScore();

    /**
     * returns the average of all games ever played by giving the total score by
     * adding the score of each game playes
     */
    private int AverageScorePerGame(int totalScore);

    /**
     * comparing words from different games to get the longest word ever found by the user
     */
    private string LongestWordPlayed(string word);
}