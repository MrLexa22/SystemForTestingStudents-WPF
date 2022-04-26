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

namespace Kursach.Prepodavatel
{
    /// <summary>
    /// Логика взаимодействия для ResultTest.xaml
    /// </summary>
    public partial class ResultTest : UserControl
    {
        public int ID_Test;
        public int ID_Disciplina;
        public int ID_Vopros;
        public int ID_Class;
        public class Students
        {
            public int ID_Students { get; set; }
            public string FIO { get; set; }
            public string status { get; set; }
            public string ochenka { get; set; }
        }

        List<Students> students = new List<Students>();
        public ResultTest(int ID_T)
        {
            InitializeComponent();
            ID_Test = ID_T;
            TEstTableTableAdapter a = new TEstTableTableAdapter();
            DataBase.TEstTableDataTable b = new DataBase.TEstTableDataTable();
            a.FillByID(b, ID_Test);
            ID_Class = b[0].Class_ID;
            ID_Disciplina = b[0].ID_Disciplina;
            name_test.Text = b[0].Name_Test;

            UpdateSpisokStudent();
            spisok_vopsov.ItemsSource = voprosi;
            tip_1.ItemsSource = tip1_list;
        }

        public void UpdateSpisokStudent()
        {
            students.Clear();
            StudentsTableTableAdapter a1 = new StudentsTableTableAdapter();
            DataBase.StudentsTableDataTable b1 = new DataBase.StudentsTableDataTable();
            a1.FillByIDClass(b1, ID_Class);
            for (int i = 0; i < b1.Rows.Count; i++)
            {
                Students student = new Students();
                student.FIO = b1[i].FIO;
                student.ID_Students = b1[i].ID_Student;
                ResultsTestTableAdapter aa = new ResultsTestTableAdapter();
                DataBase.ResultsTestDataTable bb = new DataBase.ResultsTestDataTable();
                aa.FillByIDST(bb, ID_Test, b1[i].ID_Student);
                if (bb.Rows.Count <= 0)
                {
                    student.status = "(-)";
                    student.ochenka = "";
                }
                else
                {
                    AnswersStudentsTableAdapter aaa = new AnswersStudentsTableAdapter();
                    DataBase.AnswersStudentsDataTable bbb = new DataBase.AnswersStudentsDataTable();
                    aaa.FillByIDStTest(bbb, b1[i].ID_Student, ID_Test);
                    var check = bb[0]["Ochenka"] != DBNull.Value ? bb[0]["Ochenka"].ToString() : "t";
                    if (bbb.Rows.Count > 0)
                    {
                        if (check != "t")
                        {
                            student.status = "(+):";
                            student.ochenka = bb[0].Ochenka.ToString();
                        }
                        else
                            student.status = "(+)";
                    }
                    else
                    {
                        if (check != "t")
                        {
                            student.status = "(-):";
                            student.ochenka = bb[0].Ochenka.ToString();
                        }
                        else
                            student.status = "(-)";
                    }
                }
                students.Add(student);
            }
            spisok_students.ItemsSource = null;
            spisok_students.ItemsSource = students;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new Lenta(ID_Disciplina);
        }

        public int ID_Student_Selected = 0;
        private void spisok_students_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (spisok_students.SelectedValue == null)
                return;
            TableStudentsTableAdapter a = new TableStudentsTableAdapter();
            DataBase.TableStudentsDataTable b = new DataBase.TableStudentsDataTable();
            a.FillByID(b, Convert.ToInt16(spisok_students.SelectedValue));

            grid_fioandochenka.Visibility = Visibility.Visible;
            familia.Text = b[0].Familia;
            ima.Text = b[0].Ima;
            ID_Student_Selected = b[0].ID_Student;
            SchetStudentBall();

            VoprosTableAdapter aa = new VoprosTableAdapter();
            DataBase.VoprosDataTable bb = new DataBase.VoprosDataTable();
            aa.FillByIDTest(bb, ID_Test);
            int max_ball = 0;
            for(int i = 0; i<bb.Rows.Count; i++)
                max_ball += bb[i].Ball;
            max_balli.Text = max_ball.ToString();

            ResultsTestTableAdapter a1 = new ResultsTestTableAdapter();
            DataBase.ResultsTestDataTable b1= new DataBase.ResultsTestDataTable();
            a1.FillByIDST(b1, ID_Test, b[0].ID_Student);
            if (b1.Rows.Count > 0)
            {
                var check = b1[0]["Ochenka"] != DBNull.Value ? b1[0]["Ochenka"].ToString() : "t";
                if (check == "t")
                    ochenka.Text = "";
                else
                    ochenka.Text = b1[0].Ochenka.ToString();

                AnswersStudentsTableAdapter aaa = new AnswersStudentsTableAdapter();
                DataBase.AnswersStudentsDataTable bbb = new DataBase.AnswersStudentsDataTable();
                aaa.FillByIDStTest(bbb, ID_Student_Selected, ID_Test);
                if (bbb.Rows.Count > 0)
                {
                    status.Text = "Сдано";
                    grid_voprosi.Visibility = Visibility.Visible;

                    VoprosTableAdapter a2 = new VoprosTableAdapter();
                    DataBase.VoprosDataTable b2 = new DataBase.VoprosDataTable();
                    a2.FillByIDTest(b2, ID_Test);
                    list_voprosi_update();
                }
                else
                {
                    status.Text = "Не сдано";
                    grid_voprosi.Visibility = Visibility.Hidden;
                    zarabot_balli.Text = "0";
                }
            }
            else
            {
                ochenka.Text = "";
                status.Text = "Не сдано";
                grid_voprosi.Visibility = Visibility.Hidden;
                zarabot_balli.Text = "0";
            }
        }
        public ObservableCollection<ModelListVoprosov> voprosi = new ObservableCollection<ModelListVoprosov>();
        public ObservableCollection<ModelVoros_tip1> tip1_list = new ObservableCollection<ModelVoros_tip1>();
        public void SchetStudentBall()
        {
            int itog_ball = 0;

            VoprosTableAdapter a = new VoprosTableAdapter();
            DataBase.VoprosDataTable b = new DataBase.VoprosDataTable();
            a.FillByIDTest(b, ID_Test);
            for (int i = 0; i < b.Rows.Count; i++)
            {
                int ball = 0;
                AnswerTableAdapter a1 = new AnswerTableAdapter();
                DataBase.AnswerDataTable b1 = new DataBase.AnswerDataTable();
                a1.FillByIDVopros(b1, b[i].ID_Vopros);
                if (Convert.ToInt16(b[i].TypeVopros) == 0 || Convert.ToInt16(b[i].TypeVopros) == 1)
                {
                    int count_true_answers = 0;
                    for (int j = 0; j < b1.Rows.Count; j++)
                        if (b1[j].IsTrue)
                            count_true_answers++;

                    AnswersStudentsTableAdapter ss = new AnswersStudentsTableAdapter();
                    if ((count_true_answers == ss.HowMuchTrue(b[i].ID_Vopros, ID_Student_Selected)) && ss.HowMuchFalse(b[i].ID_Vopros, ID_Student_Selected) == 0)
                        ball = b[i].Ball;
                    else
                        ball = 0;

                    DataBase.AnswersStudentsDataTable sb = new DataBase.AnswersStudentsDataTable();
                    ss.FillByIDVopros(sb, b[i].ID_Vopros, ID_Student_Selected);
                    if (sb.Rows.Count != 0)
                    {
                        var check = sb[0]["BallStudent"] != DBNull.Value ? sb[0]["BallStudent"].ToString() : "t";
                        if (check != "t")
                            ball = sb[0].BallStudent;
                    }
                }
                else if (Convert.ToInt16(b[i].TypeVopros) == 2)
                {
                    StudentAnswerTableAdapter s = new StudentAnswerTableAdapter();
                    DataBase.StudentAnswerDataTable sb = new DataBase.StudentAnswerDataTable();
                    s.FillByIDAnswStud(sb, ID_Student_Selected, b1[0].ID_Answer);
                    if (sb.Rows.Count != 0)
                    {
                        if (sb[0].TextAnswer.ToLower() == b1[0].TextAnswer.ToLower())
                            ball = b[i].Ball;
                        else
                            ball = 0;
                    }
                    else
                        ball = 0;

                    AnswersStudentsTableAdapter ss = new AnswersStudentsTableAdapter();
                    DataBase.AnswersStudentsDataTable sbs = new DataBase.AnswersStudentsDataTable();
                    ss.FillByIDVopros(sbs, b[i].ID_Vopros, ID_Student_Selected);
                    if (sbs.Rows.Count != 0)
                    {
                        var check = sbs[0]["BallStudent"] != DBNull.Value ? sbs[0]["BallStudent"].ToString() : "t";
                        if (check != "t")
                            ball = sbs[0].BallStudent;
                    }
                }
                itog_ball += ball;
            }
            zarabot_balli.Text = itog_ball.ToString();
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
            private SolidColorBrush _colortest;
            public SolidColorBrush ColorText
            {
                get
                {
                    return _colortest;
                }
                set
                {
                    _colortest = value;
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
        public void list_voprosi_update()
        {
            voprosi.Clear();
            VoprosTableAdapter aa = new VoprosTableAdapter();
            DataBase.VoprosDataTable bb = new DataBase.VoprosDataTable();
            aa.FillByIDTest(bb, Convert.ToInt32(ID_Test));
            for (int i = 0; i < bb.Rows.Count; i++)
                voprosi.Add(new ModelListVoprosov { IdVopros = bb[i].ID_Vopros, NomerVoprosa = (i + 1).ToString() });
            spisok_vopsov.SelectedIndex = 0;
        }
        public void clearVopros()
        {
            tip1_list.Clear();
            //ID_Vopros = 0;
            name_vopros.Text = "";
            //tip_voprosa.SelectedIndex = -1;
            balli.Text = "";
            answer_text.Text = "";
            nomer_vopros.Text = (voprosi.Count + 1).ToString();
        }
        public void TypeVopros(int tipss)
        {
            tip_1.Visibility = Visibility.Hidden;
            tip_2.Visibility = Visibility.Hidden;
            tip_1.ItemsSource = null;
            tip_2.ItemsSource = null;
            grid_tip_1.Visibility = Visibility.Hidden;
            grid_tip_3.Visibility = Visibility.Hidden;
            answer_text.Text = "";
            if (tipss == 0 || tipss == 1)
            {
                grid_tip_1.Visibility = Visibility.Visible;
                if (tipss == 0)
                {
                    for (int i = 0; i < tip1_list.Count; i++)
                        tip1_list[i].IsChecked = false;
                    tip_1.ItemsSource = null;
                    tip_1.ItemsSource = tip1_list;
                    tip_1.Visibility = Visibility.Visible;
                }
                if (tipss == 1)
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
            if (tipss == 2)
            {
                grid_tip_3.Visibility = Visibility.Visible;
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
            nomer_vopros.Text = customer.NomerVoprosa;
            ID_Vopros = Convert.ToInt32(spisok_vopsov.SelectedValue);

            AnswerTableAdapter a1 = new AnswerTableAdapter();
            DataBase.AnswerDataTable b1 = new DataBase.AnswerDataTable();
            a1.FillByIDVopros(b1, b[0].ID_Vopros);

            TypeVopros(Convert.ToInt16(b[0].TypeVopros));

            if (Convert.ToInt16(b[0].TypeVopros) == 0 || Convert.ToInt16(b[0].TypeVopros) == 1)
            {
                int count_true_answers = 0;
                for(int i = 0; i < b1.Rows.Count; i++)
                    if(b1[i].IsTrue)
                        count_true_answers++;

                AnswersStudentsTableAdapter ss = new AnswersStudentsTableAdapter();
                if ((count_true_answers == ss.HowMuchTrue(b[0].ID_Vopros, ID_Student_Selected)) && ss.HowMuchFalse(b[0].ID_Vopros, ID_Student_Selected) == 0)
                    balli.Text = b[0].Ball.ToString();
                else
                    balli.Text = "0";
                DataBase.AnswersStudentsDataTable sb = new DataBase.AnswersStudentsDataTable();
                ss.FillByIDVopros(sb, b[0].ID_Vopros, ID_Student_Selected);
                var check = sb[0]["BallStudent"] != DBNull.Value ? sb[0]["BallStudent"].ToString() : "t";
                if (check != "t")
                    balli.Text = sb[0].BallStudent.ToString();

                tip1_list.Clear();
                for (int i = 0; i < b1.Rows.Count; i++)
                {
                    SolidColorBrush co = new SolidColorBrush(Colors.White);
                    if (b1[i].IsTrue)
                        co = new SolidColorBrush(Colors.Green);
                    else
                        co = new SolidColorBrush(Colors.Red);
                    StudentAnswerTableAdapter aas = new StudentAnswerTableAdapter();
                    bool ch = false;
                    if (aas.FindAnswerStudent(b1[i].ID_Answer, ID_Student_Selected) > 0)
                        ch = true;
                    tip1_list.Add(new ModelVoros_tip1 
                    {
                        IsChecked = ch, 
                        NameVopros = b1[i].TextAnswer,
                        ColorText = co                       
                    });
                }
            }
            if (Convert.ToInt16(b[0].TypeVopros) == 2)
            {
                tip1_list.Clear();
                answer_text.Text = b1[0].TextAnswer;
                StudentAnswerTableAdapter s = new StudentAnswerTableAdapter();
                DataBase.StudentAnswerDataTable sb = new DataBase.StudentAnswerDataTable();
                s.FillByIDAnswStud(sb, ID_Student_Selected, b1[0].ID_Answer);
                student_answer_text.Text = sb[0].TextAnswer;
                if (student_answer_text.Text.ToLower() == answer_text.Text.ToLower())
                {
                    student_answer_text.Foreground = new SolidColorBrush(Colors.Green);
                    balli.Text = b[0].Ball.ToString();
                }
                else
                {
                    student_answer_text.Foreground = new SolidColorBrush(Colors.Red);
                    balli.Text = "0";
                }

                AnswersStudentsTableAdapter ss = new AnswersStudentsTableAdapter();
                DataBase.AnswersStudentsDataTable sbs = new DataBase.AnswersStudentsDataTable();
                ss.FillByIDVopros(sbs, b[0].ID_Vopros, ID_Student_Selected);
                var check = sbs[0]["BallStudent"] != DBNull.Value ? sbs[0]["BallStudent"].ToString() : "t";
                if (check != "t")
                    balli.Text = sbs[0].BallStudent.ToString();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ochenka.Text = ochenka.Text.Trim();
                Convert.ToInt32(ochenka.Text);
            }
            catch { 
                MessageBox.Show("Ошибка. Введите корректную оценку!");
                return;
            }
            if(Convert.ToInt32(ochenka.Text) < 2 || Convert.ToInt32(ochenka.Text) > 5)
            {
                MessageBox.Show("Укажите оценку в диапазона 2-5");
                return;
            }
            ResultsTestTableAdapter a = new ResultsTestTableAdapter();
            if(a.FindExist(ID_Student_Selected,ID_Test) > 0)
            {
                a.UpdateQuery(Convert.ToInt32(ochenka.Text), ID_Student_Selected, ID_Test);
                MessageBox.Show("Оценка обновлена!");
            }
            else
            {
                a.InsertQuery(ID_Student_Selected, ID_Test, Convert.ToInt32(ochenka.Text));
                MessageBox.Show("Оценка поставлена!");
            }
            UpdateSpisokStudent();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            balli.Text = balli.Text.Trim();
            try
            {
                Convert.ToInt16(balli.Text);
            }
            catch
            {
                MessageBox.Show("Количество баллов указано некорректно!");
                return;
            }
            if(Convert.ToInt16(balli.Text) > Convert.ToInt16(max_ball.Text) || (Convert.ToInt16(balli.Text) < 0))
            {
                MessageBox.Show("Количество баллов не может превышать максимальный балл. И не может быть меньше 0");
                return;
            }
            StudentAnswerTableAdapter a = new StudentAnswerTableAdapter();
            a.UpdateBallStudent(Convert.ToInt16(balli.Text), ID_Vopros, ID_Student_Selected);
            MessageBox.Show("Балл студента за вопрос обновлён!");
            SchetStudentBall();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что желаете изменить срок сдачи?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                SrokSdachi t = new SrokSdachi();
                if (t.ShowDialog() == false)
                    MessageBox.Show("Срок выполнения теста не указан!");
                else
                {
                    TestTableAdapter a = new TestTableAdapter();
                    a.UpdateSrok(Controllers.DateTimeTest, ID_Test);
                    MessageBox.Show("Срок выполнения обновлён!");
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что желаете отправить данный тест в черновики?\nВсе результаты учащихся будут обнулены!!", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                TestTableAdapter a = new TestTableAdapter();
                a.PublishTest(null, false, null, ID_Test);
                ResultsTestTableAdapter a1 = new ResultsTestTableAdapter();
                a1.DeleteWhereTestID(ID_Test);
                StudentAnswerTableAdapter a2 = new StudentAnswerTableAdapter();
                a2.DeleteStudAnswers(ID_Test);
                Controllers.fram_prep.Content = new CreateTest(ID_Test.ToString(), ID_Disciplina);
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что желаете удалить данный тест?\nВсе результаты учащихся будут обнулены!!", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                TestTableAdapter a = new TestTableAdapter();
                a.DeleteQuery(ID_Test);
                Controllers.fram_prep.Content = new Lenta(ID_Disciplina);
            }
        }
    }
}
