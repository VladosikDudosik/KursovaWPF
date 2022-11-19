using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KursovaWPF.Pages;
namespace KursovaWPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        Dictionary<Pages, Page> pages;
        public AdminWindow()
        {
            InitializeComponent();
            pages = new Dictionary<Pages, Page>
            {
                {Pages.DataTypes, new DataTypesPage() },
                {Pages.Functions,new FunctionsPage() },
                {Pages.Operators,new OperatorsPage() }
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
        enum Pages
        {
            Constructions,
            DataTypes,
            Functions,
            Operators
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var window = new LoginWindow();
            window.Show();
            Close();
        }
    }
}
