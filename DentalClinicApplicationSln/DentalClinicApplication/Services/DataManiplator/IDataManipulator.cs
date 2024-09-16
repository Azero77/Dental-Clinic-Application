using Configurations.DataContext;
using DentalClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataManiplator
{
    public interface IDataManipulator
    {
        DbContext DbContext { get; }
        Task Manipulate(Client client);
        event Action? DataManipulated;
    }
}
