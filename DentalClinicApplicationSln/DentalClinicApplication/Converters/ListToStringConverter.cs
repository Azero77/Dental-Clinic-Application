using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace DentalClinicApplication.Converters
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<ValidationError>? list = value as IEnumerable<ValidationError>;
            StringBuilder stringBuilder = new();
            if (list is not null)
            {
                foreach (var item in list)
                {
                    stringBuilder.Append(item.ErrorContent.ToString() + "\n");
                }
            }
            return stringBuilder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
