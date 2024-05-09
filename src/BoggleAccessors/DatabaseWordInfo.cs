using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using BoggleContracts;

namespace BoggleAccessors
{
    public class DatabaseWordInfo : IDatabaseWordInfo
    {
        private readonly SqlConnection _connection;

        public DatabaseWordInfo(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task AddWordsToDatabaseAsync(string filepath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    string line;
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        int wordLength = line.Length;
                        int points = 0;
                        line = line.Trim();
                        if (!string.IsNullOrEmpty(line) && line.All(char.IsLetter))
                        {   
                            if (wordLength <= 2)
                            {
                                points = 0;
                                Console.WriteLine("Invalid word");
                            }

                            if (wordLength == 3 || wordLength == 4)
                            {
                                points = 1;
                            }
                            else if (wordLength == 5)
                            {
                                points = 2;
                            }
                            else if (wordLength == 6)
                            {
                                points = 3;
                            }
                            else if (wordLength == 7)
                            {
                                points = 5;
                            }
                            else
                            {
                                points = 11;
                            }
                            
                            if (points > 0)
                            {
                                try
                                {
                                    using (SqlCommand command = new SqlCommand("INSERT INTO Word (Word, Points) VALUES (@Word, @Points)", _connection))
                                    {
                                        command.Parameters.AddWithValue("@Word", line.ToLower());
                                        command.Parameters.AddWithValue("@Points", points);
                                        await command.ExecuteNonQueryAsync();
                                    }
                                }
                                catch (SqlException ex)
                                {
                                    if (ex.Number == 2627) 
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
            using (SqlCommand command = new SqlCommand("SELECT WordID FROM Word WHERE Word = @Word", _connection))
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
            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Word WHERE Word = @Word", _connection))
            {
                command.Parameters.AddWithValue("@Word", word.ToLower());
                int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    
                if (count > 0) {
                    return true;
                } else {
                    return false;
                }
            }
        }
    }
}
