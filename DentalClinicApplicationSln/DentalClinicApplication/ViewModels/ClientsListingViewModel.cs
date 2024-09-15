using DentalClinicApp.Models;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DentalClinicApp.ViewModels
{
    public class ClientsListingViewModel : ViewModelBase
    {
        public IClientsProvider DbClientsProvider { get; }
        public ObservableCollection<Client>? Clients { get; set; }

        public ClientsListingViewModel(IClientsProvider dbClientsProvider)
        {
            DbClientsProvider = dbClientsProvider;
            Load();
        }

        public void Load()
        {
            Clients = new ObservableCollection<Client>(DbClientsProvider.GetClients().Result);
        }
    }
}
