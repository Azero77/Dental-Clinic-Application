using DentalClinicApplication.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.ViewModels
{
    public class MakeEditClientViewModel
        : ErrorViewModelBase
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
		private string? _gender;
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

		private DateTime _dateOfBirth;
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
		#endregion
	}
}
