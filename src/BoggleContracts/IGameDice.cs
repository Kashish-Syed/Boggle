using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace BoggleContracts;

public interface IGameDice
{

    /// <summary>
    /// Simulates rolling one of the boggle game dice
    /// </summary>
    /// <returns>a single char representing a character on the die</returns>
    char GetRandomCharFromDie(int dieIndex);

    /// <summary>
    /// Shuffles all the boggle dice in a random order
    /// </summary>
    /// <returns>a character array representing the dice rolls</returns>
    char[] ShuffleAllDice();
}
