using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for HomePageView.xaml
    /// </summary>
    public partial class HomePageView : UserControl
    {
        public HomePageView()
        {
            InitializeComponent();
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }

        private void CalendarSelection_Loaded(object sender, RoutedEventArgs e)
        {
            double scaleX = 1.2;
            double scaleY = 1.2;
            ScaleTransform scaleTransform = new(scaleX, scaleY);

            CalendarSelection.RenderTransform = scaleTransform;
            CalendarSelection.Width *= scaleX;
            CalendarSelection.Height *= scaleY;
            Button n = new();
        }

        private void DataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SearchStackPanel.Width = e.NewSize.Width / 2;
        }
    }
}
