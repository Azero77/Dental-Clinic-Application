﻿using AutoMapper;
using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    class AppointmentsProvider : Provider<Appointment, AppointmentDTO>
    {
        string? _whereClause;
        string? _orderClause;
        public AppointmentsProvider(DbContext dataContext,
                                    IMapper mapper,
                                    string? whereClause = null,
                                    string? orderClause = null) : base(dataContext, mapper)
        {
            _whereClause = whereClause;
            _orderClause = orderClause;
        }

        public override IProvider<Appointment> ChangeProvider(string? whereClause, string? orderClause)
        {
            return new AppointmentsProvider(this.DataContext,this._mapper,whereClause,orderClause);
        }

        public override async Task<IEnumerable<Appointment>> GetItems()
        {
            string sql = $"SELECT FirstName,LastName,StartDate,EndDate,Description FROM Appointments a JOIN Clients c ON c.Id = a.ClientId {_whereClause} {_orderClause};";
            IEnumerable<Appointment> result = await DataContext.RunAsync<IEnumerable<Appointment>>(conn =>
            {
                    return conn.QueryAsync<AppointmentDTO, ClientDTO, Appointment>(sql,
                    (appointmentDTO,clientDTO) => 
                    {
                        return new Appointment()
                        {
                            Client = Mapper.Map<ClientDTO, Client>(clientDTO),
                            StartDate = appointmentDTO.StartDate,
                            Duration = appointmentDTO.EndDate - appointmentDTO.StartDate,
                            Description = appointmentDTO.Description
                        };
                    }
                    ,splitOn:"ClientId");
            });
            return result;
        }
    }
}