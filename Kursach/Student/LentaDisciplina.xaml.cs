using Kursach.DataBaseTableAdapters;
using System;
using System.Collections.Generic;
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

namespace Kursach.Student
{
    /// <summary>
    /// Логика взаимодействия для LentaDisciplina.xaml
    /// </summary>
    public partial class LentaDisciplina : UserControl
    {
        public int ID_Disciplina = 0;
        public class Tests
        {
            public int ID_Test { get; set; }
            public string Name_Test { get; set; }
            public DateTime SrokSdachi { get; set; }
            public DateTime WhenPublished { get; set; }
            public string Status { get; set; }
        }
        public List<Tests> list_tests = new List<Tests>();

        DiscipliniTablePrepodavatelTableAdapter aaa = new DiscipliniTablePrepodavatelTableAdapter();
        DataBase.DiscipliniTablePrepodavatelDataTable bbb = new DataBase.DiscipliniTablePrepodavatelDataTable();
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
        public LentaDisciplina(int ID_Disc)
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

            StudentAnswerTableAdapter ass = new StudentAnswerTableAdapter();
            ResultsTestTableAdapter asss = new ResultsTestTableAdapter();

            StudentsTableTableAdapter abb = new StudentsTableTableAdapter();
            DataBase.StudentsTableDataTable baa = new DataBase.StudentsTableDataTable();
            abb.FillByLogin(baa, Controllers.auth_login);

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
                if (ass.FindStudentsAsnwersExist(b[i].ID_Test, baa[0].ID_Student) > 0)
                    t.Status = "Выполнен";
                if (asss.FindExist(baa[0].ID_Student, b[i].ID_Test) > 0)
                    t.Status = "Выполнен";
                list_tests.Add(t);
            }
            tests_Lenta.ItemsSource = list_tests;
        }

        public void AllTrueButtons()
        {
            Lentas.IsEnabled = true;
            Results.IsEnabled = true;
        }

        private void Lenta_Click(object sender, RoutedEventArgs e)
        {
            AllTrueButtons();
            Lentas.IsEnabled = false;
            tests_Lenta.Visibility = Visibility.Visible;
            data_grid.Visibility = Visibility.Hidden;
            PublishedTest();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new StudentListDisciplins();
        }

        private void Results_Click(object sender, RoutedEventArgs e)
        {
            AllTrueButtons();
            try
            {
                StudentsTableTableAdapter aa1 = new StudentsTableTableAdapter();
                DataBase.StudentsTableDataTable bb1 = new DataBase.StudentsTableDataTable();
                aa1.FillByLogin(bb1, Controllers.auth_login);

                Results.IsEnabled = false;
                tests_Lenta.Visibility = Visibility.Hidden;
                data_grid.Visibility = Visibility.Visible;

                DiscipliniTableAdapter a = new DiscipliniTableAdapter();
                DataBase.DiscipliniDataTable b = new DataBase.DiscipliniDataTable();
                a.FillByID(b, ID_Disciplina);

                StudentsTableTableAdapter a1 = new StudentsTableTableAdapter();
                DataBase.StudentsTableDataTable b1 = new DataBase.StudentsTableDataTable();
                a1.FillBy22(b1, b[0].Class_ID, bb1[0].ID_Student);

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
                for (int i = 1; i < data_grid.Columns.Count; i++)
                {
                    data_grid.Columns[i].Width = 130;
                }
            }
            catch { data_grid.Visibility = Visibility.Hidden; }
        }

        private void tests_Lenta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tests_Lenta.SelectedValue == null)
                return;
            TEstTableTableAdapter a = new TEstTableTableAdapter();
            DataBase.TEstTableDataTable b = new DataBase.TEstTableDataTable();
            a.FillByID(b, Convert.ToInt16(tests_Lenta.SelectedValue));

            StudentAnswerTableAdapter ass = new StudentAnswerTableAdapter();
            ResultsTestTableAdapter asss = new ResultsTestTableAdapter();
            StudentsTableTableAdapter abb = new StudentsTableTableAdapter();
            DataBase.StudentsTableDataTable baa = new DataBase.StudentsTableDataTable();
            abb.FillByLogin(baa, Controllers.auth_login);
            if (ass.FindStudentsAsnwersExist(Convert.ToInt16(tests_Lenta.SelectedValue), baa[0].ID_Student) > 0)
            {
                MessageBox.Show("Вы уже выполнили данный тест!");
                return;
            }
            if (asss.FindExist(baa[0].ID_Student, Convert.ToInt16(tests_Lenta.SelectedValue)) > 0)
            {
                MessageBox.Show("Вы уже выполнили данный тест!");
                return;
            }

            if (b[0].SrokSdachi <= DateTime.Now)
            {
                MessageBox.Show("Вы не успели завершить тест до завершения срока сдачи!");
                return;
            }

            Controllers.fram_prep.Content = new PassingTest(Convert.ToInt16(tests_Lenta.SelectedValue));
        }
    }
}
