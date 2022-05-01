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

namespace Kursach.Student
{
    /// <summary>
    /// Логика взаимодействия для ChangePasswordModalWindow.xaml
    /// </summary>
    public partial class ChangeEmailModalWindow : Window
    {
        public int ID_Student = 0;
        public string Old_email = "";
        public ChangeEmailModalWindow(int ID_S, string old_e)
        {
            InitializeComponent();
            ID_Student = ID_S;
            Old_email = old_e;
            oldEmail.Text = old_e;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            newEmail.Text = newEmail.Text.ToString().Trim();
            repeatnewEmail.Text = repeatnewEmail.Text.ToString().Trim();
            if (!IsValidEmail(newEmail.Text))
            {
                MessageBox.Show("Email указан некорректно");
                return;
            }
            if (newEmail.Text != repeatnewEmail.Text)
            {
                MessageBox.Show("Email'ы не совпадают!");
                return;
            }
            StudentsTableAdapter a = new StudentsTableAdapter();
            if (Convert.ToInt32(a.FindExistStudentEdit(newEmail.Text, ID_Student)) > 0)
            {
                MessageBox.Show("Такой Email уже указан у другого пользователя!");
                return;
            }
            a.UpdateEmail(newEmail.Text, ID_Student);
            MessageBox.Show("Email обновлён!");
            this.Close();
        }
    }
}
