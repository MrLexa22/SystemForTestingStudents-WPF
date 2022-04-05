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
    /// Логика взаимодействия для ClassuGrupiPage.xaml
    /// </summary>
    public partial class ClassuGrupiPage : UserControl
    {
        public List<ModelClassesGroups> list = new List<ModelClassesGroups>();
        public ClassuGrupiPage()
        {
            InitializeComponent();
            fillPage();
        }

        public void fillPage()
        {
            TableParalleliTableAdapter a = new TableParalleliTableAdapter();
            DataBase.TableParalleliDataTable b = new DataBase.TableParalleliDataTable();
            a.Fill(b);
            for (int i = 0; i < b.Rows.Count; i++)
            {
                if (b[i].HasClasses != 0)
                {
                    ModelClassesGroups t = new ModelClassesGroups();
                    t.ClassesList = new List<ModelListClasses>();
                    t.NameParallel = b[i].ParallelName;
                    t.IsCours = b[i].IsCourse;
                    TableClassesDataGridTableAdapter a1 = new TableClassesDataGridTableAdapter();
                    DataBase.TableClassesDataGridDataTable b1 = new DataBase.TableClassesDataGridDataTable();
                    a1.FillByIDParallel(b1, b[i].ID_Parallel);
                    for (int j = 0; j < b1.Rows.Count; j++)
                    {
                        ModelListClasses t1 = new ModelListClasses();
                        t1.ID_Class = b1[j].ID_Class;
                        t1.NameClass = b1[j].NameClassParallel;
                        t1.KolichestvoUcha = b1[j].KolvoUchashisa;
                        t.ClassesList.Add(t1);
                    }
                    list.Add(t);
                }
            }
            DataContext = list;
        }

        public class ModelClassesGroups
        {
            public string NameParallel { get; set; }
            public bool IsCours { get; set; }
            public List<ModelListClasses> ClassesList { get; set; }
        }
        public class ModelListClasses
        {
            public string NameClass { get; set; }
            public string KolichestvoUcha { get; set; }
            public int ID_Class { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AdminControl();
        }

        private void Cours_Checked(object sender, RoutedEventArgs e)
        {
            List<ModelClassesGroups> newlist = new List<ModelClassesGroups>();
            newlist = list;
            if(Cours.IsChecked == true && !(Cours.IsChecked == true && Class.IsChecked == true))
            {
                var evens1 = newlist.Where(i => i.IsCours == true);
                DataContext = evens1;
            }
            else if (Class.IsChecked == true && !(Cours.IsChecked == true && Class.IsChecked == true))
            {
                var evens1 = newlist.Where(i => i.IsCours == false);
                DataContext = evens1;
            }
            else
            {
                DataContext = list;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string d = ((sender as Button).Tag).ToString();
            Controllers.fram.Content = new AddEditClassGroup(d, false);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AddEditClassGroup(null, true);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AddEditClassGroup(null, false);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что желаете удалить данный класс?\nВсе дисциплины, студенты и их результаты, которые закреплены за данной параллелью, будут удалены", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ClassesTableAdapter adapter = new ClassesTableAdapter();
                adapter.DeleteQuery(Convert.ToInt32((sender as Button).Tag));

                UsersTableAdapter a1 = new UsersTableAdapter();
                a1.DeleteUsersLishnie();

                list = new List<ModelClassesGroups>();
                fillPage();

                Cours.IsChecked = false;
                Class.IsChecked = false;
            }
        }
    }
}
