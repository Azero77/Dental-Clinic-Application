using AutoMapper;
using DentalClinicApp.Models;
using DentalClinicApplication.Commands;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Validations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    public class MakeEditClientViewModel
        : MakeEditItemViewModel<Client>
    {
        #region props
        private int _id;
		
		public int Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
				OnPropertyChanged(nameof(Id));
			}
		}
		private string? _firstName;
		public string? FirstName
		{
			get
			{
				return _firstName;
			}
			set
			{
				_firstName = value;
				OnPropertyChanged(nameof(FirstName));
			}
		}
		private string? _lastName;
        [Required("Last Name is Required")]
        public string? LastName
		{
			get
			{
				return _lastName;
			}
			set
			{
				_lastName = value;
				OnPropertyChanged(nameof(LastName));
			}
		}
		private string? _email;

        public string? Email
		{
			get
			{
				return _email;
			}
			set
			{
				_email = value;
				OnPropertyChanged(nameof(Email));
			}
		}
		private string? _gender = null;
		public string? Gender
		{
			get
			{
				return _gender;
			}
			set
			{
				_gender = value;
				OnPropertyChanged(nameof(Gender));
			}
		}

		private DateTime _dateOfBirth = DateTime.Now;
		[Required("DateOfBirth Must be Specified")]
		public DateTime DateOfBirth
		{
			get
			{
				return _dateOfBirth;
			}
			set
			{
				_dateOfBirth = value;
				OnPropertyChanged(nameof(DateOfBirth));
				OnPropertyChanged(nameof(Age));
			}
		}
		public int Age => (int) ((DateTime.Now - DateOfBirth).TotalDays / 365.25);

		public Client? Client => Mapper?.Map<Client>(this);
		

        public MakeEditClientViewModel(IMapper mapper,
			INavigationService navigationService,
			INavigationService modalNavigationService,
			IDataService<Client> dataCreator,
			MessageService messageService,
			Client? client = null,
			SubmitStatus submitStatus = SubmitStatus.Create)
			: base(mapper,dataCreator,navigationService,modalNavigationService,messageService,submitStatus)
        {
			AssignClient(client);
        }

        private void AssignClient(Client? client)
        {
			if (client is not null)
			{
				Id = client.Id;
				FirstName = client.FirstName;
				LastName = client.LastName;
				Email = client.Email;
				Gender = client.Gender.ToString();
				DateOfBirth = client.DateOfBirth;
			}
        }

        public override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
			base.OnPropertyChanged(nameof(Client));
        }


        #endregion
    }
}
