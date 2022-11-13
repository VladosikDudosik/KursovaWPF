using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KursovaWPF.Pages;
namespace KursovaWPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для GuessWindow.xaml
    /// </summary>
    public partial class GuestWindow : Window
    {
        Dictionary<Pages, Page> pages;
        public GuestWindow()
        {
            InitializeComponent();
            pages = new Dictionary<Pages, Page>
            {
                {Pages.DataTypes, new DataTypesGuestPage() },
                {Pages.Functions,new FunctionsGuestPage() },
                {Pages.Operators,new OperatorsGuestPage() }
            };
        }


        private void ButtonOperatorsPage_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Navigate(pages[Pages.Operators]);
        }

        private void ButtonDataTypesPage_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Navigate(pages[Pages.DataTypes]);
        }

        private void ButtonFunctionsPage_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Navigate(pages[Pages.Functions]);
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }
        enum Pages
        {
            DataTypes,
            Functions,
            Operators
        }
    }
}
