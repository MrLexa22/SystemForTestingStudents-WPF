using Kursach.DataBaseTableAdapters;
using Kursach.Prepodavatel;
using Kursach.Student;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kursach
{
    /// <summary>
    /// Логика взаимодействия для Authentication.xaml
    /// </summary>
    public partial class Authentication : UserControl
    {
        public Authentication()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = Application.Current.MainWindow as MainWindow;
            win.control.Content = new ForgotPassword();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (login.Text.Length == 0 || password.Password.Length == 0)
            {
                MessageBox.Show("Поля логин/пароль заполнены некорректно!");
                return;
            }
            UsersTableAdapter a = new UsersTableAdapter();
            DataBase.UsersDataTable b = new DataBase.UsersDataTable();
            a.Authenticate(b, login.Text, password.Password);
            if (b.Rows.Count <= 0)
            {
                MessageBox.Show("Авторизация не удалась!");
                return;
            }

            Controllers.auth_login = b[0].Login;
            if (b[0].Role == "A")
                Controllers.fram.Content = new AdminControl();
            if (b[0].Role == "S")
                Controllers.fram.Content = new StudentControl();
            if (b[0].Role == "P")
                Controllers.fram.Content = new PrepodavatelControl();
        }
    }
}
