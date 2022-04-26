using Kursach.DataBaseTableAdapters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для CreateTest.xaml
    /// </summary>
    public partial class CreateTest : UserControl
    {
        public string ID_Tes = "";
        public int ID_Dicp = 0;
        public CreateTest(string id_test, int ID_Disciplina)
        {
            InitializeComponent();
            ID_Dicp = ID_Disciplina;
            tip_voprosa.Items.Add("Один вариант ответа");
            tip_voprosa.Items.Add("Несколько вариантов ответа");
            tip_voprosa.Items.Add("Краткий текст. вариант ответа");
            tip_voprosa.SelectionChanged += Tip_voprosa_SelectionChanged;
            TablePrepodavatelTableAdapter a = new TablePrepodavatelTableAdapter();
            DataBase.TablePrepodavatelDataTable b = new DataBase.TablePrepodavatelDataTable();
            a.FillByLogin(b,Controllers.auth_login);

            TableClassesTableAdapter a1 = new TableClassesTableAdapter();
            DataBase.TableClassesDataTable b1 = new DataBase.TableClassesDataTable();
            a1.FillForCB(b1, b[0].ID_Prepodavatel);
            if(b1.Rows.Count <= 0)
            {
                MessageBox.Show("Нету дисцпилин для создания теста!");
                return;
            }
            classes.ItemsSource = b1;
            classes.SelectionChanged += Disciplins_SelectionChanged;
            classes.SelectedIndex = 0;
            rezhim_create.Text = "черновик (не сохранён)";
            delete_btn.Visibility = Visibility.Hidden;
            ID_Tes = id_test;
            spisok_vopsov.ItemsSource = voprosi;
            if (ID_Tes != null)
            {
                TEstTableTableAdapter a2 = new TEstTableTableAdapter();
                DataBase.TEstTableDataTable b2 = new DataBase.TEstTableDataTable();
                rezhim_create.Text = "черновик (сохранён)";
                name_test.IsEnabled = false;
                delete_btn.Visibility = Visibility.Visible;
                classes.IsEnabled = false;
                disciplins.IsEnabled = false;
                btn_publish.Visibility = Visibility.Visible;
                btn_create.Visibility = Visibility.Hidden;
                grid2.Visibility = Visibility.Visible;
                grid3.Visibility = Visibility.Visible;
                a2.FillByID(b2, Convert.ToInt32(ID_Tes));
                name_test.Text = b2[0].Name_Test;
                classes.SelectedValue = b2[0].Class_ID;
                disciplins.SelectedValue = b2[0].Disciplini_ID;
                if(nomer_vopros.Text == "1" && spisok_vopsov.Items.Count <= 0)
                    btn_delete_vopros.Visibility = Visibility.Hidden;
                list_voprosi_update();
            }

            DiscipliniTablePrepodavatelTableAdapter a12 = new DiscipliniTablePrepodavatelTableAdapter();
            DataBase.DiscipliniTablePrepodavatelDataTable b12 = new DataBase.DiscipliniTablePrepodavatelDataTable();
            a12.FillByIDDiscipl(b12, ID_Disciplina);
            classes.SelectedValue = b12[0].ID_Class;
            disciplins.SelectedValue = b12[0].ID_Disciplina;
        }

        public void list_voprosi_update()
        {
            voprosi.Clear();
            VoprosTableAdapter aa = new VoprosTableAdapter();
            DataBase.VoprosDataTable bb = new DataBase.VoprosDataTable();
            aa.FillByIDTest(bb, Convert.ToInt32(ID_Tes));
            for (int i = 0; i < bb.Rows.Count; i++)
                voprosi.Add(new ModelListVoprosov { IdVopros = bb[i].ID_Vopros, NomerVoprosa = (i + 1).ToString() });
            nomer_vopros.Text = (voprosi.Count + 1).ToString();
        }
        public class ModelVoros_tip1 : INotifyPropertyChanged
        {
            private string _namevopros;
            public string NameVopros
            {
                get
                {
                    return _namevopros;
                }
                set
                {
                    _namevopros = value;
                }
            }

            private bool _ischecked;
            public bool IsChecked
            {
                get
                {
                    return _ischecked;
                }
                set
                {
                    _ischecked = value;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        public class ModelListVoprosov : INotifyPropertyChanged
        {
            private int _idvopros;
            public int IdVopros
            {
                get
                {
                    return _idvopros;
                }
                set
                {
                    _idvopros = value;
                }
            }

            private string _nomervoprosa;
            public string NomerVoprosa
            {
                get
                {
                    return _nomervoprosa;
                }
                set
                {
                    _nomervoprosa = value;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        public ObservableCollection<ModelVoros_tip1> tip1_list = new ObservableCollection<ModelVoros_tip1>();
        public ObservableCollection<ModelListVoprosov> voprosi = new ObservableCollection<ModelListVoprosov>();

        private void Tip_voprosa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tip_1.Visibility = Visibility.Hidden;
            tip_2.Visibility = Visibility.Hidden;
            tip_1.ItemsSource = null;
            tip_2.ItemsSource = null;
            grid_tip_1.Visibility = Visibility.Hidden;
            grid_tip_3.Visibility = Visibility.Hidden;
            answer_text.Text = "";
            if (tip_voprosa.SelectedIndex == 0 || tip_voprosa.SelectedIndex == 1)
            {
                grid_tip_1.Visibility = Visibility.Visible;
                if (tip_voprosa.SelectedIndex == 0)
                {
                    for (int i = 0; i < tip1_list.Count; i++)
                        tip1_list[i].IsChecked = false;
                    tip_1.ItemsSource = null;
                    tip_1.ItemsSource = tip1_list;
                    tip_1.Visibility = Visibility.Visible;
                }
                if (tip_voprosa.SelectedIndex == 1)
                {
                    for (int i = 0; i < tip1_list.Count; i++)
                        tip1_list[i].IsChecked = false;
                    tip_2.ItemsSource = null;
                    tip_2.ItemsSource = tip1_list;
                    tip_2.Visibility = Visibility.Visible;
                }
                if ((tip1_list.Count == 2 && (tip1_list[0].NameVopros.Trim() == "" && tip1_list[1].NameVopros.Trim() == "")) || tip1_list.Count == 0)
                {
                    tip1_list.Clear();
                    ModelVoros_tip1 tip = new ModelVoros_tip1();
                    tip.NameVopros = "";
                    tip.IsChecked = false;
                    tip1_list.Add(tip);
                    tip = new ModelVoros_tip1();
                    tip.NameVopros = "";
                    tip.IsChecked = true;
                    tip1_list.Add(tip);
                }
            }
            if (tip_voprosa.SelectedIndex == 2)
            {
                grid_tip_3.Visibility = Visibility.Visible;
            }
        }

        private void Disciplins_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TablePrepodavatelTableAdapter a = new TablePrepodavatelTableAdapter();
            DataBase.TablePrepodavatelDataTable b = new DataBase.TablePrepodavatelDataTable();
            a.FillByLogin(b, Controllers.auth_login);

            DiscipliniTableAdapter a1 = new DiscipliniTableAdapter();
            DataBase.DiscipliniDataTable b1 = new DataBase.DiscipliniDataTable();
            a1.FillFroCBTest(b1, Convert.ToInt16(classes.SelectedValue), b[0].ID_Prepodavatel);
            disciplins.ItemsSource = b1;
            disciplins.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new Lenta(ID_Dicp);
            /*            if(ID_Tes == null)
                            Controllers.fram_prep.Content = new UpravlenieTests();
                        else
                            Controllers.fram_prep.Content = new Chernoviki();*/
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TablePrepodavatelTableAdapter a = new TablePrepodavatelTableAdapter();
            DataBase.TablePrepodavatelDataTable b = new DataBase.TablePrepodavatelDataTable();
            a.FillByLogin(b, Controllers.auth_login);

            name_test.Text = name_test.Text.Trim();
            if (name_test.Text.Trim().Length < 2)
            {
                MessageBox.Show("Наименование теста введено некорректно");
                return;
            }

            TestTableAdapter aa = new TestTableAdapter();
            TEstTableTableAdapter a1 = new TEstTableTableAdapter();
            DataBase.TEstTableDataTable b1 = new DataBase.TEstTableDataTable();
            a1.Fill(b1, name_test.Text.Trim(), Convert.ToInt32(disciplins.SelectedValue), Convert.ToInt32(classes.SelectedValue), b[0].ID_Prepodavatel);
            if(b1.Rows.Count > 0)
            {
                MessageBox.Show("Введённое наименование теста уже существует");
                return;
            }
            aa.InsertTest(name_test.Text, Convert.ToInt32(disciplins.SelectedValue), null, false);

            b1 = new DataBase.TEstTableDataTable();
            a1.Fill(b1, name_test.Text.Trim(), Convert.ToInt32(disciplins.SelectedValue), Convert.ToInt32(classes.SelectedValue), b[0].ID_Prepodavatel);

            name_test.IsEnabled = false;
            classes.IsEnabled = false;
            disciplins.IsEnabled = false;
            btn_publish.Visibility = Visibility.Visible;
            btn_create.Visibility = Visibility.Hidden;
            ID_Tes = b1[0].ID_Test.ToString();
            grid2.Visibility = Visibility.Visible;
            grid3.Visibility = Visibility.Visible;
            rezhim_create.Text = "черновик (сохранён)";
            delete_btn.Visibility = Visibility.Visible;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (tip_voprosa.SelectedIndex == 0 || tip_voprosa.SelectedIndex == 1)
            {
                if(tip1_list.Count >= 8)
                {
                    MessageBox.Show("Максимальное количество вариантов ответа - 8!");
                    return;
                }
                ModelVoros_tip1 tip = new ModelVoros_tip1();
                tip.NameVopros = "";
                tip.IsChecked = false;
                tip1_list.Add(tip);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (tip_voprosa.SelectedIndex == 0)
            {
                var customer = tip_1.SelectedItem as ModelVoros_tip1;
                //MessageBox.Show(customer.NameVopros + " " + customer.IsChecked.ToString());
                if (tip1_list.Count <= 2)
                {
                    MessageBox.Show("Минимальное количество вариантов ответа - 2!");
                    return;
                }
                tip1_list.Remove(customer);
            }
            if(tip_voprosa.SelectedIndex == 1)
            {
                var customer = tip_2.SelectedItem as ModelVoros_tip1;
                //MessageBox.Show(customer.NameVopros + " " + customer.IsChecked.ToString());
                if (tip1_list.Count <= 2)
                {
                    MessageBox.Show("Минимальное количество вариантов ответа - 2!");
                    return;
                }
                tip1_list.Remove(customer);
            }
        }

        public void clearVopros()
        {
            tip1_list.Clear();
            ID_Vopros = 0;
            name_vopros.Text = "";
            tip_voprosa.SelectedIndex = -1;
            balli.Text = "";
            answer_text.Text = "";
            nomer_vopros.Text = (voprosi.Count + 1).ToString();
            add_vopros_btn.Visibility = Visibility.Visible;
            edit_vopros_btn.Visibility = Visibility.Hidden;
            btn_delete_vopros.Visibility = Visibility.Hidden;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            name_vopros.Text = name_vopros.Text.Trim();
            balli.Text = balli.Text.Trim();
            if(name_vopros.Text.Length < 3)
            {
                MessageBox.Show("Минимальное количество символов в вопросе - 3");
                return;
            }
            try
            {
                if(Convert.ToInt32(balli.Text) > 50 || Convert.ToInt32(balli.Text) < 1)
                {
                    MessageBox.Show("Минимальное количество баллов за вопрос 1. Максимальное - 50");
                    return;
                }    
            }
            catch
            {
                MessageBox.Show("Некорректно введены баллы");
                return;
            }
            if(tip_voprosa.SelectedIndex == -1)
            {
                MessageBox.Show("Необходимо выбрать тип вопроса!");
                return;
            }
            if(tip_voprosa.SelectedIndex == 0 || tip_voprosa.SelectedIndex == 1)
            {
                if(tip1_list.Where(s=>s.IsChecked == true).Count() < 1)
                {
                    MessageBox.Show("Необходимо выбрать хотя бы один правильный вариант ответа!");
                    return;
                }
                for(int i = 0; i < tip1_list.Count; i++)
                {
                    tip1_list[i].NameVopros = tip1_list[i].NameVopros.Trim();
                    if (tip1_list[i].NameVopros.Length < 1)
                    {
                        MessageBox.Show("Вариант ответа №" + (i + 1) + " заполнено некорректно!");
                        return;
                    }
                }
                VoprosTableAdapter a = new VoprosTableAdapter();
                if(a.FindExistVopros(Convert.ToInt32(ID_Tes),name_vopros.Text) > 0)
                {
                    MessageBox.Show("Такой вопрос уже существуе в данном тесте");
                    return;
                }
                a.InsertQuery(name_vopros.Text, Convert.ToInt32(ID_Tes), tip_voprosa.SelectedIndex.ToString(), Convert.ToInt32(balli.Text));
                DataBase.VoprosDataTable b = new DataBase.VoprosDataTable();
                a.FillByNameVoprosIDTest(b, Convert.ToInt32(ID_Tes), name_vopros.Text);
                AnswerTableAdapter a1 = new AnswerTableAdapter();
                for (int i = 0; i < tip1_list.Count; i++)
                    a1.InsertQuery(tip1_list[i].NameVopros, tip1_list[i].IsChecked, b[0].ID_Vopros);
                voprosi.Add(new ModelListVoprosov { IdVopros = b[0].ID_Vopros, NomerVoprosa = (voprosi.Count + 1).ToString() });
                clearVopros();
            }
            if(tip_voprosa.SelectedIndex == 2)
            {
                answer_text.Text = answer_text.Text.Trim();
                if(answer_text.Text.Length < 1)
                {
                    MessageBox.Show("Заполните корректно поле ответа");
                    return;
                }
                VoprosTableAdapter a = new VoprosTableAdapter();
                if (a.FindExistVopros(Convert.ToInt32(ID_Tes), name_vopros.Text) > 0)
                {
                    MessageBox.Show("Такой вопрос уже существуе в данном тесте");
                    return;
                }

                a.InsertQuery(name_vopros.Text, Convert.ToInt32(ID_Tes), tip_voprosa.SelectedIndex.ToString(), Convert.ToInt32(balli.Text));
                DataBase.VoprosDataTable b = new DataBase.VoprosDataTable();
                a.FillByNameVoprosIDTest(b, Convert.ToInt32(ID_Tes), name_vopros.Text);
                AnswerTableAdapter a1 = new AnswerTableAdapter();
                a1.InsertQuery(answer_text.Text, true, b[0].ID_Vopros);
                voprosi.Add(new ModelListVoprosov { IdVopros = b[0].ID_Vopros, NomerVoprosa = (voprosi.Count + 1).ToString() });
                clearVopros();
            }
        }

        public int ID_Vopros = 0;
        private void edit_vopros_btn_Click(object sender, RoutedEventArgs e)
        {
            if (tip_voprosa.SelectedIndex == 0 || tip_voprosa.SelectedIndex == 1)
            {
                if (tip1_list.Where(s => s.IsChecked == true).Count() < 1)
                {
                    MessageBox.Show("Необходимо выбрать хотя бы один правильный вариант ответа!");
                    return;
                }
                for (int i = 0; i < tip1_list.Count; i++)
                {
                    tip1_list[i].NameVopros = tip1_list[i].NameVopros.Trim();
                    if (tip1_list[i].NameVopros.Length < 1)
                    {
                        MessageBox.Show("Вариант ответа №" + (i + 1) + " заполнено некорректно!");
                        return;
                    }
                }
                VoprosTableAdapter a = new VoprosTableAdapter();
                if (a.FindVoprosEdit(Convert.ToInt32(ID_Tes), name_vopros.Text, ID_Vopros) > 0)
                {
                    MessageBox.Show("Такой вопрос уже существуе в данном тесте");
                    return;
                }
                a.UpdateQuery(name_vopros.Text, Convert.ToInt32(ID_Tes), tip_voprosa.SelectedIndex.ToString(), Convert.ToInt32(balli.Text),ID_Vopros);
                AnswerTableAdapter a1 = new AnswerTableAdapter();
                a1.DeleteQueryByVoprosID(ID_Vopros);
                for (int i = 0; i < tip1_list.Count; i++)
                    a1.InsertQuery(tip1_list[i].NameVopros, tip1_list[i].IsChecked, ID_Vopros);
                clearVopros();
            }
            if(tip_voprosa.SelectedIndex == 2)
            {
                answer_text.Text = answer_text.Text.Trim();
                if (answer_text.Text.Length < 1)
                {
                    MessageBox.Show("Заполните корректно поле ответа");
                    return;
                }
                VoprosTableAdapter a = new VoprosTableAdapter();
                if (a.FindVoprosEdit(Convert.ToInt32(ID_Tes), name_vopros.Text, ID_Vopros) > 0)
                {
                    MessageBox.Show("Такой вопрос уже существуе в данном тесте");
                    return;
                }
                a.UpdateQuery(name_vopros.Text, Convert.ToInt32(ID_Tes), tip_voprosa.SelectedIndex.ToString(), Convert.ToInt32(balli.Text), ID_Vopros);
                AnswerTableAdapter a1 = new AnswerTableAdapter();
                a1.DeleteQueryByVoprosID(ID_Vopros);
                a1.InsertQuery(answer_text.Text, true, ID_Vopros);
                clearVopros();
            }
        }

        private void spisok_vopsov_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (spisok_vopsov.SelectedValue == null)
                return;
            clearVopros();
            var customer = spisok_vopsov.SelectedItem as ModelListVoprosov;
            VoprosTableAdapter a = new VoprosTableAdapter();
            DataBase.VoprosDataTable b = new DataBase.VoprosDataTable();
            a.FillByID(b, Convert.ToInt32(spisok_vopsov.SelectedValue));
            name_vopros.Text = b[0].NameVopros;
            tip_voprosa.SelectedIndex = Convert.ToInt16(b[0].TypeVopros);
            balli.Text = b[0].Ball.ToString();
            nomer_vopros.Text = customer.NomerVoprosa;

            AnswerTableAdapter a1 = new AnswerTableAdapter();
            DataBase.AnswerDataTable b1 = new DataBase.AnswerDataTable();
            a1.FillByIDVopros(b1, b[0].ID_Vopros);

            if(Convert.ToInt16(b[0].TypeVopros) == 0 || Convert.ToInt16(b[0].TypeVopros) == 1)
            {
                tip1_list.Clear();
                for (int i = 0; i < b1.Rows.Count; i++)
                    tip1_list.Add(new ModelVoros_tip1 { IsChecked = b1[i].IsTrue, NameVopros = b1[i].TextAnswer });
            }
            if(Convert.ToInt16(b[0].TypeVopros) == 2)
            {
                tip1_list.Clear();
                answer_text.Text = b1[0].TextAnswer;
            }

            add_vopros_btn.Visibility = Visibility.Hidden;
            edit_vopros_btn.Visibility = Visibility.Visible;
            btn_delete_vopros.Visibility = Visibility.Visible;
            ID_Vopros = b[0].ID_Vopros;
            spisok_vopsov.UnselectAll();
        }

        private void btn_delete_vopros_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что желаете удалить данный вопрос?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                VoprosTableAdapter a = new VoprosTableAdapter();
                a.DeleteQuery(ID_Vopros);
                list_voprosi_update();
                clearVopros();
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            clearVopros();
        }

        private void btn_publish_Click(object sender, RoutedEventArgs e)
        {
            if(spisok_vopsov.Items.Count < 1)
            {
                MessageBox.Show("Невозможно опубликовать тест без вопросов!");
                return;
            }    
            if (MessageBox.Show("Вы уверены что желаете опубликовать тест группе/классу?\nОпубликованный тест нельзя изменять!", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            SrokSdachi t = new SrokSdachi();
            if (t.ShowDialog() == false)
                MessageBox.Show("Срок выполнения теста не указан!");
            else
            {
                TestTableAdapter a = new TestTableAdapter();
                a.PublishTest(Controllers.DateTimeTest, true, DateTime.Now, Convert.ToInt32(ID_Tes));
                MessageBox.Show("Тест опубликован студентам!");
                Controllers.fram_prep.Content = new Lenta(ID_Dicp);
            }
        }

        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что желаете удалить данный черновик?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                TestTableAdapter a = new TestTableAdapter();
                a.DeleteQuery(Convert.ToInt32(ID_Tes));
                Controllers.fram_prep.Content = new Lenta(ID_Dicp);
            }
        }
    }
}
