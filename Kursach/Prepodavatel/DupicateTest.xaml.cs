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
    /// Логика взаимодействия для DupicateTest.xaml
    /// </summary>
    public partial class DupicateTest : UserControl
    {
        public int ID_Disciplina;

        public class ModelDisciplin
        {
            public int ID_Disciplina { get; set; }
            public string Name { get; set; }
        }
        public DupicateTest(int ID_Disc)
        {
            InitializeComponent();
            ID_Disciplina = ID_Disc;
            variant1_rb.Checked += variant1_rb_Checked;
            variant2_rb.Checked += variant1_rb_Checked;
            variant1_rb.Unchecked += variant1_rb_Checked;
            variant2_rb.Unchecked += variant1_rb_Checked;
            variant1_rb.IsChecked = true;
        }
        private void variant1_rb_Checked(object sender, RoutedEventArgs e)
        {
            variant1.Visibility = Visibility.Hidden;
            variant2.Visibility = Visibility.Hidden;
            if (variant1_rb.IsChecked == true)
            {
                variant1.Visibility = Visibility.Visible;

                DiscipliniTablePrepodavatelTableAdapter a = new DiscipliniTablePrepodavatelTableAdapter();
                DataBase.DiscipliniTablePrepodavatelDataTable b = new DataBase.DiscipliniTablePrepodavatelDataTable();
                a.FillByIDDiscipl(b, ID_Disciplina);
                List<ModelDisciplin> list_disc = new List<ModelDisciplin>();
                tek_disciplina.Text = b[0].NameClassParallel + " " + b[0].ShortName;

                list_disc.Clear();
                a.FillBy(b, b[0].Prepodavatel_ID, ID_Disciplina);
                list_disc = new List<ModelDisciplin>();
                for (int i = 0; i < b.Rows.Count; i++)
                {
                    ModelDisciplin disciplin = new ModelDisciplin();
                    disciplin.Name = b[i].NameClassParallel + " " + b[i].ShortName;
                    disciplin.ID_Disciplina = b[i].ID_Disciplina;
                    list_disc.Add(disciplin);
                }
                disciplins_import.ItemsSource = list_disc;
            }
            if (variant2_rb.IsChecked == true)
            {
                variant2.Visibility = Visibility.Visible;

                DiscipliniTablePrepodavatelTableAdapter a = new DiscipliniTablePrepodavatelTableAdapter();
                DataBase.DiscipliniTablePrepodavatelDataTable b = new DataBase.DiscipliniTablePrepodavatelDataTable();
                a.FillByIDDiscipl(b, ID_Disciplina);
                disciplins_tek.Text = b[0].NameClassParallel + " " + b[0].ShortName;

                TestTableAdapter a1 = new TestTableAdapter();
                DataBase.TestDataTable b1 = new DataBase.TestDataTable();
                a1.FillByIDDiscNot(b1, b[0].ID_Disciplina);
                tests_disciplin2.ItemsSource = b1;

                int Id_pr = b[0].Prepodavatel_ID;
                b = new DataBase.DiscipliniTablePrepodavatelDataTable();
                a.FillBy(b, Id_pr, ID_Disciplina);
                List<ModelDisciplin> list_disc = new List<ModelDisciplin>();
                for (int i = 0; i < b.Rows.Count; i++)
                {
                    ModelDisciplin disciplin = new ModelDisciplin();
                    disciplin.Name = b[i].NameClassParallel + " " + b[i].ShortName;
                    disciplin.ID_Disciplina = b[i].ID_Disciplina;
                    list_disc.Add(disciplin);
                }
                disciplins_export.ItemsSource = list_disc;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new Lenta(ID_Disciplina);
        }

        private void disciplins_import_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TestTableAdapter a = new TestTableAdapter();
            DataBase.TestDataTable b = new DataBase.TestDataTable();
            a.FillByIDDiscNot(b, Convert.ToInt32(disciplins_import.SelectedValue));
            test_by_disciplins.ItemsSource = b;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Кнопка импорта
            if(disciplins_import.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите дисциплину!");
                return;
            }
            if (test_by_disciplins.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите тест для импорта!");
                return;
            }
            name_test.Text = name_test.Text.Trim();
            if (name_test.Text.Length < 2)
            {
                MessageBox.Show("Название теста введено некорректо!");
                return;
            }

            DiscipliniTablePrepodavatelTableAdapter a = new DiscipliniTablePrepodavatelTableAdapter();
            DataBase.DiscipliniTablePrepodavatelDataTable b = new DataBase.DiscipliniTablePrepodavatelDataTable();
            a.FillByIDDiscipl(b, ID_Disciplina);

            TEstTableTableAdapter a1 = new TEstTableTableAdapter();
            DataBase.TEstTableDataTable b1 = new DataBase.TEstTableDataTable();
            a1.Fill(b1, name_test.Text, b[0].ID_Disciplina, b[0].Class_ID, b[0].Prepodavatel_ID);
            if (b1.Rows.Count > 0)
            {
                MessageBox.Show("Введённое наименование теста уже существует в данном курсе. Введите другое название");
                return;
            }

            TestTableAdapter ta = new TestTableAdapter();
            DataBase.TestDataTable tb = new DataBase.TestDataTable();
            ta.FillByInserTestCopy(tb, name_test.Text, b[0].ID_Disciplina);

            VoprosTableAdapter va = new VoprosTableAdapter();
            DataBase.VoprosDataTable vb = new DataBase.VoprosDataTable();
            va.FillByIDTest(vb, Convert.ToInt16(test_by_disciplins.SelectedValue));

            for(int i = 0; vb.Rows.Count > i; i++)
            {
                DataBase.VoprosDataTable bb = new DataBase.VoprosDataTable();
                va.FillByCopy(bb, vb[i].NameVopros, tb[0].ID_Test, vb[i].TypeVopros, vb[i].Ball);

                AnswerTableAdapter ass = new AnswerTableAdapter();
                DataBase.AnswerDataTable abb = new DataBase.AnswerDataTable();
                ass.FillByIDVopros(abb, vb[0].ID_Vopros);

                for(int j = 0; j<abb.Rows.Count; j++)
                {
                    ass.InsertQueryCopy(abb[j].TextAnswer, abb[j].IsTrue, bb[0].ID_Vopros);
                }
            }

            MessageBox.Show("Тест успешно скопирован! Тест сохранён в черновики");
            Controllers.fram_prep.Content = new CreateTest(tb[0].ID_Test.ToString(), b[0].ID_Disciplina);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Кнопка экспорта
            if (disciplins_export.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите дисциплину!");
                return;
            }
            if (tests_disciplin2.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите тест для экспорта!");
                return;
            }
            test_name.Text = test_name.Text.Trim();
            if (test_name.Text.Length < 2)
            {
                MessageBox.Show("Название теста введено некорректо!");
                return;
            }

            DiscipliniTablePrepodavatelTableAdapter a = new DiscipliniTablePrepodavatelTableAdapter();
            DataBase.DiscipliniTablePrepodavatelDataTable b = new DataBase.DiscipliniTablePrepodavatelDataTable();
            a.FillByIDDiscipl(b, Convert.ToInt16(disciplins_export.SelectedValue));

            TEstTableTableAdapter a1 = new TEstTableTableAdapter();
            DataBase.TEstTableDataTable b1 = new DataBase.TEstTableDataTable();
            a1.Fill(b1, test_name.Text, b[0].ID_Disciplina, b[0].Class_ID, b[0].Prepodavatel_ID);
            if (b1.Rows.Count > 0)
            {
                MessageBox.Show("Введённое наименование теста уже существует в данном курсе. Введите другое название");
                return;
            }

            TestTableAdapter ta = new TestTableAdapter();
            DataBase.TestDataTable tb = new DataBase.TestDataTable();
            ta.FillByInserTestCopy(tb, test_name.Text, b[0].ID_Disciplina);

            VoprosTableAdapter va = new VoprosTableAdapter();
            DataBase.VoprosDataTable vb = new DataBase.VoprosDataTable();
            va.FillByIDTest(vb, Convert.ToInt16(tests_disciplin2.SelectedValue));

            for (int i = 0; i< vb.Rows.Count; i++)
            {
                DataBase.VoprosDataTable bb = new DataBase.VoprosDataTable();
                va.FillByCopy(bb, vb[i].NameVopros, tb[0].ID_Test, vb[i].TypeVopros, vb[i].Ball);

                AnswerTableAdapter ass = new AnswerTableAdapter();
                DataBase.AnswerDataTable abb = new DataBase.AnswerDataTable();
                ass.FillByIDVopros(abb, vb[0].ID_Vopros);

                for (int j = 0; j < abb.Rows.Count; j++)
                {
                    ass.InsertQueryCopy(abb[j].TextAnswer, abb[j].IsTrue, bb[0].ID_Vopros);
                }
            }

            MessageBox.Show("Тест успешно скопирован! Тест сохранён в черновики");
            Controllers.fram_prep.Content = new CreateTest(tb[0].ID_Test.ToString(), b[0].ID_Disciplina);
        }
    }
}
