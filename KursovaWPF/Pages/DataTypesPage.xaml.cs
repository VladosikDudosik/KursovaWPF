using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using KursovaWPF.Helpers;
namespace KursovaWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для DataTypesPage.xaml
    /// </summary>
    public partial class DataTypesPage : Page
    {
        string EditId = "";
        public DataTypesPage()
        {
            InitializeComponent();
        }
        //--------------------------------------------------|
        //Основні операції (Вставка, редагування, видалення)|
        //--------------------------------------------------|
        private void ButtonInsert_Click(object sender, RoutedEventArgs e)
        {
            string DataType = TextBoxDataType.Text;
            string Description = TextBoxDesctiption.Text;
            string Example = TextBoxExample.Text;
            if (DataType != "" && Description != "" && Example != "")
            {
                try
                {
                    SqlConnection connection = DataBase.Connection;
                    SqlCommand command = new SqlCommand($"INSERT INTO Examples (Example,Description) VALUES ('{Example}','{Description}')", connection);
                    command.ExecuteNonQuery();
                    command.CommandText = $"INSERT INTO DataTypes (DataType,Example_id) VALUES ('{DataType}',(SELECT max(Example_id) FROM Examples))";
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
            TextBoxDataType.Clear();
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
                int DataType_id = Convert.ToInt32(dataRowView[0].ToString());

                int Example_id = 0;
                SqlCommand command = new SqlCommand($"SELECT Example_id FROM DataTypes WHERE DataType_id = {DataType_id}", connection);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    Example_id = reader.GetInt32(0);
                reader.Close();
                command.CommandText = $"DELETE FROM DataTypes WHERE DataType_id = {DataType_id}";
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
                SqlCommand command = new SqlCommand($"UPDATE DataTypes SET DataType = '{TextBoxUpdateDataType.Text}' WHERE DataType_id = {EditId}", connection);
                command.ExecuteNonQuery();
                command.CommandText = $"UPDATE Examples SET Example = '{TextBoxUpdateExample.Text}',Description = '{TextBoxUpdateDesctiption.Text}' WHERE Example_id = (SELECT Example_id FROM DataTypes WHERE DataType_id = {EditId})";
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
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxSearch.Text == "")
            {
                MessageBox.Show("Введіть тип даних", "Незаповнене поле", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SqlConnection connection = DataBase.Connection;
            SqlCommand com = new SqlCommand("SELECT DataTypes.DataType_id,DataTypes.DataType,Examples.Example,Examples.Description FROM DataTypes" +
               $" JOIN Examples ON DataTypes.Example_id = Examples.Example_id WHERE DataTypes.DataType LIKE ('%{TextBoxSearch.Text}%')", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            com.Dispose();
            adapter.Dispose();
            TableDataTypes.ItemsSource = dt.DefaultView;
        }
        //--------------------------------------------------|
        //Події для різних кнопок та вікон.                 |
        //--------------------------------------------------|
        private void Page_Initialized(object sender, EventArgs e)
        {
            LoadTable();
        }
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
            TextBoxUpdateDataType.Text = dataRowView[1].ToString();
            TextBoxUpdateExample.Text = dataRowView[2].ToString();
            TextBoxUpdateDesctiption.Text = dataRowView[3].ToString();
        }
        private void ImageRefresh_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LoadTable();
        }
        //--------------------------------------------------|
        //Додаткові методи                                  |
        //--------------------------------------------------|
        void LoadTable()
        {
            SqlConnection connection = DataBase.Connection;
            SqlCommand com = new SqlCommand("SELECT DataTypes.DataType_id,DataTypes.DataType,Examples.Example,Examples.Description FROM DataTypes" +
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
