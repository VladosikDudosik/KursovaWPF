using System;
using System.Data;
using System.Data.SqlClient;
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
    }
}
