using DentalClinicApp.Commands;
using DentalClinicApp.Models;
using DentalClinicApp.Services;
using DentalClinicApp.Stores;
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
        public ICommand EditItemNavigationCommand { get; }
        public VirtualizedViewModel(
            VirtualizedCollectionComponentViewModel<T> componentViewModel,
            INavigationService editItemNavigationService
            )
        {
            ComponentViewModel = componentViewModel;
            EditItemNavigationCommand = new NavigationCommand(editItemNavigationService);
        }
    }
    public class AllAppointmentsViewModel : VirtualizedViewModel<Appointment>
    {
        public AllAppointmentsViewModel(VirtualizedCollectionComponentViewModel<Appointment> componentViewModel, INavigationService addItemNavigationService ) : base(componentViewModel, addItemNavigationService)
        {
        }

        
    }

    public class AllClientsViewModel : VirtualizedViewModel<Client>
    {
        public AllClientsViewModel(VirtualizedCollectionComponentViewModel<Client> componentViewModel, INavigationService addItemNavigationService,INavigationService viewItemNavigationService) : base(componentViewModel, addItemNavigationService)
        {
            ViewItemNavigationCommand = new NavigationCommand(viewItemNavigationService);
        }

        public ICommand ViewItemNavigationCommand { get; }
    }
}
