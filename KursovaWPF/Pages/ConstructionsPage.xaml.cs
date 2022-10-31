using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KursovaWPF.Helpers;
using KursovaWPF.Models;
using KursovaWPF.Windows;
namespace KursovaWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для ConstructionsPage.xaml
    /// </summary>
    public partial class ConstructionsPage : Page
    {
        public ConstructionsPage()
        {
            InitializeComponent();
        }
        private void Page_Initialized(object sender, EventArgs e)
        {
            LoadTable();
        }
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                SqlConnection connection = DataBase.Connection;
                int id = Convert.ToInt32(dataRowView[0].ToString());

                SqlCommand command = new SqlCommand($"DELETE FROM Constructions WHERE Construction_id = {id}", connection);
                command.ExecuteNonQuery();
                command.CommandText = $"DELETE FROM Examples " +
                    $"WHERE Example_id = (SELECT Example_id FROM Constructions WHERE Construction_id = {id})";
                command.ExecuteNonQuery();
                command.CommandText = $"DELETE FROM Descriptions " +
                    $"WHERE Description_id = (SELECT Description_id FROM Constructions WHERE Construction_id = {id})";
                command.ExecuteNonQuery();
                LoadTable();
            }
            catch
            {
                MessageBox.Show("Виникла помилка!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonInsert_Click(object sender, RoutedEventArgs e)
        {
            
            string Name = TextBoxName.Text;
            string Description = TextBoxDesctiption.Text;
            string Example = TextBoxExample.Text;
            if (Name != "" && Description != "" && Example != "")
            {
                try
                {
                    SqlConnection connection = DataBase.Connection;
                    SqlCommand command = new SqlCommand($"INSERT INTO Descriptions VALUES ('{Description}')", connection);
                    command.ExecuteNonQuery();
                    command.CommandText = $"INSERT INTO Examples VALUES ('{Example}')";
                    command.ExecuteNonQuery();
                    command.CommandText = $"INSERT INTO Constructions (Name,Description_id,Example_id) VALUES ('{Name}',(SELECT max(Description_id) FROM Descriptions),(SELECT max(Example_id) FROM Examples))";
                    command.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Виникла помилка!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Заповніть всі поля!","Увага",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            TextBoxName.Clear();
            TextBoxDesctiption.Clear();
            TextBoxExample.Clear();
            LoadTable();

        }
        void LoadTable()
        {
            SqlConnection connection = DataBase.Connection;
            SqlCommand com = new SqlCommand("SELECT Constructions.Construction_id, Constructions.Name, Descriptions.Description, Examples.Example FROM Constructions " +
                " JOIN Descriptions ON Constructions.Description_id = Descriptions.Description_id" +
                " JOIN Examples ON Constructions.Example_id = Examples.Example_id;", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            adapter.Fill(dt);
            com.Dispose();
            adapter.Dispose();
            TableConstructions.ItemsSource = dt.DefaultView;
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
