using AutoMapper;
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
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    public class MakeEditAppointmentViewModel :
        MakeEditItemViewModel<Appointment>
    {
        #region props
        private int _id = 0;
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
        private TimeSpan _startDate = TimeSpan.FromTicks(0);
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

        private TimeSpan _endDate = TimeSpan.FromTicks(0);
        [Required("Please Select The End Time")]
        [DelegateValidation("End Date Must be greater than Start Date",
            nameof(isEndDateValid))]
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

        private bool isEndDateValid(object? value)
        {
            if (value is TimeSpan newEndDate)
            {
                return newEndDate > StartDate;
            }
            return false;
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
        public bool IsClientSelected => Client is not null;

        [Required("Paitent Should be Selected")]
        public Client? Client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
                OnPropertyChanged(nameof(Client));
                OnPropertyChanged(nameof(IsClientSelected));
            }
        }
        public Appointment Appointment => GetAppointment();

        private Appointment GetAppointment()
        {
            return new Appointment()
            {
                Id = _id,
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
        
        #endregion

        public MakeEditAppointmentViewModel(
            VirtualizedCollectionComponentViewModel<Client> collectionViewModel,
            INavigationService navigationService,
            IDataService<Appointment> dataService,
            MessageService messageService,
            IMapper mapper,
            Appointment? appointment = null,
            SubmitStatus submitStatus = SubmitStatus.Create)
            : base(mapper,dataService,navigationService,messageService)
        {
            ClientSelectionViewModel clientSelectionViewModel = new ClientSelectionViewModel(collectionViewModel, OnItemSelected);
            ClientSelectionCommand = new ShowWindowCommand<ClientSelectionWindow>(
                (obj) => new ClientSelectionWindow(clientSelectionViewModel));
            SubmitCommand = new SubmitItemCommand<Appointment>(
                this,
                navigationService,
                dataService,
                messageService,
                submitStatus
                );
            AssignAppointment(appointment);
        }

        private void AssignAppointment(Appointment? appointment)
        {
            if (appointment is not null)
            {
                _id = appointment.Id;
                DayDate = appointment.StartDate.Date;
                StartDate = appointment.StartDate.TimeOfDay;
                EndDate = (StartDate + appointment.Duration);
                Description = appointment.Description ?? string.Empty;
                Client = appointment.Client!;
            }
            
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
