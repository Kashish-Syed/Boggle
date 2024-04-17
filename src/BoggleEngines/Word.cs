using System.Security;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

using BoggleContracts;
using System.Linq.Expressions;

namespace BoggleEngines;

public class Word : IWord
{
    private string word;
    private int wordLength;
    private int points;

    private List<string> jsonWords;

    private static readonly string[] dictionary = { "GEEKS", "FOR", "QUIZ", "GUQ", "EE" };

    // trying 3x3 matrix before starting with 4x4
    static readonly int M = 3, N = 3;


    public Word()
    {
        word = "";
        wordLength = 0;
        points = 0;
    }

    public int GetPoints(string word)
    {
        int wordLength = word.Length;
        int points = 0;

        if (wordLength <= 2)
        {
            points = 0;
            Console.WriteLine("Invalid word");
        }

        if (wordLength == 3 || wordLength == 4)
        {
            points = 1;
        }
        else if (wordLength == 5)
        {
            points = 2;
        }
        else if (wordLength == 6)
        {
            points = 3;
        }
        else if (wordLength == 7)
        {
            points = 5;
        }
        else
        {
            points = 11;
        }

        return points;
    }

    /**
    need to check if the word is present in the json file or not
    */
    public bool IsValidWord(string word)
    {


        // Reading from the json file
        string jsonText = System.IO.File.ReadAllText(@"resources/words.json");

        Word? words = JsonConvert.DeserializeObject<Word>(jsonText);
        string serialized = JsonConvert.SerializeObject(words);

        Console.WriteLine("Enter a word: ");
        string? input = Console.ReadLine();

        if (words?.Equals(input) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}