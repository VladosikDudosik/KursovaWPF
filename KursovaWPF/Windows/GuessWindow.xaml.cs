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
namespace KursovaWPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для GuessWindow.xaml
    /// </summary>
    public partial class GuessWindow : Window
    {
        public GuessWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            TableDataTypes.ItemsSource = DataBase.SelectAllDataTypes();
            TableFunctions.ItemsSource = DataBase.SelectAllFunctions();
            TableConstructions.ItemsSource = DataBase.SelectAllConstructions();
            TableOperators.ItemsSource = DataBase.SelectAllOperators();
        }
    }
}
