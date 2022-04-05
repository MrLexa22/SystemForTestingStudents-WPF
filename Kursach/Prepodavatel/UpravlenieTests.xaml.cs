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
    /// Логика взаимодействия для UpravlenieTests.xaml
    /// </summary>
    public partial class UpravlenieTests : UserControl
    {
        public UpravlenieTests()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new CreateTest(null);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Controllers.fram_prep.Content = new Chernoviki();
        }
    }
}
