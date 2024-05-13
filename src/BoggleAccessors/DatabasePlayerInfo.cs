// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: DatabasePlayerInfo.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Accessor class for interating with player data in the SQL database.
// ----------------------------------------------------------------------------------------------------

using BoggleContracts;
using BoggleEngines;
using System.Data;
using System.Data.SqlClient;

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
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                try
                {
                    using (var command = new SqlCommand("DELETE FROM Player WHERE Username = @Username AND Password = @Password", _connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        int affectedRows = await command.ExecuteNonQueryAsync();
                        Console.WriteLine($"Rows affected: {affectedRows}");
                        return affectedRows > 0;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"SQL Error during RemovePlayerAsync: {ex.Message}");
                    return false;
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

                using (var transaction = _connection.BeginTransaction())
                {
                    try
                    {
                        using (var insertCommand = new SqlCommand("INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES (@GameCode, @PlayerID, @WordID)", _connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@GameCode", gameCode);
                            insertCommand.Parameters.AddWithValue("@PlayerID", playerId);
                            insertCommand.Parameters.AddWithValue("@WordID", wordId);
                            int affectedRows = await insertCommand.ExecuteNonQueryAsync();
                            if (affectedRows == 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }

                        using (var pointCommand = new SqlCommand("SELECT Points FROM Word WHERE WordID = @WordID", _connection, transaction))
                        {
                            pointCommand.Parameters.AddWithValue("@WordID", wordId);
                            object result = await pointCommand.ExecuteScalarAsync();
                            
                            if (result != null)
                            {
                                int points = Convert.ToInt32(result); 

                                using (var updateScoreCommand = new SqlCommand("UPDATE GamePlayer SET TotalScore = TotalScore + @Points WHERE GameCode = @GameCode AND PlayerID = @PlayerID", _connection, transaction))
                                {
                                    updateScoreCommand.Parameters.AddWithValue("@Points", points);
                                    updateScoreCommand.Parameters.AddWithValue("@GameCode", gameCode);
                                    updateScoreCommand.Parameters.AddWithValue("@PlayerID", playerId);
                                    await updateScoreCommand.ExecuteNonQueryAsync();
                                }
                            }
                            else
                            {
                                Console.WriteLine("No points found for the given WordID.");
                                transaction.Rollback(); 
                                return false;
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        return false;
                    }
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
