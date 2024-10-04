using DentalClinicApplication.Exceptions;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Stores;
using DentalClinicApplication.ViewModels;
using System;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DentalClinicApplication.Commands
{
    public class SubmitItemCommand<T> : AsyncCommandBase
    {
        public SubmitItemCommand(ErrorViewModelBase viewModel,
                                        INavigationService navigationService,
                                        IDataService<T> dataService,
                                        MessageService messageService,
                                        SubmitStatus submitStatus)
        {
            ViewModel = viewModel;
            NavigationService = navigationService;
            DataService = dataService;
            MessageService = messageService;
            _submitStatus = submitStatus;
        }
        public ErrorViewModelBase ViewModel { get; }
        public INavigationService NavigationService { get; }
        public IDataService<T> DataService { get; }
        public MessageService MessageService { get; }
        private SubmitStatus _submitStatus;
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
            try
            {
                switch (_submitStatus)
                {
                    case SubmitStatus.Edit:
                        await DataService.EditAsync(Item);
                        break;
                    case SubmitStatus.Create:
                        await DataService.CreateAsync(Item);
                        break;
                    case SubmitStatus.Delete:
                        await DataService.DeleteAsync(Item);
                        break;
                    default:
                        break;
                }
            }
            catch (AppointmentAlreadyTakenException exception)
            {
                MessageService.SetMessage(exception.Message, MessageType.Error);
                return;
            }

            MessageService.SetMessage("Operation is made Successfully", MessageType.Status);
            NavigationService.Navigate(Item);
        }
    }
    public enum SubmitStatus
    {
        Edit,
        Create,
        Delete
    }
}
