using DentalClinicApplication.ComponentsViewModels;
using DentalClinicApplication.DTOs;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    public class SearchCommand<T> : AsyncCommandBase
    {
        private readonly VirtualizedCollectionComponentViewModel<T> _viewModel;

        public ProviderChangerService<T> ProviderChangerService { get; }
        public SearchCommand(ProviderChangerService<T> providerChangerService,
            VirtualizedCollectionComponentViewModel<T> viewModel)
        {
            ProviderChangerService = providerChangerService;
            _viewModel = viewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            //parameter is a wrapper for the propertyName and the value for seacrh

            //get the provider
            object[]? list = parameter as object[];
            if (list is null)
            {
                throw new InvalidCastException("Search is not supported");
            }
            object value = list[0];
            string propertyName = (string) list[1];

            _viewModel.IsLoading = true;
            await ProviderChangerService.Change(propertyName, value);
            _viewModel.IsLoading = false;
        }
    }
}
