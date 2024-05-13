using NUnit.Framework;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
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

        [TearDown]
        public async Task Cleanup()
        {
            await RemovePlayer("TestPlayer1", "TestPassword1");
            await RemovePlayer("TestPlayer2", "TestPassword1");
            await RemovePlayer("TestUser", "TestPass1");
        }

        private async Task RemovePlayer(string username, string password)
        {
            // This is a simple method to clean up players created during tests
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM Player WHERE Username = @Username AND Password = @Password", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                await command.ExecuteNonQueryAsync();
            }
        }

        [Test]
        public async Task AddPlayerAddsPlayerToDatabase_Async()
        {
            bool result = await _dbPlayerInfo.AddPlayerAsync("TestPlayer1", "TestPassword1");
            Assert.That(result, Is.True, "Player should be successfully added");

            // Cleanup inside the test if needed immediately
            await RemovePlayer("TestPlayer1", "TestPassword1");
        }

        [Test]
        public async Task DeletePlayerRemovesPlayerFromDatabase_Async()
        {
            await _dbPlayerInfo.AddPlayerAsync("TestPlayer2", "TestPassword1");
            bool result = await _dbPlayerInfo.RemovePlayerAsync("TestPlayer2", "TestPassword1");
            Assert.That(result, Is.True, "Player should be successfully removed");
        }

        [Test]
        public async Task AuthenticateReturnsCorrectPlayerId_Async()
        {
            string username = "TestUser";
            string password = "TestPass1";

            await _dbPlayerInfo.AddPlayerAsync(username, password);
            int playerId = await _dbPlayerInfo.AuthenticateAsync(username, password);
            Assert.That(playerId, Is.Not.EqualTo(-1), "Player was not found");

            // Cleanup might be redundant if done in TearDown, but ensures immediate cleanup
            await RemovePlayer(username, password);
        }
    }
}
