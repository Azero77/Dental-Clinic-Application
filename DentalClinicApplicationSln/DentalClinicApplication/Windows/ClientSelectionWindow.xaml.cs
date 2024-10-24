﻿using DentalClinicApp.Models;
using DentalClinicApplication.ViewModels;
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

namespace DentalClinicApplication.Windows
{
    /// <summary>
    /// Interaction logic for ClientSelectionWindow.xaml
    /// </summary>
    public partial class ClientSelectionWindow : Window
    {
        public ClientSelectionWindow(ClientSelectionViewModel clientSelectionViewModel)
        {
            DataContext = clientSelectionViewModel;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
