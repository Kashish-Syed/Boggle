// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: DatabaseWordInfo.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Accessor class for interating with Word data in the SQL database.
// ----------------------------------------------------------------------------------------------------

using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BoggleContracts;

namespace BoggleAccessors
{
    public class DatabaseWordInfo : IDatabaseWordInfo
    {
        private readonly string _connectionString;

        public DatabaseWordInfo(string connectionString)
        {
            _connectionString = connectionString;
        }

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
                            int points = CalculatePoints(line.Length);
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
                if (ex.Number != 2627) 
                {
                    throw; 
                }
            }
        }


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

        private int CalculatePoints(int wordLength)
        {
            return wordLength switch
            {
                <= 2 => 0,
                3 or 4 => 1,
                5 => 2,
                6 => 3,
                7 => 5,
                _ => 11,
            };
        }
    }
}
