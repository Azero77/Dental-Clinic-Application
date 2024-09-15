using Configurations.DataContext;
using DentalClinicApp.Services;
using DentalClinicApp.Stores;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Services;
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
                    sc.AddSingleton<INavigationService>(sp => MakeLayoutNavigationService<ClientsListingViewModel>(sp));
                    sc.AddSingleton<Func<object?, ClientsListingViewModel>>(sp => 
                    (obj) => 
                    {
                        return new ClientsListingViewModel(sp.GetRequiredService<IClientsProvider>());
                    });
                    sc.AddSingleton<Func<object?, ClientsManipulationViewModel>>(sp =>
                    (obj) =>
                    {
                        return new ClientsManipulationViewModel();
                    });
                    sc.AddSingleton<NavigationBarViewModel>(sp =>
                        new (
                            MakeLayoutNavigationService<ClientsListingViewModel>(sp),
                            MakeLayoutNavigationService<ClientsManipulationViewModel>(sp)
                            )
                    ) ;
                    sc.AddTransient<ClientsListingViewModel>();
                    sc.AddTransient<ClientsManipulationViewModel>();
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

        private INavigationService MakeLayoutNavigationService<TViewModel>(IServiceProvider sp)
            where TViewModel : ViewModelBase
        {
            return new LayoutNavigationService<TViewModel>
                (
                sp.GetRequiredService<NavigationStore>(),
                (obj) => sp.GetRequiredService<TViewModel>(),
                (obj) => sp.GetRequiredService<NavigationBarViewModel>()
                );
        }

    }
}
