using Kursach.DataBaseTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для DiscipliniPage.xaml
    /// </summary>
    public partial class DiscipliniPage : UserControl
    {
        DiscipliniDataGridTableAdapter a = new DiscipliniDataGridTableAdapter();
        DataBase.DiscipliniDataGridDataTable table = new DataBase.DiscipliniDataGridDataTable();
        public DiscipliniPage()
        {
            InitializeComponent();
            a.Fill(table);
            data_grid.ItemsSource = table;

            List<PrepodavateliModel> list_prepodavateli = new List<PrepodavateliModel>();
            PrepodavateliModel te = new PrepodavateliModel();
            te.ID_Prepodavatel = -1;
            te.FIO_Prepodavatel = "Не выбран";
            list_prepodavateli.Add(te);
            TablePrepodavatelTableAdapter a1 = new TablePrepodavatelTableAdapter();
            DataBase.TablePrepodavatelDataTable b1 = new DataBase.TablePrepodavatelDataTable();
            a1.FillForComboBox(b1);
            for(int i = 0; i < b1.Rows.Count; i++)
            {
                te = new PrepodavateliModel();
                te.ID_Prepodavatel = b1[i].ID_Prepodavatel;
                te.FIO_Prepodavatel = b1[i].FIO;
                list_prepodavateli.Add(te);
            }
            Prepodavatel.ItemsSource = list_prepodavateli;
            Prepodavatel.SelectedIndex = 0;

            List<ClassesModel> list_classes = new List<ClassesModel>();
            ClassesModel ta = new ClassesModel();
            ta.ID_Class = -1;
            ta.NameClass = "Не выбран";
            list_classes.Add(ta);
            TableClassesTableAdapter a2 = new TableClassesTableAdapter();
            DataBase.TableClassesDataTable b2 = new DataBase.TableClassesDataTable();
            a2.FillForComboBox(b2);
            for(int i = 0; i < b2.Rows.Count; i++)
            {
                ta = new ClassesModel();
                ta.ID_Class = b2[i].ID_Class;
                ta.NameClass = b2[i].NameClassParallel;
                list_classes.Add(ta);
            }
            Classes.ItemsSource = list_classes;
            Classes.SelectedIndex = 0;

            Classes.SelectionChanged += search_SelectionChanged;
            Prepodavatel.SelectionChanged += search_SelectionChanged;
            search.SelectionChanged += search_SelectionChanged;
        }

        public class PrepodavateliModel
        {
            public int ID_Prepodavatel { get; set; }
            public string FIO_Prepodavatel { get; set; }
        }

        public class ClassesModel
        {
            public int ID_Class { get; set; }
            public string NameClass { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AdminControl();
        }

        private void search_SelectionChanged(object sender, RoutedEventArgs e)
        {
            DataView dv = new DataView(table);
            string row = "";
            if (search.Text.Trim() != "")
            {
                row = $@"[NameClass] LIKE '%{search.Text}%' or [FIO] LIKE '%{search.Text}%' or [NameDisciplina] LIKE '%{search.Text}%'";
                if (Classes.SelectedIndex != 0)
                    row += $@" AND [Class_ID] = {Classes.SelectedValue}";
                if(Prepodavatel.SelectedIndex != 0)
                    row += $@" AND [Prepodavatel_ID] = {Prepodavatel.SelectedValue}";
            }
            else if (Classes.SelectedIndex != 0 || Prepodavatel.SelectedIndex != 0)
            {
                if(Classes.SelectedIndex != 0 && Prepodavatel.SelectedIndex != 0)
                    row = $@"[Class_ID] = {Classes.SelectedValue} AND [Prepodavatel_ID] = {Prepodavatel.SelectedValue}";
                else if(Classes.SelectedIndex != 0)
                    row = $@"[Class_ID] = {Classes.SelectedValue}";
                else if(Prepodavatel.SelectedIndex != 0)
                    row = $@"[Prepodavatel_ID] = {Prepodavatel.SelectedValue}";
            }
            dv.RowFilter = row;
            data_grid.ItemsSource = dv.ToTable().DefaultView;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AddEditDisciplina(null);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AddEditDisciplina(data_grid.SelectedValue.ToString());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что желаете удалить данную дисцилину?\nВсе результаты тестов студентов будут удалены", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DiscipliniTableAdapter a1 = new DiscipliniTableAdapter();
                a1.DeleteQuery(Convert.ToInt32(data_grid.SelectedValue));

                table = new DataBase.DiscipliniDataGridDataTable();
                a.Fill(table);
                data_grid.ItemsSource = table;

                search.Text = "";
                Prepodavatel.SelectedIndex = 0;
                Classes.SelectedIndex = 0;
            }
        }
    }
}
