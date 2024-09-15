using DentalClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.DTOs
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime Date { get; set; }

        public static AppointmentDTO ToAppointmentDTO(Appointment appointment)
        {
            return new AppointmentDTO()
            {
                Id = appointment.Id,
                ClientId = appointment.ClientId,
                Date = appointment.Date
            };
        }
    }
}
