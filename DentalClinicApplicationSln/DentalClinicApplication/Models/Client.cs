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
        public int Age { get; set; }
        public IList<Appointment>? Appointments { get; set; } = Enumerable.Empty<Appointment>().ToList();

        public static Client ToClient(ClientDTO clientDTO)
        {
            return new Client()
            {
                Id = clientDTO.Id,
                FirstName = clientDTO.FirstName,
                LastName = clientDTO.LastName,
                Email = clientDTO.Email,
                Gender = GetGender(clientDTO.Gender),
                Age = clientDTO.Age
            };
        }

        private static Gender GetGender(string gender)
        {
            if (gender == "Male")
                return Gender.Male;
            else if (gender == "Female")
                return Gender.Female;
            return Gender.Undefinded;
        }
    }

    public enum Gender
    {
        Male,
        Female,
        Undefinded
    }
}
