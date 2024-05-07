using System;
using System.Data;
using System.Data.SqlClient;
using BoggleContracts;
using BoggleEngines;
using System.Linq;

namespace BoggleAccessors
{
    public class DatabaseGameInfo : IDatabaseGameInfo
    {
        private readonly string _connectionString = @"Server=localhost\SQLEXPRESS;Database=boggle;Trusted_Connection=True;";

        public string CreateGame()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var dice = new GameDice();
                var gameCode = GenerateGameCode();
                var board = dice.ShuffleAllDice();
                var boardString = new string(board);

                using (var command = new SqlCommand("INSERT INTO Game (GameCode, Board) VALUES (@GameCode, @Board)", connection))
                {
                    command.Parameters.AddWithValue("@GameCode", gameCode);
                    command.Parameters.AddWithValue("@Board", boardString);
                    command.ExecuteNonQuery();
                }

                return gameCode;
            }
        }

        public int DeleteGame(string gameCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM GameWord WHERE GameCode = @GameCode; DELETE FROM GamePlayer WHERE GameCode = @GameCode; DELETE FROM Game WHERE GameCode = @GameCode;", connection))
                {
                    command.Parameters.AddWithValue("@GameCode", gameCode);
                    return command.ExecuteNonQuery();  // This executes all deletes in one command.
                }
            }
        }

        public char[] GetBoard(string gameCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT Board FROM Game WHERE GameCode = @GameCode", connection))
                {
                    command.Parameters.AddWithValue("@GameCode", gameCode);
                    var boardString = command.ExecuteScalar() as string;
                    return boardString?.ToCharArray();
                }
            }
        }

        public void AddPlayer(string gameCode, string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    @"DECLARE @PlayerID INT;
                    SELECT @PlayerID = PlayerID FROM Player WHERE Username = @Username;
                    IF @PlayerID IS NOT NULL
                    BEGIN
                        INSERT INTO GamePlayer (GameCode, PlayerID) VALUES (@GameCode, @PlayerID);
                    END", connection))
                {
                    command.Parameters.AddWithValue("@GameCode", gameCode);
                    command.Parameters.AddWithValue("@Username", username);
                    command.ExecuteNonQuery();
                }
            }
        }

        public string GetWinner(string gameCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    @"SELECT TOP 1 p.Username
                    FROM GamePlayer gp
                    JOIN Player p ON gp.PlayerID = p.PlayerID
                    WHERE gp.GameCode = @GameCode
                    ORDER BY gp.TotalScore DESC", connection))
                {
                    command.Parameters.AddWithValue("@GameCode", gameCode);
                    return command.ExecuteScalar() as string;
                }
            }
        }

        private string GenerateGameCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
