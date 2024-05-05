using System;
using System.Data;
using System.Data.SqlClient;
using BoggleContracts;

namespace BoggleAccessors
{
    public class DatabasePlayerInfo : IDatabasePlayerInfo
    {
        private readonly string _connectionString = @"Server=localhost\SQLEXPRESS;Database=boggle;Trusted_Connection=True;";
        private SqlConnection _connection;

        public DatabasePlayerInfo()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public void AddPlayer(string username, string password)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO Player (Username, Password) VALUES (@Username, @Password)", _connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("Error adding player to the database.", ex);
            }
        }

        public void RemovePlayer(string username, string password)
        {
            int userId = Authenticate(username, password);
            if (userId == -1)
            {
                throw new ArgumentException("Authentication failed. Player not found.");
            }

            try
            {
                using (SqlCommand command = new SqlCommand("DELETE FROM GameWord WHERE PlayerID = @PlayerID", _connection))
                {
                    command.Parameters.AddWithValue("@PlayerID", userId);
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand("DELETE FROM GamePlayer WHERE PlayerID = @PlayerID", _connection))
                {
                    command.Parameters.AddWithValue("@PlayerID", userId);
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand("DELETE FROM Player WHERE Username = @Username AND Password = @Password", _connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("Error removing player from the database.", ex);
            }
        }


        public int Authenticate(string username, string password)
        {
            using (SqlCommand command = new SqlCommand("SELECT PlayerID FROM Player WHERE Username = @Username AND Password = @Password", _connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public DataTable GetGames(string username)
        {
            DataTable gamesTable = new DataTable();
            using (SqlCommand command = new SqlCommand(
                "SELECT g.GameCode, gp.TotalScore " +
                "FROM GamePlayer gp " +
                "JOIN Player p ON gp.PlayerID = p.PlayerID " +
                "JOIN Game g ON gp.GameCode = g.GameCode " +
                "WHERE p.Username = @Username", _connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(gamesTable);
                }
            }
            return gamesTable;
        }



        public DataTable GetWordsPlayed(string gameCode, string username)
        {
            DataTable wordsTable = new DataTable();
            using (SqlCommand command = new SqlCommand(
                "SELECT w.Word, w.Points FROM GameWord gw " +
                "JOIN Player p ON gw.PlayerID = p.PlayerID " +
                "JOIN Word w ON gw.WordID = w.WordID " +
                "WHERE gw.GameCode = @GameCode AND p.Username = @Username", _connection))
            {
                command.Parameters.AddWithValue("@GameCode", gameCode);
                command.Parameters.AddWithValue("@Username", username);
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(wordsTable);
                }
            }
            return wordsTable;
        }


        ~DatabasePlayerInfo()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
