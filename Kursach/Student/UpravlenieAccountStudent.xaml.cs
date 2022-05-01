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

namespace Kursach.Student
{
    /// <summary>
    /// Логика взаимодействия для UpravlenieAccount.xaml
    /// </summary>
    public partial class UpravlenieAccountStudent : UserControl
    {
        UsersTableAdapter a = new UsersTableAdapter();
        DataBase.UsersDataTable b = new DataBase.UsersDataTable();

        public int ID_Student = 0;
        public string emal = "";
        public UpravlenieAccountStudent()
        {
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            StudentsTableTableAdapter a1 = new StudentsTableTableAdapter();
            DataBase.StudentsTableDataTable b1 = new DataBase.StudentsTableDataTable();
            a1.FillByLogin(b1, Controllers.auth_login);
            a.FinByLogin(b, Controllers.auth_login);
            var check = b1[0]["Otchestvo"] != DBNull.Value ? b1[0]["Otchestvo"].ToString() : "t";
            if (check != "t")
                FIO.Text = b1[0].Familia + " " + b1[0].Ima + " " + b1[0].Otchestvo;
            else
                FIO.Text = b1[0].Familia + " " + b1[0].Ima;
            login.Text = b1[0].Login;
            snils.Text = b1[0].SNILS;
            check = b1[0]["Email"] != DBNull.Value ? b1[0]["Email"].ToString() : "t";
            if (check != "t")
            {
                email.Text = b1[0].Email;
            }
            else
            {
                email.Text = "не указан";
            }
            ID_Student = b1[0].ID_Student;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChangePasswordModalWindow n = new ChangePasswordModalWindow(b[0].ID_User.ToString(),b[0].Password);
            n.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new StudentListDisciplins();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ChangeEmailModalWindow n = new ChangeEmailModalWindow(ID_Student, email.Text);
            n.ShowDialog();
            Load();
        }
    }
}
