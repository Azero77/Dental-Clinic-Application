using DentalClinicApp.ViewModels;
using DentalClinicApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DentalClinicApplication.Converters
{
    internal class CurrentViewModelNavigationBarIsCheckedConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            //check if the current view model is Layoutviewmodel to get the inner view model
            if (parameter is Type vmType)
            {
                if (value is LayoutViewModel layoutViewModel)
                {
                    return layoutViewModel.ContentViewModel.GetType() == vmType;
                }
                else if (value is ViewModelBase currentViewModel)
                {
                    return currentViewModel.GetType() == vmType;
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
