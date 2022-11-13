using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using KursovaWPF.Helpers;
namespace KursovaWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для OperatorsPage.xaml
    /// </summary>
    public partial class OperatorsPage : Page
    {
        string EditId;
        List<string> operatorsTypes = new List<string>();
        public OperatorsPage()
        {
            InitializeComponent();
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTable();
            FillComboBoxOperatorsTypes(ComboBoxType);
        }
        //-------------------------
        //Основні операції (Вставка, редагування, видалення)
        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection connection = DataBase.Connection;
                SqlCommand command = new SqlCommand($"UPDATE Operators SET Operator = '{TextBoxUpdateOperator.Text}',Operator_name = '{TextBoxUpdateName.Text}', Type_id = {ComboBoxUpdateType.SelectedIndex + 1} WHERE Operator_id = {EditId}", connection);
                command.ExecuteNonQuery();
                command.CommandText = $"UPDATE Examples SET Example = '{TextBoxUpdateExample.Text}',Description = '{TextBoxUpdateDesctiption.Text}' WHERE Example_id = (SELECT Example_id FROM Operators WHERE Operator_id = {EditId})";
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Виникла помилка!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            InsertForm.Visibility = Visibility.Visible;
            UpdateForm.Visibility = Visibility.Hidden;
            EditId = "";
            LoadTable();
        }
        private void ButtonInsert_Click(object sender, RoutedEventArgs e)
        {
            string Name = TextBoxName.Text;
            string Operator = TextBoxOperator.Text;
            string Description = TextBoxDesctiption.Text;
            int TypeId = ComboBoxType.SelectedIndex + 1;
            string Example = TextBoxExample.Text;
            if (Name != "" && Description != "" && Example != "" && TypeId != -1 && Operator != "")
            {
                try
                {
                    SqlConnection connection = DataBase.Connection;
                    SqlCommand command = new SqlCommand($"INSERT INTO Examples (Example,Description) VALUES ('{Description}','{Example}')", connection);
                    command.ExecuteNonQuery();
                    command.CommandText = $"INSERT INTO Operators (Operator,Operator_name,Type_id,Example_id) VALUES ('{Name}','{Operator}',{TypeId},(SELECT max(Example_id) FROM Examples))";
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
            TextBoxName.Clear();
            TextBoxDesctiption.Clear();
            TextBoxExample.Clear();
            ComboBoxType.SelectedIndex = -1;
            TextBoxOperator.Clear();
            LoadTable();
        }
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                SqlConnection connection = DataBase.Connection;
                int Operator_id = Convert.ToInt32(dataRowView[0].ToString());
                int Example_id = 0;
                SqlCommand command = new SqlCommand($"SELECT Example_id FROM Operators WHERE Operator_id = {Operator_id}", connection);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    Example_id = reader.GetInt32(0);
                reader.Close();

                command.CommandText =  $"DELETE FROM Operators WHERE Operator_id = {Operator_id}";
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
        //-------------------------
        //Методи для відкриття вікон і тп.
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            
            InsertForm.Visibility = Visibility.Hidden;
            UpdateForm.Visibility = Visibility.Visible;
            DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
            if (dataRowView[0].ToString() != EditId)
            {
                FillComboBoxOperatorsTypes(ComboBoxUpdateType);
                EditId = dataRowView[0].ToString();
                TextBoxUpdateOperator.Text = dataRowView[1].ToString();
                TextBoxUpdateName.Text = dataRowView[2].ToString();
                ComboBoxUpdateType.SelectedItem = dataRowView[3].ToString();
                TextBoxUpdateExample.Text = dataRowView[4].ToString();
                TextBoxUpdateDesctiption.Text = dataRowView[5].ToString();
            }
        }
        private void ButtonCansel_Click(object sender, RoutedEventArgs e)
        {
            InsertForm.Visibility = Visibility.Visible;
            UpdateForm.Visibility = Visibility.Hidden;
        }
        //-------------------------
        //Додаткові методи
        void FillComboBoxOperatorsTypes(ComboBox box)
        {
            SqlConnection connection = DataBase.Connection;
            SqlCommand com = new SqlCommand("SELECT Type FROM TypesOfOperators", connection);
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                box.Items.Add(read[0].ToString());
            }
        }
        void LoadTable()
        {
            SqlConnection connection = DataBase.Connection;
            SqlCommand com = new SqlCommand("SELECT Operators.Operator_id,Operators.Operator,Operators.Operator_name,TypesOfOperators.Type,Examples.Example,Examples.Description FROM Operators" +
                " JOIN Examples ON Operators.Example_id = Examples.Example_id" +
                " JOIN TypesOfOperators ON Operators.Type_id = TypesOfOperators.Type_id", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            adapter.Fill(dt);
            com.Dispose();
            adapter.Dispose();
            TableOperators.ItemsSource = dt.DefaultView;
        }
    }
}
