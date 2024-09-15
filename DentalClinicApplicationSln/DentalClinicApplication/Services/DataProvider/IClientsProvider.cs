using DentalClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    public interface IClientsProvider
    {
        Task<IEnumerable<Client>> GetClients();
    }
}
