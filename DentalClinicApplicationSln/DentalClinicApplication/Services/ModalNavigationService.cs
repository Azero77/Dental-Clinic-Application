using DentalClinicApp.ViewModels;
using DentalClinicApplication.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services
{
    class ModalNavigationService<TViewModel> : INavigationService<TViewModel>
        where TViewModel : ViewModelBase
    {
        public Func<object?, TViewModel> _modalFactory;
        public ModalNavigationStore _modalNavigationStore;

        public ModalNavigationService(
            ModalNavigationStore modalNavigationStore,
            Func<object?, TViewModel> modalFactory
            )
        {
            _modalFactory = modalFactory;
            _modalNavigationStore = modalNavigationStore;
        }

        public void Navigate(object? param)
        {
            _modalNavigationStore.CurrentModal = _modalFactory(param);
        }
    }
}
