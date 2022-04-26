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

namespace Kursach.Prepodavatel
{
    /// <summary>
    /// Логика взаимодействия для StudentsInfoPage.xaml
    /// </summary>
    public partial class StudentsInfoPage : UserControl
    {
        public class StudentsInfo
        {
            public string FIO { get; set; }
            public string Email { get; set; }
            public string SNILS { get; set; }
            public string MobileZakPred { get; set; }
        }
        public StudentsInfoPage()
        {
            List<StudentsInfo> list = new List<StudentsInfo>();
            InitializeComponent();

            PrepodavatelSelectedTableAdapter a = new PrepodavatelSelectedTableAdapter();
            DataBase.PrepodavatelSelectedDataTable b = new DataBase.PrepodavatelSelectedDataTable();
            a.FillByLoginPrepodavatel(b, Controllers.auth_login);

            TableClassesTableAdapter a1 = new TableClassesTableAdapter();
            DataBase.TableClassesDataTable b1 = new DataBase.TableClassesDataTable();
            a1.FindClassByID(b1, b[0].Class_ID);
            grupa_class.Text = b1[0].NameClassParallel;

            TableStudentsTableAdapter a2 = new TableStudentsTableAdapter();
            DataBase.TableStudentsDataTable b2 = new DataBase.TableStudentsDataTable();
            a2.FillByIDClass(b2,b[0].Class_ID);

            for(int i = 0; i<b2.Rows.Count; i++)
            {
                StudentsInfo st = new StudentsInfo();
                st.SNILS = b2[i].SNILS;
                st.FIO = b2[i].FIO;
                var check = b2[i]["MobileZakPred"] != DBNull.Value ? b2[i]["MobileZakPred"].ToString() : "t";
                if (check != "t")
                    st.MobileZakPred = b2[i].MobileZakPred;
                else
                    st.MobileZakPred = "Не указан";
                check = b2[i]["Email"] != DBNull.Value ? b2[i]["Email"].ToString() : "t";
                if(check != "t")
                    st.Email = b2[i].Email;
                else
                    st.Email = "Не указан";
                list.Add(st);
            }

            data_grid.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new UpravlenieClRukovod();
        }
    }
}
