using DentalClinicApp.ViewModels;
using DentalClinicApplication.ComponentsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.ViewModels
{
    public class LayoutViewModel : ViewModelBase
    {
        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, ViewModelBase contentViewModel, MessageViewModel messageViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;
            MessageViewModel = messageViewModel;
        }

        public NavigationBarViewModel NavigationBarViewModel { get; }
        public MessageViewModel MessageViewModel { get; }
        public ViewModelBase ContentViewModel { get; }
        public override void Dispose()
        {
            ContentViewModel.Dispose();
        }
    }
}
