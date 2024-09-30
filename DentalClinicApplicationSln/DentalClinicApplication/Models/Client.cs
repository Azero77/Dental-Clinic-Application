using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentalClinicApp.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Gender Gender { get; set; } = Gender.Undefinded;
        public DateTime DateOfBirth { get; set; }
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
        public IList<Appointment>? Appointments { get; set; } = Enumerable.Empty<Appointment>().ToList();
    }

    public enum Gender
    {
        Male,
        Female,
        Undefinded
    }
}
