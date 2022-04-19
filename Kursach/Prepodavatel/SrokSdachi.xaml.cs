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
using System.Windows.Shapes;

namespace Kursach.Prepodavatel
{
    /// <summary>
    /// Логика взаимодействия для SrokSdachi.xaml
    /// </summary>
    public partial class SrokSdachi : Window
    {
        public SrokSdachi()
        {
            InitializeComponent();
            date.DisplayDateStart = DateTime.Now;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(date.Text == "")
            {
                MessageBox.Show("Дата указана некорректно!");
                return;
            }
            if(time.Text == "")
            {
                MessageBox.Show("Время указано некорректно!");
                return;
            }

            string[] data1 = date.Text.Split(new char[] { '.' });
            string[] time1 = time.Text.Split(new char[] { ':' });
            DateTime date1 = new DateTime(Convert.ToInt16(data1[2]), Convert.ToInt16(data1[1]), Convert.ToInt16(data1[0]), Convert.ToInt16(time1[0]), Convert.ToInt16(time1[1]), 00);
            DateTime sr_datetime = new DateTime();
            sr_datetime = DateTime.Now;
            sr_datetime = sr_datetime.AddMinutes(4);
            if (date1 < sr_datetime)
            {
                MessageBox.Show("Срок выполнения не может быть меншье текущей даты. \nСрок выполнения должен быть задан на 5 минут больше от текущей даты!");
                return;
            }
            Controllers.DateTimeTest = date1;
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
