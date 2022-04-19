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
            PublishedTest();
        }

        private void Chernoviki_Click(object sender, RoutedEventArgs e)
        {
            AllTrueButtons();
            Chernoviki.IsEnabled = false;
            tests_Chernovik.Visibility = Visibility.Visible;
            tests_Lenta.Visibility = Visibility.Hidden;
            ChernovikiTest();
        }

        private void Results_Click(object sender, RoutedEventArgs e)
        {
            AllTrueButtons();
            Results.IsEnabled = false;
        }

        private void tests_Chernovik_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Controllers.fram_prep.Content = new CreateTest(tests_Chernovik.SelectedValue.ToString(), ID_Disciplina);
        }

        private void tests_Lenta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Controllers.fram_prep.Content = new ResultTest(Convert.ToInt16(tests_Lenta.SelectedValue));
        }
    }
}
