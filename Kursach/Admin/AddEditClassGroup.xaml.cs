using Kursach.DataBaseTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddEditClassGroup.xaml
    /// </summary>
    public partial class AddEditClassGroup : UserControl
    {
        string id_class;
        bool IsGr;
        public AddEditClassGroup(string id,bool IsGroup)
        {
            InitializeComponent();
            id_class = id;
            //IsGr = IsGroup;
            if(id_class != null)
            {
                rezhim.Text = "Изменение";
                TableClassesTableAdapter a = new TableClassesTableAdapter();
                DataBase.TableClassesDataTable b = new DataBase.TableClassesDataTable();
                a.FindClassByID(b, Convert.ToInt32(id_class));
                if (b[0].IsCourse == true)
                {
                    tip.Text = "группа";
                    grup_or_class.Text = "группы";
                    rezhim_2.Text = "группы";
                    IsGr = true;
                    nomer_paralleli.MaxLength = 15;
                }
                else
                {
                    tip.Text = "класс";
                    grup_or_class.Text = "класса";
                    rezhim_2.Text = "класса";
                    IsGr = false;
                    nomer_paralleli.MaxLength = 1;
                }

                TableParalleliTableAdapter a1 = new TableParalleliTableAdapter();
                DataBase.TableParalleliDataTable b1 = new DataBase.TableParalleliDataTable();
                a1.FillByIsCourse(b1, b[0].IsCourse);
                parallel_cb.ItemsSource = b1;
                parallel_cb.SelectedValue = b[0].ID_Parallel;
                nomer_paralleli.Text = b[0].NameClass;

                StudentsTableTableAdapter a2 = new StudentsTableTableAdapter();
                DataBase.StudentsTableDataTable b2 = new DataBase.StudentsTableDataTable();
                a2.FillByIDClass(b2, Convert.ToInt32(id_class));
                if(b2.Rows.Count > 0)
                {
                    listbox.ItemsSource = b2;
                    gr.Visibility = Visibility.Visible;
                    listbox.Visibility = Visibility.Visible;
                }
            }
            else
            {
                IsGr = IsGroup;
                rezhim.Text = "Добавление";
                if (IsGr == true)
                {
                    tip.Text = "группа";
                    grup_or_class.Text = "группы";
                    rezhim_2.Text = "группы";
                    nomer_paralleli.MaxLength = 15;
                }
                else
                {
                    tip.Text = "класс";
                    grup_or_class.Text = "класса";
                    rezhim_2.Text = "класса";
                    nomer_paralleli.MaxLength = 1;
                }
                TableParalleliTableAdapter a1 = new TableParalleliTableAdapter();
                DataBase.TableParalleliDataTable b1 = new DataBase.TableParalleliDataTable();
                a1.FillByIsCourse(b1, IsGr);
                parallel_cb.ItemsSource = b1;
                if(b1.Rows.Count <= 0)
                {
                    MessageBox.Show("Error");
                    Controllers.fram.Content = new ClassuGrupiPage();
                }
                parallel_cb.SelectedIndex = 0;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (nomer_paralleli.Text.Trim().Length < 2 && IsGr == true)
            {
                MessageBox.Show("В наименовании группы должно быть минимум два символа");
                return;
            }
            if ((nomer_paralleli.Text.Trim().Length < 1 || !Regex.IsMatch(nomer_paralleli.Text.Trim(), @"^[a-zA-Zа-яА-ЯёЁ]+$")) && IsGr == false)
            {
                MessageBox.Show("Некорректная буква класса");
                return;
            }
            else if(IsGr == false)
                nomer_paralleli.Text = nomer_paralleli.Text.Trim().ToUpper();

            ClassesTableAdapter a = new ClassesTableAdapter();
            if (id_class != null)
            {
                if (tip.Text == "класс")
                {
                    if (a.FindExistClassEdit(nomer_paralleli.Text.Trim(), Convert.ToInt32(parallel_cb.SelectedValue), Convert.ToInt32(id_class)) > 0)
                    {
                        MessageBox.Show("Такой класс уже существует в выбранной параллели!");
                        return;
                    }
                }
                if (tip.Text == "группа")
                {
                    if (a.FindExistGroupEdit(nomer_paralleli.Text.Trim(), Convert.ToInt32(id_class)) > 0)
                    {
                        MessageBox.Show("Такая группа уже существует!");
                        return;
                    }
                }
                a.UpdateQuery(nomer_paralleli.Text.Trim(), Convert.ToInt32(parallel_cb.SelectedValue), Convert.ToInt32(id_class));
                MessageBox.Show("Данные успешно обновлены!");
            }
            else
            {
                if (tip.Text == "класс")
                {
                    if (a.FindExistClassAdd(nomer_paralleli.Text.Trim(), Convert.ToInt32(parallel_cb.SelectedValue)) > 0)
                    {
                        MessageBox.Show("Такой класс уже существует в выбранной параллели!");
                        return;
                    }
                }
                if (tip.Text == "группа")
                {
                    if (a.FindExistGroupAdd(nomer_paralleli.Text.Trim()) > 0)
                    {
                        MessageBox.Show("Такая группа уже существует!");
                        return;
                    }
                }
                a.InsertQuery(nomer_paralleli.Text.Trim(), Convert.ToInt32(parallel_cb.SelectedValue));
                MessageBox.Show("Данные успешно добавлены!");
            }
            Controllers.fram.Content = new ClassuGrupiPage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new ClassuGrupiPage();
        }

        private void nomer_paralleli_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (nomer_paralleli.Text.Trim() != "")
            {
                TableParalleliTableAdapter a = new TableParalleliTableAdapter();
                DataBase.TableParalleliDataTable b = new DataBase.TableParalleliDataTable();
                a.FillByIDParallel(b, Convert.ToInt32(parallel_cb.SelectedValue));
                if (IsGr == true)
                {
                    example.Text = b[0].ParallelName + ". Группа " + nomer_paralleli.Text;
                }
                else
                {
                    string text = b[0].ParallelName;
                    string[] words = text.Split(new char[] { ' ' });
                    example.Text = words[0] + nomer_paralleli.Text + " " + words[1];
                }
            }
        }

        private void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Controllers.fram.Content = new AddEditContingent(listbox.SelectedValue.ToString());
        }
    }
}
