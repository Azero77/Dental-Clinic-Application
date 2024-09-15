using DentalClinicApp.Stores;
using DentalClinicApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentalClinicApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel => NavigationStore.CurrentViewModel;
        public NavigationStore NavigationStore { get; }
        public NavigationBarViewModel NavigationBarViewModel { get; }

        public MainViewModel(NavigationStore navigationStore, NavigationBarViewModel navigationBarViewModel)
        {
            NavigationStore = navigationStore;
            NavigationStore.CurrentViewModelChanged += CurrentViewModelChanged;
            NavigationBarViewModel = navigationBarViewModel;
        }

        private void CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
