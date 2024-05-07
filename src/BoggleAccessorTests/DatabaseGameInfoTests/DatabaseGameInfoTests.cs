using NUnit.Framework;
using System.Data;
using System.Data.SqlClient;
using BoggleAccessors;

namespace BoggleAccessorTests.DatabaseGameInfoTests
{
    [TestFixture]
    public class DatabaseGameInfoTests
    {
        private DatabaseGameInfo _dbGameInfo;

        [SetUp]
        public void Setup()
        {
            _dbGameInfo = new DatabaseGameInfo();
        }

        [Test]
        public void CreateGameCreatesValidGame()
        {
            string gameCode = _dbGameInfo.CreateGame();
            Assert.That(gameCode, Is.Not.EqualTo(null), "Game code should not be null");
            Assert.That(gameCode.Length, Is.EqualTo(6), "Game code should be 6 characters long");

            char[] board = _dbGameInfo.GetBoard(gameCode);
            Assert.That(board, Is.Not.EqualTo(null), "Board should not be null");
            Assert.That(board.Length, Is.EqualTo(16), "Board should contain 16 characters");

            int deleteResult = _dbGameInfo.DeleteGame(gameCode);
            Assert.That(deleteResult, Is.EqualTo(1), "Game was not deleted");
        }

        [Test]
        public void GetBoardReturnsCorrectBoard()
        {
            string gameCode = _dbGameInfo.CreateGame();
            char[] expectedBoard = _dbGameInfo.GetBoard(gameCode);

            char[] retrievedBoard = _dbGameInfo.GetBoard(gameCode);
            Assert.That(retrievedBoard, Is.EqualTo(expectedBoard), "Retrieved board did not match stored board");

            _dbGameInfo.DeleteGame(gameCode);
        }
    }
}
