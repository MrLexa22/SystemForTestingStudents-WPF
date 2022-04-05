using Kursach.DataBaseTableAdapters;
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

namespace Kursach.Admin
{
    /// <summary>
    /// Логика взаимодействия для UpravlenieAccount.xaml
    /// </summary>
    public partial class UpravlenieAccount : UserControl
    {
        UsersTableAdapter a = new UsersTableAdapter();
        DataBase.UsersDataTable b = new DataBase.UsersDataTable();
        public UpravlenieAccount()
        {
            InitializeComponent();
            a.FinByLogin(b, Controllers.auth_login);
            var check = b[0]["Otchestvo"] != DBNull.Value ? b[0]["Otchestvo"].ToString() : "t";
            if (check != "t")
                FIO.Text = b[0].Familia + " " + b[0].Ima + " " + b[0].Otchestvo;
            else
                FIO.Text = b[0].Familia + " " + b[0].Ima;
            login.Text = b[0].Login;
            snils.Text = b[0].SNILS;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AdminControl();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChangePasswordModalWindow n = new ChangePasswordModalWindow(b[0].ID_User.ToString(),b[0].Password);
            n.ShowDialog();
            a.FinByLogin(b, Controllers.auth_login);
        }
    }
}
