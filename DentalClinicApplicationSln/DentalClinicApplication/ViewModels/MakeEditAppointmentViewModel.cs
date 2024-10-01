using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.ComponentsViewModels;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Validations;
using DentalClinicApplication.VirtualizationCollections;
using DentalClinicApplication.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    public class MakeEditAppointmentViewModel : ErrorViewModelBase
    {
        #region props

        //Date of the day (yyyy-MM-dd)
        private DateTime _dayDate = DateTime.Now;
        [Required("Please Select The Day")]
        public DateTime DayDate
        {
            get
            {
                return _dayDate;
            }
            set
            {
                _dayDate = value;
                OnPropertyChanged(nameof(DayDate));
            }
        }

        //Datetime without date (just hours and minutes)
        private TimeSpan _startDate;
        [Required("Please Select The Start Time")]
        public TimeSpan StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private TimeSpan _endDate;
        [Required("Please Select The End Time")]
        public TimeSpan EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private Client? _client;
        [Required("Paitent Should be Selected")]
        public Client Client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
                OnPropertyChanged(nameof(Client));
            }
        }
        public Appointment Appointment => GetAppointment();

        private Appointment GetAppointment()
        {
            return new Appointment()
            {
                StartDate = DayDate.Date.Add(StartDate),
                Duration = DayDate.Date.Add(EndDate) - DayDate.Date.Add(StartDate),
                Client = Client,
                ClientId = Client?.Id ?? 0,
                Description = Description
            };
        }

        public override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            base.OnPropertyChanged(nameof(Appointment));
        }
        #endregion
        #region Commands
        public ICommand ClientSelectionCommand { get; }
        public ICommand SubmitAppointmentCommand { get; }
        #endregion

        public MakeEditAppointmentViewModel(
            VirtualizedCollectionComponentViewModel<Client> collectionViewModel,
            INavigationService<HomePageViewModel> navigationService,
            IDataService<Appointment> dataService,
            MessageService messageService)
        {
            ClientSelectionViewModel clientSelectionViewModel = new ClientSelectionViewModel(collectionViewModel, OnItemSelected);
            ClientSelectionCommand = new ShowWindowCommand<ClientSelectionWindow>(
                (obj) => new ClientSelectionWindow(clientSelectionViewModel));
            SubmitAppointmentCommand = new SubmitAppointmentCommand(navigationService, dataService, messageService);
        }

        private void OnItemSelected(Client? client)
        {
            if (client is not null)
            {
                Client = client;
            }
        }
    }
}
