using DentalClinicApp.Commands;
using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.ComponentsViewModels;
using DentalClinicApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    public class VirtualizedViewModel<T> : ViewModelBase
    {
        public VirtualizedCollectionComponentViewModel<T> ComponentViewModel { get; }
        public ICommand AddItem { get; }
        public VirtualizedViewModel(VirtualizedCollectionComponentViewModel<T> componentViewModel,
            INavigationService navigationService)
        {
            ComponentViewModel = componentViewModel;
            AddItem = new NavigationCommand(navigationService);
        }
    }
    public class AllAppointmentsViewModel : VirtualizedViewModel<Appointment>
    {
        public ICommand AddAppointmentNavigationCommand { get; }
        public AllAppointmentsViewModel(VirtualizedCollectionComponentViewModel<Appointment> componentViewModel, INavigationService<MakeEditAppointmentViewModel> navigationService) : base(componentViewModel, navigationService)
        {
            AddAppointmentNavigationCommand = AddItem;
        }
    }

    public class AllClientsViewModel : VirtualizedViewModel<Client>
    {
        public ICommand AddClientsNavigaitonCommand { get; }
        public AllClientsViewModel(VirtualizedCollectionComponentViewModel<Client> componentViewModel, INavigationService<MakeEditClientViewModel> navigationService) : base(componentViewModel, navigationService)
        {
            AddClientsNavigaitonCommand = AddItem;
        }
    }
}
