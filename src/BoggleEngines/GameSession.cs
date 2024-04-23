using BoggleContracts;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Models;

namespace BoggleEngines;

public class GameSession : IGameSession
{
    private double startTime;
    private double endTime;
    private double gameDuration;
    //private int score;
    //private string? winner;

    private readonly IWord _word;

    /// <summary>
    /// Constructor for game session
    /// </summary>
    /// <param name="word">Word class injection</param>
    public GameSession(IWord word)
    {
        _word = word;
    }

    /// <inheritdoc />
    public int GetScore(string userWordJson)
    {
        // Awaited input format from Front-End API
        //{
        //    "username": "idk123"
        //    "words": ["abf", "jump", "ace", "heh"]
        //}

        var data = JsonConvert.DeserializeObject<User>(userWordJson);
        var score = 0;

        if (data == null || data.Words == null || data.UserName == null)
        {
            throw new Exception("User data is missing");
        }

        foreach (string userWord in data.Words)
        {
            score += _word.GetPoints(userWord);
        }

        return score;
    }

    /// <inheritdoc />
    public Guid? GetWinner(string allPlayersJson)
    {
        List<User>? userList = JsonConvert.DeserializeObject<List<User>>(allPlayersJson);
        User? winner = null;
        int score = 0;

        if (userList == null || userList.Count == 0)
        {
            return null;
        }

        // Go through each user
        foreach (var user in userList)
        {
            string jsonUserString = JsonConvert.SerializeObject(user);
            if (GetScore(jsonUserString) > score)
            {
                // Set the winnder as the user with leading score
                winner = user;
            }
        }
        return winner?.Id;
    }

    /// <inheritdoc />
    public int StartGameSession()
    {
        throw new NotImplementedException();
    }
}