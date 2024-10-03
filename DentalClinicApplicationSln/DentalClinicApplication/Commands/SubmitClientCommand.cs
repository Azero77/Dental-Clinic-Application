using DentalClinicApp.Models;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    public class SubmitClientCommand
        : SubmitItemCommand<Client>
    {
        public SubmitClientCommand(ErrorViewModelBase viewModel, INavigationService navigationService, IDataService<Client> itemCreator, MessageService messageService) : base(viewModel, navigationService, itemCreator, messageService)
        {
        }

        public override Task SubmitExecute(Client? item)
        {
            return ItemCreator.CreateAsync(item!);
        }
    }
}
