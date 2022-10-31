using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursovaWPF.Models;
namespace KursovaWPF.Helpers
{
    public static class DataBase
    {
        static string connectionString = @"Data Source=DESKTOP-SU7SNM7\SQLEXPRESS01;Initial Catalog=LispDB;Integrated Security=True";
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
            //OpenConnection();
            SqlCommand sqlCommand = new SqlCommand(command,Connection);
            SqlDataReader res = sqlCommand.ExecuteReader();
            //CloseConnection();
            return res;
        }
        static public List<DataTypes> SelectAllDataTypes()
        {
            List<DataTypes> res = new List<DataTypes>();
            SqlDataReader reader = Select("SELECT DataTypes.DataType_id,DataTypes.Name,Examples.Example FROM DataTypes " +
                " JOIN Examples ON DataTypes.Example_id = Examples.Example_id;");
            while (reader.Read())
            {
                DataTypes data = new DataTypes();
                data.id = reader.GetInt32(0);
                data.Name = reader.GetString(1);
                data.Example = reader.GetString(2);
                res.Add(data);
            }
            reader.Close();
            return res;
        }
        static public List<Functions> SelectAllFunctions()
        {
            List<Functions> res = new List<Functions>();
            SqlDataReader reader = Select("SELECT Functions.Function_id,Functions.Name,Descriptions.Description,Examples.Example FROM Functions " +
                " JOIN Descriptions ON Functions.Description_id = Descriptions.Description_id" +
                " JOIN Examples ON Functions.Example_id = Examples.Example_id;");
            while (reader.Read())
            {
                Functions func = new Functions();
                func.id = reader.GetInt32(0);
                func.Name = reader.GetString(1);
                func.Description = reader.GetString(2);
                func.Example = reader.GetString(3);
                res.Add(func);
            }
            reader.Close();
            return res;
        }
        static public List<Operators> SelectAllOperators()
        {
            List<Operators> res = new List<Operators>();
            SqlDataReader reader = Select("SELECT Operators.Operator_id,Operators.Name,Descriptions.Description,Examples.Example FROM Operators" +
                " JOIN Descriptions ON Operators.Description_id = Descriptions.Description_id" +
                " JOIN Examples ON Operators.Example_id = Examples.Example_id;");
            while (reader.Read())
            {
                Operators op = new Operators();
                op.id = reader.GetInt32(0);
                op.Name = reader.GetString(1);
                op.Description = reader.GetString(2);
                op.Example = reader.GetString(3);
                res.Add(op);
            }
            reader.Close();
            return res;
        }
        static public List<Constructions> SelectAllConstructions()
        {
            List<Constructions> res = new List<Constructions>();
            SqlDataReader reader = Select("SELECT Constructions.Construction_id,Constructions.Name,Descriptions.Description,Examples.Example FROM Constructions " +
                " JOIN Descriptions ON Constructions.Description_id = Descriptions.Description_id" +
                " JOIN Examples ON Constructions.Example_id = Examples.Example_id;");
            while (reader.Read())
            {
                Constructions cons = new Constructions();
                cons.id = reader.GetInt32(0);
                cons.Name = reader.GetString(1);
                cons.Description = reader.GetString(2);
                cons.Example = reader.GetString(3);
                res.Add(cons);
            }
            reader.Close();
            return res;
        }
    }
}
