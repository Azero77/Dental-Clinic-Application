using DentalClinicApp.Commands;
using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataProvider;
using ResourceDictionariesContainer.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    //Home Page For Appointment for today
    public class HomePageViewModel : CollectionViewModelBase<Appointment>
    {
        public ObservableCollection<Appointment> Appointments { get; } = new();
        public ICommand SearchCommand { get; }
        public static IEnumerable<string> HomePageProperties =>
            new string[] 
            {
                "FirstName",
                "LastName",
                "StartDate",
                "Description",
                "Email",
            }
            ;
        public string? FirstProperty => HomePageProperties.FirstOrDefault();

        public ICommand AddAppointmentNavigationCommand { get; }
        public ICommand ResetCommand { get; }
        public HomePageViewModel(
            IProvider<Appointment> collectionProvider,
            INavigationService<MakeEditAppointmentViewModel> makeEditAppointmentNavigationService,
            MessageService messageService) : base(collectionProvider)
        {
            SearchCommand = new SearchCommand<Appointment>(
                this,
                new Services.ProviderChangerService<Appointment,Client>(this.CollectionProvider,OnProviderChanged),
                    messageService
                );
            CollectionChagned += OnCollectionChanged;
            AddAppointmentNavigationCommand = new NavigationCommand(makeEditAppointmentNavigationService );
            ResetCommand = new RelayCommand<object>(ResetDelegate);
        }

        private void ResetDelegate(object? obj)
        {
            CollectionProvider.ResetProvider();
            LoadViewModel().ConfigureAwait(false);
        }

        public override async Task LoadViewModel()
        {
            IsLoading = true;
            Collection = await CollectionProvider.GetItems();
            IsLoading = false;
            OnCollectionChanged();
        }
        public override Task OnProviderChanged()
        {
            return LoadViewModel();
        }
        private void OnCollectionChanged()
        {
            Appointments.Clear();
            foreach (Appointment appointment in Collection)
            {
                Appointments.Add(appointment);
            }
        }

        public static HomePageViewModel LoadHomePageViewModel(
            IProvider<Appointment> collectionProvider,
            INavigationService<MakeEditAppointmentViewModel> navigationService,
            MessageService messageService)
        {
            HomePageViewModel homePageViewModel = new(collectionProvider,navigationService, messageService);
            return (HomePageViewModel) LoadCollectionViewModel(homePageViewModel);
        }

        public override Dictionary<string, object> SearchMapper(string property, object value)
        {
            return new Dictionary<string, object>()
            {
                {property,value }
            };
        }
    }
}
