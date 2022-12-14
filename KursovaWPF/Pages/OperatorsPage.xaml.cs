using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using KursovaWPF.Helpers;
namespace KursovaWPF.Pages
{
    public partial class OperatorsPage : Page
    {
        SqlConnection connection = DataBase.GetConnection();
        string EditId;
        public OperatorsPage()
        {
            InitializeComponent();
        }

        //--------------------------------------------------|
        //Основні операції (Вставка, редагування, видалення)|
        //--------------------------------------------------|
        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                    SqlCommand command = new SqlCommand($"SELECT [Operator] FROM Operators WHERE [Operator] = '{Operator}'", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        MessageBox.Show("Такий оператор уже існує!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    reader.Close();
                    command.CommandText = $"INSERT INTO Examples (Example,Description) VALUES ('{Example}','{Description}')";
                    command.ExecuteNonQuery();
                    command.CommandText = $"INSERT INTO Operators (Operator,Operator_name,Type_id,Example_id) VALUES ('{Operator}','{Name}',{TypeId},(SELECT max(Example_id) FROM Examples))";
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
            if (MessageBox.Show("Видалити цей запис?","Підтвердження видалення",MessageBoxButton.YesNo,MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            try
            {
                DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
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
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxSearch.SelectedIndex< 2 && TextBoxSearch.Text == "")
            {
                MessageBox.Show("Заповніть поле пошуку", "Незаповнене поле", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }else if (ComboBoxSearch.SelectedIndex == 2 && ComboBoxSearchOperators.SelectedIndex == -1)
            {
                MessageBox.Show("Оберіть тип", "Незаповнене поле", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } 
            
            SqlCommand com;
            switch (ComboBoxSearch.SelectedIndex)
            {
                case 0:
                    com = new SqlCommand("SELECT Operators.Operator_id,Operators.Operator,Operators.Operator_name,TypesOfOperators.Type,Examples.Example,Examples.Description FROM Operators" +
                           " JOIN Examples ON Operators.Example_id = Examples.Example_id" +
                           $" JOIN TypesOfOperators ON Operators.Type_id = TypesOfOperators.Type_id WHERE Operators.Operator LIKE ('%{TextBoxSearch.Text}%')", connection);
                    break;
                case 1:
                    com = new SqlCommand("SELECT Operators.Operator_id,Operators.Operator,Operators.Operator_name,TypesOfOperators.Type,Examples.Example,Examples.Description FROM Operators" +
                           " JOIN Examples ON Operators.Example_id = Examples.Example_id" +
                           $" JOIN TypesOfOperators ON Operators.Type_id = TypesOfOperators.Type_id WHERE Operators.Operator_name LIKE ('%{TextBoxSearch.Text}%')", connection);
                    break;
                case 2:
                    com = new SqlCommand("SELECT Operators.Operator_id,Operators.Operator,Operators.Operator_name,TypesOfOperators.Type,Examples.Example,Examples.Description FROM Operators" +
                           " JOIN Examples ON Operators.Example_id = Examples.Example_id" +
                           $" JOIN TypesOfOperators ON Operators.Type_id = TypesOfOperators.Type_id WHERE TypesOfOperators.Type LIKE ('%{ComboBoxSearchOperators.SelectedItem.ToString()}%')", connection);
                    break;
                default:
                    MessageBox.Show("Помилка пошуку.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            adapter.Fill(dt);
            com.Dispose();
            adapter.Dispose();
            TableOperators.ItemsSource = dt.DefaultView;
        }
        //--------------------------------------------------|
        //Події для різних кнопок та вікон.                 |
        //--------------------------------------------------|
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
        private void ImageRefresh_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBoxSearch.Clear();
            LoadTable();
        }
        private void ComboBoxSearch_DropDownClosed(object sender, EventArgs e)
        {
            if (ComboBoxSearch.SelectedIndex == 2)
            {
                FillComboBoxOperatorsTypes(ComboBoxSearchOperators);
                ComboBoxSearchOperators.Visibility = Visibility.Visible;
                TextBoxSearch.Visibility = Visibility.Hidden;
            }
            else
            {
                ComboBoxSearchOperators.Visibility = Visibility.Hidden;
                TextBoxSearch.Visibility = Visibility.Visible;
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTable();
            FillComboBoxOperatorsTypes(ComboBoxType);
        }
        //--------------------------------------------------|
        //Додаткові методи                                  |
        //--------------------------------------------------|
        void FillComboBoxOperatorsTypes(ComboBox box)
        {
            box.Items.Clear();
            SqlCommand com = new SqlCommand("SELECT Type FROM TypesOfOperators", connection);
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                box.Items.Add(read[0].ToString());
            }
            read.Close();
        }
        void LoadTable()
        {
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
        //--------------------------------------------------
    }
}
