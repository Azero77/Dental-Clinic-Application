using DentalClinicApplication.Services.DataProvider;
using DentalClinicApplication.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services
{
    /// <summary>
    /// Class For Changing the provider For the Collection Store COllection
    /// </summary>
    /// <typeparam name="T">type of Item to provide</typeparam>
    public class ProviderChangerService<T>
    {
        public ProviderChangerService(IProvider<T> newProvider, ICollectionStore<T> collectionStore)
        {
            NewProvider = newProvider;
            CollectionStore = collectionStore;
        }

        public IProvider<T> NewProvider { get; }
        public ICollectionStore<T> CollectionStore { get; }

        public void Change()
        {
            CollectionStore.ChangeProvider(NewProvider);
        }
    }
}
