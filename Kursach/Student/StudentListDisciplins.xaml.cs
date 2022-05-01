using Kursach.DataBaseTableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для StudentListDisciplins.xaml
    /// </summary>
    public partial class StudentListDisciplins : UserControl
    {
        public StudentListDisciplins()
        {
            InitializeComponent();

            TableStudentsTableAdapter a = new TableStudentsTableAdapter();
            DataBase.TableStudentsDataTable b = new DataBase.TableStudentsDataTable();
            a.FindByLogin(b, Controllers.auth_login);

            DiscipliniTablePrepodavatelTableAdapter a1 = new DiscipliniTablePrepodavatelTableAdapter();
            DataBase.DiscipliniTablePrepodavatelDataTable b1 = new DataBase.DiscipliniTablePrepodavatelDataTable();
            a1.FillByIdClass(b1, b[0].Class_ID);

            TvBox.ItemsSource = b1;
            DataContext = countModel;
        }

        public class CountModel : INotifyPropertyChanged
        {
            private int _counter;

            public int Counter
            {
                get { return _counter; }
                set
                {
                    _counter = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        CountModel countModel = new CountModel();

        private void TvBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            countModel.Counter = (int)TvBox.ActualWidth / 460;
            if (countModel.Counter < 2)
                countModel.Counter = 2;
        }

        private void TvBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TvBox.SelectedValue != null)
                Controllers.fram_prep.Content = new LentaDisciplina(Convert.ToInt16(TvBox.SelectedValue));
        }
    }
}
