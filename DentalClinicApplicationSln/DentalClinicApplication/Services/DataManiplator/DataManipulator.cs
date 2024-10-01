using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataManiplator
{
    public class DataManipulator : IDataManipulator
    {
        public DbContext DbContext { get; }

        public DataManipulator(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public event Action? DataManipulated;

        
        public async Task Manipulate(string sql, object param)
        {
            await DbContext.RunAsync<int>(async conn =>
            {
                int result = conn.Execute(sql, param);
                if (result == 1)
                {
                    OnDataManipulated();
                    return result;
                }
                throw new InvalidDataContractException();
            });
        }

        public void OnDataManipulated()
        {
            DataManipulated?.Invoke();
        }
    }
}
