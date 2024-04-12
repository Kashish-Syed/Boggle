using System.Data.SqlClient;

namespace BoggleAccessors
{
    internal class ConnectionAccessor
    {
        public void TestDatabaseConnection()
        {
            SqlConnection sqlConn = null;
            SqlDataReader sqlDr = null;

            try
            {
                // Open a connection to SQL Server
                sqlConn = new SqlConnection("Server=NUGWIN-LAPTOP\\SQLEXPRESS;Initial Catalog=Boggle;Integrated Security=True");
                sqlConn.Open();
                Console.WriteLine("Connection successful!");
                sqlConn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection unsuccessful!" + e.Message);
            }
        }
    }
}