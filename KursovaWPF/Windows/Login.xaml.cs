using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KursovaWPF.Helpers;
using KursovaWPF.Windows;
namespace KursovaWPF
{
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
            string PasswordHash = TextToSHA256(PasswordBox.Password);
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
        public static string TextToSHA256(string text)
        {
            StringBuilder res = new StringBuilder();
            byte[] bytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text));
            for (int i = 0; i < bytes.Length; i++)
            {
                res.Append(bytes[i].ToString("x2"));
            }
            return res.ToString();
        }
    }
}
