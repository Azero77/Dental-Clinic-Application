using DentalClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.DTOs
{
    public class ClientDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = "Undefined";
        public int Age { get; set; }

        public static ClientDTO CreateClientDTO(Client client)
        {
            
            return new ClientDTO()
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                Gender =  GetGender(client.Gender),
                Age = client.Age
            };
        }

        private static string GetGender(Gender gender)
        {
            if (gender == DentalClinicApp.Models.Gender.Male)
                return "Male";
            else if (gender == DentalClinicApp.Models.Gender.Female)
                return "Female";
            else
                return "Undefined";
        }
    }
}
