using DentalClinicApp.Commands;
using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    public class ClientsListingViewModel : ViewModelBase
    {
        public IClientsProvider DbClientsProvider { get; }
        public ObservableCollection<Client> Clients { get; set; } = new();


        #region Commands
        public ICommand NavigateToEditClientView { get; }
        public ClientsManipulationCommand DeleteClientCommand { get; }
        #endregion

        public ClientsListingViewModel(IClientsProvider dbClientsProvider,
            INavigationService<ClientsManipulationViewModel> navigationService,
            IDataManipulator dataDeleter
            )
        {
            DbClientsProvider = dbClientsProvider;
            NavigateToEditClientView = new NavigationCommand(navigationService);
            DeleteClientCommand = new ClientsDeleteCommand(dataDeleter);
            DeleteClientCommand.DataManipulated += Clients_CollectionChanged;
            Load();
        }

        public async Task Load()
        {
            var result = await DbClientsProvider.GetClients();
            Clients.Clear();
            foreach (Client client in result)
            {
                Clients.Add(client);
            }

        }

        private void Clients_CollectionChanged()
        {
            OnPropertyChanged(nameof(Clients));
            Load();
        }
    }
}
