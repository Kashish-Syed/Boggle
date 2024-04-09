namespace Boggle;

public class Word : IWord
{
    private string word;
    private int wordLength;
    private int points;

    public Word()
    {
        word = '';
        wordLength = 0;
        points = 0;
    }

    private int GetPoints(string word)
    {
        int wordLength = word.Length;
        int points = 0;

        if (wordLength <= 2)
        {
            points = 0;
            Console.Writeline("Invalid word");
        }
        
        if (wordLength == 3 || wordLength == 4)
        {
            points = 1;
        } else if (wordLength == 5)
        {
            points = 2;
        } else if (wordLength == 6)
        {
            points = 3;
        } else if (wordLength == 7)
        {
            points = 5
        }
        else
        {
            points = 11
        }
    }
}