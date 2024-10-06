using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurations
{
    public class ConfigureService
    {
        public IConfiguration Configuration { get; }
        public ConfigureService()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appConfigurations.json")
                .Build();
        }
    }
}
