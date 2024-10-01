using AutoMapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.AutoMapperProfiles
{
    public static class MapperExtensions
    {
        public static Appointment MapAppointments(this IMapper mapper, AppointmentDTO appointmentDTO, ClientDTO clientDTO)
        {
            return new Appointment()
            {
                Client = mapper.Map<ClientDTO, Client>(clientDTO),
                StartDate = appointmentDTO.StartDate,
                Duration = appointmentDTO.EndDate - appointmentDTO.StartDate,
                Description = appointmentDTO.Description
            };
        }
    }
}
