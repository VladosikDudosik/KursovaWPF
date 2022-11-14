using System.Data.SqlClient;
namespace KursovaWPF.Helpers
{
    public static class DataBase
    {
        static string connectionString = @"Data Source=DESKTOP-SU7SNM7\SQLEXPRESS01;Initial Catalog=KursovaDB;Integrated Security=True";
        static public SqlConnection Connection = new SqlConnection(connectionString);
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
        static public SqlDataReader Select(string command)
        {
            SqlCommand sqlCommand = new SqlCommand(command,Connection);
            SqlDataReader res = sqlCommand.ExecuteReader();
            return res;
        }
    }
}