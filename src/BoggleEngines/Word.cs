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

    private static readonly string[] dictionary = { "GEEKS", "FOR", "QUIZ", "GUQ", "EE" };

    // trying 3x3 matrix before starting with 4x4
    static readonly int M = 3, N = 3;


    public Word()
    {
        word = "";
        wordLength = 0;
        points = 0;
    }

    /// <inheritdoc />
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
    public void IsValidWord(string word)
    {


        // Reading from the json file
        string jsonText = File.ReadAllText("resources/words.json");

        List<string> words = JsonConvert.DeserializeObject<List<string>>(jsonText);

        Console.WriteLine("Enter a word: ");
        string input = Console.ReadLine();

        if (words.Contains(input))
        {
            Console.WriteLine("found");
        }
        else
        {
            Console.WriteLine("not present");
        }


        // int n = dictionary.Length;
        // for (int i = 0; i < n; i++)
        // {
        //     if (word.Equals(dictionary[i]))
        //     {
        //         return true;
        //     }
        // }
    }


    // public void FindWordsUntil(char[,] boggle, bool[,] visited, int i, int j, string word)
    // {
    //     visited[i, j] = true; //setting true for first instance
    //     word = word + boggle[i, j]; // appending current character to the word
    /*
    do not need these because finding words is frontend's and players' work.
    */

    // public void FindWordsUntil(char[,] boggle, bool[,] visited, int i, int j, string word)
    // {
    //     visited[i, j] = true; //setting true for first instance
    //     word = word + boggle[i, j]; // appending current character to the word

    //     // if the word is present in the dictionary then print it
    //     var isWord = IsWord(word);
    //     if (isWord)
    //         Console.WriteLine(word);

    //     // Traverse the cells of boggle[i, j]
    //     for (int row = i - 1; row <= i + 1 && row < M; row++)
    //         for (int col = j - 1; col <= j + 1 && col < N; col++)
    //             if (row >= 0 && col >= 0 && !visited[row, col])
    //                 FindWordsUntil(boggle, visited, row, col, word);

    //     //Erase the current character from the word
    //     //mark the visited og current cell as false
    //     word = "" + word[word.Length - 1];
    //     visited[i, j] = false;

    //     public void FindWords(char[,] boggle)
    //     {
    //         bool[,] visited = new bool[M, N]; //3x3 matrix 
    // =======
    //     // }
    // >>>>>>> Stashed changes

    // public void FindWords(char[,] boggle)
    // {
    //     bool[,] visited = new bool[M, N]; //3x3 matrix 

    //     //initializing a current string
    //     string str = "";

    //     //consider every character and look for all words starting with this character
    //     for (int i = 0; i < M; i++)
    //         for (int j = 0; j < N; j++)
    //             FindWordsUntil(boggle, visited, i, j, str);

    // }

    // static void Main(String[] args)
    // {
    //     IsValidWord("vain");
    // }

}