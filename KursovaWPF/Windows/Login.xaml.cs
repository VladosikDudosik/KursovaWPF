using System;
using System.Collections.Generic;
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
using KursovaWPF.Windows;
namespace KursovaWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918";
        string Login = "admin";
        public MainWindow()
        {
            DataBase.OpenConnection();
            
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string Login = TextBoxLogin.Text;
            string PasswordHash = Methods.TextToSHA256(PasswordBox.Password);
            //SqlDataReader data = DataBase.Select($"SELECT * FROM Users WHERE Login = '{Login}' and PasswordHash = '{Password}'");
            if(Login == this.Login && PasswordHash == this.PasswordHash)
            {
                var window = new AdminWindow();
                window.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неправильний логін або пароль!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                PasswordBox.Clear();
                TextBoxLogin.Clear();
            }
        }

        private void ButtonGuest_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = new GuestWindow();
            window.Show();
            Close();
            
        }
    }
}
