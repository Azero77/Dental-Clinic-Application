using IconRadioButton;
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

namespace DentalClinicApplication.Components
{
    /// <summary>
    /// Interaction logic for NavigationBarView.xaml
    /// </summary>
    public partial class NavigationBarView : UserControl
    {
        public NavigationBarView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            double maxWidth = 0;
            List<IconRadioButton.IconRadioButton> iconRadioButtons = FindVisualChildren<IconRadioButton.IconRadioButton>(this);
            //get maximum width
            foreach (var item in iconRadioButtons)
            {
                double width = item.ActualWidth;
                maxWidth = Math.Max(maxWidth, width);
            }
            foreach (var item in iconRadioButtons)
            {
                item.Width = maxWidth * 1.4;
            }

        }
        private static List<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            List<T> result = new List<T>();

            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child is T)
                    {
                        result.Add((T)child);
                    }

                    result.AddRange(FindVisualChildren<T>(child)); // Recursive call to find children
                }
            }

            return result;
        }


    }
}
