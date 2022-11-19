using System.Data.SqlClient;
namespace KursovaWPF.Helpers
{
    public static class DataBase
    {
        static string connectionString = @"Data Source=DESKTOP-SU7SNM7\SQLEXPRESS01;Initial Catalog=KursovaDB;Integrated Security=True";
        static SqlConnection Connection = new SqlConnection(connectionString);
        static public SqlConnection GetConnection()
        {
            return Connection;
        }
        static public bool OpenConnection()
        {
            try
            {
                Connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
        static public bool CloseConnection()
        {
            try
            {
                Connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}