using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using BoggleContracts;
using BoggleEngines;

namespace BoggleAccessors
{
    public class DatabaseWordInfo : IDatabaseWordInfo
    {
        private readonly SqlConnection _connection;
        private readonly IWord _word;

        public DatabaseWordInfo(SqlConnection connection, IWord word)
        {
            _connection = connection;
            _word = word;
        }

        public async Task AddWordsToDatabaseAsync(string filepath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    await _connection.OpenAsync();
                    string line;
                    while ((line = await sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (!string.IsNullOrEmpty(line) && line.All(char.IsLetter))
                        {
                            int points = _word.GetPoints(line);
                            if (points > 0)
                            {
                                try
                                {
                                    using (SqlCommand command = new SqlCommand("INSERT INTO Word (Word, Points) VALUES (@Word, @Points)", connection))
                                    {
                                        command.Parameters.AddWithValue("@Word", line.ToLower());
                                        command.Parameters.AddWithValue("@Points", points);
                                        await command.ExecuteNonQueryAsync();
                                    }
                                }
                                catch (SqlException ex)
                                {
                                    if (ex.Number == 2627) // Handle duplicate primary key error
                                    {
                                        Console.WriteLine("Duplicate word skipped: " + line);
                                    }
                                    else
                                    {
                                        throw;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid word: " + line);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to add words to the database: {ex.Message}", ex);
            }
        }

        public async Task<int> GetWordIDAsync(string word)
        {
            await _connection.OpenAsync();
            using (SqlCommand command = new SqlCommand("SELECT WordID FROM Word WHERE Word = @Word", connection))
            {
                command.Parameters.AddWithValue("@Word", word);
                object result = await command.ExecuteScalarAsync();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public async Task<bool> IsValidWordAsync(string word)
        {
            word = word.ToLower();
            await _connection.OpenAsync();
            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Word WHERE Word = @Word", connection))
            {
                command.Parameters.AddWithValue("@Word", word.ToLower());
                int count = Convert.ToInt32(command.ExecuteScalarAsync());
                    
                if (count > 0) {
                    return true;
                } else {
                    return false;
                }
            }
        }
    }
}
