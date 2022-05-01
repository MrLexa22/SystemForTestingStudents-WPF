using Kursach.DataBaseTableAdapters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Kursach.Student
{
    /// <summary>
    /// Логика взаимодействия для PassingTest.xaml
    /// </summary>
    public partial class PassingTest : UserControl
    {
        public int ID_Test;
        public int ID_Disciplina;
        public int ID_Vopros;
        public int ID_Class;
        public int ID_Student;
        public class ModelVoros_tip1 : INotifyPropertyChanged
        {
            private int _idanswer;
            public int IDAnswer
            {
                get
                {
                    return _idanswer;
                }
                set
                {
                    _idanswer = value;
                }
            }

            private int _type;
            public int Type
            {
                get
                {
                    return _type;
                }
                set
                {
                    _type = value;
                }
            }

            private int _idvopros;
            public int IDVopros
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

        public ObservableCollection<ModelListVoprosov> voprosi = new ObservableCollection<ModelListVoprosov>();
        public ObservableCollection<ModelVoros_tip1> tip1_list = new ObservableCollection<ModelVoros_tip1>();
        public PassingTest(int ID_T)
        {
            InitializeComponent();
            ID_Test = ID_T;

            StudentsTableTableAdapter aa = new StudentsTableTableAdapter();
            DataBase.StudentsTableDataTable bb = new DataBase.StudentsTableDataTable();
            aa.FillByLogin(bb, Controllers.auth_login);
            ID_Student = bb[0].ID_Student;

            TEstTableTableAdapter a = new TEstTableTableAdapter();
            DataBase.TEstTableDataTable b = new DataBase.TEstTableDataTable();
            a.FillByID(b, ID_Test);
            ID_Class = b[0].Class_ID;
            ID_Disciplina = b[0].ID_Disciplina;
            name_test.Text = b[0].Name_Test;
            srok_sdachi.Text = b[0].SrokSdachi.ToString("dd.MM.yyyy HH:mm");

            VoprosTableAdapter a1 = new VoprosTableAdapter();
            DataBase.VoprosDataTable b1 = new DataBase.VoprosDataTable();
            a1.FillByIDTest(b1, ID_Test);
            for(int i = 0; i < b1.Rows.Count; i++)
            {
                ModelListVoprosov ml = new ModelListVoprosov();
                ml.IdVopros = b1[i].ID_Vopros;
                ml.NomerVoprosa = (i+1).ToString();
                voprosi.Add(ml);
                    AnswerTableAdapter aa1 = new AnswerTableAdapter();
                    DataBase.AnswerDataTable bb1 = new DataBase.AnswerDataTable();
                    aa1.FillByIDVopros(bb1, b1[i].ID_Vopros);
                    for (int j = 0; j < bb1.Rows.Count; j++)
                    {
                        if (b1[i].TypeVopros == "0" || b1[i].TypeVopros == "1")
                        {
                            tip1_list.Add(new ModelVoros_tip1
                            {
                                IsChecked = false,
                                NameVopros = bb1[j].TextAnswer,
                                IDAnswer = bb1[j].ID_Answer,
                                IDVopros = b1[i].ID_Vopros,
                                Type = Convert.ToInt16(b1[i].TypeVopros)
                            });
                        }
                        else if(b1[i].TypeVopros == "2")
                        {
                            tip1_list.Add(new ModelVoros_tip1
                            {
                                IsChecked = false,
                                NameVopros = "",
                                IDAnswer = bb1[j].ID_Answer,
                                IDVopros = b1[i].ID_Vopros,
                                Type = Convert.ToInt16(b1[i].TypeVopros)
                            });
                        }
                    }
            }

            SelectVopros(1);
        }
        public int now_nomervopros = 0;
        public List<ModelVoros_tip1> now_answers = new List<ModelVoros_tip1>();
        public void SelectVopros(int next_rew)
        {
            if (next_rew == 1)
                now_nomervopros++;
            else
                now_nomervopros--;

            nomer_vopros.Text = now_nomervopros.ToString();

            if (nomer_vopros.Text == "1")
                btn_forward.IsEnabled = false;
            else
                btn_forward.IsEnabled = true;

            if (voprosi.Count == Convert.ToInt16(nomer_vopros.Text))
                btn_next.Content = "Завершить";
            else
                btn_next.Content = "Далее";

            tip_1.Visibility = Visibility.Hidden;
            tip_2.Visibility = Visibility.Hidden;
            tip_3.Visibility = Visibility.Hidden;
            tip_1.ItemsSource = null;
            tip_2.ItemsSource = null;
            tip_3.ItemsSource = null;
            grid_tip_1.Visibility = Visibility.Hidden;

            VoprosTableAdapter a = new VoprosTableAdapter();
            DataBase.VoprosDataTable b = new DataBase.VoprosDataTable();
            a.FillByID(b, voprosi.Where(g => Convert.ToInt16(g.NomerVoprosa) == now_nomervopros).First().IdVopros);

            name_vopros.Text = b[0].NameVopros;
            ID_Vopros = b[0].ID_Vopros;

            AnswerTableAdapter a1 = new AnswerTableAdapter();
            DataBase.AnswerDataTable b1 = new DataBase.AnswerDataTable();
            a1.FillByIDVopros(b1, ID_Vopros);

            if (Convert.ToInt16(b[0].TypeVopros) == 0)
            {
                now_answers = tip1_list.Where(g => g.IDVopros == ID_Vopros).ToList();
                tip_1.ItemsSource = tip1_list.Where(g => g.IDVopros == ID_Vopros);
                grid_tip_1.Visibility = Visibility.Visible;
                tip_1.Visibility = Visibility.Visible;
            }
            if (Convert.ToInt16(b[0].TypeVopros) == 1)
            {
                now_answers = tip1_list.Where(g => g.IDVopros == ID_Vopros).ToList();
                tip_2.ItemsSource = tip1_list.Where(g => g.IDVopros == ID_Vopros);
                grid_tip_1.Visibility = Visibility.Visible;
                tip_2.Visibility = Visibility.Visible;
            }
            if (Convert.ToInt16(b[0].TypeVopros) == 2)
            {
                now_answers = tip1_list.Where(g => g.IDVopros == ID_Vopros).ToList();
                grid_tip_1.Visibility = Visibility.Visible;
                tip_3.Visibility = Visibility.Visible;
                tip_3.ItemsSource = tip1_list.Where(g => g.IDVopros == ID_Vopros);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new LentaDisciplina(ID_Disciplina);
        }

        private void btn_forward_Click(object sender, RoutedEventArgs e)
        {
            SelectVopros(0);
        }

        private void vtn_next_Click(object sender, RoutedEventArgs e)
        {
            if (tip_1.Visibility == Visibility.Visible || tip_2.Visibility == Visibility.Visible)
            {
                if (now_answers.Where(g => g.IsChecked == true).Count() <= 0)
                {
                    MessageBox.Show("Вы не ответили на вопрос!");
                    return;
                }
            }
            if(tip_3.Visibility == Visibility.Visible)
            {
                if(now_answers[0].NameVopros.Trim().Length < 1)
                {
                    MessageBox.Show("Ответьте на вопрос!");
                    return;
                }    
            }
            if(btn_next.Content == "Далее")
                SelectVopros(1);
            else
            {
                if (!(MessageBox.Show("Вы уверены в правильности своих овтетов? При согласии ответы будут отправлены на проверку, изменить их будет нельзя!", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes))
                {
                    return;
                }
                ResultsTestTableAdapter at = new ResultsTestTableAdapter();
                at.InsertQuery(ID_Student, ID_Test, null);

                StudentAnswerTableAdapter a = new StudentAnswerTableAdapter();
                for(int i = 0; i < tip1_list.Count; i++)
                {
                    if (tip1_list[i].IsChecked == true && (tip1_list[i].Type != 0 || tip1_list[i].Type != 1))
                        a.InsertQuery(ID_Student, tip1_list[i].IDAnswer, null, null);
                    if (tip1_list[i].Type == 2)
                        a.InsertQuery(ID_Student, tip1_list[i].IDAnswer, tip1_list[i].NameVopros, null);
                }

                MessageBox.Show("Тест успешно завершён! После проверки преподавателем оценка будет доступна в разделе 'Результаты'");
                Controllers.fram_prep.Content = new LentaDisciplina(ID_Disciplina);
            }
        }
    }
}
