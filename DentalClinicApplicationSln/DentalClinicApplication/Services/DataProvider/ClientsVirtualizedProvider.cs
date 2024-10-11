using AutoMapper;
using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    class ClientsVirtualizedProvider
        : VirtualizedProvider<Client, ClientDTO>
    {
        public ClientsVirtualizedProvider(DbContext dbContext,
                                           IMapper mapper,
                                           MessageService messageService,
                                           string? whereClause = null,
                                           string? orderByClause = null) : base(dbContext, mapper, messageService, whereClause, orderByClause)
        {
        }

        protected override async Task<int> InitializeCount()
        {
            string sql = "SELECT COUNT(DISTINCT c.Id) FROM " +
                "Clients c JOIN Appointments a " +
                "ON c.Id = a.ClientId " +
                $"{whereClause};";
            //may cause trouble because splitter is not found
            return await RunSqlCount(sql);
        }
        public override async Task<IEnumerable<Client>> GetItems(int start, int size)
        {
            string clientsSql = "SELECT Id,FirstName,LastName,Gender,DateOfBirth,Email " +
                $"FROM Clients {whereClause} {orderByClause} LIMIT @size OFFSET @start;";
            //get the clients , get appointments of the clients, map them together
            IEnumerable<ClientDTO> clients = await DataContext.RunAsync(conn =>
            conn.QueryAsync<ClientDTO>(clientsSql, param: new { size, start })
            ) ;
            IEnumerable<int> clientIds = clients.Select(c => c.Id);
            string appointmentsSql = "SELECT ClientId,StartDate,EndDate " +
                "FROM Appointments " +
                "WHERE ClientId IN @clientIds";
            IEnumerable<AppointmentDTO> appointments = await DataContext.RunAsync(conn =>
            conn.QueryAsync<AppointmentDTO>(appointmentsSql, param: new { clientIds }));
            Dictionary<int, Client> ClientsHash = clients.
                Select(cDTO => _mapper.Map<ClientDTO, Client>(cDTO))
                .ToDictionary(c => c.Id);

            foreach (AppointmentDTO appointmentDTO in appointments)
            {
                if (ClientsHash.TryGetValue(appointmentDTO.ClientId, out Client? client))
                {
                    if (client is null)
                        continue;
                    client!.Appointments!.Add(_mapper.Map<AppointmentDTO, Appointment>(appointmentDTO));
                }
            }
            return ClientsHash.Values;
        }
    }
}
