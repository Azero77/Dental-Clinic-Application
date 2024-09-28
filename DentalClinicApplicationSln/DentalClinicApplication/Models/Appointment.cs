using System;

namespace DentalClinicApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime StartDate{ get; set; }
        public TimeSpan Duration { get; set; }
        public string? Description { get; set; }
        public Client? Client { get; set; }
    }
}