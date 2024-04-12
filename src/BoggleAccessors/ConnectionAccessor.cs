using System;
using System.Data.SqlClient;

public class ConnectionAccessor : ISQLConnection
{
    private string connectionString;
    private SqlConnection connection;

    public SQLConnection()
    {
        connectionString = "Server=NUGWIN-LAPTOP\\SQLEXPRESS;Initial Catalog=Boggle;Integrated Security=True";
        connection = new SqlConnection(connectionString);
    }

    public void Connect()
    {
        try
        {
            connection.Open();
            Console.WriteLine("Connected to SQL Server");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error connection to SQL server: " + e.Message);
        }
    }

    public DataTable ExecuteQuery(string query)
    {
        DataTable dataTable = new DataTable();
        try
        {
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error executing query: " + e.Message);
        }
        return dataTable;
    }

    public void Close()
    {
        try
        {
            connection.Close();
            Console.WriteLine("Connection to SQL Server closed");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error closing connection: " + e.Message);
        }
    }
}