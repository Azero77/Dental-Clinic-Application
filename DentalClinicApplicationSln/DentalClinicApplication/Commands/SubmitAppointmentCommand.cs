using DentalClinicApp.Models;
using DentalClinicApplication.Exceptions;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Services.DataProvider;
using DentalClinicApplication.Stores;
using DentalClinicApplication.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    public class SubmitAppointmentCommand
        : SubmitItemCommand<Appointment>
    {
        public SubmitAppointmentCommand(ErrorViewModelBase viewModel, INavigationService navigationService, IDataService<Appointment> itemCreator, MessageService messageService) : base(viewModel, navigationService, itemCreator, messageService)
        {
        }

        public override async Task SubmitExecute(Appointment? item)
        {
            try
            {
                await ItemCreator.CreateAsync(item!);
                MessageService.SetMessage("Appointment Added Successfully", MessageType.Status);
                NavigationService.Navigate(item);
            }
            catch (AppointmentAlreadyTakenException exception)
            {
                MessageService.SetMessage(exception.Message, MessageType.Error);
            }
        }
    }
}
