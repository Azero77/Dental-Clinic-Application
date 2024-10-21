using DentalClinicApp.ViewModels;
using DentalClinicApplication.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services
{
    public class CloseModalNavigationService : INavigationService
    {
        public ModalNavigationStore ModalNavigationStore { get; }

        public CloseModalNavigationService(ModalNavigationStore modalNavigationStore)
        {
            ModalNavigationStore = modalNavigationStore;
        }

        public void Navigate(object? param)
        {
            ModalNavigationStore.Close();
        }
    }
}
