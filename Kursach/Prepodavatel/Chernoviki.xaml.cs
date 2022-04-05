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
    /// Логика взаимодействия для Chernoviki.xaml
    /// </summary>
    public partial class Chernoviki : UserControl
    {
        public class Classes
        {
            public int ID_Class { get; set; }
            public string GroupName { get; set; }
        }
        public class Tests
        {
            public int ID_Test { get; set; }
            public string Name_Test { get; set; }
            public string NameClassParallel { get; set; }
            public string NameDisciplina { get; set; }
            public int ID_Disciplina {get;set;}
            public int ID_Class { get; set; }

        }
        
        public List<Classes> list_cl = new List<Classes>();
        public List<Classes> list_disciplins = new List<Classes>();
        public List<Tests> list_tests = new List<Tests>();

        public int ID_Prepodavatel = 0;
        public Chernoviki()
        {
            InitializeComponent();
            TablePrepodavatelTableAdapter a = new TablePrepodavatelTableAdapter();
            DataBase.TablePrepodavatelDataTable b = new DataBase.TablePrepodavatelDataTable();
            a.FillByLogin(b, Controllers.auth_login);
            ID_Prepodavatel = b[0].ID_Prepodavatel;

            AllTestsPrepodTableAdapter a1 = new AllTestsPrepodTableAdapter();
            DataBase.AllTestsPrepodDataTable b1 = new DataBase.AllTestsPrepodDataTable();
            a1.Fill(b1, b[0].ID_Prepodavatel);
            for(int i = 0; i < b1.Count; i++)
            {
                Tests t = new Tests();
                t.ID_Test = b1[i].ID_Test;
                t.Name_Test = b1[i].Name_Test;
                t.NameClassParallel = b1[i].NameClassParallel;
                t.NameDisciplina = b1[i].NameDisciplina;
                t.ID_Class = b1[i].ID_Class;
                t.ID_Disciplina = b1[i].ID_Disciplina;
                list_tests.Add(t);
            }
            data_grid.ItemsSource = list_tests;

            TableClassesTableAdapter a11 = new TableClassesTableAdapter();
            DataBase.TableClassesDataTable b11 = new DataBase.TableClassesDataTable();
            a11.FillForCB(b11, b[0].ID_Prepodavatel);

            Classes tt = new Classes();
            tt.ID_Class = -1;
            tt.GroupName = "Не выбрано";
            list_cl.Add(tt);

            for (int i = 0; i<b11.Rows.Count; i++)
            {
                Classes t = new Classes();
                t.ID_Class = b11[i].ID_Class;
                t.GroupName = b11[i].GroupName;
                list_cl.Add(t);
            }
            groups.ItemsSource = list_cl;
            groups.SelectionChanged += Groups_SelectionChanged;
            groups.SelectionChanged += Disciplins_SelectionChanged;
            disciplins.SelectionChanged += Disciplins_SelectionChanged;
        }

        public void UpdateSpisok()
        {
            if (disciplins.SelectedIndex == 0)
                disciplins.SelectedIndex = -1;

            data_grid.ItemsSource = list_tests;
            if (groups.SelectedIndex > 0)
                data_grid.ItemsSource = list_tests.Where(g => g.ID_Class == Convert.ToInt32(groups.SelectedValue));
            if (groups.SelectedIndex > 0 && disciplins.SelectedIndex > 0)
                data_grid.ItemsSource = list_tests.Where(g => g.ID_Class == Convert.ToInt32(groups.SelectedValue) && g.ID_Disciplina == Convert.ToInt32(disciplins.SelectedValue));
        }

        private void Disciplins_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSpisok();
        }

        private void Groups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(groups.SelectedIndex > 0)
            {
                TablePrepodavatelTableAdapter a = new TablePrepodavatelTableAdapter();
                DataBase.TablePrepodavatelDataTable b = new DataBase.TablePrepodavatelDataTable();
                a.FillByLogin(b, Controllers.auth_login);

                DiscipliniTableAdapter a1 = new DiscipliniTableAdapter();
                DataBase.DiscipliniDataTable b1 = new DataBase.DiscipliniDataTable();
                a1.FillFroCBTest(b1, Convert.ToInt16(groups.SelectedValue), b[0].ID_Prepodavatel);

                list_disciplins = new List<Classes>();
                Classes tt = new Classes();
                tt.ID_Class = -1;
                tt.GroupName = "Не выбрано";
                list_disciplins.Add(tt);
                for (int i = 0; i < b1.Rows.Count; i++)
                {
                    Classes t = new Classes();
                    t.ID_Class = b1[i].ID_Disciplina;
                    t.GroupName = b1[i].ShortName;
                    list_disciplins.Add(t);
                }
                disciplins.ItemsSource = list_disciplins;
            }
            else
            {
                disciplins.ItemsSource = null;
                disciplins.SelectedIndex = -1;
                groups.SelectedIndex = -1;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new UpravlenieTests();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new CreateTest(data_grid.SelectedValue.ToString());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что желаете удалить данный черновик?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                TestTableAdapter a = new TestTableAdapter();
                a.DeleteQuery(Convert.ToInt16(data_grid.SelectedValue));
                list_tests.RemoveAll(g => g.ID_Test == Convert.ToInt16(data_grid.SelectedValue));
                data_grid.ItemsSource = null;
                data_grid.ItemsSource = list_tests;
                UpdateSpisok();
            }
        }
    }
}
