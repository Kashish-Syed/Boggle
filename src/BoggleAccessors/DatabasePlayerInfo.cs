using BoggleContracts;
using System;
using System.Data;
using System.Data.SqlClient;
using BoggleAccessors;

namespace BoggleAccessors
{
    public class DatabasePlayerInfo : IDatabasePlayerInfo
    {
        private readonly string _connectionString;
        DatabaseWordInfo dbWordInfo;

        public DatabasePlayerInfo(string connectionString)
        {
            _connectionString = connectionString;
            dbWordInfo = new DatabaseWordInfo(connectionString);
        }

        public async Task<bool> AddPlayerAsync(string username, string password)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("INSERT INTO Player (Username, Password) VALUES (@Username, @Password)", _connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    int affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows == 1; 
                }
            }
        }

        public async Task<string> GetUsernameAsync(int userId)
        {
            using (var _connection = new SqlConnection(_connectionString))
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
        }

        public async Task<bool> RemovePlayerAsync(string username, string password)
        {
            int userId = await AuthenticateAsync(username, password);
            if (userId == -1)
                return false; 

            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("DELETE FROM Player WHERE Username = @Username AND Password = @Password", _connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    int affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows > 0; 
                }
            }
        }

        public async Task<int> AuthenticateAsync(string username, string password)
        {
            using (var _connection = new SqlConnection(_connectionString))
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
        }

        public async Task<DataTable> GetGamesAsync(string username)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                DataTable gamesTable = new DataTable();
                using (var command = new SqlCommand("SELECT g.GameCode, gp.TotalScore FROM GamePlayer gp JOIN Player p ON gp.PlayerID = p.PlayerID JOIN Game g ON gp.GameCode = g.GameCode WHERE p.Username = @Username", _connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        gamesTable.Load(reader);
                    }
                }
                return gamesTable;
            }
        }

        public async Task<DataTable> GetWordsPlayedAsync(string gameCode, string username)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                DataTable wordsTable = new DataTable();
                using (var command = new SqlCommand("SELECT w.Word, w.Points FROM GameWord gw JOIN Player p ON gw.PlayerID = p.PlayerID JOIN Word w ON gw.WordID = w.WordID WHERE gw.GameCode = @GameCode AND p.Username = @Username", _connection))
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

        public async Task<bool> AddWordPlayedAsync(string gameCode, string username, string word)
        {
            int playerId = await GetPlayerIdAsync(username);
            if (playerId == -1)
                return false;

            int wordId = await dbWordInfo.GetWordIDAsync(word);
            if (wordId == -1)
                return false;

            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES (@GameCode, @PlayerID, @WordID)", _connection))
                {
                    command.Parameters.AddWithValue("@GameCode", gameCode);
                    command.Parameters.AddWithValue("@PlayerID", playerId);
                    command.Parameters.AddWithValue("@WordID", wordId);
                    int affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows == 1;
                }
            }
        }

        public async Task<int> GetPlayerIdAsync(string username)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT PlayerID FROM Player WHERE Username = @Username", _connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    var result = await command.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
    }
}
