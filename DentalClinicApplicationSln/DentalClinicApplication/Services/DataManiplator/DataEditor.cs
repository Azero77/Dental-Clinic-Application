using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataManiplator
{
    public class DataEditor : DataManipulator
    {
        public DataEditor(DbContext dbContext) : base(dbContext)
        {
        }


        public async override Task Manipulate(Client client)
        {
            ClientDTO clientDTO = ClientDTO.CreateClientDTO(client);
            string sql = "UPDATE Clients SET FirstName = @firstName, LastName = @lastName, Email = @email, Gender=@gender  WHERE Id = @id";
            object param = new 
            {
                firstName = clientDTO.FirstName,
                lastName = clientDTO.LastName,
                email = clientDTO.Email,
                gender = clientDTO.Gender,
                id = clientDTO.Id
            };
            await DbContext.RunAsync<int>(async conn =>
            {
                int result = await conn.ExecuteAsync(sql,param);
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
