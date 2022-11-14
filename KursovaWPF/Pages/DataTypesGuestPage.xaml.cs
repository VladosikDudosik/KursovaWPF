using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
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
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxSearch.Text == "")
            {
                MessageBox.Show("Введіть тип даних", "Незаповнене поле", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SqlConnection connection = DataBase.Connection;
            SqlCommand com = new SqlCommand("SELECT DataTypes.DataType as 'Тип даних',Examples.Example as 'Приклад',Examples.Description as 'Опис' FROM DataTypes" +
               $" JOIN Examples ON DataTypes.Example_id = Examples.Example_id WHERE DataTypes.DataType LIKE ('%{TextBoxSearch.Text}%')", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            com.Dispose();
            adapter.Dispose();
            TableDataTypes.ItemsSource = dt.DefaultView;
        }
        private void ImageRefresh_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBoxSearch.Clear();
            LoadTable();
        }
    }
}
