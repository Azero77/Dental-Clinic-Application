using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.ViewModels
{
    public class ClientsListingViewModel : ViewModelBase
    {
        public IClientsProvider DbClientsProvider { get; }
        public ObservableCollection<Client> Clients { get; set; } = new();

        public ClientsListingViewModel(IClientsProvider dbClientsProvider)
        {
            DbClientsProvider = dbClientsProvider;

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

        private void Clients_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Clients));
        }
    }
}
