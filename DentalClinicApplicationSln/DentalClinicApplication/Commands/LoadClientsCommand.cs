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
    public class LoadClientsCommand : AsyncCommandBase
    {
        public LoadClientsCommand(ClientsListingViewModel clientsListingViewModel,
            ClientsStore clientsStore)
        {
            ClientsListingViewModel = clientsListingViewModel;
            ClientsStore = clientsStore;
        }

        public ClientsListingViewModel ClientsListingViewModel { get; }
        public ClientsStore ClientsStore { get; }

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
