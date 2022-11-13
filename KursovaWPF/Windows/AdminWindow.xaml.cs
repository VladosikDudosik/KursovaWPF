using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using KursovaWPF.Pages;
using KursovaWPF.Windows;
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
            var window = new MainWindow();
            window.Show();
            Close();
        }
    }
}
