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
    /// Логика взаимодействия для ContingentPage.xaml
    /// </summary>
    public partial class ContingentPage : UserControl
    {
        TableStudentsTableAdapter a = new TableStudentsTableAdapter();
        DataBase.TableStudentsDataTable table = new DataBase.TableStudentsDataTable();
        public ContingentPage()
        {
            InitializeComponent();
            a.Fill(table);
            data_grid.ItemsSource = table;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AdminControl();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AddEditContingent(null);
        }

        private void search_SelectionChanged(object sender, RoutedEventArgs e)
        {
            DataView dv = new DataView(table);
            string row = "";
            if (search.Text.Trim() != "")
                row = $@"[FIO] LIKE '%{search.Text}%' or [NameClass] LIKE '%{search.Text}%'";
            dv.RowFilter = row;
            data_grid.ItemsSource = dv.ToTable().DefaultView;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AddEditContingent(data_grid.SelectedValue.ToString());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что желаете удалить данного студента?\nВсе результаты тестов студента будут удалены", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                StudentsTableAdapter adapter = new StudentsTableAdapter();
                adapter.DeleteQuery(Convert.ToInt32(data_grid.SelectedValue));

                UsersTableAdapter a1 = new UsersTableAdapter();
                a1.DeleteUsersLishnie();

                table = new DataBase.TableStudentsDataTable();
                a.Fill(table);
                data_grid.ItemsSource = table;

                search.Text = "";
            }
        }
    }
}
