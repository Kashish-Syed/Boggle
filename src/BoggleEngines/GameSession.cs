using BoggleContracts;

namespace BoggleEngines;

public class GameSession : IGameSession
{
    private double startTime;
    private double endTime;
    private double gameDuration;
    private int score;
    private string winner;

    /// <inheritdoc />
    public int GetScore()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public int GetWinner()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public int StartGameSession()
    {
        throw new NotImplementedException();
    }
}