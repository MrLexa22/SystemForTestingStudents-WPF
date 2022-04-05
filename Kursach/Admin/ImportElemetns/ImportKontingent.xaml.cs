using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Kursach.DataBaseTableAdapters;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

namespace Kursach.Admin.ImportElemetns
{
    /// <summary>
    /// Логика взаимодействия для ImportKontingent.xaml
    /// </summary>
    public partial class ImportKontingent : UserControl
    {
        public List<CSVReadStudent> list = new List<CSVReadStudent>();
        public string path_import = "";
        public ImportKontingent()
        {
            InitializeComponent();
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

        public class CSVReadStudent
        {
            [Name("Фамилия")]
            public string Familia { get; set; }
            [Name("Имя")]
            public string Ima { get; set; }
            [Name("Отчество")]
            public string Otchestvo { get; set; } //null
            [Name("СНИЛС")]
            public string SNILS { get; set; }
            [Name("Номер телефона одного из родителей")]
            public string NumberPhone { get; set; } //null
            [Name("Email для восстановления пароля")]
            public string Email { get; set; } //null

            [Name("Логин")]
            public string Login { get; set; } //null
            [Name("Пароль")]
            public string Password { get; set; } //null
            [Name("Класс или группа")]
            public string ClassGroup { get; set; } 
        }
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("При заполнении обязательно наличие полей: Фамилия, Имя, Отчество, СНИЛС, Номер телефона, Email, Логин, Пароль, Класс или группа руководителя\n\n" +
                "Необязательные для заполнения - отчество, логин, пароль, номер телефона, email\n\n" +
                "При не указании логина или пароля - данные генерируются автоматически. Есил сгенерированный логин уже существует, то запис в систему занесена не будет!\n\n" +
                "Чтобы указать класс обучающегося необходимо указать наименование группы (П50-1-19) или класс (2В)");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                string FilePath = "";
                saveFileDialog.FileName = "Пример заполнения учащиеся";
                saveFileDialog.AddExtension = true;
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                if (saveFileDialog.ShowDialog() == true)
                {
                    FilePath = saveFileDialog.FileName;
                    string ext_file = Directory.GetCurrentDirectory() + @"\ImportFiles\Пример заполнения учащиеся.csv";
                    File.Copy(ext_file, FilePath);
                }
            }
            catch
            {
                MessageBox.Show("Непредвиденная ошибка!");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                string FilePath = "";
                saveFileDialog.FileName = "Импорт учащиеся";
                saveFileDialog.AddExtension = true;
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                if (saveFileDialog.ShowDialog() == true)
                {
                    FilePath = saveFileDialog.FileName;
                    string ext_file = Directory.GetCurrentDirectory() + @"\ImportFiles\Импорт учащиеся.csv";
                    File.Copy(ext_file, FilePath);
                }
            }
            catch
            {
                MessageBox.Show("Непредвиденная ошибка!");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    FileInfo t = new FileInfo(openFileDialog.FileName);
                    filename.Text = t.Name;
                    path_import = openFileDialog.FileName;

                    IEnumerable<CSVReadStudent> l2 = new List<CSVReadStudent>();

                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ";"
                    };
                    using (var reader = new StreamReader(path_import))
                    using (var csv = new CsvReader(reader, config))
                    {
                        l2 = csv.GetRecords<CSVReadStudent>();
                        list = l2.ToList();
                        btn_exp.IsEnabled = true;
                        grid1.Visibility = Visibility.Hidden;
                        grid2.Visibility = Visibility.Hidden;
                        btn_notpass.Visibility = Visibility.Hidden;
                        btn_reasons.Visibility = Visibility.Hidden;
                    }
                    if (list.Count <= 0)
                    {
                        filename.Text = "";
                        path_import = "";
                        btn_exp.IsEnabled = false;
                        MessageBox.Show("Файл пустой!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Непредвиденная ошибка!\n" + ex.Message.ToString());
                    filename.Text = "";
                    path_import = "";
                    btn_exp.IsEnabled = false;
                }
            }
        }

        public List<CSVReadStudent> list_not = new List<CSVReadStudent>();
        public List<CSVReadStudent> list_yes = new List<CSVReadStudent>();
        public List<CSVErrorsPrepodavat> list_errors = new List<CSVErrorsPrepodavat>();

        public bool check_add(CSVReadStudent t)
        {
            CSVErrorsPrepodavat error = new CSVErrorsPrepodavat();
            error.FIO = t.Familia.Trim() + " " + t.Ima.Trim();
            error.login = t.Login;

            if (t.Familia.Trim().Length < 2 || t.Ima.Trim().Length < 2 || t.Ima.Trim().Length >= 30 || t.Familia.Trim().Length >= 30)
            {
                list_not.Add(t);
                error.reason = "Недопустимая длина фамилии/имени. Некорректное заполнение";
                list_errors.Add(error);
                return false;
            }
            if (t.Familia.Trim().Where(c => char.IsDigit(c)).Count() > 0 || t.Ima.Trim().Where(c => char.IsDigit(c)).Count() > 0)
            {
                list_not.Add(t);
                error.reason = "Цифры в фамилии/имени недопустимы";
                list_errors.Add(error);
                return false;
            }
            if (t.Otchestvo.Trim() != "")
            {
                if (t.Otchestvo.Trim().Length < 2 || t.Otchestvo.Trim().Length >= 30)
                {
                    list_not.Add(t);
                    error.reason = "Недопустимая длина отчества. Некорректное заполнение";
                    list_errors.Add(error);
                    return false;
                }
                if (t.Otchestvo.Trim().Where(c => char.IsDigit(c)).Count() > 0)
                {
                    list_not.Add(t);
                    error.reason = "Цифры в отчестве недопустимы";
                    list_errors.Add(error);
                    return false;
                }
            }

            if (t.SNILS.Trim().Where(c => char.IsDigit(c)).Count() != 11)
            {
                list_not.Add(t);
                error.reason = "СНИЛС указан некорректно";
                list_errors.Add(error);
                return false;
            }
            else
            {
                string strIn1 = t.SNILS;
                string strPattern1 = @"(\d{3})[\s-]*(\d{3})[\s-]*(\d{3})[\s-]*(\d{2})";
                MatchCollection mm1 = Regex.Matches(strIn1, strPattern1);
                string g1 = "";
                foreach (Match m1 in mm1)
                {
                    g1 += g1 + (Regex.Replace(m1.Value, "^" + strPattern1 + "$", "$1-$2-$3 $4"));
                }
                t.SNILS = g1;
            }

            if (t.NumberPhone.Length <= 0)
            {
                string strIn = t.NumberPhone;
                string strPattern = @"(\+7|8|\b)[\(\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[)\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)";
                MatchCollection mm = Regex.Matches(strIn, strPattern);
                string g = "";
                foreach (Match m in mm)
                {
                    g += g + (Regex.Replace(m.Value, "^" + strPattern + "$", "+7($2$3$4)$5$6$7-$8$9-$10$11"));
                }
                if (g.Length == 0)
                {
                    list_not.Add(t);
                    error.reason = "Номер телефона указан некорректно";
                    list_errors.Add(error);
                    return false;
                }
                t.NumberPhone = g;
            }

            if (t.Email.Trim().Length > 0)
            {
                if (IsValidEmail(t.Email.Trim()) == false || t.Email.Trim().Length >= 80)
                {
                    list_not.Add(t);
                    error.reason = "Email указан некорректно";
                    list_errors.Add(error);
                    return false;
                }
            }

            var cyrillic = Enumerable.Range(1024, 256).Select(ch => (char)ch);
            if (t.Login.Trim().Any(cyrillic.Contains) || t.Login.Trim().Length < 3 || t.Login.Contains(" ") || t.Login.Trim().Length >= 30)
            {
                list_not.Add(t);
                error.reason = "Логин указан некорректно";
                list_errors.Add(error);
                return false;
            }

            if (t.Password.Trim().Length < 6 || t.Password.Trim().Length >= 30)
            {
                list_not.Add(t);
                error.reason = "Пароль указан некорректно";
                list_errors.Add(error);
                return false;
            }

            TableClassesTableAdapter a2 = new TableClassesTableAdapter();
            DataBase.TableClassesDataTable b2 = new DataBase.TableClassesDataTable();
            a2.FinClassByNameCSV(b2, t.ClassGroup.Trim());
            if (b2.Rows.Count <= 0)
            {
                list_not.Add(t);
                error.reason = "Указанный класс не существует";
                list_errors.Add(error);
                return false;
            }

            UsersTableAdapter a1 = new UsersTableAdapter();
            StudentsTableAdapter a = new StudentsTableAdapter();
            if (Convert.ToInt32(a1.FindExistUser(t.Login.Trim(), t.SNILS.Trim(), t.Familia.Trim(), t.Ima.Trim())) > 0)
            {
                list_not.Add(t);
                error.reason = "Такой пользователь уже сущетсвует";
                list_errors.Add(error);
                return false;
            }
            if (Convert.ToInt32(a.FindExistStudentAdd(t.Email.Trim())) > 0)
            {
                list_not.Add(t);
                error.reason = "Такой пользователь уже сущетсвует";
                list_errors.Add(error);
                return false;
            }

            if (t.Otchestvo.Trim() != "")
                a1.InsertQueryAdd(t.Login.Trim(), t.Password.Trim(), "S", t.Familia.Trim(), t.Ima.Trim(), t.Otchestvo.Trim(), t.SNILS.Trim());
            else
                a1.InsertQueryAdd(t.Login.Trim(), t.Password.Trim(), "S", t.Familia.Trim(), t.Ima.Trim(), null, t.SNILS.Trim());

            DataBase.UsersDataTable b1 = new DataBase.UsersDataTable();
            a1.FinByLogin(b1, t.Login.Trim());

            string nm = t.NumberPhone.Trim();
            string em = t.Email.Trim();
            if (t.NumberPhone.Trim().Where(c => char.IsDigit(c)).Count() == 1)
                nm = null;
            if (IsValidEmail(t.Email.Trim()) == false)
                em = null;
            a.InsertQuery(b2[0].ID_Class, b1[0].ID_User, em, nm);

            return true;
        }
        private void btn_exp_Click(object sender, RoutedEventArgs e)
        {
            list_not = new List<CSVReadStudent>();
            list_errors = new List<CSVErrorsPrepodavat>();
            list_yes = new List<CSVReadStudent>();
            for (int i = 0; i < list.Count; i++)
            {
                CSVReadStudent t = new CSVReadStudent();

                t = list[i];

                t.Familia = ToUpperFirstLetter(t.Familia.Trim());
                t.Ima = ToUpperFirstLetter(t.Ima.Trim());
                t.Otchestvo = ToUpperFirstLetter(t.Otchestvo.Trim());
                if (t.Password.Trim().Length <= 0)
                    t.Password = GetRandomPassword(6);

                if (t.Login.Trim().Length <= 0)
                {
                    TranslitMethods.Translitter trn = new TranslitMethods.Translitter();
                    t.Login = trn.Translit(ToUpperFirstLetter(t.Familia.Trim()), TranslitMethods.TranslitType.Gost); ;
                    t.Login += "." + trn.Translit(ToUpperFirstLetter(t.Ima.Trim()), TranslitMethods.TranslitType.Gost)[0].ToString().ToUpper();
                    if (t.Otchestvo.Trim().Length > 0)
                        t.Login += "." + trn.Translit(ToUpperFirstLetter(t.Otchestvo.Trim()), TranslitMethods.TranslitType.Gost)[0].ToString().ToUpper();
                }

                t.Login = t.Login.Replace(" ", "");

                if (check_add(t) == true)
                    list_yes.Add(t);
            }
            no_datagrid.ItemsSource = list_not;
            yes_datagrid.ItemsSource = list_yes;
            grid1.Visibility = Visibility.Visible;
            grid2.Visibility = Visibility.Visible;
            path_import = "";
            filename.Text = "";
            btn_exp.IsEnabled = false;

            if (list_not.Count > 0)
                btn_notpass.Visibility = Visibility.Visible;
            if (list_errors.Count > 0)
                btn_reasons.Visibility = Visibility.Visible;
        }

        private void btn_notpass_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string FilePath = "";
            saveFileDialog.FileName = "Не сохранённые учащиеся";
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };
                FilePath = saveFileDialog.FileName;
                var wr = new StreamWriter(FilePath, false, Encoding.UTF8);
                using (var csvWriter = new CsvWriter(wr, config))
                {
                    csvWriter.WriteRecords(list_not);
                }
            }
        }

        private void btn_reasons_Click(object sender, RoutedEventArgs e)
        {
            ImportErrors t = new ImportErrors(list_errors);
            t.ShowDialog();
        }
    }
}
