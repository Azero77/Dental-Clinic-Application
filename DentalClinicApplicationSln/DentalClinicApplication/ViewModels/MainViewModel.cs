using DentalClinicApp.Stores;
using DentalClinicApplication.Stores;
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
        public ModalNavigationStore ModalNavigationStore { get; }
        public ViewModelBase? CurrentModal => ModalNavigationStore.CurrentModal;
        public bool IsOpen => ModalNavigationStore.IsOpen;

        public MainViewModel(
            NavigationStore navigationStore,ModalNavigationStore modalNavigationStore)

        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            ModalNavigationStore.CurrentModalChanged += OnCurrentModalChanged;
            NavigationStore.CurrentViewModelChanged += CurrentViewModelChanged;
        }

        private void OnCurrentModalChanged()
        {
            OnPropertyChanged(nameof(CurrentModal));
            OnPropertyChanged(nameof(IsOpen));

        }

        private void CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
