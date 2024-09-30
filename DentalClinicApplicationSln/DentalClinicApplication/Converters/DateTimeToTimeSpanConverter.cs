using System;
using System.Globalization;
using System.Windows.Data;


namespace DentalClinicApplication.Converters
{
    public class TimeSpanToDateTimeConverter : IValueConverter
    {
        // Converts TimeSpan to DateTime (for binding to the TimePicker)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                return new DateTime(timeSpan.Ticks); // Create DateTime using TimeSpan's ticks
            }
            return DateTime.MinValue;
        }

        // Converts DateTime back to TimeSpan (when user changes time in the TimePicker)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.TimeOfDay; // Return only the TimeOfDay as a TimeSpan
            }
            return TimeSpan.Zero;
        }
    }

}
