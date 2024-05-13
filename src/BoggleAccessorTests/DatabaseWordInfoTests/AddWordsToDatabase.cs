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
        private string connectionString;

        [SetUp]
        public void Setup()
        {
            connectionString = "Server=localhost\\SQLEXPRESS;Database=boggle;Trusted_Connection=True;";
            _dbWordInfo = new DatabaseWordInfo(connectionString);
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
