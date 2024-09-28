using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.ViewModels
{
    //Home Page For Appointment for today
    public class HomePageViewModel : ViewModelBase
    {
        public IProvider<Appointment> AppointmentsProvider { get; }

        public HomePageViewModel(IProvider<Appointment> appointmentsProvider)
        {
            AppointmentsProvider = appointmentsProvider;
        }
    }
}
