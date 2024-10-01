using Configurations.DataContext;
using DentalClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataManiplator
{
    //one interface and one manipulator for the lifecycle of the app
    public interface IDataManipulator
    {
        DbContext DbContext { get; }
        Task Manipulate(string sql,object param);
        event Action? DataManipulated;

    }

}
