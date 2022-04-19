using Kursach.Admin;
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

namespace Kursach.Prepodavatel
{
    /// <summary>
    /// Логика взаимодействия для PrepodavatelControl.xaml
    /// </summary>
    public partial class PrepodavatelControl : UserControl
    {
        public PrepodavatelControl()
        {
            InitializeComponent();
            TablePrepodavatelTableAdapter a = new TablePrepodavatelTableAdapter();
            DataBase.TablePrepodavatelDataTable b = new DataBase.TablePrepodavatelDataTable();
            a.FillByLogin(b, Controllers.auth_login);
            if (b[0].KLRukovod == "Не назначено")
                ClRuk.IsEnabled = false;
            if (b[0].Prepodavanie == "Не преподаёт")
                prepod.IsEnabled = false;
            if(b[0].KLRukovod == "Не назначено" && b[0].Prepodavanie == "Не преподаёт")
            {
                MessageBox.Show("Вам не назначены группы и дисциплины для преподавания, а также классное руководство!\nОбратитесь к администратору.");
                Controllers.fram.Content = new Authentication();
            }
            Controllers.fram_prep = control;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.auth_login = "";
            Controllers.fram.Content = new Authentication();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new UpravlenieAccountPrepod();
        }

        private void prepod_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new UpravlenieTests();
        }
    }
}
