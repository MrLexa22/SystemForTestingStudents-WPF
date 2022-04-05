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
    /// Логика взаимодействия для AddEditAdministrator.xaml
    /// </summary>
    public partial class AddEditAdministrator : UserControl
    {
        public static string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
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

        public string id_user = "";
        public AddEditAdministrator(string id)
        {
            InitializeComponent();
            id_user = id;
            rezhim.Text = "Добавление";
            if (id_user != null)
            {
                rezhim.Text = "Изменение";
                AdministratorsTableAdapter a2 = new AdministratorsTableAdapter();
                DataBase.AdministratorsDataTable b2 = new DataBase.AdministratorsDataTable();
                a2.FillByID(b2, Convert.ToInt16(id_user));
                var check = b2[0]["Otchestvo"] != DBNull.Value ? b2[0]["Otchestvo"].ToString() : "t";
                if (check != "t")
                    otchestvo.Text = b2[0].Otchestvo;
                ima.Text = b2[0].Ima;
                familia.Text = b2[0].Familia;
                snils.Text = b2[0].SNILS;
                login.Text = b2[0].Login;
                password.Password = b2[0].Password;
                password_repeat.Password = b2[0].Password;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AdministratorsPage();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (familia.Text.Trim().Length > 0 && ima.Text.Trim().Length > 0)
            {
                TranslitMethods.Translitter trn = new TranslitMethods.Translitter();
                login.Text = trn.Translit(ToUpperFirstLetter(familia.Text.Trim()), TranslitMethods.TranslitType.Gost); ;
                login.Text += "." + trn.Translit(ToUpperFirstLetter(ima.Text.Trim()), TranslitMethods.TranslitType.Gost)[0].ToString().ToUpper();
                if (otchestvo.Text.Trim().Length > 0)
                    login.Text += "." + trn.Translit(ToUpperFirstLetter(otchestvo.Text.Trim()), TranslitMethods.TranslitType.Gost)[0].ToString().ToUpper();
            }
            else
            {
                MessageBox.Show("не заполнены фамилия и имя!");
                return;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            password.Password = GetRandomPassword(8);
            password_repeat.Password = password.Password;
            generated_password.Visibility = Visibility.Visible;
            generated_password.Text = "Сген. пароль: " + password.Password;
            MessageBox.Show("Пароль сгенерирован и был указан в поля пароль и повтор пароля!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (familia.Text.Trim() == "" || ima.Text.Trim() == "" || familia.Text.Trim().Length < 2 || ima.Text.Trim().Length < 2)
            {
                MessageBox.Show("Поля Фамилия и Имя обязательны для заполненя");
                return;
            }
            if (familia.Text.Trim().Where(c => char.IsDigit(c)).Count() > 0 || ima.Text.Trim().Where(c => char.IsDigit(c)).Count() > 0)
            {
                MessageBox.Show("Поля Фамилия и Имя не могут содержать цифр");
                return;
            }
            if (otchestvo.Text.Trim() != "")
            {
                if (otchestvo.Text.Trim().Length < 2)
                {
                    MessageBox.Show("Поле Отчество должно иметь минимум 2 символа");
                    return;
                }
                if (otchestvo.Text.Trim().Where(c => char.IsDigit(c)).Count() > 0)
                {
                    MessageBox.Show("Поле Отчество не может содержать в себе цифры");
                    return;
                }
            }
            familia.Text = ToUpperFirstLetter(familia.Text);
            ima.Text = ToUpperFirstLetter(ima.Text);
            otchestvo.Text = ToUpperFirstLetter(otchestvo.Text);
            if (snils.Text.Trim().Where(c => char.IsDigit(c)).Count() != 11)
            {
                MessageBox.Show("Поле СНИЛС заполнено некорректно");
                return;
            }
            var cyrillic = Enumerable.Range(1024, 256).Select(ch => (char)ch);
            if (login.Text.Trim().Any(cyrillic.Contains) || login.Text.Trim().Length < 3 || login.Text.Contains(" "))
            {
                MessageBox.Show("Поле Логин заполнено некорректно");
                return;
            }
            if (password.Password.Trim().Length < 6)
            {
                MessageBox.Show("Поле Пароль заполнено некорректно");
                return;
            }
            if (password.Password.Trim() != password_repeat.Password.Trim())
            {
                MessageBox.Show("Поле Повтор пароля не совпадает с паролем");
                return;
            }

            UsersTableAdapter a1 = new UsersTableAdapter();
            if (id_user == null)
            {
                if (Convert.ToInt32(a1.FindExistUser(login.Text.Trim(), snils.Text.Trim(), familia.Text.Trim(), ima.Text.Trim())) > 0)
                {
                    MessageBox.Show("Такой пользователь уже существует!");
                    return;
                }

                if (otchestvo.Text.Trim() != "")
                    a1.InsertQueryAdd(login.Text.Trim(), password.Password, "A", familia.Text.Trim(), ima.Text.Trim(), otchestvo.Text.Trim(), snils.Text.Trim());
                else
                    a1.InsertQueryAdd(login.Text.Trim(), password.Password, "A", familia.Text.Trim(), ima.Text.Trim(), null, snils.Text.Trim());

                DataBase.UsersDataTable b1 = new DataBase.UsersDataTable();
                a1.FinByLogin(b1, login.Text.Trim());
                MessageBox.Show("Администратор успешно создан!");
            }
            else
            {
                AdministratorsTableAdapter a2 = new AdministratorsTableAdapter();
                DataBase.AdministratorsDataTable b2 = new DataBase.AdministratorsDataTable();
                a2.FillByID(b2, Convert.ToInt16(id_user));
                if (Convert.ToInt32(a1.FindExistUserEdit(login.Text.Trim(), snils.Text.Trim(), familia.Text.Trim(), ima.Text.Trim(), b2[0].ID_User)) > 0)
                {
                    MessageBox.Show("Такой пользователь уже существует!");
                    return;
                }

                if (otchestvo.Text.Trim() != "")
                    a1.UpdateQuery(login.Text.Trim(), password.Password, "A", familia.Text.Trim(), ima.Text.Trim(), otchestvo.Text.Trim(), snils.Text.Trim(), b2[0].ID_User);
                else
                    a1.UpdateQuery(login.Text.Trim(), password.Password, "A", familia.Text.Trim(), ima.Text.Trim(), null, snils.Text.Trim(), b2[0].ID_User);
                MessageBox.Show("Администратор успешно обновлён!");
            }
            Controllers.fram.Content = new AdministratorsPage();
        }
    }
}
