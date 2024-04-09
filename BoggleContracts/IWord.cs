namespace BoggleContracts;

public interface IWord
{
    
    /**
     * getting the points associated with the word based on its length
     */
    int GetPoints(string word);
}