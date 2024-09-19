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
namespace DentalClinicApplication.Stores
{
    public class VirtualizedCollectionStore<T> : ICollectionStore<T>
    {
        public VirtualizedCollectionStore(
            IVirtualizationItemsProvider<T> itemsProvider,
            VirtualizationCollection<T> collection,
            ManipulationNotifierService manipulationNotifierService)
        {
            Provider = itemsProvider;
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
            await _collection.Reload();
            await _collection.LoadCount();
            await _collection.RenderPage(_collection.CurrentPageIndex);
            OnCollectionChanged();
        }
        public async Task Load()
        {
            await _initialize.Value;
        }

        private Lazy<Task> _initialize;



        public IProvider<T> Provider { get; }

        private VirtualizationCollection<T> _collection;
        public IEnumerable<T> Collection => _collection;

    }
}
