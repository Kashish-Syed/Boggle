// ----------------------------------------------------------------------------------------------------
// Project: Boggle
// Class: DatabaseGameInfo.cs
// GitHub: https://github.com/Kashish-Syed/Boggle
//
// Description: Accessor class for interating with Game data in the SQL database.
// ----------------------------------------------------------------------------------------------------

using BoggleContracts;
using BoggleEngines;
using System.Data.SqlClient;

namespace BoggleAccessors
{
    public class DatabaseGameInfo : IDatabaseGameInfo
    {
        private readonly string _connectionString;
        GameCodeGenerator generator = new GameCodeGenerator();
        public DatabaseGameInfo(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        public async Task<string> CreateGameAsync()
        {
            var dice = new GameDice();
            var gameCode = generator.GenerateGameCode();
            var board = dice.ShuffleAllDice();
            var boardString = new string(board);

            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("INSERT INTO Game (GameCode, Board) VALUES (@GameCode, @Board)", _connection))
                {
                    command.Parameters.AddWithValue("@GameCode", gameCode);
                    command.Parameters.AddWithValue("@Board", boardString);
                    await command.ExecuteNonQueryAsync();
                }
            }
            return gameCode;
        }

        /// <inheritdoc />
        public async Task<int> DeleteGameAsync(string gameCode)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("DELETE FROM GameWord WHERE GameCode = @GameCode; DELETE FROM GamePlayer WHERE GameCode = @GameCode; DELETE FROM Game WHERE GameCode = @GameCode;", _connection))
                {
                    command.Parameters.AddWithValue("@GameCode", gameCode);
                    return await command.ExecuteNonQueryAsync();  // This executes all deletes in one command.
                }
            }
        }

        /// <inheritdoc />
        public async Task<char[]?> GetBoardAsync(string gameCode)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT Board FROM Game WHERE GameCode = @GameCode", _connection))
                {
                    command.Parameters.AddWithValue("@GameCode", gameCode);
                    var boardString = await command.ExecuteScalarAsync() as string;
                    return boardString?.ToCharArray();
                }
            }
        }

        /// <inheritdoc />
        public async Task AddPlayerAsync(string gameCode, string username)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand(
                    @"DECLARE @PlayerID INT;
                    SELECT @PlayerID = PlayerID FROM Player WHERE Username = @Username;
                    IF @PlayerID IS NOT NULL
                    BEGIN
                        INSERT INTO GamePlayer (GameCode, PlayerID) VALUES (@GameCode, @PlayerID);
                    END", _connection))
                {
                    command.Parameters.AddWithValue("@GameCode", gameCode);
                    command.Parameters.AddWithValue("@Username", username);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task<string?> GetWinnerAsync(string gameCode)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand(
                    @"SELECT TOP 1 p.Username
                    FROM GamePlayer gp
                    JOIN Player p ON gp.PlayerID = p.PlayerID
                    WHERE gp.GameCode = @GameCode
                    ORDER BY gp.TotalScore DESC", _connection))
                {
                    command.Parameters.AddWithValue("@GameCode", gameCode);
                    return await command.ExecuteScalarAsync() as string;
                }
            }
        }
    }
}
