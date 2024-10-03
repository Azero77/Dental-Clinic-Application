using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.ComponentsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.ViewModels
{
    public class VirtualizedViewModel<T> : ViewModelBase
    {
        public VirtualizedCollectionComponentViewModel<T> ComponentViewModel { get; }

        public VirtualizedViewModel(VirtualizedCollectionComponentViewModel<T> componentViewModel)
        {
            ComponentViewModel = componentViewModel;
        }
    }
    public class AllAppointmentsViewModel : VirtualizedViewModel<Appointment>
    {

        public AllAppointmentsViewModel(VirtualizedCollectionComponentViewModel<Appointment> componentViewModel)
            : base(componentViewModel)
        {
        }
    }

    public class AllClientsViewModel : VirtualizedViewModel<Client>
    {
        public AllClientsViewModel(VirtualizedCollectionComponentViewModel<Client> componentViewModel) : base(componentViewModel)
        {
        }
    }
}
