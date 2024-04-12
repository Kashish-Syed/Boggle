using System;
using System.Data;
using System.Data.SqlClient;

public interface ISQLConnection
{
    void Connect();
    DataTable ExecuteQuery(string query);
    void Close();
}