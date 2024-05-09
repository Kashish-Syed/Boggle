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

        public void AddWordsToDatabase(string filepath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    connection.Open();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (!string.IsNullOrEmpty(line) && line.All(char.IsLetter))
                        {
                            int points = CalculatePoints(line);
                            if (points > 0)
                            {
                                try
                                {
                                    using (SqlCommand command = new SqlCommand("INSERT INTO Word (Word, Points) VALUES (@Word, @Points)", connection))
                                    {
                                        command.Parameters.AddWithValue("@Word", line.ToLower());
                                        command.Parameters.AddWithValue("@Points", points);
                                        command.ExecuteNonQuery();
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

        public int GetWordID(string word)
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("SELECT WordID FROM Word WHERE Word = @Word", connection))
            {
                command.Parameters.AddWithValue("@Word", word);
                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public bool IsValidWord(string word)
        {
            word = word.ToLower();
            connection.Open();
            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Word WHERE Word = @Word", connection))
            {
                command.Parameters.AddWithValue("@Word", word.ToLower());
                int count = Convert.ToInt32(command.ExecuteScalar());
                    
                if (count > 0) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        private int CalculatePoints(string word)
        {
            int wordLength = word.Length;
            if (wordLength <= 2) return 0;
            if (wordLength == 3 || wordLength == 4) return 1;
            if (wordLength == 5) return 2;
            if (wordLength == 6) return 3;
            if (wordLength == 7) return 5;
            return 11;  // 8 or more letters
        }
    }
}
