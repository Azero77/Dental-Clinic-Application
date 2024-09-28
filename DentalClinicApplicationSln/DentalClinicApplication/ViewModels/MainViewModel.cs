using DentalClinicApp.Stores;
using DentalClinicApplication.ViewModels;
using DentalClinicApplication.ViewModels.Configuration;
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
        public ConfigurationViewModel ConfigurationViewModel { get; }

        public MainViewModel(NavigationStore navigationStore, ConfigurationViewModel configurationViewModel)
        {
            NavigationStore = navigationStore;
            NavigationStore.CurrentViewModelChanged += CurrentViewModelChanged;
            ConfigurationViewModel = configurationViewModel;
        }

        private void CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
