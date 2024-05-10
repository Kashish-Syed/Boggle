using NUnit.Framework;
using System.Data.SqlClient;
using BoggleAccessors;

namespace BoggleAccessorTests.DatabasePlayerInfoTests
{
    [TestFixture]
    public class DatabasePlayerInfoTests
    {
        private DatabasePlayerInfo _dbPlayerInfo;
        private string connectionString;

        [SetUp]
        public void Setup()
        {
            connectionString = "Server=localhost\\SQLEXPRESS;Database=boggle;Trusted_Connection=True;";
            _dbPlayerInfo = new DatabasePlayerInfo(connectionString);
        }

        [Test]
        public async Task AddPlayerAddsPlayerToDatabase_Async()
        {
            bool result = await _dbPlayerInfo.AddPlayerAsync("TestPlayer1", "TestPassword");
            Assert.That(result, Is.True, "Player should be successfully added");

            bool deleteResult = await _dbPlayerInfo.RemovePlayerAsync("TestPlayer1", "TestPassword");
            Assert.That(deleteResult, Is.True, "Player should be successfully removed after test.");
        }

        [Test]
        public async Task DeletePlayerRemovesPlayerFromDatabase_Async()
        {
            await _dbPlayerInfo.AddPlayerAsync("TestPlayer2", "TestPassword");
            bool result = await _dbPlayerInfo.RemovePlayerAsync("TestPlayer2", "TestPassword");
            Assert.That(result, Is.True, "Player should be successfully removed");
        }
    }
}
