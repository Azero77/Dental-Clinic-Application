using Configurations.DataContext;
using DentalClinicApplication.VirtualizationCollections;
using DentalClinicApp.Models;
using DentalClinicApp.Services;
using DentalClinicApp.Stores;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Services.DataProvider;
using DentalClinicApplication.Stores;
using DentalClinicApplication.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using System.Windows;
using System.Windows.Navigation;
using DentalClinicApplication.DTOs;
using DentalClinicApplication.AutoMapperProfiles;
using DentalClinicApplication.ComponentsViewModels;
using Configurations;
using System.Collections.ObjectModel;
using DentalClinicApplication.Views;
using DentalClinicApplication.Commands;
using Microsoft.Extensions.Configuration;
using System.Windows.Media;
using System.Globalization;
using System.Threading;

namespace DentalClinicApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IHost _host;
        public App()
        {

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((sc) =>
                {
                    AddMapper(sc);
                    ConfigureService(sc);
                    sc.AddSingleton<DbContext>();
                    //provider for dataService where we need all appointments
                    sc.AddSingleton<IProvider<Appointment>>(sp =>
                    GetAllAppointmentsProvider(sp));
                    sc.AddSingleton<IProvider<Client>,ClientsProvider>();
                    sc.AddSingleton<IProviderHelper<Appointment>, AppointemntsProviderHelper>();
                    sc.AddSingleton<IProviderHelper<Client>, ClientsProviderHelper>();
                    //provider for the home page where we need the appointment for today
                    sc.AddSingleton<IVirtualizationItemsProvider<Client>, ClientsVirtualizedProvider>();
                    sc.AddSingleton<IVirtualizationItemsProvider<Appointment>, AppointmentsVirtualizedProvider>();
                    sc.AddSingleton<MessageStore>();
                    sc.AddSingleton<MessageService>();
                    sc.AddSingleton<VirtualizationCollection<Client>>(
                        sp =>
                        new VirtualizationCollection<Client>(
                            sp.GetRequiredService<IVirtualizationItemsProvider<Client>>()
                            )
                        ) ;
                    sc.AddSingleton<VirtualizationCollection<Appointment>>(sp => 
                    new VirtualizationCollection<Appointment>(
                        sp.GetRequiredService<IVirtualizationItemsProvider<Appointment>>()));
                    sc.AddSingleton<ICollectionStore<Client>,VirtualizedCollectionStore<Client>>();
                    sc.AddSingleton<ICollectionStore<Appointment>, VirtualizedCollectionStore<Appointment>>();
                    sc.AddSingleton<NavigationStore>();
                    sc.AddSingleton<IDataManipulator,DataManipulator>();
                    sc.AddSingleton<IDataService<Appointment>, AppointmentsDataService>();
                    sc.AddSingleton<IDataService<Client>, ClientsDataService>();

                    sc.AddSingleton<ManipulationNotifierService>();
                    
                    sc.AddTransient<HomePageViewModel>(sp => GetHomePageViewModel(sp));
                    sc.AddTransient<VirtualizedCollectionComponentViewModel<Client>>(sp => 
                    GetVirtualizedClientComponentViewModel(sp));
                    sc.AddTransient<VirtualizedCollectionComponentViewModel<Appointment>>(sp =>
                    GetVirtualizedAppointmentsComponentViewModel(sp));
                    sc.AddTransient<MakeEditAppointmentViewModel>();
                    sc.AddTransient<MakeEditClientViewModel>();
                    sc.AddTransient<AllAppointmentsViewModel>(
                        sp => GetAllAppointmentsViewModel(sp));
                    sc.AddTransient<AllClientsViewModel>(
                        sp => GetAllClientsViewModel(sp));
                    sc.AddSingleton<INavigationService<ClientsManipulationViewModel>,LayoutNavigationService<ClientsManipulationViewModel>>();
                    sc.AddSingleton<INavigationService<MakeEditAppointmentViewModel>,
                        LayoutNavigationService<MakeEditAppointmentViewModel>>();
                    sc.AddSingleton<INavigationService<AllClientsViewModel>, LayoutNavigationService<AllClientsViewModel>>();
                    sc.AddSingleton<INavigationService<HomePageViewModel>, LayoutNavigationService<HomePageViewModel>>();
                    sc.AddSingleton<INavigationService<MakeEditClientViewModel>, LayoutNavigationService<MakeEditClientViewModel>>();
                    sc.AddSingleton<INavigationService<AllAppointmentsViewModel>,LayoutNavigationService<AllAppointmentsViewModel>>();
                    sc.AddSingleton<INavigationService<ClientProfileViewModel>,LayoutNavigationService<ClientProfileViewModel>>();
                    sc.AddSingleton<Func<object?, ClientsListingViewModel>>(sp => 
                    (obj) => sp.GetRequiredService<ClientsListingViewModel>()
                    );
                    sc.AddSingleton<Func<object?, MakeEditAppointmentViewModel>>(sp => 
                    obj => 
                    {
                        if (obj is null)
                            return sp.GetRequiredService<MakeEditAppointmentViewModel>();
                        return new MakeEditAppointmentViewModel(
                                GetVirtualizedClientComponentViewModel(sp),
                                sp.GetRequiredService<INavigationService<AllAppointmentsViewModel>>(),
                                sp.GetRequiredService<IDataService<Appointment>>(),
                                sp.GetRequiredService<MessageService>(),
                                sp.GetRequiredService<IMapper>(),
                                obj as Appointment,
                                //SubmitStatus Should be edit if the factory is not a dependency injection requiredSErvice
                                SubmitStatus.Edit
                                );
                    });

                    sc.AddSingleton<Func<object?, AllAppointmentsViewModel>>(sp =>
                    obj => GetAllAppointmentsViewModel(sp));
                    sc.AddSingleton<Func<object?, NavigationBarViewModel>>(
                        sp => obj => sp.GetRequiredService<NavigationBarViewModel>());
                    sc.AddSingleton<Func<object?, MessageViewModel>>(sp => 
                    obj => sp.GetRequiredService<MessageViewModel>());
                    sc.AddSingleton<Func<object?, AllClientsViewModel>>(sp =>
                    obj => GetAllClientsViewModel(sp));
                    sc.AddSingleton<Func<object?, MakeEditClientViewModel>>(sp => 
                    obj =>
                    {
                        if (obj is null)
                            return sp.GetRequiredService<MakeEditClientViewModel>();
                        return new MakeEditClientViewModel(
                                sp.GetRequiredService<IMapper>(),
                                sp.GetRequiredService<INavigationService<AllClientsViewModel>>(),
                                sp.GetRequiredService<IDataService<Client>>(),
                                sp.GetRequiredService<MessageService>(),
                                obj as Client,
                                SubmitStatus.Edit
                                );
                    });

                    sc.AddSingleton<MessageViewModel>();

                    sc.AddSingleton<Func<object?, ClientsManipulationViewModel>>(sp => (obj) =>
                    {
                        Client? client = obj as Client;
                        //if there is no client render the new client view, else render the edit client view
                        if (client is null)
                        {
                            return new ClientsManipulationViewModel(MakeLayoutNavigationService<ClientsListingViewModel>(sp),
                                sp.GetRequiredService<IDataManipulator>()
                                );
                        }
                        return new ClientsManipulationViewModel(
                            client,
                            MakeLayoutNavigationService<ClientsListingViewModel>(sp),
                            sp.GetRequiredService<IDataManipulator>());
                    });
                    sc.AddSingleton<Func<object?, HomePageViewModel>>(sp =>
                    (obj) =>GetHomePageViewModel(sp));
                    sc.AddSingleton<NavigationBarViewModel>(sp =>
                        new(
                            sp.GetRequiredService<NavigationStore>(),
                            MakeLayoutNavigationService<HomePageViewModel>(sp),
                            MakeLayoutNavigationService<AllAppointmentsViewModel>(sp),
                            MakeLayoutNavigationService<AllClientsViewModel>(sp)
                            )
                    ) ;
                    sc.AddSingleton<Func<object?, ClientProfileViewModel>>(sp =>
                    GetClientProfileViewModel(sp)
                    );
                    sc.AddSingleton<MainViewModel>();
                    sc.AddSingleton<MainWindow>(sp => new MainWindow() { DataContext = sp.GetRequiredService<MainViewModel>()});
                })
                .Build();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            AddConfigurationResourceDictionary(_host.Services);
            INavigationService InitialNavigatioService = _host.Services.GetRequiredService<INavigationService<HomePageViewModel>>();
            InitialNavigatioService.Navigate(null);
            MainWindow window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();
            base.OnExit(e);
        }


        #region dependencyMethods
        private void ConfigureService(IServiceCollection sc)
        {
            ConfigureService configureService = new();
            //Register IConfiguration for the project
            sc.AddSingleton(configureService.Configuration);

        }

        private void AddConfigurationResourceDictionary(IServiceProvider sp)
        {
            Microsoft.Extensions.Configuration.IConfiguration configuration = sp.GetRequiredService<Microsoft.Extensions.Configuration.IConfiguration>();
            var resources = new ResourceDictionary()
            {
                {"ClinicName",configuration.GetValue<string>("ClinicName") },
                {"DoctorName",configuration.GetValue<string>("DoctorName")},
                {"NavigationBarColor",GetColor(configuration.GetValue<string>("NavigationBarColor"))},
                {"BackgroundColor",GetColor(configuration.GetValue<string>("BackgroundColor"))},
                {"ButtonStatusColor",GetColor(configuration.GetValue<string>("ButtonStatusColor"))},
                {"TextColor",GetColor(configuration.GetValue<string>("TextColor"))},
                {"Language",configuration.GetValue<string>("Language")},
                {"IsAnimated",configuration.GetValue<bool>("IsAnimated") }
            };
            App.Current.Resources.MergedDictionaries.Add(resources);
        }

        private Color GetColor(string colorHex)
        {
            return (Color)ColorConverter.ConvertFromString(colorHex);
        }
        private AllClientsViewModel GetAllClientsViewModel(IServiceProvider sp)
        {
            return new AllClientsViewModel(
                GetVirtualizedClientComponentViewModel(sp),
                sp.GetRequiredService<INavigationService<MakeEditClientViewModel>>(),
                sp.GetRequiredService<INavigationService<ClientProfileViewModel>>());
        }

        private LayoutNavigationService<TViewModel> GetEditLayoutNavigationService<TViewModel,T>(
            IServiceProvider sp,
            Func<object?,TViewModel> editViewModelFactory)
            where TViewModel : MakeEditItemViewModel<T>
        {
            return new LayoutNavigationService<TViewModel>(
                    sp.GetRequiredService<NavigationStore>(),
                    editViewModelFactory,
                    sp.GetRequiredService<Func<object?, NavigationBarViewModel>>(),
                    sp.GetRequiredService<Func<object?, MessageViewModel>>()
                );
        }
        
        private AllAppointmentsViewModel GetAllAppointmentsViewModel(IServiceProvider sp)
        {
            return new AllAppointmentsViewModel(
                GetVirtualizedAppointmentsComponentViewModel(sp),
                sp.GetRequiredService<INavigationService<MakeEditAppointmentViewModel>>());
        }
        
        private Func<object?, ClientProfileViewModel> GetClientProfileViewModel(IServiceProvider sp)
        {
            return (obj) =>
            {
                if (!int.TryParse(obj?.ToString() ?? "", out int id))
                {
                    throw new InvalidCastException();
                }
                return ClientProfileViewModel.LoadClientProfileViewModel(
                    sp.GetRequiredService<IProvider<Client>>(),
                    id
                    );

            };
        }

        private VirtualizedCollectionComponentViewModel<Appointment> GetVirtualizedAppointmentsComponentViewModel(IServiceProvider sp)
        {
            return VirtualizedCollectionComponentViewModel<Appointment>.LoadVirtualizedCollectionComponentViewModel(
                sp.GetRequiredService<VirtualizationCollection<Appointment>>(),
                sp.GetRequiredService<MessageService>(),
                sp.GetRequiredService<IProviderHelper<Appointment>>(),
                sp.GetRequiredService<ICollectionStore<Appointment>>());
        }

        
        private IProvider<Appointment> GetAllAppointmentsProvider(IServiceProvider sp)
        {
            return new AppointmentsProvider(
                sp.GetRequiredService<DbContext>(),
                sp.GetRequiredService<IMapper>());
        }

        private VirtualizedCollectionComponentViewModel<Client> GetVirtualizedClientComponentViewModel(IServiceProvider sp)
        {
            return VirtualizedCollectionComponentViewModel<Client>.LoadVirtualizedCollectionComponentViewModel(
                sp.GetRequiredService<VirtualizationCollection<Client>>(),
                sp.GetRequiredService<MessageService>(),
                sp.GetRequiredService<IProviderHelper<Client>>(),
                sp.GetRequiredService<ICollectionStore<Client>>());
        }

        private AppointmentsProvider GetTodayAppointmentsProvider(IServiceProvider sp)
        {
            return AppointmentsProvider.AppointmentsProviderForToday(sp.GetRequiredService<DbContext>(),
                sp.GetRequiredService<IMapper>());
        }

        private HomePageViewModel GetHomePageViewModel(IServiceProvider sp)
        {
            return HomePageViewModel.LoadHomePageViewModel(
                GetTodayAppointmentsProvider(sp),
                sp.GetRequiredService<INavigationService<MakeEditAppointmentViewModel>>(),
                sp.GetRequiredService<MessageService>(),
                sp.GetRequiredService<IProviderHelper<Appointment>>());
        }

        

        private INavigationService<TViewModel> MakeLayoutNavigationService<TViewModel>(IServiceProvider sp
            )
            where TViewModel : ViewModelBase
        {
            return new LayoutNavigationService<TViewModel>
                (
                sp.GetRequiredService<NavigationStore>(),
                sp.GetRequiredService<Func<object?,TViewModel>>(),
                (obj) => sp.GetRequiredService<NavigationBarViewModel>(),
                (obj) => sp.GetRequiredService<MessageViewModel>()
                );
        }
        private void AddMapper(IServiceCollection sc)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(ClientProfile));
                cfg.AddProfile(typeof(AppointmentProfile));
            });
            sc.AddSingleton(config.CreateMapper());
        }
        #endregion
    }
}
