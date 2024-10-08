﻿using DentalClinicApplication.Windows;
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

namespace DentalClinicApplication.Views
{
    /// <summary>
    /// Interaction logic for MakeEditAppointmentView.xaml
    /// </summary>
    public partial class MakeEditAppointmentView : UserControl
    {
        public MakeEditAppointmentView()
        {
            InitializeComponent();
        }

        private void DescriptionContainer_Loaded(object sender, RoutedEventArgs e)
        {
            DescriptionTextBox.Height = 0.6 *  ((StackPanel)sender).ActualHeight;
        }
    }
}
