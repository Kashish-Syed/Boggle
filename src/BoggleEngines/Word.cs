using System.Security;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

using BoggleContracts;
using System.Linq.Expressions;

namespace BoggleEngines;

public class Word : IWord
{

    public List<string> Value { get; set; }

    public Word()
    {
        // Initialize the list of words from the JSON file
        Value = ReadWordsFromJson("words.json");
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


    /*
    method to read words from a json file
    */
    private List<string> ReadWordsFromJson(string filePath)
    {

        try
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                var wordList = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
                return wordList["words"];
            }
        }
        catch (Exception ex)
        {
            // Handle any errors reading the JSON file
            Console.WriteLine("Error reading JSON file: " + ex.Message);
            return new List<string>();
        }
    }

    // Method to check if input matches any word in the list
    public bool IsInputMatch(string userInput)
    {
        return Value.Contains(userInput);
    }

}