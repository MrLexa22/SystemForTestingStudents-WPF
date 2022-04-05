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
    /// Логика взаимодействия для AddEditDisciplina.xaml
    /// </summary>
    public partial class AddEditDisciplina : UserControl
    {
        public string id_disciplina = "";
        public AddEditDisciplina(string id)
        {
            InitializeComponent();

            TableParalleliTableAdapter a = new TableParalleliTableAdapter();
            DataBase.TableParalleliDataTable b = new DataBase.TableParalleliDataTable();
            a.Fill(b);
            if(b.Rows.Count <= 0)
            {
                MessageBox.Show("Нету параллелей с классами/группами");
                Controllers.fram.Content = new DiscipliniPage();
            }
            nomer_paralleli.ItemsSource = b;
            nomer_paralleli.SelectedIndex = 0;

            TablePrepodavatelTableAdapter a1 = new TablePrepodavatelTableAdapter();
            DataBase.TablePrepodavatelDataTable b1 = new DataBase.TablePrepodavatelDataTable();
            a1.Fill(b1);
            if (b1.Rows.Count <= 0)
            {
                MessageBox.Show("Нету преподавателей");
                Controllers.fram.Content = new DiscipliniPage();
            }
            prepodavatels.ItemsSource = b1;
            prepodavatels.SelectedIndex = 0;


            id_disciplina = id;
            if(id_disciplina == null)
            {
                rezhim.Text = "Добавление";
            }    
            else
            {
                rezhim.Text = "Изменение";
                DiscipliniDataGridTableAdapter adap = new DiscipliniDataGridTableAdapter();
                DataBase.DiscipliniDataGridDataTable bb = new DataBase.DiscipliniDataGridDataTable();
                adap.FillByID(bb, Convert.ToInt32(id_disciplina));
                nomer_paralleli.SelectedValue = bb[0].Parallel_ID;
                classes.SelectedValue = bb[0].Class_ID;
                prepodavatels.SelectedValue = bb[0].Prepodavatel_ID;
                name_disciplina.Text = bb[0].NameDisciplina;
                shortname_disciplina.Text = bb[0].ShortName;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new DiscipliniPage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DiscipliniTableAdapter a = new DiscipliniTableAdapter();
            if(name_disciplina.Text.Trim().Length < 3)
            {
                MessageBox.Show("Минимальная длина дисциплины - 3 символа!");
                return;
            }
            if(id_disciplina == null)
            {
                if (a.FindExistDiscAdd(name_disciplina.Text.Trim(), shortname_disciplina.Text.Trim(), Convert.ToInt16(classes.SelectedValue)) > 0)
                {
                    MessageBox.Show("Такая дисциплина уже существует у данного класса");
                    return;
                }
                a.InsertQuery(name_disciplina.Text.Trim(), Convert.ToInt16(prepodavatels.SelectedValue), Convert.ToInt16(classes.SelectedValue), shortname_disciplina.Text.Trim());
                MessageBox.Show("Запись успешно добавлена!");
            }
            else 
            {
                if (a.FindExistDiscEdit(name_disciplina.Text.Trim(), shortname_disciplina.Text.Trim(), Convert.ToInt16(classes.SelectedValue),Convert.ToInt32(id_disciplina)) > 0)
                {
                    MessageBox.Show("Такая дисциплина уже существует у данного класса");
                    return;
                }
                a.UpdateQuery(name_disciplina.Text.Trim(), Convert.ToInt16(prepodavatels.SelectedValue), Convert.ToInt16(classes.SelectedValue), shortname_disciplina.Text.Trim(), Convert.ToInt32(id_disciplina));
                MessageBox.Show("Запись успешно обновлена!");
            }
            Controllers.fram.Content = new DiscipliniPage();
        }

        private void nomer_paralleli_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TableClassesTableAdapter a1 = new TableClassesTableAdapter();
            DataBase.TableClassesDataTable b1 = new DataBase.TableClassesDataTable();
            a1.Fill(b1, Convert.ToInt32(nomer_paralleli.SelectedValue));
            classes.ItemsSource = b1;
            classes.SelectedIndex = 0;
        }
    }
}
