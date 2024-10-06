using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DentalClinicApp.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void Dispose()
        {
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
