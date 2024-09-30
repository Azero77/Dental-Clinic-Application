﻿using Configurations.DataContext;
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
using DentalClinicApplication.ViewModels.Configuration;
using System.Collections.ObjectModel;

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
                    sc.AddSingleton<appConfigurationModel>();
                    AddMapper(sc);
                    sc.AddSingleton<DbContext>();
                    sc.AddSingleton<Provider<Appointment, AppointmentDTO>,AppointmentsProvider>(sp =>
                    GetTodayAppointmentsProvider(sp));
                    sc.AddSingleton<IProvider<Client>, VirtualizedProvider<Client,ClientDTO>>();
                    sc.AddSingleton<IVirtualizationItemsProvider<Client>, VirtualizedProvider<Client,ClientDTO>>();
                    sc.AddSingleton<MessageStore>();
                    sc.AddSingleton<MessageService>();
                    sc.AddSingleton<VirtualizationCollection<Client>>(
                        sp =>
                        new VirtualizationCollection<Client>(
                            sp.GetRequiredService<IVirtualizationItemsProvider<Client>>()
                            )
                        ) ;
                    sc.AddSingleton<ICollectionStore<Client>,VirtualizedCollectionStore<Client>>();
                    sc.AddSingleton<NavigationStore>();
                    sc.AddSingleton<DataCreator>();
                    sc.AddSingleton<DataEditor>();
                    sc.AddSingleton<DataDeleter>();


                    sc.AddSingleton<ManipulationNotifierService>(
                       sp => new ManipulationNotifierService(
                           GetManipulators(sp)
                           )
                       );
                    
                    sc.AddSingleton<ConfigurationViewModel>();
                    sc.AddTransient<HomePageViewModel>(sp => GetHomePageViewModel(sp));
                    sc.AddTransient<VirtualizedCollectionComponentViewModel<Client>>(sp => 
                    GetVirtualizedClientComponentViewModel(sp));
                    sc.AddTransient<ClientsListingViewModel>(sp =>
                        ClientsListingViewModel.GetClientsListingViewModel
                        (sp.GetRequiredService<IProvider<Client>>(),
                        sp.GetRequiredService<INavigationService<ClientsManipulationViewModel>>(),
                        sp.GetRequiredService<DataDeleter>(),
                        sp.GetRequiredService<ICollectionStore<Client>>(),
                        sp.GetRequiredService<VirtualizedClientsComponentViewModel>())
                    ) ;
                    //Default is Insert 
                    sc.AddTransient<ClientsManipulationViewModel>(sp => new ClientsManipulationViewModel(
                        sp.GetRequiredService<INavigationService>(),
                        sp.GetRequiredService<DataCreator>()
                        ));
                    sc.AddTransient<MakeEditAppointmentViewModel>();
                    sc.AddSingleton<INavigationService>(sp => MakeLayoutNavigationService<HomePageViewModel>(sp));
                    sc.AddSingleton<INavigationService<ClientsManipulationViewModel>,LayoutNavigationService<ClientsManipulationViewModel>>();
                    sc.AddSingleton<INavigationService<MakeEditAppointmentViewModel>,
                        LayoutNavigationService<MakeEditAppointmentViewModel>>();
                    sc.AddSingleton<Func<object?, ClientsListingViewModel>>(sp => 
                    (obj) => sp.GetRequiredService<ClientsListingViewModel>()
                    );
                    sc.AddSingleton<Func<object?, MakeEditAppointmentViewModel>>(sp => 
                    obj => sp.GetRequiredService<MakeEditAppointmentViewModel>());

                    sc.AddSingleton<Func<object?, NavigationBarViewModel>>(
                        sp => obj => sp.GetRequiredService<NavigationBarViewModel>());
                    sc.AddSingleton<Func<object?, MessageViewModel>>(sp => 
                    obj => sp.GetRequiredService<MessageViewModel>());
                    sc.AddSingleton<MessageViewModel>();
                    sc.AddSingleton<Func<object?, ClientsManipulationViewModel>>(sp =>
                    (obj) =>
                    {
                        Client? client = obj as Client;
                        //if there is no client render the new client view, else render the edit client view
                        if (client is null)
                        {
                            return new ClientsManipulationViewModel(MakeLayoutNavigationService<ClientsListingViewModel>(sp),
                                sp.GetRequiredService<DataCreator>()
                                );
                        }
                        return new ClientsManipulationViewModel(
                            client,
                            MakeLayoutNavigationService<ClientsListingViewModel>(sp),
                            sp.GetRequiredService<DataEditor>());
                    });
                    sc.AddSingleton<Func<object?, HomePageViewModel>>(sp =>
                    (obj) => sp.GetRequiredService<HomePageViewModel>());
                    sc.AddSingleton<NavigationBarViewModel>(sp =>
                        new (
                            MakeLayoutNavigationService<HomePageViewModel>(sp)
                            )
                    ) ;
                    sc.AddSingleton<MainViewModel>();
                    sc.AddSingleton<MainWindow>(sp => new MainWindow() { DataContext = sp.GetRequiredService<MainViewModel>()});
                })
                .Build();
        }

        private VirtualizedCollectionComponentViewModel<Client> GetVirtualizedClientComponentViewModel(IServiceProvider sp)
        {
            return VirtualizedClientsComponentViewModel.LoadVirtualizedCollectionComponentViewModel<VirtualizedClientsComponentViewModel>(
                sp.GetRequiredService<VirtualizationCollection<Client>>(),
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
                sp.GetRequiredService<Provider<Appointment, AppointmentDTO>>(),
                sp.GetRequiredService<INavigationService<MakeEditAppointmentViewModel>>());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            INavigationService InitialNavigatioService = _host.Services.GetRequiredService<INavigationService>();
            InitialNavigatioService.Navigate(null);
            MainWindow window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();
            base.OnExit(e);
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

        private IEnumerable<IDataManipulator> GetManipulators(IServiceProvider sp)
        {
            List<IDataManipulator> dataManipulators = new();
            var allManipulatorsTypes =
                Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => !type.IsAbstract
                            &&
                            typeof(IDataManipulator).IsAssignableFrom(type));
            foreach (Type type in allManipulatorsTypes)
            {
                dataManipulators.Add((IDataManipulator) sp.GetRequiredService(type));
            }
            return dataManipulators;
        }
        private void AddMapper(IServiceCollection sc)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(ClientProfile));
            });
            sc.AddSingleton(config.CreateMapper());
        }
    }
}
