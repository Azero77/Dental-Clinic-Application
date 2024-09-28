using AutoMapper;
using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    class PatientsVirtualizedProvider
        : VirtualizedProvider<Client, ClientDTO>
    {
        public PatientsVirtualizedProvider(DbContext dbContext,
                                           IMapper mapper,
                                           MessageService messageService,
                                           string? whereClause = "",
                                           string? orderByClause = "") : base(dbContext, mapper, messageService, whereClause, orderByClause)
        {
        }

        protected override async Task<int> InitializeCount()
        {
            string sql = "SELECT COUNT(Id) FROM " +
                "Clients c JOIN Appointments a " +
                "ON c.Id = a.ClientId " +
                $"{whereClause};";
            //may cause trouble because splitter is not found
            return await RunSqlCount(sql);
        }
        public override async Task<IEnumerable<Client>> GetItems(int start, int size)
        {
            Dictionary<int, Client> clients = new();
            string sql = "SELECT c.Id,FirstName,LastName,Email,StartDate,EndDate " +
                "FROM Clients c JOIN Appointments a " +
                "ON c.Id = a.ClientId " +
                $"{whereClause} " +
                $"{orderByClause}" +
                $"LIMIT @size OFFSET @start;";
            await DataContext.RunAsync<IEnumerable<Client>>(conn =>
            {
                return conn.QueryAsync<ClientDTO, AppointmentDTO, Client>(sql,
                    (cDTO,aDTO) => 
                    {
                        Client? client;
                        if (!clients.TryGetValue(cDTO.Id, out client))
                        {
                            client = new Client
                            {
                                Id = cDTO.Id,
                                FirstName = cDTO.FirstName,
                                LastName = cDTO.LastName,
                                Email = cDTO.Email,
                                Appointments = new List<Appointment>() // Initialize the appointments collection
                            };

                            clients.Add(client.Id, client);
                        }
                        if (aDTO is not null)
                        {
                            client?.Appointments?.Add(_mapper.Map<Appointment>(aDTO));
                        }
                    },
                    param:new {  size, start},
                    splitOn:"ClientId");
            });
        }
    }
}
