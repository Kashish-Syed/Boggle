using System;
using System.Data;
using System.Data.SqlClient;
using BoggleContracts;
using BoggleEngines;

namespace BoggleAccessors
{
    public class DatabaseGameInfo : IDatabaseGameInfo
    {
        private readonly string _connectionString = @"Server=localhost\SQLEXPRESS;Database=boggle;Trusted_Connection=True;";
        private SqlConnection _connection;

        public DatabaseGameInfo()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public string CreateGame()
        {
            IGameDice dice = new GameDice();
            var gameCode = GenerateGameCode();
            var board = dice.ShuffleAllDice();

            var boardString = new string(board); 

            using (SqlCommand command = new SqlCommand("INSERT INTO Game (GameCode, Board) VALUES (@GameCode, @Board)", _connection))
            {
                command.Parameters.AddWithValue("@GameCode", gameCode);
                command.Parameters.AddWithValue("@Board", boardString);
                command.ExecuteNonQuery();
            }

            return gameCode;
        }

        public int DeleteGame(string gameCode)
        {
            using (SqlCommand command = new SqlCommand("DELETE FROM GameWord WHERE GameCode = @GameCode", _connection))
            {
                command.Parameters.AddWithValue("@GameCode", gameCode);
                command.ExecuteNonQuery(); 
            }

            using (SqlCommand command = new SqlCommand("DELETE FROM GamePlayer WHERE GameCode = @GameCode", _connection))
            {
                command.Parameters.AddWithValue("@GameCode", gameCode);
                command.ExecuteNonQuery();
            }

            using (SqlCommand command = new SqlCommand("DELETE FROM Game WHERE GameCode = @GameCode", _connection))
            {
                command.Parameters.AddWithValue("@GameCode", gameCode);
                return command.ExecuteNonQuery();
            }
        }


        public char[] GetBoard(string gameCode)
        {
            using (SqlCommand command = new SqlCommand("SELECT Board FROM Game WHERE GameCode = @GameCode", _connection))
            {
                command.Parameters.AddWithValue("@GameCode", gameCode);
                var boardString = command.ExecuteScalar() as string;
                return boardString?.ToCharArray();
            }
        }

        private string GenerateGameCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        ~DatabaseGameInfo()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
