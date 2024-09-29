using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
        public IEnumerable<string> HomePageProperties =>
            typeof(Appointment).GetProperties()
            .Concat(
                typeof(Client).GetProperties()
                )
            .Where(p => p.PropertyType == typeof(string))
            .Select(p => p.Name)
            ;
        public string? FirstProperty => HomePageProperties.FirstOrDefault();

        public HomePageViewModel(IProvider<Appointment> collectionProvider) : base(collectionProvider)
        {
            SearchCommand = new SearchCommand<Appointment>(
                new Services.ProviderChangerService<Appointment,Client>(this,CollectionProvider,Services.ChangeMode.Search),
                this
                );
            CollectionChagned += OnCollectionChanged;
        }


        public override async Task LoadViewModel()
        {
            Collection = await CollectionProvider.GetItems();
            OnCollectionChanged();
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
            IProvider<Appointment> collectionProvider)
        {
            HomePageViewModel homePageViewModel = new(collectionProvider);
            return (HomePageViewModel) LoadCollectionViewModel(homePageViewModel);
        }
    }
}
