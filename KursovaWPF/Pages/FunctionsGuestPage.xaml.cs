using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using KursovaWPF.Helpers;
namespace KursovaWPF.Pages
{
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
        private void ImageRefresh_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBoxSearch.Clear();
            LoadTable();
        }
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxSearch.Text == "")
            {
                MessageBox.Show("Введіть назву функції", "Незаповнене поле", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SqlConnection connection = DataBase.Connection;
            SqlCommand com = new SqlCommand("SELECT Functions.[Function] as 'Функція',Examples.Example as 'Приклад',Examples.Description as 'Опис'FROM Functions" +
               $" JOIN Examples ON Functions.Example_id = Examples.Example_id WHERE Functions.[Function] LIKE ('%{TextBoxSearch.Text}%')", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            com.Dispose();
            adapter.Dispose();
            TableFunctions.ItemsSource = dt.DefaultView;
        }
    }
}