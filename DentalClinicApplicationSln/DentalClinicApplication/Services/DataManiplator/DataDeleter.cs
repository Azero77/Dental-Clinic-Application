using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DentalClinicApplication.Services.DataManiplator
{
    public class DataDeleter : DataManipulator
    {

        public DataDeleter(DbContext dbContext) : base(dbContext)
        {
        }

        public async override Task Manipulate(Client client)
        {
            string sql = "DELETE FROM Clients WHERE Id = @id";
            object param = new { id = client.Id };
            await DbContext.RunAsync<int>(async conn =>
            {
                int result;
                result = await conn.ExecuteAsync(sql,param);
                if (result == 1)
                {
                    OnDataManipulated();
                    return result; 
                }
                throw new InvalidDataException();
            });
            MessageBox.Show("Deleted Successfully",
                "Ok",
                MessageBoxButton.OK);
            
        }
    }
}
