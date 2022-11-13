using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;
using KursovaWPF.Helpers;
namespace KursovaWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для DataTypesGuestPage.xaml
    /// </summary>
    public partial class DataTypesGuestPage : Page
    {
        public DataTypesGuestPage()
        {
            InitializeComponent();
        }
        private void Page_Initialized(object sender, EventArgs e)
        {
            LoadTable();
        }
        void LoadTable()
        {
            SqlConnection connection = DataBase.Connection;
            SqlCommand com = new SqlCommand("SELECT DataTypes.DataType as 'Тип даних',Examples.Example as 'Приклад',Examples.Description as 'Опис' FROM DataTypes" +
                " JOIN Examples ON DataTypes.Example_id = Examples.Example_id", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            adapter.Fill(dt);
            com.Dispose();
            adapter.Dispose();
            TableDataTypes.ItemsSource = dt.DefaultView;
        }
    }
}
