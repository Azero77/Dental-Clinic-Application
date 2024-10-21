using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services
{
    class CompositeNavigationService : INavigationService
    {
        INavigationService[] NavigationServices { get; }
        public CompositeNavigationService(params INavigationService[] navigationServices)
        {
            NavigationServices = navigationServices;
        }
        public void Navigate(object? param)
        {
            foreach (INavigationService navigationService in NavigationServices)
            {
                navigationService.Navigate(param);
            }
        }
    }
}
