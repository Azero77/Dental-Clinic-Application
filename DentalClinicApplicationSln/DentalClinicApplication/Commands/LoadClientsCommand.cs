using DentalClinicApp.Models;
using DentalClinicApplication.Stores;
using DentalClinicApplication.ViewModels;
using DentalClinicApplication.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    public class LoadCommand<T> : AsyncCommandBase
    {
        public LoadCommand(ClientsListingViewModel clientsListingViewModel,
            ICollectionStore<T> clientsStore)
        {
            ClientsListingViewModel = clientsListingViewModel;
            ClientsStore = clientsStore;
        }

        public ClientsListingViewModel ClientsListingViewModel { get; }
        public ICollectionStore<T> ClientsStore { get; }

        public override async Task ExecuteAsync(object? parameter)
        {
            ClientsListingViewModel.IsLoading = true;

            try
            {
                await ClientsStore.Load();
            }
            catch (Exception)
            {

                throw;
            }
            ClientsListingViewModel.IsLoading = false;
        }
    }
}
