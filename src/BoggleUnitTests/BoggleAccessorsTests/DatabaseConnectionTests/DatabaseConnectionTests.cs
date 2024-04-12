using System.Data.SqlClient;

namespace BoggleUnitTests.BoggleAccessorsTests.DatabaseConnectionTests
{
    internal class DatabaseConnectionTests
    {
        private string connectionString =  "Server=NUGWIN-LAPTOP\\SQLEXPRESS;Initial Catalog=Boggle;Integrated Security=True";

        [Test]
        public void TestDatabaseConnection()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Assert.That(connection.State, Is.EqualTo(System.Data.ConnectionState.Open));
                }
                catch (Exception e)
                {
                    Assert.Fail("Failed to open database connection: " + e.Message);
                }
            }
        }
    }
}