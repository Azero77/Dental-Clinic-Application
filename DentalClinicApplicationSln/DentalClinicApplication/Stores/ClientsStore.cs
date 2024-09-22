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
    public class ClientsStore : ICollectionStore<Client>
    {
        public IEnumerable<Client> Collection { get; set; }
        public IProvider<Client> Provider { get; set; }

        private Lazy<Task> _initialize;

        public ClientsStore(
            IProvider<Client> clientProvider,
            ManipulationNotifierService manipulationNotifierService)
        {
            Provider = clientProvider;
            manipulationNotifierService.DataManipulated += OnDataManipulated;
            _initialize = new Lazy<Task>(Initialize);
            //Collection = new VirtualizationCollection<Client>( (IVirtualizationItemsProvider<Client>) Provider);
        }

        public void OnDataManipulated()
        {
            //reloading clients
            _initialize = new Lazy<Task>(Initialize);
            Load().ConfigureAwait(false);
        }

        public async Task Initialize()
        {
            Collection = await Provider.GetItems();
            OnCollectionChanged();
        }

        public event Action? CollectionChanged;
        private void OnCollectionChanged()
        {
            CollectionChanged?.Invoke();
        }

        public async Task Load()
        {
            await _initialize.Value;
        }

        public void ChangeProvider(IProvider<Client> newProvider)
        {
            Provider = newProvider;
            OnDataManipulated();
        }
    }
}
