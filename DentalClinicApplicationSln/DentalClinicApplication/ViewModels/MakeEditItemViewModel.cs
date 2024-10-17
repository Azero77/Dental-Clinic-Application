using AutoMapper;
using DentalClinicApp.Commands;
using DentalClinicApp.Models;
using DentalClinicApplication.Commands;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    public abstract class  MakeEditItemViewModel<T>
        : ErrorViewModelBase
    {
        protected MakeEditItemViewModel(IMapper mapper,
                                        IDataService<T> dataService,
                                        INavigationService navigationService,
                                        MessageService messageService,
                                        SubmitStatus submitStatus
                                        )
        {
            Mapper = mapper;
            DataCreator = dataService;
            NavigationService = navigationService;
            MessageService = messageService;
            SubmitCommand = new SubmitItemCommand<T>(
                this,
                navigationService,
                dataService,
                messageService,
                submitStatus
                );
            DeleteCommand =
                submitStatus == SubmitStatus.Edit ?
                new SubmitItemCommand<T>(this,
                navigationService,
                dataService,
                messageService,
                SubmitStatus.Delete) : null;

        }

        public IMapper Mapper { get; }
        public IDataService<T> DataCreator { get; }
        public INavigationService NavigationService { get; }
        public MessageService MessageService { get; }
        public ICommand? SubmitCommand { get;protected set; }
        public ICommand? DeleteCommand { get; protected set; }
        //to determine to show Delete button or not
        public bool IsEdit => DeleteCommand != null;
    }
}
