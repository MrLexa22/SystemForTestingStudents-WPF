using Kursach.DataBaseTableAdapters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace Kursach.Prepodavatel
{
    /// <summary>
    /// Логика взаимодействия для Lenta.xaml
    /// </summary>
    /// 

    public class DateTimeToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString("dd.MM.yyyy HH:mm");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    public partial class Lenta : UserControl
    {
        public class Tests
        {
            public int ID_Test { get; set; }
            public string Name_Test { get; set; }
            public DateTime SrokSdachi { get; set; }
            public DateTime WhenPublished { get; set; }
            public string Status { get; set; }
        }
        public int ID_Disciplina = 0;
        public List<Tests> list_tests = new List<Tests>();
        DiscipliniTablePrepodavatelTableAdapter aaa = new DiscipliniTablePrepodavatelTableAdapter();
        DataBase.DiscipliniTablePrepodavatelDataTable bbb = new DataBase.DiscipliniTablePrepodavatelDataTable();
        public Lenta(int ID_Disc)
        {
            InitializeComponent();
            ID_Disciplina = ID_Disc;

            aaa.FillByIDDiscipl(bbb, ID_Disciplina);

            Disciplina.Text = bbb[0].NameDisciplina;
            Group.Text = bbb[0].NameClassParallel;
            PublishedTest();
        }

        public void PublishedTest()
        {
            list_tests.Clear();
            aaa.FillByIDDiscipl(bbb, ID_Disciplina);

            TestsPrepodavatelTableAdapter a = new TestsPrepodavatelTableAdapter();
            DataBase.TestsPrepodavatelDataTable b = new DataBase.TestsPrepodavatelDataTable();
            a.FillPublishedTest(b, bbb[0].ID_Disciplina);
            for (int i = 0; i < b.Count; i++)
            {
                Tests t = new Tests();
                t.ID_Test = b[i].ID_Test;
                t.Name_Test = b[i].Name_Test;
                t.SrokSdachi = b[i].SrokSdachi;
                t.WhenPublished = b[i].WhenPublished;
                if (t.SrokSdachi <= DateTime.Now)
                    t.Status = "Завершён";
                else
                    t.Status = "В процессе выполнения";
                list_tests.Add(t);
            }
            tests_Lenta.ItemsSource = list_tests;
        }

        public void ChernovikiTest()
        {
            list_tests.Clear();
            aaa.FillByIDDiscipl(bbb, ID_Disciplina);

            TestsPrepodavatelTableAdapter a = new TestsPrepodavatelTableAdapter();
            DataBase.TestsPrepodavatelDataTable b = new DataBase.TestsPrepodavatelDataTable();
            a.FillChernoviki(b, bbb[0].ID_Disciplina);
            for (int i = 0; i < b.Count; i++)
            {
                Tests t = new Tests();
                t.ID_Test = b[i].ID_Test;
                t.Name_Test = b[i].Name_Test;
                list_tests.Add(t);
            }
            DataContext = list_tests;
            tests_Chernovik.ItemsSource = list_tests;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new UpravlenieTests();
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                return;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new CreateTest(null, ID_Disciplina);
        }
        public void AllTrueButtons()
        {
            Lentas.IsEnabled = true;
            Chernoviki.IsEnabled = true;
            Results.IsEnabled = true;
        }
        private void Lenta_Click(object sender, RoutedEventArgs e)
        {
            AllTrueButtons();
            Lentas.IsEnabled = false;
            tests_Chernovik.Visibility = Visibility.Hidden;
            tests_Lenta.Visibility = Visibility.Visible;
            data_grid.Visibility = Visibility.Hidden;
            PublishedTest();
        }

        private void Chernoviki_Click(object sender, RoutedEventArgs e)
        {
            AllTrueButtons();
            Chernoviki.IsEnabled = false;
            tests_Chernovik.Visibility = Visibility.Visible;
            tests_Lenta.Visibility = Visibility.Hidden;
            data_grid.Visibility = Visibility.Hidden;
            ChernovikiTest();
        }

        private void Results_Click(object sender, RoutedEventArgs e)
        {
            AllTrueButtons();
            ChernovikiTest();
            try
            {
                Results.IsEnabled = false;
                tests_Chernovik.Visibility = Visibility.Hidden;
                tests_Lenta.Visibility = Visibility.Hidden;
                data_grid.Visibility = Visibility.Visible;

                DiscipliniTableAdapter a = new DiscipliniTableAdapter();
                DataBase.DiscipliniDataTable b = new DataBase.DiscipliniDataTable();
                a.FillByID(b, ID_Disciplina);

                StudentsTableTableAdapter a1 = new StudentsTableTableAdapter();
                DataBase.StudentsTableDataTable b1 = new DataBase.StudentsTableDataTable();
                a1.FillByIDClass(b1, b[0].Class_ID);

                DataTable data = new DataTable();
                data.Columns.Add("Фамилия Имя Отчество");

                TestTableAdapter at = new TestTableAdapter();
                DataBase.TestDataTable bt = new DataBase.TestDataTable();
                at.FillByIDDiscipli(bt, ID_Disciplina);
                for (int i = 0; i < bt.Rows.Count; i++)
                {
                    data.Columns.Add(bt[i].Name_Test);
                }
                data.Columns.Add("Средний балл");

                for (int i = 0; i < b1.Rows.Count; i++)
                {
                    DataRow f = data.NewRow();
                    f["Фамилия Имя Отчество"] = b1[i].FIO;
                    double sr_ball = 0;
                    int count_sk = 0;
                    for (int j = 0; j < bt.Rows.Count; j++)
                    {
                        ResultsTestTableAdapter ra = new ResultsTestTableAdapter();
                        DataBase.ResultsTestDataTable rb = new DataBase.ResultsTestDataTable();
                        ra.FillByIDST(rb, bt[j].ID_Test, b1[i].ID_Student);
                        if (rb.Rows.Count > 0)
                        {
                            var check = rb[0]["Ochenka"] != DBNull.Value ? rb[0]["Ochenka"].ToString() : "t";
                            if (check != "t")
                            {
                                f[bt[j].Name_Test] = rb[0].Ochenka.ToString();
                                count_sk++;
                                sr_ball += rb[0].Ochenka;
                            }
                            else
                            {
                                f[bt[j].Name_Test] = "Не проверено";
                            }
                        }
                        else
                        {
                            f[bt[j].Name_Test] = "Не сдано";
                        }
                    }
                    if (sr_ball > 0)
                        f["Средний балл"] = Math.Round((sr_ball / count_sk), 2).ToString();
                    else
                        f["Средний балл"] = "0";
                    data.Rows.Add(f);
                }
                data_grid.ItemsSource = data.DefaultView;
                data_grid.Columns[0].Width = 250;
                for (int i = 1; i<data_grid.Columns.Count; i++)
                {
                    data_grid.Columns[i].Width = 130;
                }
            }
            catch { data_grid.Visibility = Visibility.Hidden; }
        }

        private void tests_Chernovik_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Controllers.fram_prep.Content = new CreateTest(tests_Chernovik.SelectedValue.ToString(), ID_Disciplina);
        }

        private void tests_Lenta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Controllers.fram_prep.Content = new ResultTest(Convert.ToInt16(tests_Lenta.SelectedValue));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new DupicateTest(ID_Disciplina);
        }
    }
}
