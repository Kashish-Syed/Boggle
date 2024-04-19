namespace BoggleContracts;

public interface IWord
{

    /* 
    /// Calculates the points associated with the word based on the following rubric:
    /// 3-4 letters: 1 point
    /// 5 letters: 2 point
    /// 6 letters: 3 points
    /// 7 letters: 5 points
    /// 8+ letters: 11 points
    /// Note: The "Qu" letter counts as two letters
    */
    public int GetPoints(string word);

     /*
        check if the word is present in the json file or the database
    */
    public bool IsInputMatch(string userInput);

}