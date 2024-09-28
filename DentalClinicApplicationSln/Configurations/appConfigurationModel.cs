using Microsoft.Extensions.Configuration;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurations
{
    public class appConfigurationModel
    {
        public string ClinicName { get; private set;}
        public string DoctorName { get; private set;}
        public string NavigationBarColor { get; private set;}
        public string BackgroundColor { get; private set;}
        public string Language { get; private set; }

        IConfigurationRoot _configuration;

#pragma warning disable CS8618
        public appConfigurationModel()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appConfigurations.json")
                .Build();
            PopulateData();
        }
#pragma warning restore

        private void PopulateData()
        {
            ClinicName = _configuration["ClinicName"];
            DoctorName = _configuration["DoctorName"];
            NavigationBarColor = _configuration["NavigationBarColor"];
            BackgroundColor = _configuration["NavigationBarColor"];
            Language = _configuration["Language"];
        }
    }
}
