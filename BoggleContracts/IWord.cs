namespace Boggle;

internal interface IWord
{
    
    /**
     * getting the points associated with the word based on its length
     */
    private int GetPoints(string word);
}