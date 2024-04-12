using BoggleAccessors;
using NUnit.Framework;

namespace BoggleUnitTests.BoggleAccessorsTests.DatabaseConnectionTests
{
    internal class DatabaseConnectionTests
    {
        private string connectionString; // Connection string for test database

        [SetUp]
        public void Setup()
        {
            // Set up connection string for test database
            connectionString = "Server=NUGWIN-LAPTOP\\SQLEXPRESS;Initial Catalog=Boggle;Integrated Security=True";
        }

        [Test]
        public void TestDatabaseConnection()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Assert.AreEqual(ConnectionState.Open, connection.State);
                }
                catch (Exception e)
                {
                    Assert.Fail("Failed to open database connection: " + e.Message);
                }
            }
        }
    }
}