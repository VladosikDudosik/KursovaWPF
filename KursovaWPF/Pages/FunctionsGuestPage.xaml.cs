using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;
using KursovaWPF.Helpers;
namespace KursovaWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для FunctionsGuestPage.xaml
    /// </summary>
    public partial class FunctionsGuestPage : Page
    {
        public FunctionsGuestPage()
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
            SqlCommand com = new SqlCommand("SELECT Functions.[Function] as 'Функція',Examples.Example as 'Приклад',Examples.Description as 'Опис'FROM Functions" +
                " JOIN Examples ON Functions.Example_id = Examples.Example_id", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            adapter.Fill(dt);
            com.Dispose();
            adapter.Dispose();
            TableFunctions.ItemsSource = dt.DefaultView;
        }
    }
}
