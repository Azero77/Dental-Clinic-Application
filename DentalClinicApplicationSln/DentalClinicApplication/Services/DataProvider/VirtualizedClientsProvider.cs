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
        IVirtualizationItemsProvider<Client>
    {
        public DbContext DataContext { get; }
        Lazy<Task<int>> _initializeCount;

        public VirtualizedClientsProvider(DbContext dataContext)
        {
            DataContext = dataContext;
            _initializeCount = new(InitializeCount);
        }
        private async Task<int> InitializeCount()
        {
            string sql = "SELECT COUNT(*) FROM Clients;";
            return await DataContext.RunAsync<int>(async conn =>
            {
                return await conn.ExecuteScalarAsync<int>(sql);
            });
        }
        public async Task<int> FetchCount()
        {
            return await _initializeCount.Value;
        }

        public async Task<IList<Client>> FetchRange(int start, int size)
        {
            return (await GetClients(start, size)).ToList();
        }

        public Task<IEnumerable<Client>> GetItems()
        {
            return GetClients(0,20);
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
