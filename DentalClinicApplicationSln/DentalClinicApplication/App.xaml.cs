using Configurations.DataContext;
using DentalClinicApp.Stores;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Services.DataProvider;
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
                    sc.AddSingleton<NavigationStore>(sp => new () { CurrentViewModel = new ClientsListingViewModel(sp.GetRequiredService<IClientsProvider>()) });
                    sc.AddSingleton<NavigationService>();
                    sc.AddSingleton<MainViewModel>();
                    sc.AddSingleton<MainWindow>(sp => new MainWindow() { DataContext = sp.GetRequiredService<MainViewModel>()});
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            MainWindow window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();
        }

    }
}
