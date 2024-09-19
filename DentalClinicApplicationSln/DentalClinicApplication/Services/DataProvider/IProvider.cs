using DentalClinicApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    public interface IProvider<T>
    {
        Task<IEnumerable<T>> GetItems();
    }
}