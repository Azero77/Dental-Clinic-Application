using DentalClinicApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentalClinicApp.Stores
{
    /// <summary>
    /// Stores the current ViewModel of the application
    /// </summary>
    public class NavigationStore
    {
        private ViewModelBase? _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                CurrentViewModel?.Dispose();
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        public event Action? CurrentViewModelChanged;
        public void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
