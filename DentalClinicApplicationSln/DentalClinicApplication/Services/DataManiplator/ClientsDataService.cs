using AutoMapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataManiplator
{
    public class ClientsDataService : IDataService<Client>
    {
        public IMapper Mapper { get; }
        public IDataManipulator Manipulator { get; }

        public ClientsDataService(IDataManipulator dataManipulator, IMapper mapper)
        {
            Manipulator = dataManipulator;
            Mapper = mapper;
        }

        public async Task CreateAsync(Client client)
        {
            ClientDTO clientDTO = Mapper.Map<ClientDTO>(client);
            string sql = "INSERT INTO Clients " +
                "(FirstName,LastName,Email,Gender,DateOfBirth) " +
                "VALUES (@firstName,@lastName,@email,@gender,@dateOfBirth)";
            object param = new
            {
                firstName = clientDTO.FirstName,
                lastName = clientDTO.LastName,
                email = clientDTO.Email,
                gender = clientDTO.Gender,
                dateOfBirth = clientDTO.DateOfBirth
            };
            await Manipulator.Manipulate(sql, param);
        }

        public async Task DeleteAsync(Client client)
        {
            string sql = "DELETE FROM Clients WHERE Id = @id";
            object param = new { id = client.Id };
            await Manipulator.Manipulate(sql, param);
        }

        public async Task EditAsync(Client client)
        {
            ClientDTO clientDTO = Mapper.Map<ClientDTO>(client);
            string sql = "UPDATE Clients SET FirstName = @firstName, LastName = @lastName, Email = @email, Gender=@gender  WHERE Id = @id";
            object param = new
            {
                firstName = clientDTO.FirstName,
                lastName = clientDTO.LastName,
                email = clientDTO.Email,
                gender = clientDTO.Gender,
                id = clientDTO.Id
            };
            await Manipulator.Manipulate(sql, param);
        }
    }
}
