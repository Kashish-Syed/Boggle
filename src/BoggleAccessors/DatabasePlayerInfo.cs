using System;
using System.Data;
using System.Data.SqlClient;
using BoggleContracts;

namespace BoggleAccessors
{
    public class DatabasePlayerInfo : IDatabasePlayerInfo
    {
        private readonly SqlConnection _connection;

        public DatabasePlayerInfo(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task AddPlayerAsync(string username, string password)
        {
            await _connection.OpenAsync();
            using (var command = new SqlCommand("INSERT INTO Player (Username, Password) VALUES (@Username, @Password)", _connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<string> GetUsernameAsync(int userId)
        {
            await _connection.OpenAsync();
            using (var command = new SqlCommand("SELECT Username FROM Player WHERE PlayerID = @PlayerID", _connection))
            {
                command.Parameters.AddWithValue("@PlayerID", userId);
                var result = await command.ExecuteScalarAsync();
                if (result != null)
                    return result.ToString();
                else
                    throw new InvalidOperationException("No user found with the specified ID.");
            }
        }

        public async Task RemovePlayerAsync(string username, string password)
        {
            int userId = await AuthenticateAsync(username, password);
            if (userId == -1)
                throw new ArgumentException("Authentication failed. Player not found.");
                
            await _connection.OpenAsync();
            using (var command = new SqlCommand("DELETE FROM Player WHERE Username = @Username AND Password = @Password", _connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.ExecuteNonQuery();
            }
        }

        public async Task<int> Authenticate(string username, string password)
        {
            await _connection.OpenAsync();
            using (var command = new SqlCommand("SELECT PlayerID FROM Player WHERE Username = @Username AND Password = @Password", _connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                var result = await command.ExecuteScalarAsync();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public async Task<DataTable> GetGamesAsync(string username)
        {
            DataTable gamesTable = new DataTable();
            await _connection.OpenAsync();
            using (var command = new SqlCommand(
                "SELECT g.GameCode, gp.TotalScore " +
                "FROM GamePlayer gp " +
                "JOIN Player p ON gp.PlayerID = p.PlayerID " +
                "JOIN Game g ON gp.GameCode = g.GameCode " +
                "WHERE p.Username = @Username", _connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    gamesTable.Load(reader);
                }
            }
            return gamesTable;
        }

        public async Task<DataTable> GetWordsPlayedAsync(string gameCode, string username)
        {
            DataTable wordsTable = new DataTable();
            await _connection.OpenAsync();
            using (var command = new SqlCommand(
                "SELECT w.Word, w.Points FROM GameWord gw " +
                "JOIN Player p ON gw.PlayerID = p.PlayerID " +
                "JOIN Word w ON gw.WordID = w.WordID " +
                "WHERE gw.GameCode = @GameCode AND p.Username = @Username", _connection))
            {
                command.Parameters.AddWithValue("@GameCode", gameCode);
                command.Parameters.AddWithValue("@Username", username);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    wordsTable.Load(reader);
                }
            }
            return wordsTable;
        }
    }
}
