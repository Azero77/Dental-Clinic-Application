using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PlaceHolderTextControl.Converters
{
    public class PaddingToLeftConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Thickness padding)
            {
                return value;
            }
            if (!double.TryParse(parameter.ToString(), out double leftPadding))
            {
                return value;
            }
            padding.Left += leftPadding;
            return padding;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
