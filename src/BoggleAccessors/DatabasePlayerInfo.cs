using System;
using System.Data;
using System.Data.SqlClient;
using BoggleContracts;

namespace BoggleAccessors
{
    public class DatabasePlayerInfo : IDatabasePlayerInfo
    {
        private readonly string _connectionString = @"Server=localhost\SQLEXPRESS;Database=boggle;Trusted_Connection=True;";

        public void AddPlayer(string username, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO Player (Username, Password) VALUES (@Username, @Password)", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.ExecuteNonQuery();
                }
            }
        }

        public string GetUsername(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT Username FROM Player WHERE PlayerID = @PlayerID", connection))
                {
                    command.Parameters.AddWithValue("@PlayerID", userId);
                    object result = command.ExecuteScalar();
                    if (result != null)
                        return result.ToString();
                    else
                        throw new InvalidOperationException("No user found with the specified ID.");
                }
            }
        }

        public void RemovePlayer(string username, string password)
        {
            int userId = Authenticate(username, password);
            if (userId == -1)
                throw new ArgumentException("Authentication failed. Player not found.");
                
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Player WHERE Username = @Username AND Password = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.ExecuteNonQuery();
                }
            }
        }

        public int Authenticate(string username, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT PlayerID FROM Player WHERE Username = @Username AND Password = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        public DataTable GetGames(string username)
        {
            DataTable gamesTable = new DataTable();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "SELECT g.GameCode, gp.TotalScore " +
                    "FROM GamePlayer gp " +
                    "JOIN Player p ON gp.PlayerID = p.PlayerID " +
                    "JOIN Game g ON gp.GameCode = g.GameCode " +
                    "WHERE p.Username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(gamesTable);
                    }
                }
            }
            return gamesTable;
        }

        public DataTable GetWordsPlayed(string gameCode, string username)
        {
            DataTable wordsTable = new DataTable();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    "SELECT w.Word, w.Points FROM GameWord gw " +
                    "JOIN Player p ON gw.PlayerID = p.PlayerID " +
                    "JOIN Word w ON gw.WordID = w.WordID " +
                    "WHERE gw.GameCode = @GameCode AND p.Username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@GameCode", gameCode);
                    command.Parameters.AddWithValue("@Username", username);
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(wordsTable);
                    }
                }
            }
            return wordsTable;
        }
    }
}
