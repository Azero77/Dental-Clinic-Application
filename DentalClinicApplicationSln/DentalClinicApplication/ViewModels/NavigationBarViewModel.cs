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
        public ICommand AllAppointmentsNavigationCommand { get; }
        public ICommand AllClientsNavigaitonCommand { get; }

        public NavigationBarViewModel(
            INavigationService homePageNavigationService,
            INavigationService allAppointmentsNavigationService,
            INavigationService allClientsNavigationService
            )
        {
            HomePageNavigationCommand = new NavigationCommand(homePageNavigationService);
            AllAppointmentsNavigationCommand = new NavigationCommand(allAppointmentsNavigationService);
            AllClientsNavigaitonCommand = new NavigationCommand(allClientsNavigationService);
        }
    }
}
