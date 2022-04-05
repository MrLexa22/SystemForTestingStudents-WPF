using Kursach.DataBaseTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
    /// Логика взаимодействия для ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : UserControl
    {
        public ForgotPassword()
        {
            InitializeComponent();
            btn.Content = "Далее";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = Application.Current.MainWindow as MainWindow;
            win.control.Content = new Authentication();
        }
        public static string GetRandomPassword(int length)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }

        public string code = "";
        public string logins = "";
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
                if (btn.Content.ToString() == "Далее")
                {
                    if (login.Text.Trim() == "")
                    {
                        MessageBox.Show("Некорректный логин/email.\nВосстановление пароля доступно только учащимся и студентам.");
                        return;
                    }
                    TableStudentsTableAdapter a = new TableStudentsTableAdapter();
                    DataBase.TableStudentsDataTable b = new DataBase.TableStudentsDataTable();
                    a.FindByLogin(b, login.Text.Trim());
                    if (b.Rows.Count <= 0)
                    {
                        MessageBox.Show("Аккаунт не найден.\nВосстановление пароля доступно только учащимся и студентам.");
                        return;
                    }
                    var check = b[0]["Email"] != DBNull.Value ? b[0]["Email"].ToString() : "t";
                    if (check == "t")
                    {
                        MessageBox.Show("Введённый аккаунт не содержит Email.\nДля восстановления пароля - обратитесь к администратору.");
                        return;
                    }
                    try
                    {
                        logins = b[0].Login;
                        code = GetRandomPassword(4);
                        MailAddress from = new MailAddress("test_prilozhenie@list.ru", "Приложение для тестирования");
                        MailAddress to = new MailAddress(b[0].Email);
                        MailMessage m = new MailMessage(from, to);
                        m.Subject = "Код для восстановления пароля";
                        m.Body = "<h1 style='font-weight: normal;'>Код для восстановления пароля: <b>" + code + "</b></h1>";
                        m.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                        smtp.Credentials = new NetworkCredential("test_prilozhenie@list.ru", "ggzbwjYU3W1ALhgj7AY6");
                        smtp.EnableSsl = true;
                        smtp.Send(m);
                        MessageBox.Show("Письмо отправлено!");

                        login.Text = "";
                        MaterialDesignThemes.Wpf.HintAssist.SetHint(login, "Введите код, высланный Вам на почту");
                        btn.Content = "Завершение";
                        text_addition.Text = "Введите код, который был отправлен Вам на почту";
                    }
                    catch
                    {
                        MessageBox.Show("Непредвиденная ошибка");
                    }
                }
                else if(btn.Content.ToString() == "Завершение")
                {
                try
                {
                    if (code == login.Text.Trim())
                    {
                        TableStudentsTableAdapter a = new TableStudentsTableAdapter();
                        DataBase.TableStudentsDataTable b = new DataBase.TableStudentsDataTable();
                        a.FindByLogin(b, logins);
                        MailAddress from = new MailAddress("test_prilozhenie@list.ru", "Приложение для тестирования");
                        MailAddress to = new MailAddress(b[0].Email);
                        MailMessage m = new MailMessage(from, to);
                        m.Subject = "Новый пароль для вашего аккаунта";
                        string pass = GetRandomPassword(8);
                        m.Body = "<h1 style='font-weight: normal;'>Логин: <b>" + b[0].Login + "</b></h1></br><h1 style='font-weight: normal;'>Пароль: <b>" + pass + "</b></h1>";
                        m.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                        smtp.Credentials = new NetworkCredential("test_prilozhenie@list.ru", "ggzbwjYU3W1ALhgj7AY6");
                        smtp.EnableSsl = true;
                        smtp.Send(m);
                        UsersTableAdapter a1 = new UsersTableAdapter();
                        a1.UpdatePasswordUserByLogin(pass, logins);
                        MessageBox.Show("Новый пароль выслан Вам на почту!");
                        Controllers.fram.Content = new Authentication();
                    }
                    else
                    {
                        MessageBox.Show("Код из письма не совпадает с введённым!");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("Непредвиденная ошибка");
                }
            }
        }
    }
}
