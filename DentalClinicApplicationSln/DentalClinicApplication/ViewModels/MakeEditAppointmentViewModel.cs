using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.ComponentsViewModels;
using DentalClinicApplication.Validations;
using DentalClinicApplication.VirtualizationCollections;
using DentalClinicApplication.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    public class MakeEditAppointmentViewModel : ViewModelBase
    {
        #region props
        private DateTime _startDate = DateTime.Now;
		[Required("StartDate must be specified")]
		public DateTime StartDate
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

		private TimeSpan _duration;
		[Required("Duration must be specified")]
		public TimeSpan Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				_duration = value;
				OnPropertyChanged(nameof(Duration));
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

		private Client _client;
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
        #endregion
        #region Commands
		public ICommand ClientSelectionCommand { get; }
        #endregion

        public MakeEditAppointmentViewModel(VirtualizedCollectionComponentViewModel<Client> collectionViewModel)
        {
			ClientSelectionCommand = new ShowWindowCommand<ClientSelectionWindow>(
				(obj) => new ClientSelectionWindow(collectionViewModel));
        }
    }
}
