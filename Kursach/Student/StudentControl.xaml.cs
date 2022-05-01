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

namespace Kursach.Student
{
    /// <summary>
    /// Логика взаимодействия для StudentListDisciplins.xaml
    /// </summary>
    public partial class StudentControl : UserControl
    {
        public StudentControl()
        {
            InitializeComponent();
            Controllers.fram_prep = control;
            Controllers.fram_prep.Content = new StudentListDisciplins();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.auth_login = "";
            Controllers.fram.Content = new Authentication();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new UpravlenieAccountStudent();
        }
    }
}
