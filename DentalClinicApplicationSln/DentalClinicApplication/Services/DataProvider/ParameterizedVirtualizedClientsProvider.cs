using DentalClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    public class ParameterizedVirtualizedClientsProvider
        : IVirtualizationItemsProvider<Client>
        , IParameterizedProvider<Client>
    {
        public string Predicate => throw new NotImplementedException();

        public Task<int> FetchCount()
        {
            throw new InvalidCastException();
        }

        public Task<IList<Client>> FetchRange(int start, int size)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Client>> GetItems()
        {
            throw new NotImplementedException();
        }
    }
}
