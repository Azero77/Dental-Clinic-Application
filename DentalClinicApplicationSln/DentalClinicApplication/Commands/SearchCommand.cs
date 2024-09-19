using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    public class SearchCommand<T> : AsyncCommandBase
    {
        public IProvider<T> DataProvider { get; }
        public string PropertyName { get; }

        public SearchCommand(IProvider<T> dataProvider, string propertyName)
        {
            DataProvider = dataProvider;
            PropertyName = propertyName;
        }

        public override Task ExecuteAsync(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
