using Kursach.Admin;
using Kursach.DataBaseTableAdapters;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Kursach
{
    /// <summary>
    /// Логика взаимодействия для AdminControl.xaml
    /// </summary>
    public partial class AdminControl : UserControl
    {
        public AdminControl()
        {
            InitializeComponent();
            //Controllers.ClearUsers();
            UsersTableAdapter a = new UsersTableAdapter();
            DataBase.UsersDataTable b = new DataBase.UsersDataTable();
            a.FinByLogin(b, Controllers.auth_login);
            familia.Text = b[0].Familia;
            ima.Text = b[0].Ima;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new Admin_prepodavateli();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new ParalleliPage();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new ClassuGrupiPage();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new DiscipliniPage();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new ContingentPage();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new UpravlenieAccount();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Controllers.auth_login = "";
            Controllers.fram.Content = new Authentication();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AdministratorsPage();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new ExportPage();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new ImportPage();
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new PeriodsPage();
        }
    }
}
