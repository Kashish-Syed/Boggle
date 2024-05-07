using NUnit.Framework;
using System.Data;
using System.Data.SqlClient;
using BoggleAccessors;

namespace BoggleAccessorTests.DatabaseWordInfoTests
{
    [TestFixture]
    public class AddWordsToDatabase
    {
        private DatabaseWordInfo _dbWordInfo;

        [SetUp]
        public void Setup()
        {
            _dbWordInfo = new DatabaseWordInfo();
        }

        [Test]
        public void AddWordsToDatabaseTest()
        {   
            string filePath = @"..\..\..\..\..\resources\words.txt";
            _dbWordInfo.AddWordsToDatabase(filePath);
            Assert.DoesNotThrow(() => _dbWordInfo.AddWordsToDatabase(filePath),
                "Method should not throw any exceptions when adding words successfully.");
        }
    }
}
