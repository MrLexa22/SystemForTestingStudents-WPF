﻿using Kursach.DataBaseTableAdapters;
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

namespace Kursach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Controllers.fram = control;
            Controllers.fram.Content = new Authentication();
            Controllers.ClearUsers();
        }
    }

    public static class Controllers
    {
        public static ContentControl fram;
        public static string auth_login;
        public static ContentControl fram_prep;
        public static DateTime DateTimeTest;
        public static void ClearUsers()
        {
            UsersTableAdapter a = new UsersTableAdapter();
            a.DeleteUsersLishnie();
        }
    }
}
