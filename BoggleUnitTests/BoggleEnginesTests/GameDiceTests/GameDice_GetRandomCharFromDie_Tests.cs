using BoggleContracts;
using BoggleEngines;

namespace BoggleUnitTests.BoggleEnginesTests.GameDiceTests
{
    internal class GameDice_GetRandomCharFromDie_Tests
    {
        private GameDice gameDiceTester = new GameDice();

        [Test]
        public void TestNegative1ThrowsArgumentOutOfRangeException()
        {
            TestDelegate action = delegate { gameDiceTester.GetRandomCharFromDie(-1); };

            Assert.Throws<ArgumentOutOfRangeException>(action);
        }

        [Test]
        public void Test16ThrowsArgumentOutOfRangeException()
        {
            TestDelegate action = delegate { gameDiceTester.GetRandomCharFromDie(16); };

            Assert.Throws<ArgumentOutOfRangeException>(action);
        }

        [Test, Repeat(10)]
        public void TestDie0ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(0);
            Assert.That("RIFOBX", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie1ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(1);
            Assert.That("IFEHEY", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie2ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(2);
            Assert.That("DENOWS", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie3ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(3);
            Assert.That("UTOKND", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie4ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(4);
            Assert.That("HMSRAO", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie5ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(5);
            Assert.That("LUPETS", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie6ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(6);
            Assert.That("ACITOA", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie7ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(7);
            Assert.That("YLGKUE", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie8ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(8);
            Assert.That("QBMJOA", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie9ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(9);
            Assert.That("EHISPN", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie10ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(10);
            Assert.That("VETIGN", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie11ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(11);
            Assert.That("BALIYT", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie12ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(12);
            Assert.That("EZAVND", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie13ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(13);
            Assert.That("RALESC", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie14ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(14);
            Assert.That("UWILRG", Does.Contain(result.ToString()));
        }

        [Test, Repeat(10)]
        public void TestDie15ReturnsValidChar()
        {
            char result = gameDiceTester.GetRandomCharFromDie(15);
            Assert.That("PACEMD", Does.Contain(result.ToString()));
        }
    }
}