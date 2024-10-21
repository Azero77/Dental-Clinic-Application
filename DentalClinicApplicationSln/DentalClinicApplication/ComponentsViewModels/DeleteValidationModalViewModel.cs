using DentalClinicApp.Commands;
using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DentalClinicApplication.ComponentsViewModels
{
    public class DeleteValidationModalViewModel
        : ErrorViewModelBase
    {
        public ICommand DeleteCommand { get; }
        public ICommand CancelNavigationCommand { get; }
        public Appointment Appointment { get; }

        public DeleteValidationModalViewModel(IDataService<Appointment> appointmentDataService,
                                              INavigationService navigationService,
                                              INavigationService cancelNavigationService,
                                              Appointment appointment,
                                              MessageService messageService)
        {
            DeleteCommand = new SubmitItemCommand<Appointment>(this,navigationService,appointmentDataService,messageService,SubmitStatus.Delete);
            CancelNavigationCommand = new NavigationCommand(cancelNavigationService);
            Appointment = appointment;
        }
    }
}
