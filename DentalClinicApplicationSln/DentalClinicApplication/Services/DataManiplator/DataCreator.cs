using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataManiplator
{
    internal class DataCreator : DataManipulator
    {

        public DataCreator(DbContext dbContext) : base(dbContext)
        {
        }


        public async override Task Manipulate(Client client)
        {
            ClientDTO clientDTO = ClientDTO.CreateClientDTO(client);
            string sql = "INSERT INTO Clients " +
                "(FirstName,LastName,Email,Gender,Age) " +
                "VALUES (@firstName,@lastName,@email,@gender,@age)";
            object param = new
            {
                firstName = clientDTO.FirstName,
                lastName = clientDTO.LastName,
                email = clientDTO.Email,
                gender = clientDTO.Gender,
                age=clientDTO.Age,
            };
            await DbContext.RunAsync<int>(async conn =>
            {
                int result = await conn.ExecuteAsync(sql, param);
                if (result == 1)
                {
                    OnDataManipulated();
                    return result;
                }
                throw new InvalidDataContractException();
            });
        }
    }
}
