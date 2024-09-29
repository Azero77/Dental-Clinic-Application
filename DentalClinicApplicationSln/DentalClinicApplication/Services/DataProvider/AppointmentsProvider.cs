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
            //if it was null then the values do not change
            _whereClause = whereClause;
            _orderClause = orderClause;
        }

        public override IProvider<Appointment> ChangeProvider(string? whereClause, string? orderClause)
        {
            //check if whereClause is not for date
            if (_whereClause is not null
                &&
                whereClause is not  null
                &&
            !(whereClause.StartsWith("WHERE DATE(")))
            {
                
                string newWhereClause = new string(whereClause?.SkipWhile(c => "WHERE".Contains(c)).ToArray());
                whereClause = $"{_whereClause} AND {newWhereClause}";
            }
            return new AppointmentsProvider(this.DataContext,this._mapper,whereClause ?? this._whereClause,
                orderClause ?? this._orderClause);
        }
        public override async Task<IEnumerable<Appointment>> GetItems()
        {
            string sql =
                "SELECT StartDate,EndDate,Description,ClientId,c.Id,c.FirstName,c.LastName " +
                "FROM Appointments a JOIN Clients c " +
                "ON ClientId = c.Id " +
                $"{_whereClause} {_orderClause};";
                ;
            IEnumerable<Appointment> result = await DataContext.RunAsync<IEnumerable<Appointment>>(conn =>
            {
                    return conn.QueryAsync<AppointmentDTO, ClientDTO, Appointment>(sql,
                    (appointmentDTO,clientDTO) => 
                    {
                        var r = _mapper.Map<ClientDTO,Client>(clientDTO);
                        return new Appointment()
                        {
                            Client = _mapper.Map<ClientDTO, Client>(clientDTO),
                            StartDate = appointmentDTO.StartDate,
                            Duration = appointmentDTO.EndDate - appointmentDTO.StartDate,
                            Description = appointmentDTO.Description
                        };
                    }
                    ,splitOn:"ClientId");
            });
            return result;
        }
        public static AppointmentsProvider AppointmentsProviderForToday(DbContext dataContext,
                                    IMapper mapper
                                    )
        {
            string whereClause = "WHERE DATE(startDate) = DATE('now')";
            string orderClause = "ORDER BY StartDate";
            return new AppointmentsProvider(
                dataContext,
                mapper,
                whereClause,
                orderClause
                );
        }
    }
}
