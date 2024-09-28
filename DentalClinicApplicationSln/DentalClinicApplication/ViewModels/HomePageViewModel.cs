using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.ViewModels
{
    //Home Page For Appointment for today
    public class HomePageViewModel : CollectionViewModelBase<Appointment>
    {
        public ObservableCollection<Appointment> Appointments { get; } = new();

        public HomePageViewModel(IProvider<Appointment> collectionProvider) : base(collectionProvider)
        {
        }


        public override async Task LoadViewModel()
        {
            Collection = await CollectionProvider.GetItems();
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
