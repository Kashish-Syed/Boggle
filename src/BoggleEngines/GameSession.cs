using BoggleContracts;

namespace BoggleEngines;

public class GameSession : IGameSession
{
    private double startTime;
    private double endTime;
    private double gameDuration;
    private int score;
    private string winner;

    int IGameSession.GetScore()
    {
        throw new NotImplementedException();
    }

    int IGameSession.GetWinner()
    {
        throw new NotImplementedException();
    }

    int IGameSession.StartGameSession()
    {
        throw new NotImplementedException();
    }
}