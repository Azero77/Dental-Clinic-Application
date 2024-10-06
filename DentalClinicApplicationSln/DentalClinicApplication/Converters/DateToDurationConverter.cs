using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                string startDateString = startDate.ToString("t", culture);
                string endDateString = startDate.Add(Duration).ToString("t", culture);

                return $"{startDateString} - {endDateString}";
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    
}
