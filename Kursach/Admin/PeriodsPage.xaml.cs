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
    /// Логика взаимодействия для PeriodsPage.xaml
    /// </summary>
    public partial class PeriodsPage : UserControl
    {
        public PeriodsPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AdminControl();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Внимание!\n\n" +
                "Период - смена семетра/четверти/триместра обучения у учащихся. В данном случае студенты и дисциплины сохраняются в рамках учебных групп и классов. Производится лишь удаление ранее созданных тестов и результатов!\n\n" +
                "Год - переход на новый учебный год, учащиеся переводятся на новую параллель (1А>2А), студенты 4 курсов и учащиеся 11 классов выбывают из системы. Все ранее созданные тесты и результаты удаляются!\n\n" +
                "При смене года осуществялется удаление всех дисциплин во всех группах и классах.\n\n" +
                "При смене периода все дисциплины сохраняются, в случае изменений в учебном плане администратуору необходимо внести изменения вручную!");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что желаете удалить тесты и результаты тестов у студентов 1-4 курсов?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                QueriesTableAdapter a = new QueriesTableAdapter();
                a.DeleteTestsStudents();
                MessageBox.Show("Все ранее созданные тесты и реузльтаты у студентов 1-4 курсов удалены!");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что желаете удалить тесты и результаты тестов у учащихся 1-11 классов?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                QueriesTableAdapter a = new QueriesTableAdapter();
                a.DeleteTestsUchas();
                MessageBox.Show("Все ранее созданные тесты и реузльтаты у учащихся 1-11 классов удалены!");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены что желаете удалить все результаты тестов? Все учащиеся 11 классов и студентов 4 курсов будут удалены, а другие будут переведены на новые параллели.\nДисциплины все удаляются!", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                QueriesTableAdapter a = new QueriesTableAdapter();
                a.DeleteDisciplins();
                a.DeleteEndClasses();
                a.DeleteParallels();
                a.NewParalleli();
                Controllers.ClearUsers();
                MessageBox.Show("Смена года прошла успешно");
            }
        }
    }
}
