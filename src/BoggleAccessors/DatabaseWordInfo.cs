// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: DatabaseWordInfo.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Accessor class for interating with Word data in the SQL database.
// ----------------------------------------------------------------------------------------------------

using BoggleContracts;
using BoggleEngines;
using System.Data.SqlClient;

namespace BoggleAccessors
{
    public class DatabaseWordInfo : IDatabaseWordInfo
    {
        private readonly string _connectionString;
        WordScore wordScore = new WordScore();

        public DatabaseWordInfo(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        public async Task AddWordsToDatabaseAsync(string filepath)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (StreamReader sr = new StreamReader(filepath))
                {
                    string line;
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        line = line.Trim();
                        if (!string.IsNullOrEmpty(line) && line.All(char.IsLetter))
                        {
                            int points = wordScore.CalculatePoints(line.Length);
                            if (points > 0)
                            {
                                await InsertWordAsync(_connection, line.ToLower(), points);
                            }
                            else
                            {
                                Console.WriteLine($"Invalid word: {line}");
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public async Task InsertWordAsync(SqlConnection connection, String word, Int32 points)
        {
            try
            {
                var query = "INSERT INTO dbo.Word (Word, Points) VALUES (@word, @points)";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@word", word);
                    cmd.Parameters.AddWithValue("@points", points);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number != DatabaseSQLErrorConstants.DuplicateKeyError) 
                {
                    throw; 
                }
            }
        }


        /// <inheritdoc />
        public async Task<int> GetWordIDAsync(string word)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT WordID FROM Word WHERE Word = @Word", _connection))
                {
                    command.Parameters.AddWithValue("@Word", word);
                    object result = await command.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        /// <inheritdoc />
        public async Task<bool> IsValidWordAsync(string word)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Word WHERE Word = @Word", _connection))
                {
                    command.Parameters.AddWithValue("@Word", word);
                    int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count > 0;
                }
            }
        }
    }
}
