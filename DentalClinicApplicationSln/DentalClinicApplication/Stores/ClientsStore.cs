using DentalClinicApp.Models;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Stores
{
    /// <summary>
    /// Stores the collection of the client for the application lifecycle
    /// </summary>
    public class ClientsStore
    {
        public ObservableCollection<Client> Clients { get; set; } = new();
        public IClientsProvider ClientsProvider { get; }

        private Lazy<Task> _initialize;

        public ClientsStore(IClientsProvider clientProvider,
            ManipulationNotifierService manipulationNotifierService)
        {
            ClientsProvider = clientProvider;
            manipulationNotifierService.DataManipulated += OnDataManipulated;
            _initialize = new Lazy<Task>(Initialize);
        }

        private void OnDataManipulated()
        {
            //reloading clients
            _initialize = new Lazy<Task>(Initialize);
        }

        public async Task Initialize()
        {
            var result = await ClientsProvider.GetClients();
            Clients.Clear();
            foreach (var item in result)
                Clients.Add(item);
        }

        public async Task Load()
        {
            await _initialize.Value;
        }
    }
}
