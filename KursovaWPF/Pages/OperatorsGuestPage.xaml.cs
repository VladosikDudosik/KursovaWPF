using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using KursovaWPF.Helpers;
namespace KursovaWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для OperatorsGuestPage.xaml
    /// </summary>
    public partial class OperatorsGuestPage : Page
    {
        public OperatorsGuestPage()
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
            SqlCommand com = new SqlCommand("SELECT Operators.Operator as 'Оператор',Operators.Operator_name as 'Назва',TypesOfOperators.Type as 'Тип',Examples.Example as 'Приклад',Examples.Description as 'Опис' FROM Operators" +
                " JOIN Examples ON Operators.Example_id = Examples.Example_id" +
                " JOIN TypesOfOperators ON Operators.Type_id = TypesOfOperators.Type_id", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            adapter.Fill(dt);
            com.Dispose();
            adapter.Dispose();
            TableOperators.ItemsSource = dt.DefaultView;
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

        private void ButtonSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ComboBoxSearch.SelectedIndex < 2 && TextBoxSearch.Text == "")
            {
                MessageBox.Show("Заповніть поле пошуку", "Незаповнене поле", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (ComboBoxSearch.SelectedIndex == 2 && ComboBoxSearchOperators.SelectedIndex == -1)
            {
                MessageBox.Show("Оберіть тип", "Незаповнене поле", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SqlConnection connection = DataBase.Connection;
            SqlCommand com;
            switch (ComboBoxSearch.SelectedIndex)
            {
                case 0:
                    com = new SqlCommand("SELECT Operators.Operator as 'Оператор',Operators.Operator_name as 'Назва',TypesOfOperators.Type as 'Тип',Examples.Example as 'Приклад',Examples.Description as 'Опис' FROM Operators" +
                           " JOIN Examples ON Operators.Example_id = Examples.Example_id" +
                           $" JOIN TypesOfOperators ON Operators.Type_id = TypesOfOperators.Type_id WHERE Operators.Operator LIKE ('%{TextBoxSearch.Text}%')", connection);
                    break;
                case 1:
                    com = new SqlCommand("SELECT Operators.Operator as 'Оператор',Operators.Operator_name as 'Назва',TypesOfOperators.Type as 'Тип',Examples.Example as 'Приклад',Examples.Description as 'Опис' FROM Operators" +
                           " JOIN Examples ON Operators.Example_id = Examples.Example_id" +
                           $" JOIN TypesOfOperators ON Operators.Type_id = TypesOfOperators.Type_id WHERE Operators.Operator_name LIKE ('%{TextBoxSearch.Text}%')", connection);
                    break;
                case 2:
                    com = new SqlCommand("SELECT Operators.Operator as 'Оператор',Operators.Operator_name as 'Назва',TypesOfOperators.Type as 'Тип',Examples.Example as 'Приклад',Examples.Description as 'Опис' FROM Operators" +
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
        void FillComboBoxOperatorsTypes(ComboBox box)
        {
            box.Items.Clear();
            SqlConnection connection = DataBase.Connection;
            SqlCommand com = new SqlCommand("SELECT Type FROM TypesOfOperators", connection);
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                box.Items.Add(read[0].ToString());
            }
            read.Close();
        }
    }
}
