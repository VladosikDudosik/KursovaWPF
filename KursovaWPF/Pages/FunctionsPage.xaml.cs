using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using KursovaWPF.Helpers;
namespace KursovaWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для FunctionsPage.xaml
    /// </summary>
    public partial class FunctionsPage : Page
    {
        string EditId = "";
        public FunctionsPage()
        {
            InitializeComponent();
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void ButtonInsert_Click(object sender, RoutedEventArgs e)
        {
            string Function = TextBoxFunction.Text;
            string Description = TextBoxDesctiption.Text;
            string Example = TextBoxExample.Text;
            if (Function != "" && Description != "" && Example != "")
            {
                try
                {
                    SqlConnection connection = DataBase.Connection;
                    SqlCommand command = new SqlCommand($"INSERT INTO Examples (Example,Description) VALUES ('{Example}','{Description}')", connection);
                    command.ExecuteNonQuery();
                    command.CommandText = $"INSERT INTO Functions ([Function],Example_id) VALUES ('{Function}',(SELECT max(Example_id) FROM Examples))";
                    command.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Виникла помилка!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Заповніть всі поля!", "Увага", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            TextBoxFunction.Clear();
            TextBoxDesctiption.Clear();
            TextBoxExample.Clear();
            LoadTable();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                SqlConnection connection = DataBase.Connection;
                int Functions_id = Convert.ToInt32(dataRowView[0].ToString());
                int Example_id = 0;
                SqlCommand command = new SqlCommand($"SELECT Example_id FROM Functions WHERE Function_id = {Functions_id}", connection);
                
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                    Example_id = reader.GetInt32(0);
                reader.Close();
                command.CommandText = $"DELETE FROM Functions WHERE Function_id = {Functions_id}";
                command.ExecuteNonQuery();

                command.CommandText = $"DELETE FROM Examples " +
                     $"WHERE Example_id = {Example_id}";
                command.ExecuteNonQuery();

                
                
                LoadTable();
            }
            catch
            {
                MessageBox.Show("Виникла помилка!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection connection = DataBase.Connection;
                SqlCommand command = new SqlCommand($"UPDATE Functions SET [Function] = '{TextBoxUpdateFunction.Text}' WHERE Function_id = {EditId}", connection);
                command.ExecuteNonQuery();
                command.CommandText = $"UPDATE Examples SET Example = '{TextBoxUpdateExample.Text}',Description = '{TextBoxUpdateDesctiption.Text}' WHERE Example_id = (SELECT Example_id FROM Functions WHERE Function_id = {EditId})";
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Виникла помилка!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            InsertForm.Visibility = Visibility.Visible;
            UpdateForm.Visibility = Visibility.Hidden;
            LoadTable();
        }
        //---------------------
        private void ButtonCansel_Click(object sender, RoutedEventArgs e)
        {
            InsertForm.Visibility = Visibility.Visible;
            UpdateForm.Visibility = Visibility.Hidden;
        }
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            InsertForm.Visibility = Visibility.Hidden;
            UpdateForm.Visibility = Visibility.Visible;
            DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
            EditId = dataRowView[0].ToString();
            TextBoxUpdateFunction.Text = dataRowView[1].ToString();
            TextBoxUpdateExample.Text = dataRowView[2].ToString();
            TextBoxUpdateDesctiption.Text = dataRowView[3].ToString();
        }
        //---------------------
        void LoadTable()
        {
            SqlConnection connection = DataBase.Connection;
            SqlCommand com = new SqlCommand("SELECT Functions.Function_id,Functions.[Function],Examples.Example,Examples.Description FROM Functions" +
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
