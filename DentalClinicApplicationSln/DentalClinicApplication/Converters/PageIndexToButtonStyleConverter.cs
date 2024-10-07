using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DentalClinicApplication.Converters
{
    public class PageIndexToButtonStyleConverter
        : IMultiValueConverter
    {
        private Style NormalButton =(Style) Application.Current.FindResource("PaginationNormalButtonStyle");
        private Style SelectedButton =(Style) Application.Current.FindResource("PaginationSelectedButtonStyle");


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int currentIndex && values[1] is int buttonIndex)
            {
                return currentIndex == buttonIndex ?  SelectedButton : NormalButton; // Change colors based on the index
            }
            return DependencyProperty.UnsetValue; // Default color
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
