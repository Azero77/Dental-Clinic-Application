using DentalClinicApplication.VirtualizationCollections;
using DentalClinicApp.Models;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalClinicApplication.Services;
namespace DentalClinicApplication.Stores
{
    public class VirtualizedCollectionStore<T> : ICollectionStore<T>
    {
        public VirtualizedCollectionStore(
            VirtualizationCollection<T> collection,
            ManipulationNotifierService manipulationNotifierService)
        {
            Provider = collection.ItemsProvider;
            _collection = collection;
            _initialize = new(Initialize);
            manipulationNotifierService.DataManipulated += OnDataManipulated;
        }

        public void OnDataManipulated()
        {
            _initialize = new Lazy<Task>(Initialize);
            Load().ConfigureAwait(false);
        }


        public event Action? CollectionChanged;
        public void OnCollectionChanged()
        {
            CollectionChanged?.Invoke();
        }

        private async Task Initialize()
        {
            await _collection.Load();
            OnCollectionChanged();
        }
        public async Task Load()
        {
            await _initialize.Value;
        }

        public void ChangeProvider()
        {
            _initialize = new(Initialize);
        }

        private Lazy<Task> _initialize;



        public IProvider<T> Provider { get; set; }

        private VirtualizationCollection<T> _collection;
        public IEnumerable<T> Collection => _collection;

    }
}
