﻿using AutoMapper;
using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using DentalClinicApplication.AutoMapperProfiles;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    public class AppointmentsVirtualizedProvider : VirtualizedProvider<Appointment, AppointmentDTO>
    {
        public AppointmentsVirtualizedProvider(DbContext dbContext,
                                               IMapper mapper,
                                               MessageService messageService,
                                               string? whereClause = null,
                                               string? orderByClause = null) : base(dbContext,
                                                                                  mapper,
                                                                                  messageService,
                                                                                  whereClause,
                                                                                  orderByClause)
        {
        }

        protected override async Task<int> InitializeCount()
        {
            string sql = "SELECT COUNT(a.Id) FROM " +
                "Appointments a JOIN Clients c ON " +
                "a.ClientId = c.Id " +
                $"{whereClause};";
            return await RunSqlCount(sql);
        }

        public override async Task<IEnumerable<Appointment>> GetItems(int start, int size)
        {
            string sql = "SELECT a.Id,StartDate,EndDate,Description,ClientId,c.Id,FirstName,LastName  FROM " +
                "Appointments a JOIN Clients c ON " +
                "a.ClientId = c.Id " +
                $"{whereClause} {orderByClause} " +
                $"LIMIT @size OFFSET @start";
            IEnumerable<Appointment> result = await DataContext.RunAsync<IEnumerable<Appointment>>(async conn =>
            {
                try
                {
                    return await conn.QueryAsync<AppointmentDTO, ClientDTO, Appointment>(sql,
                    (appointmentDTO, clientDTO) => _mapper.MapAppointments(appointmentDTO, clientDTO),
                    param: new { start, size }
                    ,
                    splitOn: "ClientId");
                }
                catch (SQLiteException)
                {
                    MessageService.SetMessage("Search Schema is not Allowed",
                        messageType: Stores.MessageType.Error);
                    return Enumerable.Empty<Appointment>();
                }
                
            });
            return result;
        }
    }
}
