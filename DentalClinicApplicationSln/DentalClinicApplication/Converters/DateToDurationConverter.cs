using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DentalClinicApplication.Converters
{
    public class DateToDurationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0].GetType() == typeof(DateTime))
            {
                DateTime startDate = (DateTime)values[0];
                TimeSpan Duration = (TimeSpan)values[1];

                return $"{startDate:HH:mm tt} - {startDate.Add(Duration):HH:mm tt}";
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
