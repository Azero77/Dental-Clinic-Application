using DentalClinicApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    /// <summary>
    /// Providers For Collections Only (not for singls)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProvider<T>
    {
        Task<IEnumerable<T>> GetItems();
        void ChangeProvider
            (string? whereClause,
            string? orderByClause);
    }
}