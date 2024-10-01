using DentalClinicApp.Models;
using DentalClinicApplication.Services.DataManiplator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataManiplator
{
    public interface IDataService<T>
    {
        IDataManipulator Manipulator { get; }
        Task CreateAsync(T item);
        Task EditAsync(T item);
        Task DeleteAsync(T item);

    }
}
