using System;
using System.Data;
using System.Data.SqlClient;

public interface ISQLConnection
{
    /// <summary>
    /// Starts a connection to the SQL database.
    /// </summary>
    void Connect();

    /// <summary>
    /// Function to query the database
    /// </summary>
    /// <param name="query">Query to be used</param>
    /// <returns>The DataTable model</returns>
    DataTable ExecuteQuery(string query);

    /// <summary>
    /// Closes the connection to the SQL database.
    /// </summary>
    void Close();
}