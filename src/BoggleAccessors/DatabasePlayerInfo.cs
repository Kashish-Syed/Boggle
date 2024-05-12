// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: DatabasePlayerInfo.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Accessor class for interating with player data in the SQL database.
// ----------------------------------------------------------------------------------------------------

using BoggleContracts;
using System;
using System.Data;
using System.Data.SqlClient;
using BoggleAccessors;
using BoggleEngines;
using NUnit.Framework.Constraints;

namespace BoggleAccessors
{
    public class DatabasePlayerInfo : IDatabasePlayerInfo
    {
        private readonly string _connectionString;
        DatabaseWordInfo dbWordInfo;
        Validation validation = new Validation();

        public DatabasePlayerInfo(string connectionString)
        {
            _connectionString = connectionString;
            dbWordInfo = new DatabaseWordInfo(connectionString);
        }

        /// <inheritdoc />
        public async Task<bool> AddPlayerAsync(string username, string password)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("INSERT INTO Player (Username, Password) VALUES (@Username, @Password)", _connection))
                {
                    command.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;
                    command.Parameters.AddWithValue("@Password",password);
                    int affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows == 1; 
                }
            }
        }

        /// <inheritdoc />
        public async Task<string?> GetUsernameAsync(int userId)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT Username FROM Player WHERE PlayerID = @PlayerID", _connection))
                {
                    command.Parameters.AddWithValue("@PlayerID", userId);
                    var result = await command.ExecuteScalarAsync();
                    if (result != null)
                        return result?.ToString();
                    else
                        throw new InvalidOperationException("No user found with the specified ID.");
                }
            }
        }

        /// <inheritdoc />
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
                    command.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;
                    command.Parameters.AddWithValue("@Password", password);
                    int affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows > 0; 
                }
            }
        }

        /// <inheritdoc />
        public async Task<int> AuthenticateAsync(string username, string password)
        {
            // client facing API, validate the inputs for security
            if (!validation.validateUsername(username) || !validation.validatePassword(password))
            {
                return -1;
            }
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT PlayerID FROM Player WHERE Username = @Username AND Password = @Password", _connection))
                {
                    // Since API is client-facing ensure extra SQL Query security
                    command.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;
                    command.Parameters.Add("@Password", SqlDbType.VarChar, 255).Value = password;

                    var result = await command.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        /// <inheritdoc />
        public async Task<DataTable> GetGamesAsync(string username)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                DataTable gamesTable = new DataTable();
                using (var command = new SqlCommand("SELECT g.GameCode, gp.TotalScore FROM GamePlayer gp JOIN Player p ON gp.PlayerID = p.PlayerID JOIN Game g ON gp.GameCode = g.GameCode WHERE p.Username = @Username", _connection))
                {
                    command.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        gamesTable.Load(reader);
                    }
                }
                return gamesTable;
            }
        }

        /// <inheritdoc />
        public async Task<DataTable> GetWordsPlayedAsync(string gameCode, string username)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                DataTable wordsTable = new DataTable();
                using (var command = new SqlCommand("SELECT w.Word, w.Points FROM GameWord gw JOIN Player p ON gw.PlayerID = p.PlayerID JOIN Word w ON gw.WordID = w.WordID WHERE gw.GameCode = @GameCode AND p.Username = @Username", _connection))
                {
                    command.Parameters.AddWithValue("@GameCode", gameCode);
                    command.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        wordsTable.Load(reader);
                    }
                }
                return wordsTable;
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task<int> GetPlayerIdAsync(string username)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT PlayerID FROM Player WHERE Username = @Username", _connection))
                {
                    command.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;
                    var result = await command.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
    }
}
