using AutoMapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IDataManipulator Manipulator { get; }
        IProvider<Appointment> _appointmentsProvider { get; }

        public AppointmentsDataService(IDataManipulator dataManipulator, IMapper mapper, IProvider<Appointment> appointmentsProvider)
        {
            Manipulator = dataManipulator;
            Mapper = mapper;
            _appointmentsProvider = appointmentsProvider;
        }

        public async Task CreateAsync(Appointment appointment)
        {
            string sql = "INSERT INTO Appointments VALUES(@startdate,@endDate,@clientId,@description);";
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
            string sql = "UPDATE Appointments SET(StartDate,EndDate,ClientId,Description) " +
                "VALUES(@startdate,@endDate,@clientId,@description) " +
                "WHERE Id = @id;";
            AppointmentDTO appointmentDTO = Mapper.Map<AppointmentDTO>(appointment);
            object param = new
            {
                startDate = appointmentDTO.StartDate,
                endDate = appointmentDTO.EndDate,
                clientId = appointmentDTO.ClientId,
                description = appointmentDTO.Description,
                id = appointmentDTO.Id
            };
            await Manipulator.Manipulate(sql, param);
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
    }
}
