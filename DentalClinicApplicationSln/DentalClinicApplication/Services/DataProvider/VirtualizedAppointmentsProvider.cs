using AutoMapper;
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
    public class VirtualizedAppointmentsProvider : VirtualizedProvider<Appointment, AppointmentDTO>
    {
        public VirtualizedAppointmentsProvider(DbContext dbContext,
                                               IMapper mapper,
                                               MessageService messageService,
                                               string? whereClause = "",
                                               string? orderByClause = "") : base(dbContext,
                                                                                  mapper,
                                                                                  messageService,
                                                                                  whereClause,
                                                                                  orderByClause)
        {
        }

        protected override async Task<int> InitializeCount()
        {
            string sql = "SELECT COUNT(Id) FROM " +
                "Appointments a JOIN Clients c ON " +
                "a.ClientId = c.Id " +
                $"{whereClause}";
            return await RunSqlCount(sql);
        }

        public override async Task<IEnumerable<Appointment>> GetItems(int start, int size)
        {
            string sql = "SELECT FirstName,LastName,StartDate,EndDate  FROM " +
                "Appointments a JOIN Clients c ON " +
                "a.ClientId = c.Id " +
                $"{whereClause} " +
                $"LIMIT @size OFFSET @start";
            IEnumerable<Appointment> result = await DataContext.RunAsync<IEnumerable<Appointment>>(conn =>
            {
                return conn.QueryAsync<AppointmentDTO, ClientDTO, Appointment>(sql,
                (appointmentDTO, clientDTO) =>
                {
                    return new Appointment()
                    {
                        Client = Mapper.Map<ClientDTO, Client>(clientDTO),
                        StartDate = appointmentDTO.StartDate,
                        Duration = appointmentDTO.EndDate - appointmentDTO.StartDate,
                        Description = appointmentDTO.Description
                    };
                }
                , 
                param : new { start, size }
                ,
                splitOn: "ClientId");
            });
            return result;
        }
    }
}
