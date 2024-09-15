using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Navigation;

namespace DentalClinicApp.Commands
{
    public class NavigationCommand : CommandBase
    {
        public NavigationService NavigationService { get; }

        public NavigationCommand(NavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            NavigationService.Navigate(parameter);
        }
    }
}
