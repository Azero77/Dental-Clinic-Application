using AutoMapper;
using DentalClinicApp.Commands;
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
                                        IDataService<T> dataCreator,
                                        INavigationService navigationService,
                                        MessageService messageService
                                        )
        {
            Mapper = mapper;
            DataCreator = dataCreator;
            NavigationService = navigationService;
            MessageService = messageService;
        }

        public IMapper Mapper { get; }
        public IDataService<T> DataCreator { get; }
        public INavigationService NavigationService { get; }
        public MessageService MessageService { get; }
        public ICommand? SubmitCommand { get;protected set; }
    }
}
