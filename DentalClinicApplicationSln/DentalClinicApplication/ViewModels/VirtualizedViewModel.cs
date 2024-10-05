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
        public ICommand AddItem { get; }
        //Edit Item required Seprate NavigationService so we make a dependency on NavigationStore
        //And A factory
        public ICommand EditItem { get; }
        public VirtualizedViewModel(
            VirtualizedCollectionComponentViewModel<T> componentViewModel,
            INavigationService addItemNavigationService,
            INavigationService editItemNavigationService)
        {
            ComponentViewModel = componentViewModel;
            AddItem = new NavigationCommand(addItemNavigationService);
/*            EditItem = new NavigationCommand(new NavigationService<MakeEditItemViewModel<T>>(
                navigationStore,
                viewModelFactory
                ));*/
            EditItem = new NavigationCommand(editItemNavigationService);
        }
    }
    public class AllAppointmentsViewModel : VirtualizedViewModel<Appointment>
    {
        public AllAppointmentsViewModel(VirtualizedCollectionComponentViewModel<Appointment> componentViewModel, INavigationService addItemNavigationService, INavigationService editItemNavigationService) : base(componentViewModel, addItemNavigationService, editItemNavigationService)
        {
            AddAppointmentNavigationCommand = AddItem;
            EditAppointmentNavigationCommand = EditItem;
        }

        public ICommand AddAppointmentNavigationCommand { get; }
        public ICommand EditAppointmentNavigationCommand { get; }
        
    }

    public class AllClientsViewModel : VirtualizedViewModel<Client>
    {
        public AllClientsViewModel(VirtualizedCollectionComponentViewModel<Client> componentViewModel, INavigationService addItemNavigationService, INavigationService editItemNavigationService) : base(componentViewModel, addItemNavigationService, editItemNavigationService)
        {
            AddClientNavigationCommand = AddItem;
            EditClientNavigationCommand = EditItem;
        }

        public ICommand AddClientNavigationCommand { get; }
        public ICommand EditClientNavigationCommand { get; }

        
    }
}
