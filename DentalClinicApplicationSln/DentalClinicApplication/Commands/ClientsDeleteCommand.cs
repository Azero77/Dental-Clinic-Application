using DentalClinicApp.Models;
using DentalClinicApplication.Services.DataManiplator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    public class ClientsDeleteCommand : ClientsManipulationCommand
    {
        public ClientsDeleteCommand(IDataManipulator dataDeleter):base(dataDeleter,null)
        {
            DataDeleter = dataDeleter;
        }

        public IDataManipulator DataDeleter { get; }

        public override async Task ExecuteAsync(object? parameter)
        {
            Client? client = parameter as Client;
            if (client is not null)
            { 
                await DataDeleter.Manipulate(client);
                OnDataManipulated();
                return;
            }

            throw new InvalidCastException();

        }
    }
}
