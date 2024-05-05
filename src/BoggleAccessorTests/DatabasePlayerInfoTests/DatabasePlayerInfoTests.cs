using NUnit.Framework;
using System.Data;
using System.Data.SqlClient;
using BoggleAccessors;

namespace BoggleAccessorTests.DatabasePlayerInfoTests
{
    [TestFixture]
    public class DatabasePlayerInfoTests
    {
        private DatabasePlayerInfo _dbPlayerInfo;

        [SetUp]
        public void Setup()
        {
            _dbPlayerInfo = new DatabasePlayerInfo();
        }

        [Test]
        public void AddPlayerAddsUserToDatabase()
        {
            _dbPlayerInfo.AddPlayer("TestUser", "TestPass");
            int playerId = _dbPlayerInfo.Authenticate("TestUser", "TestPass");
            Assert.That(playerId, Is.Not.EqualTo(-1), "Player was not found");
            _dbPlayerInfo.RemovePlayer("TestUser", "TestPass");
        }

        [Test]
        public void DeletePlayerDeletesUserFromDatabase()
        {
            _dbPlayerInfo.AddPlayer("TestUser", "TestPass");

            try
            {
                _dbPlayerInfo.RemovePlayer("TestUser", "TestPass");
            }
            catch (InvalidOperationException)
            {
                Assert.Fail("Error removing player from database.");
            }
            
            int playerId = _dbPlayerInfo.Authenticate("TestUser", "TestPass");
            Assert.That(playerId, Is.EqualTo(-1), "Player still exists");
        }



    }
}
