using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Stores
{

    public interface ICollectionStore<T>
    {
        public IEnumerable<T> Collection { get;}
        public IProvider<T> Provider { get; }
        event Action CollectionChanged;
        void OnDataManipulated();
        Task Load();
    }
}
