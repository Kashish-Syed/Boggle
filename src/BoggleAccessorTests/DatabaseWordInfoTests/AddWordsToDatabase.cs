using NUnit.Framework;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using BoggleAccessors;

namespace BoggleAccessorTests.DatabaseWordInfoTests
{
    [TestFixture]
    public class AddWordsToDatabaseTests
    {
        private DatabaseWordInfo _dbWordInfo;
        private SqlConnection _connection;
        private string connectionString;

        [SetUp]
        public async Task Setup()
        {
            connectionString = "Server=localhost\\SQLEXPRESS;Database=boggle;Trusted_Connection=True;";
            _connection = new SqlConnection(connectionString);
            _dbWordInfo = new DatabaseWordInfo(_connection);
            await _connection.OpenAsync();
        }

        [TearDown]
        public async Task Teardown()
        {
            await _connection.CloseAsync();
        }

        [Test]
        public async Task AddWordsToDatabaseTestAsync()
        {
            string filePath = @"..\..\..\..\..\resources\words.txt";
            if (!File.Exists(filePath))
            {
                Assert.Fail("File path does not exist: " + filePath);
            }

            await _dbWordInfo.AddWordsToDatabaseAsync(filePath);
        }
    }
}
