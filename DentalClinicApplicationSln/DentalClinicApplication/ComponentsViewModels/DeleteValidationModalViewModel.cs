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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DentalClinicApplication.ComponentsViewModels
{
    public class DeleteValidationModalViewModel<T>
        : ErrorViewModelBase
    {
        public ICommand DeleteCommand { get; }
        public ICommand CancelNavigationCommand { get; }
        public T Item { get; }

        public DeleteValidationModalViewModel(IDataService<T> dataService,
                                              INavigationService navigationService,
                                              INavigationService cancelNavigationService,
                                              T item,
                                              MessageService messageService)
        {
            DeleteCommand = new SubmitItemCommand<T>(this,navigationService,dataService,messageService,SubmitStatus.Delete);
            CancelNavigationCommand = new NavigationCommand(cancelNavigationService);
            Item = item;
        }
    }

    public class DeleteValidationAppointmentModalViewModel
        : DeleteValidationModalViewModel<Appointment>
    {
        public DeleteValidationAppointmentModalViewModel(IDataService<Appointment> dataService, INavigationService navigationService, INavigationService cancelNavigationService, Appointment item, MessageService messageService) : base(dataService, navigationService, cancelNavigationService, item, messageService)
        {
        }

        public Appointment Appointment => this.Item;
    }

    public class DeleteValidationClientModalViewModel
        : DeleteValidationModalViewModel<Client>
    {
        public DeleteValidationClientModalViewModel(IDataService<Client> dataService, INavigationService navigationService, INavigationService cancelNavigationService, Client item, MessageService messageService) : base(dataService, navigationService, cancelNavigationService, item, messageService)
        {
        }
        public Client Client => this.Item;
    }
}
