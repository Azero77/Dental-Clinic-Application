using DentalClinicApp.Commands;
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
    public class SearchCommand<T> : CommandBase
    {

        public ProviderChangerService<T> ProviderChangerService { get; }
        public SearchCommand(
            ProviderChangerService<T> providerChangerService
            )
        {
            ProviderChangerService = providerChangerService;
        }

        public override void Execute(object? parameter)
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
            ProviderChangerService.ChangeProvider(propertyName, value);
        }
    }
}
