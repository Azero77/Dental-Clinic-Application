using DentalClinicApp.Stores;
using DentalClinicApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentalClinicApp.Services
{
    public class NavigationService
    {
        public NavigationStore NavigationStore { get; }
        public Func<object, ViewModelBase> ViewModelFactory { get; }

        public NavigationService(NavigationStore navigationStore, Func<object,ViewModelBase> viewModelFactory)
        {
            NavigationStore = navigationStore;
            ViewModelFactory = viewModelFactory;
        }

        public void Navigate(object param)
        {
            NavigationStore.CurrentViewModel = ViewModelFactory(param);
        }
    }
}
