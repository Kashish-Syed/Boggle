using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using BoggleContracts;

namespace BoggleAccessors
{
    public class DatabaseWordInfo : IDatabaseWordInfo
    {
        private readonly string _connectionString = @"Server=localhost\SQLEXPRESS;Database=boggle;Trusted_Connection=True;";

        public void AddWordsToDatabase(string filepath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    using (var connection = new SqlConnection(_connectionString))
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
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to add words to the database: {ex.Message}", ex);
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

        public int GetWordID(string word)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT WordID FROM Word WHERE Word = @Word", connection))
                {
                    command.Parameters.AddWithValue("@Word", word);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
    }
}
