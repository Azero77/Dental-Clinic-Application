using DentalClinicApp.Stores;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services
{
    public class LayoutNavigationService<TViewModel>
        : INavigationService<TViewModel>
        where TViewModel : ViewModelBase
    {
        public LayoutNavigationService(
                                       NavigationStore navigationStore,
                                       Func<object?, TViewModel> contentViewModelFactory,
                                       Func<object?, NavigationBarViewModel> navigationBarViewModelFactory)
        {
            ContentViewModelFactory = contentViewModelFactory;
            NavigationStore = navigationStore;
            NavigationBarViewModelFactory = navigationBarViewModelFactory;
        }

        public Func<object?, TViewModel> ContentViewModelFactory { get; }
        public Func<object?, NavigationBarViewModel> NavigationBarViewModelFactory { get; }
        public NavigationStore NavigationStore { get; }

        public void Navigate(object? param)
        {
            NavigationStore.CurrentViewModel = new LayoutViewModel(NavigationBarViewModelFactory(param),ContentViewModelFactory(param));
        }
    }
}
