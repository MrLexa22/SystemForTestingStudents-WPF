using Kursach.DataBaseTableAdapters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Kursach.Prepodavatel
{
    /// <summary>
    /// Логика взаимодействия для UpravlenieTests.xaml
    /// </summary>
    public partial class UpravlenieTests : UserControl
    {

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

        public int ID_Prepodavatel = 0;
        CountModel countModel = new CountModel();
        public UpravlenieTests()
        {
            InitializeComponent();

            TablePrepodavatelTableAdapter a = new TablePrepodavatelTableAdapter();
            DataBase.TablePrepodavatelDataTable b = new DataBase.TablePrepodavatelDataTable();
            a.FillByLogin(b, Controllers.auth_login);
            ID_Prepodavatel = b[0].ID_Prepodavatel;

            DiscipliniTablePrepodavatelTableAdapter a1 = new DiscipliniTablePrepodavatelTableAdapter();
            DataBase.DiscipliniTablePrepodavatelDataTable b1 = new DataBase.DiscipliniTablePrepodavatelDataTable();
            a1.Fill(b1, ID_Prepodavatel);

            TvBox.ItemsSource = b1;
            DataContext = countModel;
        }

        private void TvBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            countModel.Counter = (int)TvBox.ActualWidth / 460;
            if (countModel.Counter < 2)
                countModel.Counter = 2;
        }

        private void TvBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(TvBox.SelectedValue != null)
                Controllers.fram_prep.Content = new Lenta(Convert.ToInt16(TvBox.SelectedValue));
        }
    }
}
