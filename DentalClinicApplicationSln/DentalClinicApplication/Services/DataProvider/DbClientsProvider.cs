using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    public class DbClientsProvider : IClientsProvider
    {
        public DbContext DataContext { get; }

        public DbClientsProvider(DbContext dataContext)
        {
            DataContext = dataContext;
        }
        public async Task<IEnumerable<Client>> GetClients()
        {
            string sql = "SELECT * FROM Clients";
            IEnumerable<ClientDTO> ClientsDTO = await DataContext.RunAsync(async (conn) => 
            {
                return await conn.QueryAsync<ClientDTO>(sql);
            });

            return ClientsDTO.Select(cDTO => Client.ToClient(cDTO));
        }
    }
}
