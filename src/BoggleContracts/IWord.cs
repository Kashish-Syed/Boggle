namespace BoggleContracts;

public interface IWord
{

    /**
     * getting the points associated with the word based on its length
     */
    public int GetPoints(string word);

    /*
     * checking if the word is present in the database or not given a word/string 
     */
    public bool IsWord(string word);

    public void FindWordsUntil(char[,] boggle, bool[,] visited, int i, int j, string word);

    public void FindWords(char[,] boggle);
}