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
                                           string? whereClause = "",
                                           string? orderByClause = "") : base(dbContext, mapper, messageService, whereClause, orderByClause)
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
            string sql = "SELECT c.Id,FirstName,LastName,Email,ClientId,StartDate,EndDate " +
                "FROM Clients c JOIN Appointments a " +
                "ON c.Id = a.ClientId " +
                $"{whereClause} " +
                $"{orderByClause} " +
                $"LIMIT @size OFFSET @start;";
            return await DataContext.RunAsync<IEnumerable<Client>>(async conn =>
            {

                Dictionary<int, Client> clients = new();
                await conn.QueryAsync<ClientDTO, AppointmentDTO, Client>(sql,
                    (cDTO,aDTO) => 
                    {
                        
                        Client? client;
                        //if client found in the dictionary the appointments only will be added
                        //if not a new key to dictionary is added
                        if (!clients.TryGetValue(cDTO.Id, out client))
                        {
                            client = _mapper.Map<Client>(cDTO);
                            client.Appointments = new List<Appointment>();
                            clients.Add(client.Id, client);
                        }
                        if (aDTO is not null)
                        {
                            Appointment appointment = _mapper.Map<Appointment>(aDTO);
                            client!.Appointments!.Add(appointment);
                        }
                        return client!;
                    },
                    param:new {  size, start},
                    splitOn:"ClientId");
                return clients.Values;
            });
        }

        public override IProvider<Client> ChangeProvider(string? whereClause, string? orderByClause)
        {
            return new ClientsVirtualizedProvider(
               this.DataContext,
               this._mapper,
               this.MessageService,
               whereClause ?? this.whereClause,
               orderByClause ?? this.orderByClause
               );
        }
    }
}
