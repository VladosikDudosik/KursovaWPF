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
using System.Windows.Shapes;
using KursovaWPF.Helpers;
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
                {Pages.Constructions,new ConstructionsGuestPage() },
                {Pages.DataTypes, new DataTypesGuestPage() },
                {Pages.Functions,new FunctionsGuestPage() },
                {Pages.Operators,new OperatorsGuestPage() }
            };
        }

        private void Window_Initialized(object sender, EventArgs e)
        {

        }

        private void ButtonOperatorsPage_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Navigate(pages[Pages.Operators]);
        }

        private void ButtonConstructionsPage_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Navigate(pages[Pages.Constructions]);
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
    }
}
