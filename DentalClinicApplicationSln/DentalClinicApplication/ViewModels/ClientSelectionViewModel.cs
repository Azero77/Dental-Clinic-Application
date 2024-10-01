using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.ComponentsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DentalClinicApplication.ViewModels
{
    public class ClientSelectionViewModel : ViewModelBase
    {
        public CollectionViewModelBase<Client> CollectionViewModel { get; }
        public ICommand SelectClientCommand { get; }

        public ClientSelectionViewModel(CollectionViewModelBase<Client> collectionViewModel,
            Action<Client?> onItemSelected)
        {
            CollectionViewModel = collectionViewModel;
            ItemSelected += onItemSelected;
            SelectClientCommand = new RelayCommand<Client>(OnItemSelected);
        }
        public event Action<Client?>? ItemSelected;
        private void OnItemSelected(Client? client)
        {
            ItemSelected?.Invoke(client);
        }
    }
}
