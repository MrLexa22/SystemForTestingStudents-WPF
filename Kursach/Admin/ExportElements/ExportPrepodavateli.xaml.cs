using CsvHelper;
using Kursach.DataBaseTableAdapters;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Логика взаимодействия для ExportPrepodavateli.xaml
    /// </summary>
    public partial class ExportPrepodavateli : UserControl
    {
        public ExportPrepodavateli()
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
            List<PrepodavatelAllDannie> list = new List<PrepodavatelAllDannie>();
            List<PrepodavatelAuthen> list2 = new List<PrepodavatelAuthen>();
            CSVPrepodavateliTableAdapter a = new CSVPrepodavateliTableAdapter();
            if (ch1_1.IsChecked == true && ch2_2.IsChecked == true)
            {
                DataBase.CSVPrepodavateliDataTable b = new DataBase.CSVPrepodavateliDataTable();
                a.FillAllPrepodav(b);
                for (int i = 0; i < b.Rows.Count; i++)
                {
                    PrepodavatelAllDannie t = new PrepodavatelAllDannie();
                    t.Id_Prepodavatel = b[i].ID_Prepodavatel;
                    t.Familia = b[i].Familia;
                    t.Ima = b[i].Ima;
                    var check = b[i]["Otchestvo"] != DBNull.Value ? b[i]["Otchestvo"].ToString() : "t";
                    if (check != "t")
                        t.Otchestvo = b[i].Otchestvo;
                    else
                        t.Otchestvo = "";
                    t.SNILS = b[i].SNILS;
                    t.Login = b[i].Login;
                    t.Password = b[i].Password;
                    t.SeriaPasporta = b[i].SeriaPasporta;
                    t.NomerPasporta = b[i].NomerPasporta;
                    t.INN = b[i].INN;
                    t.TelefonNumber = b[i].TelefonNumber;
                    list.Add(t);
                }
            }
            if(ch1_1.IsChecked == true && ch2_1.IsChecked == true)
            {
                DataBase.CSVPrepodavateliDataTable b = new DataBase.CSVPrepodavateliDataTable();
                a.FillAllPrepodav(b);
                for (int i = 0; i < b.Rows.Count; i++)
                {
                    PrepodavatelAuthen t = new PrepodavatelAuthen();

                    t.Familia = b[i].Familia;
                    t.Ima = b[i].Ima;
                    var check = b[i]["Otchestvo"] != DBNull.Value ? b[i]["Otchestvo"].ToString() : "t";
                    if (check != "t")
                        t.Otchestvo = b[i].Otchestvo;
                    else
                        t.Otchestvo = "";
                    t.Login = b[i].Login;
                    t.Password = b[i].Password;
                    list2.Add(t);
                }
            }
            if(ch1_2.IsChecked == true && ch2_2.IsChecked == true)
            {
                DataBase.CSVPrepodavateliDataTable b = new DataBase.CSVPrepodavateliDataTable();
                if (onlycl.IsChecked == true)
                    a.FillOnlyClR(b);
                if (onlyprepods.IsChecked == true)
                     a.FillOnlyPrep(b);
                for (int i = 0; i < b.Rows.Count; i++)
                {
                    PrepodavatelAllDannie t = new PrepodavatelAllDannie();
                    t.Id_Prepodavatel = b[i].ID_Prepodavatel;
                    t.Familia = b[i].Familia;
                    t.Ima = b[i].Ima;
                    var check = b[i]["Otchestvo"] != DBNull.Value ? b[i]["Otchestvo"].ToString() : "t";
                    if (check != "t")
                        t.Otchestvo = b[i].Otchestvo;
                    else
                        t.Otchestvo = "";
                    t.SNILS = b[i].SNILS;
                    t.Login = b[i].Login;
                    t.Password = b[i].Password;
                    t.SeriaPasporta = b[i].SeriaPasporta;
                    t.NomerPasporta = b[i].NomerPasporta;
                    t.INN = b[i].INN;
                    t.TelefonNumber = b[i].TelefonNumber;
                    list.Add(t);
                }
            }
            if(ch1_2.IsChecked == true && ch2_1.IsChecked == true)
            {
                DataBase.CSVPrepodavateliDataTable b = new DataBase.CSVPrepodavateliDataTable();
                if (onlycl.IsChecked == true)
                    a.FillOnlyClR(b);
                if (onlyprepods.IsChecked == true)
                    a.FillOnlyPrep(b);
                for (int i = 0; i < b.Rows.Count; i++)
                {
                    PrepodavatelAuthen t = new PrepodavatelAuthen();

                    t.Familia = b[i].Familia;
                    t.Ima = b[i].Ima;
                    var check = b[i]["Otchestvo"] != DBNull.Value ? b[i]["Otchestvo"].ToString() : "t";
                    if (check != "t")
                        t.Otchestvo = b[i].Otchestvo;
                    else
                        t.Otchestvo = "";
                    t.Login = b[i].Login;
                    t.Password = b[i].Password;
                    list2.Add(t);
                }
            }

            
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string FilePath = "";
            saveFileDialog.FileName = "Преподаватели";
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

        public class PrepodavatelAuthen
        {
            public string Familia { get; set; }
            public string Ima { get; set; }
            public string Otchestvo { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
        }
        public class PrepodavatelAllDannie
        {
            public int Id_Prepodavatel { get; set; }
            public string Familia { get; set; }
            public string Ima { get; set; }
            public string Otchestvo { get; set; }
            public string SNILS { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public string SeriaPasporta { get; set; }
            public string NomerPasporta { get; set; }
            public string INN { get; set; }
            public string TelefonNumber { get; set; }
        }
    }
}
