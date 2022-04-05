using Kursach.Admin.ImportElemetns;
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
    /// Логика взаимодействия для ImportPage.xaml
    /// </summary>
    public partial class ImportPage : UserControl
    {
        public ImportPage()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Controllers.fram.Content = new AdminControl();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            export_content.Content = new ImportPrepodavateli();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            export_content.Content = new ImportKontingent();
        }
    }
}
