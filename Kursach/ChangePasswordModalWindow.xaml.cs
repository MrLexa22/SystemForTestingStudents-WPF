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
using System.Windows.Shapes;

namespace Kursach
{
    /// <summary>
    /// Логика взаимодействия для ChangePasswordModalWindow.xaml
    /// </summary>
    public partial class ChangePasswordModalWindow : Window
    {
        public string id_user = "";
        public string old_password = "";
        public ChangePasswordModalWindow(string id_us, string old_pas)
        {
            InitializeComponent();
            id_user = id_us;
            old_password = old_pas;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(oldPassword.Text != old_password)
            {
                MessageBox.Show("Старый пароль указан неверно!");
                return;
            }
            if (newPassword.Password.Trim().Length < 6)
            {
                MessageBox.Show("Поле Пароль заполнено некорректно");
                return;
            }
            if (newPassword.Password != repeatNewPassword.Password)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }
            UsersTableAdapter a = new UsersTableAdapter();
            a.UpdatePasswordUser(newPassword.Password, Convert.ToInt32(id_user));
            MessageBox.Show("Пароль обновлён!");
            this.Close();
        }
    }
}
