using DentalClinicApp.Models;
using DentalClinicApplication.Exceptions;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Services.DataProvider;
using DentalClinicApplication.Stores;
using DentalClinicApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    public class SubmitAppointmentCommand
        : AsyncCommandBase
    {
        public SubmitAppointmentCommand(
                                        ErrorViewModelBase viewModel,
                                        INavigationService navigationService,
                                        IDataService<Appointment> appointmentsCreator,
                                        MessageService messageService)
        {
            ViewModel = viewModel;
            NavigationService = navigationService;
            AppointmentsCreator = appointmentsCreator;
            MessageService = messageService;
        }

        public ErrorViewModelBase ViewModel { get; }
        public INavigationService NavigationService { get; }
        public IDataService<Appointment> AppointmentsCreator { get; }
        public MessageService MessageService { get; }
        public override async Task ExecuteAsync(object? parameter)
        {
            //checking valid data before execution of command
            if (!ViewModel.Validate())
            {
                return;
            }
            Appointment? appointment = parameter as Appointment;
            
            if (appointment is null)
            {
                throw new InvalidCastException("Change Parameter");
            }
            try
            {
                await AppointmentsCreator.CreateAsync(appointment);
                MessageService.SetMessage("Appointment Added Successfully", MessageType.Status);
                NavigationService.Navigate(parameter);
            }
            catch (AppointmentAlreadyTakenException exception)
            {
                MessageService.SetMessage(exception.Message, MessageType.Error);
            }
        }
    }
}
