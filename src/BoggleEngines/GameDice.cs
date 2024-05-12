// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: GameDice.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Class for managing dice for the Boggle game.
// ----------------------------------------------------------------------------------------------------

using BoggleContracts;

namespace BoggleEngines
{
    public class GameDice : IGameDice
    {
        private static readonly string[] Dice =
        {
            "RIFOBX", "IFEHEY", "DENOWS", "UTOKND",
            "HMSRAO", "LUPETS", "ACITOA", "YLGKUE",
            "QBMJOA", "EHISPN", "VETIGN", "BALIYT",
            "EZAVND", "RALESC", "UWILRG", "PACEMD"
        };

        private static readonly Random rng = new Random();

        /// <inheritdoc />
        public char GetRandomCharFromDie(int die)
        {
            if (die < 0 || die >= Dice.Length)
            {
                throw new ArgumentOutOfRangeException("Argument " + die + " is out of range. Must be from 0 to " + (Dice.Length - 1));
            }

            string selectedDie = Dice[die];

            return selectedDie[rng.Next(6)];
        }

        /// <inheritdoc />
        public char[] ShuffleAllDice()
        {
            int[] randomIndices = new int[Dice.Length];

            for (int i = 0; i < Dice.Length; i++)
            {
                randomIndices[i] = i;
            }

            // Fisher-Yates shuffle
            for (int i = randomIndices.Length - 1; i > 0; i--)
            {
                int swapIndex = rng.Next(i + 1);
                int temp = randomIndices[i];
                randomIndices[i] = randomIndices[swapIndex];
                randomIndices[swapIndex] = temp;
            }

            char[] shuffled = new char[Dice.Length];
            for (int i = 0; i < Dice.Length; i++)
            {
                shuffled[i] = GetRandomCharFromDie(randomIndices[i]);
            }

            return shuffled;
        }

    }
}
