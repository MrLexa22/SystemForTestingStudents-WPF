using Kursach.DataBaseTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    /// Логика взаимодействия для Admin_prepodavateli.xaml
    /// </summary>
    public partial class Admin_prepodavateli : UserControl
    {
        DataBase.TablePrepodavatelDataTable table = new DataBase.TablePrepodavatelDataTable();
        public Admin_prepodavateli()
        {
            InitializeComponent();

            TablePrepodavatelTableAdapter a = new TablePrepodavatelTableAdapter();
            a.Fill(table);
            data_grid.ItemsSource = table;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AdminControl();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AddEditPrepodavatel(null);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AddEditPrepodavatel(data_grid.SelectedValue.ToString());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Вы уверены что желаете удалить данного преподавателя?\nВсе дисциплины, тесты, результаты тестов, которые закреплены за данным преподавателем, будут удалены", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                PrepodavateliTableAdapter a = new PrepodavateliTableAdapter();
                PrepodavatelSelectedTableAdapter a1 = new PrepodavatelSelectedTableAdapter();
                DataBase.PrepodavatelSelectedDataTable b1 = new DataBase.PrepodavatelSelectedDataTable();
                a1.Fill(b1, Convert.ToInt32(data_grid.SelectedValue));
                a.DeleteQuery(Convert.ToInt32(data_grid.SelectedValue));

                UsersTableAdapter a2 = new UsersTableAdapter();
                a2.DeleteQuery(b1[0].User_ID);

                MessageBox.Show("Пользователь удалён!");

                TablePrepodavatelTableAdapter a3 = new TablePrepodavatelTableAdapter();
                a3.Fill(table);
                data_grid.ItemsSource = table;
                search.Text = "";
                KlRuk.IsChecked = false;
            }
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            DataView dv = new DataView(table);
            string row = "";
            if (search.Text.Trim() != "")
            {
                row = $@"[FIO] LIKE '%{search.Text}%' or [Prepodavanie] LIKE '%{search.Text}%' or [KLRukovod] LIKE '%{search.Text}%'";
                if (KlRuk.IsChecked == true)
                {
                    row += " AND [KLRukovod] <> 'Не назначено'";
                }
            }
            else if(KlRuk.IsChecked == true)
            {
                row += "[KLRukovod] <> 'Не назначено'";
            }
            dv.RowFilter = row;
            data_grid.ItemsSource = dv.ToTable().DefaultView;
        }
    }
}
