using NUnit.Framework;
using System.Data.SqlClient;
using BoggleAccessors;

namespace BoggleAccessorTests.DatabaseGameInfoTests
{
    [TestFixture]
    public class DatabaseGameInfoTests
    {
        private string connectionString;
        private DatabaseGameInfo _dbGameInfo;

        [SetUp]
        public void Setup()
        {
            connectionString = "Server=localhost\\SQLEXPRESS;Database=boggle;Trusted_Connection=True;";
            _dbGameInfo = new DatabaseGameInfo(connectionString);
        }

        [Test]
        public async Task CreateGameCreatesValidGame_Async()
        {
            string gameCode = await _dbGameInfo.CreateGameAsync();
            Assert.That(gameCode, Is.Not.Null, "Game code should not be null");
            Assert.That(gameCode.Length, Is.EqualTo(6), "Game code should be 6 characters long");

            char[] board = await _dbGameInfo.GetBoardAsync(gameCode);
            Assert.That(board, Is.Not.Null, "Board should not be null");
            Assert.That(board.Length, Is.EqualTo(16), "Board should contain 16 characters");

            int deleteResult = await _dbGameInfo.DeleteGameAsync(gameCode);
            Assert.That(deleteResult, Is.EqualTo(1), "Game was not deleted");
        }

        [Test]
        public async Task GetBoardReturnsCorrectBoard_Async()
        {
            string gameCode = await _dbGameInfo.CreateGameAsync();
            char[] expectedBoard = await _dbGameInfo.GetBoardAsync(gameCode);

            char[] retrievedBoard = await _dbGameInfo.GetBoardAsync(gameCode);
            Assert.That(retrievedBoard, Is.EqualTo(expectedBoard), "Retrieved board did not match stored board");

            await _dbGameInfo.DeleteGameAsync(gameCode);
        }

        [Test]
        public async Task DeleteGameDeletesGame_Async()
        {
            string gameCode = await _dbGameInfo.CreateGameAsync();
            int deleteResult = await _dbGameInfo.DeleteGameAsync(gameCode);
            Assert.That(deleteResult, Is.EqualTo(1), "Game was not deleted");
        }
    }
}
