using System;

namespace DentalClinicApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime Date{ get; set; }
        public Client? Client { get; set; }
    }
}