using Configurations;
using DentalClinicApp.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.ViewModels.Configuration
{
    public class ConfigurationViewModel : ViewModelBase
    {
        public appConfigurationModel ConfigurationModel { get; }

        public ConfigurationViewModel(appConfigurationModel configurationModel)
        {
            ConfigurationModel = configurationModel;
        }
    }
}
