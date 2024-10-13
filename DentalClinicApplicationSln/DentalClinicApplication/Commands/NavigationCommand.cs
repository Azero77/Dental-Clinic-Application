using DentalClinicApp.Services;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace DentalClinicApp.Commands
{
    public class NavigationCommand : CommandBase
    {
        public INavigationService NavigationService { get; }

        public NavigationCommand(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
                NavigationService.Navigate(parameter);
        }
    }
}
