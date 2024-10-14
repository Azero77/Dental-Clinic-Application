﻿using System;
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

namespace DentalClinicApplication.Views
{
    /// <summary>
    /// Interaction logic for ClientProfileView.xaml
    /// </summary>
    public partial class ClientProfileView : UserControl
    {
        public ClientProfileView()
        {
            InitializeComponent();
        }

        private void PropertiesWrapPanel_Loaded(object sender, RoutedEventArgs e)
        {
            PropertiesWrapPanel.Width = 0.5 * PropertiesWrapPanel.ActualWidth;
        }
    }
}