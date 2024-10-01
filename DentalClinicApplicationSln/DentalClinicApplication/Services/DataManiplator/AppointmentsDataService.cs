using AutoMapper;
using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using DentalClinicApplication.AutoMapperProfiles;
using DentalClinicApplication.DTOs;
using DentalClinicApplication.Exceptions;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataManiplator
{

    /// <summary>
    /// All the Editing,Inserting,Deletion logic is written is one service
    /// </summary>
    //reading is complicated so i separate in other service
    public class AppointmentsDataService : IDataService<Appointment>
    {
        public IMapper Mapper { get; }
        public DbContext DataContext { get; }
        public IDataManipulator Manipulator { get; }

        public AppointmentsDataService(
            IDataManipulator dataManipulator, 
            IMapper mapper,
            DbContext dataContext)
        {
            Manipulator = dataManipulator;
            Mapper = mapper;
            DataContext = dataContext;
        }

        public async Task CreateAsync(Appointment appointment)
        {
            await ValidateAppointment(appointment);
            string sql = "INSERT INTO Appointments (StartDate, EndDate, ClientId, Description)  " +
                $"VALUES (@startDate,@endDate,@clientId,@description);";
            AppointmentDTO appointmentDTO = Mapper.Map<AppointmentDTO>(appointment);
            object param = new
            {
                startDate = appointmentDTO.StartDate,
                endDate = appointmentDTO.EndDate,
                clientId = appointmentDTO.ClientId,
                description = appointmentDTO.Description
            };
            await Manipulator.Manipulate(sql, param);
        }
        public async Task EditAsync(Appointment appointment)
        {
            await ValidateAppointment(appointment);
            string sql = "UPDATE Appointments SET(StartDate,EndDate,ClientId,Description) " +
                "VALUES(@startdate,@endDate,@clientId,@description) " +
                "WHERE Id = @id;";
            AppointmentDTO appointmentDTO = Mapper.Map<AppointmentDTO>(appointment);
            object param = new
            {
                startDate = appointmentDTO.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                endDate = appointmentDTO.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                clientId = appointmentDTO.ClientId,
                description = appointmentDTO.Description,
                id = appointmentDTO.Id
            };
            await Manipulator.Manipulate(sql, param);
        }

        private async Task ValidateAppointment(Appointment appointment)
        {
            Appointment? conflictedAppointment = await SearchConflictedAppointment(appointment);
            if (conflictedAppointment is not null)
            {
                string message = $"You have an Appointment" +
                    $" For {conflictedAppointment.Client?.FirstName + " " + conflictedAppointment.Client?.LastName} From {conflictedAppointment.StartDate:HH:mm}" +
                    $" To {(conflictedAppointment.StartDate + conflictedAppointment.Duration):HH:mm}";
                throw new AppointmentAlreadyTakenException(conflictedAppointment, message);
            }
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            int id = appointment.Id;
            string sql = "DELETE FROM Appointments WHERE Id = @id";
            object param = new
            {
                id
            };
            await Manipulator.Manipulate(sql, param);
        }
        public async Task<Appointment?> SearchConflictedAppointment(Appointment appointment)
        {
            AppointmentDTO appointmentDTO = Mapper.Map<Appointment,AppointmentDTO>(appointment);
            string sql = "SELECT a.Id,StartDate,EndDate,ClientId,FirstName,LastName " +
                "FROM Appointments a JOIN Clients c " +
                "ON a.ClientId = c.Id " +
                "WHERE @startDate < EndDate AND @endDate > StartDate";
            object param = new { startDate = appointmentDTO.StartDate,endDate = appointmentDTO.EndDate};
            IEnumerable<Appointment> appointments = await DataContext.RunAsync<IEnumerable<Appointment>>(conn =>
            {
                return conn.QueryAsync<AppointmentDTO, ClientDTO, Appointment>(sql,
                    (aDTO,cDTO) => Mapper.MapAppointments(aDTO,cDTO)
                    , 
                    param,
                    splitOn: "ClientId");
            });
            Appointment? conflictedAppointment = appointments.FirstOrDefault();
            return conflictedAppointment;
        }
    }
}
