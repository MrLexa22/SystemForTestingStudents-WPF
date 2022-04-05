using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Kursach.DataBaseTableAdapters;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace Kursach.Admin.ExportElements
{
    /// <summary>
    /// Логика взаимодействия для ExportContingent.xaml
    /// </summary>
    public partial class ExportContingent : UserControl
    {
        public ExportContingent()
        {
            InitializeComponent();
            ch1_1.Checked += ch1_1_Checked;
            ch1_1.Unchecked += ch1_1_Checked;
            ch1_2.Unchecked += ch1_1_Checked;
            ch1_2.Checked += ch1_1_Checked;
        }

        private void ch1_1_Checked(object sender, RoutedEventArgs e)
        {
            if (ch1_2.IsChecked == true)
            {
                shag1.Visibility = Visibility.Visible;
            }
            else
                shag1.Visibility = Visibility.Hidden;
        }

        private void btn_exp_Click(object sender, RoutedEventArgs e)
        {
            List<AllStudentsInfo> list = new List<AllStudentsInfo>();
            List<StudentsAuth> list2 = new List<StudentsAuth>();
            CSVStudentsTableAdapter a = new CSVStudentsTableAdapter();
            if (ch1_1.IsChecked == true && ch2_2.IsChecked == true)
            {
                DataBase.CSVStudentsDataTable b = new DataBase.CSVStudentsDataTable();
                a.FillAll(b);
                for (int i = 0; i < b.Rows.Count; i++)
                {
                    AllStudentsInfo t = new AllStudentsInfo();
                    t.Id_Student = b[i].ID_Student;
                    t.Familia = b[i].Familia;
                    t.Ima = b[i].Ima;
                    var check = b[i]["Otchestvo"] != DBNull.Value ? b[i]["Otchestvo"].ToString() : "t";
                    if (check != "t")
                        t.Otchestvo = b[i].Otchestvo;
                    else
                        t.Otchestvo = "";

                    check = b[i]["Email"] != DBNull.Value ? b[i]["Email"].ToString() : "t";
                    if (check != "t")
                        t.EmailStudent = b[i].Email;
                    else
                        t.EmailStudent = "";

                    check = b[i]["MobileZakPred"] != DBNull.Value ? b[i]["MobileZakPred"].ToString() : "t";
                    if (check != "t")
                        t.MobilePhoneRoditela = b[i].MobileZakPred;
                    else
                        t.MobilePhoneRoditela = "";
                    t.SNILS = b[i].SNILS;
                    t.Login = b[i].Login;
                    t.Password = b[i].Password;
                    t.Gruppa = b[i].NameClass;
                    list.Add(t);
                }
            }
            if (ch1_1.IsChecked == true && ch2_1.IsChecked == true)
            {
                DataBase.CSVStudentsDataTable b = new DataBase.CSVStudentsDataTable();
                a.FillAll(b);
                for (int i = 0; i < b.Rows.Count; i++)
                {
                    StudentsAuth t = new StudentsAuth();

                    t.Familia = b[i].Familia;
                    t.Ima = b[i].Ima;
                    var check = b[i]["Otchestvo"] != DBNull.Value ? b[i]["Otchestvo"].ToString() : "t";
                    if (check != "t")
                        t.Otchestvo = b[i].Otchestvo;
                    else
                        t.Otchestvo = "";
                    t.Login = b[i].Login;
                    t.Password = b[i].Password;
                    t.Gruppa = b[i].NameClass;
                    list2.Add(t);
                }
            }

            if (ch1_2.IsChecked == true && ch2_2.IsChecked == true)
            {
                DataBase.CSVStudentsDataTable b = new DataBase.CSVStudentsDataTable();
                if (onlyuch.IsChecked == true)
                    a.FillOnlyUchas(b);
                if (onlystud.IsChecked == true)
                    a.FillOnlyStudents(b);
                for (int i = 0; i < b.Rows.Count; i++)
                {
                    AllStudentsInfo t = new AllStudentsInfo();
                    t.Id_Student = b[i].ID_Student;
                    t.Familia = b[i].Familia;
                    t.Ima = b[i].Ima;
                    var check = b[i]["Otchestvo"] != DBNull.Value ? b[i]["Otchestvo"].ToString() : "t";
                    if (check != "t")
                        t.Otchestvo = b[i].Otchestvo;
                    else
                        t.Otchestvo = "";

                    check = b[i]["Email"] != DBNull.Value ? b[i]["Email"].ToString() : "t";
                    if (check != "t")
                        t.EmailStudent = b[i].Email;
                    else
                        t.EmailStudent = "";

                    check = b[i]["MobileZakPred"] != DBNull.Value ? b[i]["MobileZakPred"].ToString() : "t";
                    if (check != "t")
                        t.MobilePhoneRoditela = b[i].MobileZakPred;
                    else
                        t.MobilePhoneRoditela = "";
                    t.SNILS = b[i].SNILS;
                    t.Login = b[i].Login;
                    t.Password = b[i].Password;
                    t.Gruppa = b[i].NameClass;
                    list.Add(t);
                }
            }
            if (ch1_2.IsChecked == true && ch2_1.IsChecked == true)
            {
                DataBase.CSVStudentsDataTable b = new DataBase.CSVStudentsDataTable();
                if (onlyuch.IsChecked == true)
                    a.FillOnlyUchas(b);
                if (onlystud.IsChecked == true)
                    a.FillOnlyStudents(b);
                for (int i = 0; i < b.Rows.Count; i++)
                {
                    StudentsAuth t = new StudentsAuth();

                    t.Familia = b[i].Familia;
                    t.Ima = b[i].Ima;
                    var check = b[i]["Otchestvo"] != DBNull.Value ? b[i]["Otchestvo"].ToString() : "t";
                    if (check != "t")
                        t.Otchestvo = b[i].Otchestvo;
                    else
                        t.Otchestvo = "";
                    t.Login = b[i].Login;
                    t.Password = b[i].Password;
                    t.Gruppa = b[i].NameClass;
                    list2.Add(t);
                }
            }


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string FilePath = "";
            saveFileDialog.FileName = "Учащиеся";
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                var wr = new StreamWriter(FilePath, false, Encoding.UTF8);
                if (ch2_1.IsChecked == false && list.Count > 0)
                {
                    using (var csvWriter = new CsvWriter(wr, CultureInfo.InvariantCulture, false))
                    {
                        csvWriter.WriteRecords(list);
                    }
                }
                else if (ch2_2.IsChecked == false && list2.Count > 0)
                {
                    using (var csvWriter = new CsvWriter(wr, CultureInfo.InvariantCulture, false))
                    {
                        csvWriter.WriteRecords(list2);
                    }
                }
                else
                {
                    MessageBox.Show("Отсутствуют записи для сохранения!");
                }
            }
        }
        public class AllStudentsInfo
        {
            public int Id_Student { get; set; }
            public string Familia { get; set; }
            public string Ima { get; set; }
            public string Otchestvo { get; set; }        
            public string Gruppa { get; set; }
            public string SNILS { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public string MobilePhoneRoditela { get; set; }
            public string EmailStudent { get; set; }
        }

        public class StudentsAuth
        {
            public string Familia { get; set; }
            public string Ima { get; set; }
            public string Otchestvo { get; set; }
            public string Gruppa { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
        }
    }
}
