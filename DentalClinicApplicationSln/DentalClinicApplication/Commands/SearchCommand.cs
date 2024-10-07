using DentalClinicApplication.ComponentsViewModels;
using DentalClinicApplication.DTOs;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataProvider;
using DentalClinicApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    public class SearchCommand<T> : AsyncCommandBase
    {
        private readonly CollectionViewModelBase<T> _viewModel;

        public ProviderChangerService<T> ProviderChangerService { get; }
        public SearchCommand(
            ProviderChangerService<T> providerChangerService,
            CollectionViewModelBase<T> viewModel)
        {
            ProviderChangerService = providerChangerService;
            _viewModel = viewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is null)
            {
                return;
            }
            //parameter is a wrapper for the propertyName and the value for seacrh
            object? value = null;
            string propertyName;
            //Search Provider (property, value)
            if (parameter is object[] list)
            {
                value = list[0];
                propertyName = (string)list[1];
            }
            //Order Provider
            else
            {
                propertyName = parameter as string ?? throw new InvalidCastException();
            }


            _viewModel.IsLoading = true;
            await ProviderChangerService.Change(propertyName, value);
            _viewModel.IsLoading = false;
        }
    }
}
