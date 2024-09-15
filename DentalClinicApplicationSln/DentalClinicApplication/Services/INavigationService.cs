using DentalClinicApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services
{
    public interface INavigationService
    {
        void Navigate(object? param);
    }

    public interface INavigationService<TViewModel> : INavigationService
    where TViewModel : ViewModelBase
    {
    }
}
