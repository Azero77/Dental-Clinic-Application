using DentalClinicApp.Commands;
using DentalClinicApp.Services;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace DentalClinicApplication.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {
        public ICommand HomePageNavigationCommand { get; }

        public NavigationBarViewModel(
            INavigationService homePageNavigationCommand
            )
        {
            HomePageNavigationCommand = new NavigationCommand(homePageNavigationCommand);
        }
    }
}
