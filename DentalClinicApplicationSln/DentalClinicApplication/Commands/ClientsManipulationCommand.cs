using DentalClinicApp.Commands;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    /// <summary>
    /// Base Class for Update And insert commands
    /// </summary>
    // Make SUre that ClientsCreate and ClientsEdit do different logic, if not let the ManipulationCOmmand do all the logic
    public abstract class ClientsManipulationCommand : AsyncCommandBase
    {
        public ClientsManipulationCommand(IDataManipulator dataManipulator,INavigationService? navigationService)
        {
            DataManipulator = dataManipulator;
            NavigationService = navigationService;
        }

        public IDataManipulator DataManipulator { get; }
        public INavigationService? NavigationService { get; }

        public event Action? DataManipulated;
        public void OnDataManipulated()
        {
            DataManipulated?.Invoke();
        }
    }
}
