using DentalClinicApplication.Stores;
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
    public class TypeToStyleConverter : IValueConverter
    {
        private readonly Style _statusStyle = (Style) Application.Current.FindResource("MessageTypeStatusStyle");
        private readonly Style _errorStyle = (Style) Application.Current.FindResource("MessageTypeErrorStyle");
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MessageType messageType)
            {
                switch (messageType)
                {
                    case MessageType.Status:
                        return _statusStyle;
                    case MessageType.Error:
                        return _errorStyle;
                    default:
                        break;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
