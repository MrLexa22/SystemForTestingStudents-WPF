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

namespace Kursach.Prepodavatel
{
    /// <summary>
    /// Логика взаимодействия для UpravlenieClRukovod.xaml
    /// </summary>
    public partial class UpravlenieClRukovod : UserControl
    {
        public UpravlenieClRukovod()
        {
            InitializeComponent();
            PrepodavatelSelectedTableAdapter a = new PrepodavatelSelectedTableAdapter();
            DataBase.PrepodavatelSelectedDataTable b = new DataBase.PrepodavatelSelectedDataTable();
            a.FillByLoginPrepodavatel(b, Controllers.auth_login);

            TableClassesTableAdapter a1 = new TableClassesTableAdapter();
            DataBase.TableClassesDataTable b1 = new DataBase.TableClassesDataTable();
            a1.FindClassByID(b1, b[0].Class_ID);
            grupa_class.Text = b1[0].NameClassParallel;

            DiscipliniDataGridTableAdapter a2 = new DiscipliniDataGridTableAdapter();
            DataBase.DiscipliniDataGridDataTable b2 = new DataBase.DiscipliniDataGridDataTable();
            a2.FillByIDClass(b2, b[0].Class_ID);

            search.ItemsSource = b2;
            search.SelectedIndex = 0;
        }

        private void search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int ID_Disciplina = Convert.ToInt32(search.SelectedValue);
            try
            {
                DiscipliniTableAdapter a = new DiscipliniTableAdapter();
                DataBase.DiscipliniDataTable b = new DataBase.DiscipliniDataTable();
                a.FillByID(b, ID_Disciplina);

                StudentsTableTableAdapter a1 = new StudentsTableTableAdapter();
                DataBase.StudentsTableDataTable b1 = new DataBase.StudentsTableDataTable();
                a1.FillByIDClass(b1, b[0].Class_ID);

                DataTable data = new DataTable();
                data.Columns.Add("Фамилия Имя Отчество");

                TestTableAdapter at = new TestTableAdapter();
                DataBase.TestDataTable bt = new DataBase.TestDataTable();
                at.FillByIDDiscipli(bt, ID_Disciplina);
                for (int i = 0; i < bt.Rows.Count; i++)
                {
                    data.Columns.Add(bt[i].Name_Test);
                }
                data.Columns.Add("Средний балл");

                for (int i = 0; i < b1.Rows.Count; i++)
                {
                    DataRow f = data.NewRow();
                    f["Фамилия Имя Отчество"] = b1[i].FIO;
                    double sr_ball = 0;
                    int count_sk = 0;
                    for (int j = 0; j < bt.Rows.Count; j++)
                    {
                        ResultsTestTableAdapter ra = new ResultsTestTableAdapter();
                        DataBase.ResultsTestDataTable rb = new DataBase.ResultsTestDataTable();
                        ra.FillByIDST(rb, bt[j].ID_Test, b1[i].ID_Student);
                        if (rb.Rows.Count > 0)
                        {
                            var check = rb[0]["Ochenka"] != DBNull.Value ? rb[0]["Ochenka"].ToString() : "t";
                            if (check != "t")
                            {
                                f[bt[j].Name_Test] = rb[0].Ochenka.ToString();
                                count_sk++;
                                sr_ball += rb[0].Ochenka;
                            }
                            else
                            {
                                f[bt[j].Name_Test] = "Не проверено";
                            }
                        }
                        else
                        {
                            f[bt[j].Name_Test] = "Не сдано";
                        }
                    }
                    if (sr_ball > 0)
                        f["Средний балл"] = Math.Round((sr_ball / count_sk), 2).ToString();
                    else
                        f["Средний балл"] = "0";
                    data.Rows.Add(f);
                }
                data_grid.ItemsSource = data.DefaultView;
                data_grid.Columns[0].Width = 250;
                for (int i = 1; i < data_grid.Columns.Count; i++)
                {
                    data_grid.Columns[i].Width = 130;
                }
            }
            catch {  }
        }
    }
}
