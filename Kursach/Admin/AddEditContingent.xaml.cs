using Kursach.DataBaseTableAdapters;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для AddEditContingent.xaml
    /// </summary>
    public partial class AddEditContingent : UserControl
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
        public AddEditContingent(string id)
        {
            InitializeComponent();
            id_user = id;
            rezhim.Text = "Добавление";
            TableParalleliTableAdapter a1 = new TableParalleliTableAdapter();
            DataBase.TableParalleliDataTable b1 = new DataBase.TableParalleliDataTable();
            a1.Fill(b1);
            if (b1.Rows.Count <= 0)
            {
                MessageBox.Show("Отсутствуют параллели и классы! Ошибка!");
                Controllers.fram.Content = new ContingentPage();
            }
            Parallel.ItemsSource = b1;
            Parallel.SelectedIndex = 0;
            if(id_user != null)
            {
                rezhim.Text = "Изменение";
                TableStudentsTableAdapter a2 = new TableStudentsTableAdapter();
                DataBase.TableStudentsDataTable b2 = new DataBase.TableStudentsDataTable();
                a2.FillByID(b2, Convert.ToInt16(id_user));
                var check = b2[0]["Otchestvo"] != DBNull.Value ? b2[0]["Otchestvo"].ToString() : "t";
                if (check != "t")
                    otchestvo.Text = b2[0].Otchestvo;
                check = b2[0]["Email"] != DBNull.Value ? b2[0]["Email"].ToString() : "t";
                if (check != "t")
                    email.Text = b2[0].Email;
                check = b2[0]["MobileZakPred"] != DBNull.Value ? b2[0]["MobileZakPred"].ToString() : "t";
                if (check != "t")
                    NomerTelefona.Text = b2[0].MobileZakPred;
                ima.Text = b2[0].Ima;
                familia.Text = b2[0].Familia;
                snils.Text = b2[0].SNILS;
                login.Text = b2[0].Login;
                password.Password = b2[0].Password;
                password_repeat.Password = b2[0].Password;
                Parallel.SelectedValue = b2[0].Parallel_ID;
                Grupa.SelectedValue = b2[0].Class_ID;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new ContingentPage();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            password.Password = GetRandomPassword(8);
            password_repeat.Password = password.Password;
            generated_password.Visibility = Visibility.Visible;
            generated_password.Text = "Сген. пароль: " + password.Password;
            MessageBox.Show("Пароль сгенерирован и был указан в поля пароль и повтор пароля!");
        }

        private void Parallel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            TableClassesTableAdapter a2 = new TableClassesTableAdapter();
            DataBase.TableClassesDataTable b2 = new DataBase.TableClassesDataTable();
            a2.Fill(b2, Convert.ToInt32(Parallel.SelectedValue));
            Grupa.ItemsSource = b2;
            Grupa.SelectedIndex = 0;
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
            if (NomerTelefona.Text.Trim().Where(c => char.IsDigit(c)).Count() > 1)
            {
                if (NomerTelefona.Text.Trim().Where(c => char.IsDigit(c)).Count() != 11)
                {
                    MessageBox.Show("Поле Номер телефона заполнено некорректно");
                    return;
                }
            }
            if(email.Text.Trim().Length > 0)
            {
                if(IsValidEmail(email.Text.Trim()) == false)
                {
                    MessageBox.Show("Поле Email заполнено некорректно");
                    return;
                }
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
            StudentsTableAdapter a = new StudentsTableAdapter();
            if (id_user == null)
            {
                if (Convert.ToInt32(a1.FindExistUser(login.Text.Trim(), snils.Text.Trim(), familia.Text.Trim(), ima.Text.Trim())) > 0)
                {
                    MessageBox.Show("Такой пользователь уже существует!");
                    return;
                }
                if (Convert.ToInt32(a.FindExistStudentAdd(email.Text.Trim())) > 0)
                {
                    MessageBox.Show("Такой пользователь уже существует!");
                    return;
                }

                if (otchestvo.Text.Trim() != "")
                    a1.InsertQueryAdd(login.Text.Trim(), password.Password, "S", familia.Text.Trim(), ima.Text.Trim(), otchestvo.Text.Trim(), snils.Text.Trim());
                else
                    a1.InsertQueryAdd(login.Text.Trim(), password.Password, "S", familia.Text.Trim(), ima.Text.Trim(), null, snils.Text.Trim());

                DataBase.UsersDataTable b1 = new DataBase.UsersDataTable();
                a1.FinByLogin(b1, login.Text.Trim());

                string nm = NomerTelefona.Text.Trim();
                string em = email.Text.Trim();
                if (NomerTelefona.Text.Trim().Where(c => char.IsDigit(c)).Count() == 1)
                    nm = null;
                if (IsValidEmail(email.Text.Trim()) == false)
                    em = null;
                a.InsertQuery(Convert.ToInt16(Grupa.SelectedValue), b1[0].ID_User, em, nm);
                MessageBox.Show("Обучающийся успешно создан!");
            }
            else
            {
                TableStudentsTableAdapter a2 = new TableStudentsTableAdapter();
                DataBase.TableStudentsDataTable b2 = new DataBase.TableStudentsDataTable();
                a2.FillByID(b2, Convert.ToInt16(id_user));
                if (Convert.ToInt32(a1.FindExistUserEdit(login.Text.Trim(), snils.Text.Trim(), familia.Text.Trim(), ima.Text.Trim(), b2[0].User_ID)) > 0)
                {
                    MessageBox.Show("Такой пользователь уже существует!");
                    return;
                }
                if (Convert.ToInt32(a.FindExistStudentEdit(email.Text.Trim(),Convert.ToInt16(id_user))) > 0)
                {
                    MessageBox.Show("Такой пользователь уже существует!");
                    return;
                }

                if (otchestvo.Text.Trim() != "")
                    a1.UpdateQuery(login.Text.Trim(), password.Password, "S", familia.Text.Trim(), ima.Text.Trim(), otchestvo.Text.Trim(), snils.Text.Trim(),b2[0].User_ID);
                else
                    a1.UpdateQuery(login.Text.Trim(), password.Password, "S", familia.Text.Trim(), ima.Text.Trim(), null, snils.Text.Trim(), b2[0].User_ID);

                string nm = NomerTelefona.Text.Trim();
                string em = email.Text.Trim();
                if (NomerTelefona.Text.Trim().Where(c => char.IsDigit(c)).Count() == 1)
                    nm = null;
                if (IsValidEmail(email.Text.Trim()) == false)
                    em = null;
                a.UpdateQuery(Convert.ToInt16(Grupa.SelectedValue), b2[0].User_ID, em, nm, Convert.ToInt16(id_user));
                MessageBox.Show("Обучающийся успешно обновлён!");
            }
            Controllers.fram.Content = new ContingentPage();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if(familia.Text.Trim().Length > 0 && ima.Text.Trim().Length > 0)
            {
                TranslitMethods.Translitter trn = new TranslitMethods.Translitter();
                login.Text = trn.Translit(ToUpperFirstLetter(familia.Text.Trim()), TranslitMethods.TranslitType.Gost);;
                login.Text += "."+trn.Translit(ToUpperFirstLetter(ima.Text.Trim()), TranslitMethods.TranslitType.Gost)[0].ToString().ToUpper();
                if(otchestvo.Text.Trim().Length > 0)
                    login.Text += "." + trn.Translit(ToUpperFirstLetter(otchestvo.Text.Trim()), TranslitMethods.TranslitType.Gost)[0].ToString().ToUpper();
                login.Text = login.Text.Replace(" ", "");
            }
            else
            {
                MessageBox.Show("не заполнены фамилия и имя!");
                return;
            }
        }
    }
}
