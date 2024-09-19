using DentalClinicApp.Models;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    public class ClientsEditCommand : ClientsManipulationCommand
    {
        public ClientsEditCommand(IDataManipulator dataManipulator, INavigationService navigationService) : base(dataManipulator,navigationService)
        {
            
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            //do logic
            Client? client = parameter as Client;
            if (client is null)
            {
                throw new InvalidCastException();
            }
            await DataManipulator.Manipulate(client);
            OnDataManipulated();
            //navigat
            NavigationService.Navigate(null);
        }
    }
}
