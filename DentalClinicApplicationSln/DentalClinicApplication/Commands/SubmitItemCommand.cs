﻿using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Stores;
using DentalClinicApplication.ViewModels;
using System;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    public abstract class SubmitItemCommand<T> : AsyncCommandBase
    {
        public SubmitItemCommand(ErrorViewModelBase viewModel,
                                        INavigationService navigationService,
                                        IDataService<T> itemCreator,
                                        MessageService messageService)
        {
            ViewModel = viewModel;
            NavigationService = navigationService;
            ItemCreator = itemCreator;
            MessageService = messageService;
        }
        public ErrorViewModelBase ViewModel { get; }
        public INavigationService NavigationService { get; }
        public IDataService<T> ItemCreator { get; }
        public MessageService MessageService { get; }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (!ViewModel.Validate())
            {
                return;
            }
            T? Item;
            try
            {
                Item = (T?)parameter;

            }
            catch (Exception)
            {

                throw;
            }
            if (Item is null)
            {
                throw new InvalidCastException("Change Parameter");
            }
            await SubmitExecute(Item);
            MessageService.SetMessage("Item Added Successfully", MessageType.Status);
            NavigationService.Navigate(parameter);

        }
        public abstract Task SubmitExecute(T? item);
    }
}
