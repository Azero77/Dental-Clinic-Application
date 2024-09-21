using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    internal class VirtualizedClientsProvider
        :
        VirtualizedProvider<Client>
    {
        public DbContext DataContext { get; }

        public VirtualizedClientsProvider(DbContext dataContext)
            : base(dataContext,"","Clients")
        {
            DataContext = dataContext;
        }
       
        public override async Task<IList<Client>> FetchRange(int start, int size)
        {
            return (await GetClients(start, size)).ToList();
        }

        public async Task<IEnumerable<Client>> GetClients(int start,int size)
        {

            string sql = "SELECT * FROM Clients LIMIT @size OFFSET @start;";
            object param = new
            {
                start = start,
                size = size
            };
            IEnumerable<ClientDTO> ClientsDTOs = await DataContext.RunAsync<IEnumerable<ClientDTO>>(async conn =>
            {
                return await conn.QueryAsync<ClientDTO>(sql, param);
            });
            return ClientsDTOs.Select(cDTO => Client.ToClient(cDTO));
        }
    }
}
