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
        private SqlConnection _connection;

        public DatabaseWordInfo()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public void AddWordsToDatabase(string filepath)
{
    try
    {
        using (StreamReader sr = new StreamReader(filepath))
        {
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
                            using (SqlCommand command = new SqlCommand("INSERT INTO Word (Word, Points) VALUES (@Word, @Points)", _connection))
                            {
                                command.Parameters.AddWithValue("@Word", line.ToLower());
                                command.Parameters.AddWithValue("@Points", points);
                                command.ExecuteNonQuery();
                            }
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 2627) // 2627 is the SQL error code for a duplicate primary key value, need so the program doesn't crash on repeat words
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


        private int CalculatePoints(string word)
        {
            int wordLength = word.Length;
            if (wordLength <= 2)
            {
                return 0; 
            }
            if (wordLength == 3 || wordLength == 4)
            {
                return 1;
            }
            else if (wordLength == 5)
            {
                return 2;
            }
            else if (wordLength == 6)
            {
                return 3;
            }
            else if (wordLength == 7)
            {
                return 5;
            }
            else
            {
                return 11; 
            }
        }

        public int GetWordID(string word)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("SELECT WordID FROM Word WHERE Word = @Word", _connection))
                {
                    command.Parameters.AddWithValue("@Word", word);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException($"Failed to retrieve Word ID: {ex.Message}", ex);
            }
        }

        ~DatabaseWordInfo()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
