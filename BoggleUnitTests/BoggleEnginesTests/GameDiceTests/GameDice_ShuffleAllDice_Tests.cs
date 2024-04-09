using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoggleEngines;

namespace BoggleUnitTests.BoggleEnginesTests.GameDiceTests
{
    internal class GameDice_ShuffleAllDice_Tests
    {
        public GameDice gameDiceTester = new GameDice();

        [Test]
        public void TestArrayLengthIs16()
        {
            char[] array = gameDiceTester.ShuffleAllDice();

            Assert.That(array.Length, Is.EqualTo(16));
        }

        [Test, Repeat(10)]
        public void TestOutputContainsOnlyValidUppercaseLetters()
        {
            char[] array = gameDiceTester.ShuffleAllDice();
            foreach (char letter in array)
            {
                bool isUppercaseLetter = char.IsLetter(letter) && letter >= 'A' && letter <= 'Z';
                Assert.That(isUppercaseLetter, Is.True, $"'{letter}' is not a valid uppercase letter.");
            }
        }
    }
}
