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
    /// Логика взаимодействия для ParalleliPage.xaml
    /// </summary>
    public partial class ParalleliPage : UserControl
    {
        DataBase.TableParalleliDataGridDataTable table = new DataBase.TableParalleliDataGridDataTable();
        public ParalleliPage()
        {
            InitializeComponent();
            TableParalleliDataGridTableAdapter a = new TableParalleliDataGridTableAdapter();
            a.Fill(table);
            data_grid.ItemsSource = table;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AdminControl();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AddEditCourseClass(null,true);
        }

        private void search_SelectionChanged(object sender, RoutedEventArgs e)
        {
            DataView dv = new DataView(table);
            string row = "";
            if (search.Text.Trim() != "")
            {
                row = $@"[KolvoUchashisa] LIKE '%{search.Text}%' or [NameParallel] LIKE '%{search.Text}%'";
                if(Cours.IsChecked == true && Class.IsChecked == true)
                {
                    row += " AND ([IsCourse] = 1 OR [IsCourse] = 0)";
                }
                else
                {
                    if (Cours.IsChecked == true)
                    {
                        row += " AND [IsCourse] = 1";
                    }
                    if (Class.IsChecked == true)
                    {
                        row += " AND [IsCourse] = 0";
                    }
                }
            }
            else if (Cours.IsChecked == true && Class.IsChecked == true)
            {
                row += "[IsCourse] = 1 OR [IsCourse] = 0";
            }
            else if(Cours.IsChecked == true)
            {
                row += "[IsCourse] = 1";
            }
            else if (Class.IsChecked == true)
            {
                row += "[IsCourse] = 0";
            }
            dv.RowFilter = row;
            data_grid.ItemsSource = dv.ToTable().DefaultView;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AddEditCourseClass(null, false);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ParallelTableAdapter adapter = new ParallelTableAdapter();
            DataBase.ParallelDataTable b = new DataBase.ParallelDataTable();
            adapter.FindByID(b, Convert.ToInt32(data_grid.SelectedValue));
            Controllers.fram.Content = new AddEditCourseClass(data_grid.SelectedValue.ToString(), b[0].IsCourse);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что желаете удалить данную параллель?\nВсе дисциплины, класс, группы, студенты и их результаты, которые закреплены за данной параллелью, будут удалены", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ParallelTableAdapter adapter = new ParallelTableAdapter();
                adapter.DeleteQuery(Convert.ToInt32(data_grid.SelectedValue));

                TableParalleliDataGridTableAdapter a = new TableParalleliDataGridTableAdapter();
                a.Fill(table);
                data_grid.ItemsSource = table;

                UsersTableAdapter a1 = new UsersTableAdapter();
                a1.DeleteUsersLishnie();

                search.Text = "";
                Cours.IsChecked = false;
                Class.IsChecked = false;
            }
        }
    }
}
