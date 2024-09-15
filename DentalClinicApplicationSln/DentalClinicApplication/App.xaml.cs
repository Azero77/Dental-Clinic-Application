using Configurations.DataContext;
using DentalClinicApp.Models;
using DentalClinicApp.Services;
using DentalClinicApp.Stores;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataManiplator;
using DentalClinicApplication.Services.DataProvider;
using DentalClinicApplication.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

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
                    sc.AddSingleton<DbContext>();
                    sc.AddTransient<IClientsProvider, DbClientsProvider>();
                    sc.AddSingleton<NavigationStore>();
                    sc.AddSingleton<DataCreator>();
                    sc.AddSingleton<DataEditor>();
                    sc.AddSingleton<DataDeleter>();
                    sc.AddTransient<ClientsListingViewModel>(sp =>
                    new ClientsListingViewModel(
                        sp.GetRequiredService<IClientsProvider>(),
                        MakeLayoutNavigationService<ClientsManipulationViewModel>(sp),
                        sp.GetRequiredService<DataDeleter>()
                        )) ;
                    //Default is Insert 
                    sc.AddTransient<ClientsManipulationViewModel>(sp => new ClientsManipulationViewModel(
                        sp.GetRequiredService<INavigationService>(),
                        sp.GetRequiredService<DataCreator>()
                        ));
                    sc.AddSingleton<INavigationService>(sp => MakeLayoutNavigationService<ClientsListingViewModel>(sp));
                    sc.AddSingleton<INavigationService<ClientsManipulationViewModel>,LayoutNavigationService<ClientsManipulationViewModel>>();
                    sc.AddSingleton<Func<object?, ClientsListingViewModel>>(sp => 
                    (obj) => sp.GetRequiredService<ClientsListingViewModel>()
                    );
                    sc.AddSingleton<Func<object?, NavigationBarViewModel>>(
                        sp => obj => sp.GetRequiredService<NavigationBarViewModel>());
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
                    sc.AddSingleton<NavigationBarViewModel>(sp =>
                        new (
                            MakeLayoutNavigationService<ClientsListingViewModel>(sp),
                            MakeLayoutNavigationService<ClientsManipulationViewModel>(sp)
                            )
                    ) ;
                    sc.AddSingleton<MainViewModel>();
                    sc.AddSingleton<MainWindow>(sp => new MainWindow() { DataContext = sp.GetRequiredService<MainViewModel>()});
                })
                .Build();
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
                (obj) => sp.GetRequiredService<NavigationBarViewModel>()
                );
        }
    }
}
