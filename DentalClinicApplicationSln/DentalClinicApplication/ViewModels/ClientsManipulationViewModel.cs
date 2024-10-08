﻿using DentalClinicApp.Commands;
using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    public class ClientsManipulationViewModel : ErrorViewModelBase
    {
		//update Client Constructor
        public ClientsManipulationViewModel(Client client,INavigationService navigationService, IDataManipulator editDataManipulator)
        {
			Id = client.Id;
			FirstName = client.FirstName;
			LastName = client.LastName;
			Email = client.Email;
			Gender = client.Gender;
			DateOfBirth = client.DateOfBirth;

			CancelCommand = new NavigationCommand(navigationService);
			//SubmitCommand = new ClientsEditCommand(editDataManipulator, navigationService);
            
        }

        //Insert Client Constructor
        public ClientsManipulationViewModel(INavigationService navigationService,IDataManipulator InsertDataManipulator)
		{
            CancelCommand = new NavigationCommand(navigationService);
			//SubmitCommand = new ClientsEditCommand(InsertDataManipulator, navigationService);

        }

        


        #region properites
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
			private string _firstName = string.Empty;
			[Required("First Name is required")]
			public string FirstName
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

			private string _lastName = string.Empty;
			[Required("Last Name is required")]
			public string LastName
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

			private string _email = string.Empty;
			public string Email
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
			private Gender _gender;
			public Gender Gender
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
			}
		}

		#endregion
		#region commands
		public ICommand CancelCommand { get; }
		public ICommand SubmitCommand { get; }
        #endregion

		//getting the client domain model of the view model to be passed to commands instead of the whole view model
		public Client Client => new Client()
        {
            Id = Id,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Gender = Gender,
			DateOfBirth = DateOfBirth
        };

		//for every propertychanged we need to update the client
		public override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);
			base.OnPropertyChanged(nameof(Client));
		}
        

    }



}
