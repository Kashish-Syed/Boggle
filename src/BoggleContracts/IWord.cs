namespace BoggleContracts;

public interface IWord
{

    /// <summary>
    /// Calculates the points associated with the word based on the following rubric:
    /// 3-4 letters: 1 point
    /// 5 letters: 2 point
    /// 6 letters: 3 points
    /// 7 letters: 5 points
    /// 8+ letters: 11 points
    /// Note: The "Qu" letter counts as two letters
    /// </summary>
    public int GetPoints(string word);

    /// <summary>
    /// Checks if the word is present in the database or is not a valid word.
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public bool IsWord(string word);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="boggle"></param>
    /// <param name="visited"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="word"></param>
    public void FindWordsUntil(char[,] boggle, bool[,] visited, int i, int j, string word);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="boggle"></param>
    public void FindWords(char[,] boggle);
}