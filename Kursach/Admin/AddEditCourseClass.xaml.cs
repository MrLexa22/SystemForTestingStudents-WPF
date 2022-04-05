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

namespace Kursach.Admin
{
    /// <summary>
    /// Логика взаимодействия для AddEditCourseClass.xaml
    /// </summary>
    public partial class AddEditCourseClass : UserControl
    {
        public string id_par = null;
        public bool IsCourse = false;
        public AddEditCourseClass(string id_parallel, bool IsCr)
        {
            InitializeComponent();

            id_par = id_parallel;
            IsCourse = IsCr;

            if (id_par != null)
            {
                TableClassesTableAdapter ad = new TableClassesTableAdapter();
                DataBase.TableClassesDataTable ab = new DataBase.TableClassesDataTable();
                ad.Fill(ab, Convert.ToInt32(id_parallel));
                if (ab.Rows.Count > 0)
                {
                    if (IsCourse == true)
                        gr.Text = "Группы параллели";
                    else
                        gr.Text = "Классы параллели";
                    gr.Visibility = Visibility.Visible;
                    listbox.Visibility = Visibility.Visible;
                    listbox.ItemsSource = ab;
                }
            }

            rezhim.Text = "Добавление";
            if(IsCourse == false)
            {
                tip.Text = "класс";
            }
            if(id_par != null)
            {
                rezhim.Text = "Изменение";
                ParallelTableAdapter adapter = new ParallelTableAdapter();
                DataBase.ParallelDataTable b = new DataBase.ParallelDataTable();
                adapter.FindByID(b, Convert.ToInt32(id_parallel));
                nomer_paralleli.Text = b[0].NomerParalleli.ToString();
            }
        }

        private void CheckDigit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text.ToString() != "")
            {
                for (int i = 0; i < (sender as TextBox).Text.Length; i++)
                {
                    bool t = true;
                    string g = (sender as TextBox).Text.ToString();
                    t = Char.IsDigit((sender as TextBox).Text[i]);
                    if (t == false)
                    {
                        g = g.Remove(i, 1);
                    }
                    (sender as TextBox).Text = g;
                    (sender as TextBox).SelectionStart = (sender as TextBox).Text.ToString().Length;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new ParalleliPage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                nomer_paralleli.Text = Convert.ToInt16(nomer_paralleli.Text).ToString();
            }
            catch 
            { MessageBox.Show("Поле Номер параллели заполнено некорректно"); return; }

            if(IsCourse == true)
            {
                if(Convert.ToInt16(nomer_paralleli.Text) > 4 || Convert.ToInt16(nomer_paralleli.Text) < 1)
                {
                    MessageBox.Show("Слишком большой/малый курс"); 
                    return;
                }
            }
            else
            {
                if (Convert.ToInt16(nomer_paralleli.Text) > 11 || Convert.ToInt16(nomer_paralleli.Text) < 1)
                {
                    MessageBox.Show("Слишком большой/малый класс");
                    return;
                }
            }

            ParallelTableAdapter a = new ParallelTableAdapter();
            if(id_par == null)
            {
                if(a.FindExistParallelAdd(Convert.ToInt16(nomer_paralleli.Text),IsCourse) > 0)
                {
                    MessageBox.Show("Такая параллель в данном типе уже существует!");
                    return;
                }
                a.InsertQueray(Convert.ToInt16(nomer_paralleli.Text), IsCourse);
                MessageBox.Show("Запись успешно добавлена!");
            }
            else
            {
                if (a.FindExistParallelEdit(Convert.ToInt16(nomer_paralleli.Text), IsCourse, Convert.ToInt16(id_par)) > 0)
                {
                    MessageBox.Show("Такая параллель в данном типе уже существует!");
                    return;
                }
                a.UpdateQuery(Convert.ToInt16(nomer_paralleli.Text), IsCourse, Convert.ToInt16(id_par));
                MessageBox.Show("Запись успешно обновлена!");
            }

            Controllers.fram.Content = new ParalleliPage();
        }

        private void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Controllers.fram.Content = new AddEditClassGroup(listbox.SelectedValue.ToString(),false);
        }
    }
}
