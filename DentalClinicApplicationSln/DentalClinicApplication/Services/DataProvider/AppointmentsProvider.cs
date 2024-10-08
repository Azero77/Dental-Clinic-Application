using AutoMapper;
using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using DentalClinicApplication.AutoMapperProfiles;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    public class AppointmentsProvider : Provider<Appointment, AppointmentDTO>
    {
        
        public AppointmentsProvider(DbContext dataContext,
                                    IMapper mapper,
                                    string? whereClause = null,
                                    string? orderClause = null) : base(dataContext, mapper)
        {
            //if it was null then the values do not change
            _whereClause = whereClause;
            _orderClause = orderClause;
        }

        public override void ChangeProvider(string? whereClause, string? orderClause)
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
            this._whereClause = whereClause ?? this._whereClause;
            this._orderClause = orderClause ?? this._orderClause;
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
                    return conn.QueryAsync<AppointmentDTO, ClientDTO, Appointment>(
                    sql,
                    (aDTO, cDTO) => _mapper.MapAppointments(aDTO, cDTO),
                    splitOn:"ClientId");
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

        public Task<Appointment?> GetItem(int id)
        {
            return GetItem("Id", id);
        }
        public async Task<Appointment?> GetItem(string propName, object value)
        {
            string sql = "SELECT a.Id,StartDate,EndDate,ClientId,FirstName,LastName " +
                "FROM Appointments a JOIN Clients c" +
                "ON a.ClientId = c.Id " +
                $"WHERE a.{propName} = @val";
            object param = new { value };
            IEnumerable<Appointment> appointments = await DataContext.RunAsync<IEnumerable<Appointment>>(conn =>
            {
                return conn.QueryAsync<AppointmentDTO, ClientDTO, Appointment>(sql,
                    (aDTO,cDTO) => _mapper.MapAppointments(aDTO,cDTO)
                    , param,
                   splitOn: "ClientId");
            });
            Appointment? appointment = appointments.FirstOrDefault();
            return appointment;
        }
    }
}
