using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.ComponentsViewModels;
using DentalClinicApplication.Services.DataProvider;
using DentalClinicApplication.VirtualizationCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    public class AllAppointmentsViewModel : ViewModelBase
    {
        public VirtualizedCollectionComponentViewModel<Appointment> CollectionViewModel { get; }

        public AllAppointmentsViewModel(VirtualizedCollectionComponentViewModel<Appointment> collectionViewModel)
        {
            CollectionViewModel = collectionViewModel;
        }
    }
}
