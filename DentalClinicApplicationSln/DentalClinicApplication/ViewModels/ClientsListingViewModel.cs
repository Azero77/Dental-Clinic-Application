using DentalClinicApp.Commands;
using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Services.DataProvider;
using DentalClinicApplication.Stores;
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
        public IProvider<Client> DbClientsProvider { get; }
        public ICollectionStore<Client> ClientsStore { get; }
        public IEnumerable<Client> Clients => ClientsStore.Collection;

        private bool _isLoading = false;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        #region Commands
        public ICommand NavigateToEditClientView { get; }
        public ClientsManipulationCommand DeleteClientCommand { get; }
        #endregion

        public ClientsListingViewModel(
            IProvider<Client> dbClientsProvider,
            INavigationService<ClientsManipulationViewModel> navigationService,
            IDataManipulator dataDeleter,
            ICollectionStore<Client> clientsStore
            )
        {
            DbClientsProvider = dbClientsProvider;
            ClientsStore = clientsStore;
            ClientsStore.CollectionChanged += ClientsStore_CollectionChanged;
            NavigateToEditClientView = new NavigationCommand(navigationService);
            DeleteClientCommand = new ClientsDeleteCommand(dataDeleter);
        }

        private void ClientsStore_CollectionChanged()
        {
            OnPropertyChanged(nameof(Clients));
        }

        public static ClientsListingViewModel GetClientsListingViewModel(
            IProvider<Client> dbClientsProvider,
            INavigationService<ClientsManipulationViewModel> navigationService,
            IDataManipulator dataDeleter,
            ICollectionStore<Client> clientsStore
            )
        {
            ClientsListingViewModel viewModel = new ClientsListingViewModel
                (dbClientsProvider,
                navigationService,
                dataDeleter,
                clientsStore);
            ICommand LoadClients = new LoadCommand<Client>(viewModel, clientsStore);
            LoadClients.Execute(null);
            return viewModel;
        }
        
    }
}
