using Configurations.DataContext;
using DentalClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataManiplator
{
    public abstract class DataManipulator : IDataManipulator
    {
        public DbContext DbContext { get; }

        protected DataManipulator(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public event Action? DataManipulated;

        public abstract Task Manipulate(Client client);
        
        public void OnDataManipulated()
        {
            DataManipulated?.Invoke();
        }
    }
}
