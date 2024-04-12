using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleContracts;

public interface IGameSession
{
    int GetScore();

    int GetWinner();

    int StartGameSession();
}