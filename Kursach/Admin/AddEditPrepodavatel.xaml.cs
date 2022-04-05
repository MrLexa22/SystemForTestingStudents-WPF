using Kursach.DataBaseTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddEditPrepodavatel.xaml
    /// </summary>
    public partial class AddEditPrepodavatel : UserControl
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

        string id_user = null;
        public AddEditPrepodavatel(string id)
        {
            InitializeComponent();
            id_user = id;
            rezhim.Text = "Добавление";
            TableParalleliTableAdapter a1 = new TableParalleliTableAdapter();
            DataBase.TableParalleliDataTable b1 = new DataBase.TableParalleliDataTable();
            a1.Fill(b1);
            if (b1.Rows != null)
            {
                Parallel.ItemsSource = b1;
                Parallel.SelectedIndex = 0;
            }
            HidParCl();

            if (id_user != null)
            {
                rezhim.Text = "Изменение";
                PrepodavatelSelectedTableAdapter a = new PrepodavatelSelectedTableAdapter();
                DataBase.PrepodavatelSelectedDataTable b = new DataBase.PrepodavatelSelectedDataTable();
                a.Fill(b, Convert.ToInt32(id_user));

                familia.Text = b[0].Familia;
                ima.Text = b[0].Ima;
                var check = b[0]["Otchestvo"] != DBNull.Value ? b[0]["Otchestvo"].ToString() : "t";
                if (check != "t")
                    otchestvo.Text = b[0].Otchestvo;
                snils.Text = b[0].SNILS;
                inn.Text = b[0].INN;
                seria_pasporta.Text = b[0].SeriaPasporta;
                nomer_pasporta.Text = b[0].NomerPasporta;
                NomerTelefona.Text = b[0].TelefonNumber;
                login.Text = b[0].Login;
                password.Password = b[0].Password;
                password_repeat.Password = b[0].Password;
                check = b[0]["Class_ID"] != DBNull.Value ? b[0]["Class_ID"].ToString() : "t";
                if (check != "t")
                {
                    KlRuk.IsChecked = true;
                    ClRukSelectedTableAdapter a2 = new ClRukSelectedTableAdapter();
                    DataBase.ClRukSelectedDataTable b2 = new DataBase.ClRukSelectedDataTable();
                    a2.Fill(b2, Convert.ToInt32(id_user));
                    Parallel.SelectedValue = b2[0].Parallel_ID;
                    Grupa.SelectedValue = b2[0].Class_ID;
                }       
            }
        }

        public void HidParCl()
        {
            Parallel.Visibility = Visibility.Hidden;
            Parallel_lab.Visibility = Visibility.Hidden;
            Grupa_lab.Visibility = Visibility.Hidden;
            Grupa.Visibility = Visibility.Hidden;
        }
        public void VisParCl()
        {
            Parallel.Visibility = Visibility.Visible;
            Parallel_lab.Visibility = Visibility.Visible;
            Grupa_lab.Visibility = Visibility.Visible;
            Grupa.Visibility = Visibility.Visible;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new Admin_prepodavateli();
        }

        private void CheckDigit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text.ToString() != "")
            {
                for (int i = 0; i < (sender as TextBox).Text.Length; i++)
                {
                    bool t = true;
                    string g = (sender as TextBox).Text.ToString();
                    t = Char.IsDigit((sender as TextBox).Text[i]);
                    if (t == false)
                    {
                        g = g.Remove(i, 1);
                    }
                    (sender as TextBox).Text = g;
                    (sender as TextBox).SelectionStart = (sender as TextBox).Text.ToString().Length;
                }
            }
        }

        private void KlRuk_Checked(object sender, RoutedEventArgs e)
        {
            VisParCl();
        }

        private void KlRuk_Unchecked(object sender, RoutedEventArgs e)
        {
            HidParCl();
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
            if(familia.Text.Trim() == "" || ima.Text.Trim() == "" || familia.Text.Trim().Length < 2 || ima.Text.Trim().Length < 2)
            {
                MessageBox.Show("Поля Фамилия и Имя обязательны для заполненя");
                return;
            }
            if(familia.Text.Trim().Where(c => char.IsDigit(c)).Count() > 0 || ima.Text.Trim().Where(c => char.IsDigit(c)).Count() > 0)
            {
                MessageBox.Show("Поля Фамилия и Имя не могут содержать цифр");
                return;
            }
            if(otchestvo.Text.Trim() != "")
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
            if (inn.Text.Trim().Where(c => char.IsDigit(c)).Count() != 12)
            {
                MessageBox.Show("Поле ИНН заполнено некорректно");
                return;
            }
            if (seria_pasporta.Text.Trim().Where(c => char.IsDigit(c)).Count() != 4)
            {
                MessageBox.Show("Поле Серия паспорта заполнено некорректно");
                return;
            }
            if (nomer_pasporta.Text.Trim().Where(c => char.IsDigit(c)).Count() != 6)
            {
                MessageBox.Show("Поле Номер паспорта заполнено некорректно");
                return;
            }
            if (NomerTelefona.Text.Trim().Where(c => char.IsDigit(c)).Count() != 11)
            {
                MessageBox.Show("Поле Номер телефона заполнено некорректно");
                return;
            }

            var cyrillic = Enumerable.Range(1024, 256).Select(ch => (char)ch);
            if (login.Text.Trim().Any(cyrillic.Contains) || login.Text.Trim().Length < 3 || login.Text.Contains(" "))
            {
                MessageBox.Show("Поле Логин заполнено некорректно");
                return;
            }
            if(password.Password.Trim().Length < 6)
            {
                MessageBox.Show("Поле Пароль заполнено некорректно");
                return;
            }
            if (password.Password.Trim() != password_repeat.Password.Trim())
            {
                MessageBox.Show("Поле Повтор пароля не совпадает с паролем");
                return;
            }

            if (KlRuk.IsChecked == true)
            {
                if (Parallel.SelectedValue == null)
                {
                    MessageBox.Show("Вы не выбрали параллель");
                    return;
                }
                if (Grupa.SelectedValue == null)
                {
                    MessageBox.Show("Вы не выбрали класс/группу");
                    return;
                }
            }

            PrepodavateliTableAdapter a = new PrepodavateliTableAdapter();
            UsersTableAdapter a1 = new UsersTableAdapter();
            if (id_user == null)
            {
                if (Convert.ToInt16(a.FindExistPrepodavatel(inn.Text.Trim(), NomerTelefona.Text.Trim(), seria_pasporta.Text.Trim(), nomer_pasporta.Text.Trim())) > 0)
                {
                    MessageBox.Show("Такой преподаватель уже существует!");
                    return;
                }

                if(Convert.ToInt32(a1.FindExistUser(login.Text.Trim(),snils.Text.Trim(),familia.Text.Trim(),ima.Text.Trim())) > 0)
                {
                    MessageBox.Show("Такой пользователь уже существует!");
                    return;
                }

                bool upd_cl = true;
                if (KlRuk.IsChecked == true)
                {
                    DataBase.PrepodavateliDataTable b = new DataBase.PrepodavateliDataTable();
                    a.FindExistClRuk(b,Convert.ToInt16(Grupa.SelectedValue));
                    if (b.Rows.Count > 0)
                    {
                        if (MessageBox.Show("Внимание! У выбранного класса уже назначен классный руководитель!\nВы желаете сменить классного руководителя у выбранного класса?\nПри выборе НЕТ - создаваемому преподавателю классное руководство назначено не будет!\nПри выборе ДА - классное руководство назначено будет, но у другого преподавателя оно пропадёт!", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            upd_cl = true;
                            a.UpdateClRukovodstov(null, b[0].ID_Prepodavatel);
                        }
                        else
                            upd_cl = false;
                    }
                }

                if(otchestvo.Text.Trim() != "")
                    a1.InsertQueryAdd(login.Text.Trim(), password.Password, "P", familia.Text.Trim(), ima.Text.Trim(), otchestvo.Text.Trim(), snils.Text.Trim());
                else
                    a1.InsertQueryAdd(login.Text.Trim(), password.Password, "P", familia.Text.Trim(), ima.Text.Trim(), null, snils.Text.Trim());

                DataBase.UsersDataTable b1 = new DataBase.UsersDataTable();
                a1.FinByLogin(b1, login.Text.Trim());

                if(KlRuk.IsChecked == true)
                {
                    if (upd_cl == true)
                        a.InsertQueryAdd(seria_pasporta.Text.Trim(), nomer_pasporta.Text.Trim(), inn.Text.Trim(), b1[0].ID_User, Convert.ToInt32(Grupa.SelectedValue), NomerTelefona.Text.Trim());
                    else
                        a.InsertQueryAdd(seria_pasporta.Text.Trim(), nomer_pasporta.Text.Trim(), inn.Text.Trim(), b1[0].ID_User, null, NomerTelefona.Text.Trim());
                }
                else
                    a.InsertQueryAdd(seria_pasporta.Text.Trim(), nomer_pasporta.Text.Trim(), inn.Text.Trim(), b1[0].ID_User, null, NomerTelefona.Text.Trim());
                MessageBox.Show("Преподаватель успешно создан!");
            }

            else
            {
                if (Convert.ToInt16(a.FindExistPrepodavatelEdit(inn.Text.Trim(), NomerTelefona.Text.Trim(), seria_pasporta.Text.Trim(), nomer_pasporta.Text.Trim(),Convert.ToInt16(id_user))) > 0)
                {
                    MessageBox.Show("Такой преподаватель уже существует!");
                    return;
                }

                PrepodavatelSelectedTableAdapter a2 = new PrepodavatelSelectedTableAdapter();
                DataBase.PrepodavatelSelectedDataTable b2 = new DataBase.PrepodavatelSelectedDataTable();
                a2.Fill(b2, Convert.ToInt32(id_user));
                if (Convert.ToInt32(a1.FindExistUserEdit(login.Text.Trim(), snils.Text.Trim(), familia.Text.Trim(), ima.Text.Trim(),b2[0].User_ID)) > 0)
                {
                    MessageBox.Show("Такой пользователь уже существует!");
                    return;
                }

                bool upd_cl = true;
                if (KlRuk.IsChecked == true)
                {
                    DataBase.PrepodavateliDataTable b = new DataBase.PrepodavateliDataTable();
                    a.FindExistClRukEdit(b, Convert.ToInt16(Grupa.SelectedValue), Convert.ToInt16(id_user));
                    if (b.Rows.Count > 0)
                    {
                        if (MessageBox.Show("Внимание! У выбранного класса уже назначен классный руководитель!\nВы желаете сменить классного руководителя у выбранного класса?\nПри выборе НЕТ - создаваемому преподавателю классное руководство назначено не будет!\nПри выборе ДА - классное руководство назначено будет, но у другого преподавателя оно пропадёт!", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            upd_cl = true;
                            a.UpdateClRukovodstov(null, b[0].ID_Prepodavatel);
                        }
                        else
                            upd_cl = false;
                    }
                }

                if (otchestvo.Text.Trim() != "")
                    a1.UpdateQuery(login.Text.Trim(), password.Password, "P", familia.Text.Trim(), ima.Text.Trim(), otchestvo.Text.Trim(), snils.Text.Trim(), b2[0].User_ID);
                else
                    a1.UpdateQuery(login.Text.Trim(), password.Password, "P", familia.Text.Trim(), ima.Text.Trim(), null, snils.Text.Trim(), b2[0].User_ID);

                if (KlRuk.IsChecked == true)
                {
                    if (upd_cl == true)
                        a.UpdateQuery(seria_pasporta.Text.Trim(), nomer_pasporta.Text.Trim(), inn.Text.Trim(), b2[0].User_ID, Convert.ToInt32(Grupa.SelectedValue), NomerTelefona.Text.Trim(),Convert.ToInt32(id_user));
                    else
                        a.UpdateQuery(seria_pasporta.Text.Trim(), nomer_pasporta.Text.Trim(), inn.Text.Trim(), b2[0].User_ID,  null, NomerTelefona.Text.Trim(), Convert.ToInt32(id_user));
                }
                else
                    a.UpdateQuery(seria_pasporta.Text.Trim(), nomer_pasporta.Text.Trim(), inn.Text.Trim(), b2[0].User_ID, null, NomerTelefona.Text.Trim(), Convert.ToInt32(id_user));
                MessageBox.Show("Преподаватель успешно обновлён!");
            }

            Controllers.fram.Content = new Admin_prepodavateli();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            password.Password = GetRandomPassword(8);
            password_repeat.Password = password.Password;
            generated_password.Visibility = Visibility.Visible;
            generated_password.Text = "Сген. пароль: " + password.Password;
            MessageBox.Show("Пароль сгенерирован и был указан в поля пароль и повтор пароля!");
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
