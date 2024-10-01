using DentalClinicApp.Commands;
using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.ComponentsViewModels;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Services.DataProvider;
using DentalClinicApplication.Stores;
using DentalClinicApplication.VirtualizationCollections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    public class ClientsListingViewModel : ViewModelBase
    {
        public IProvider<Client> DbClientsProvider { get; }
        public ICollectionStore<Client> ClientsStore { get; }
        public VirtualizedClientsComponentViewModel VirtualizedClientsComponentViewModel { get; set; }

        public IEnumerable<Client> Clients => ClientsStore.Collection;

        #region Commands
        public ICommand NavigateToEditClientView { get; }
        public ClientsManipulationCommand DeleteClientCommand { get; }
        #endregion

        public ClientsListingViewModel(
            IProvider<Client> dbClientsProvider,
            INavigationService<ClientsManipulationViewModel> navigationService,
            IDataManipulator dataDeleter,
            ICollectionStore<Client> clientsStore,
            VirtualizedClientsComponentViewModel virtualizedClientsComponentViewModel)
            
        {
            DbClientsProvider = dbClientsProvider;
            ClientsStore = clientsStore;
            ClientsStore.CollectionChanged += ClientsStore_CollectionChanged;
            NavigateToEditClientView = new NavigationCommand(navigationService);
            //DeleteClientCommand = new ClientsDeleteCommand(dataDeleter);
            VirtualizedClientsComponentViewModel = virtualizedClientsComponentViewModel;
        }

        private void ClientsStore_CollectionChanged()
        {
            OnPropertyChanged(nameof(Clients));
        }

        public static ClientsListingViewModel GetClientsListingViewModel(
            IProvider<Client> dbClientsProvider,
            INavigationService<ClientsManipulationViewModel> navigationService,
            IDataManipulator dataDeleter,
            ICollectionStore<Client> clientsStore,
            VirtualizedClientsComponentViewModel virtualizedClientsComponentViewModel
            )
        {
            ClientsListingViewModel viewModel = new ClientsListingViewModel
                (dbClientsProvider,
                navigationService,
                dataDeleter,
                clientsStore,
                virtualizedClientsComponentViewModel);
            //Change hereeeee
            //virtualizedClientsComponentViewModel.Load();
            ICommand LoadCommand = new LoadVirtualizationCollectionCommand<Client>(virtualizedClientsComponentViewModel, clientsStore);
            LoadCommand.Execute(null);
            return viewModel;
        }
    }
}
