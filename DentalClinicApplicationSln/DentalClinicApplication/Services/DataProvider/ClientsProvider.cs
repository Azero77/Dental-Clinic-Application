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
using System.Windows.Controls;

namespace DentalClinicApplication.Services.DataProvider
{
    public class ClientsProvider : Provider<Client, ClientDTO>
    {
        public ClientsProvider(DbContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
        }

        public override void ChangeProvider(string? whereClause, string? orderClause)
        {
            throw new NotImplementedException();
        }

        public override async Task<Client?> GetItem(string propertyName, object value)
        {
            string sql = "SELECT c.Id,FirstName,LastName,Email,Gender,DateOfBirth " +
                "ClientId,a.Id,StartDate,EndDate,Description" +
                " From Clients JOIN Appointments ON " +
                "a.ClientId = c.Id" +
                $"WHERE {propertyName} = @value";
            Client client = new();
            client.Appointments = null;
            await DataContext.RunAsync(conn =>
            conn.QueryAsync<ClientDTO,AppointmentDTO,Client>(sql,
            (cDTO,aDTO) => 
            {
                if (client.Appointments is null)
                {
                    client = _mapper.Map<ClientDTO, Client>(cDTO);
                    client.Appointments = new List<Appointment>();
                    
                }
                Appointment appointment = _mapper.Map<AppointmentDTO, Appointment>(aDTO);
                client.Appointments.Add(appointment);
                return client;
            }
            ,new { value },
            splitOn: "ClientId"));
            return client;
        }


        public override Task<IEnumerable<Client>> GetItems()
        {
            throw new NotImplementedException();
        }
    }
}
