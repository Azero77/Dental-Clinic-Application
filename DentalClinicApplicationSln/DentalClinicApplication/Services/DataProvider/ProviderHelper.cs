using DentalClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    public interface IProviderHelper<T>
    {
        IEnumerable<string> SearchProperties { get; }
        Dictionary<string, string> SearchDataProviderKeyValues { get; }
        Func<string, object, Dictionary<string, object>> PropertiesFactory { get; }
    }

    public class AppointemntsProviderHelper : IProviderHelper<Appointment>
    {
        public IEnumerable<string> SearchProperties => new string[]
        {
            "Name",
            "Date",
            "Description",
            "Email"
        };

        public Dictionary<string, string> SearchDataProviderKeyValues => new Dictionary<string, string>()
            {
                {"Date","StartDate" },
                {"Description","Description" },
                {"Email","Email" }
            };

        public Func<string, object, Dictionary<string, object>> PropertiesFactory => (propertyName, value) =>
        {
            if (SearchDataProviderKeyValues.TryGetValue(propertyName, out string? propertyDataName))
            {
                return new Dictionary<string, object>() {
                    {propertyDataName,value }
                };
            }
            else
            {
                switch (propertyName)
                {
                    case "Name":
                        return ProviderChangerServiceHelpers.NameSearch(value);
                    default:
                        return new()
                        {
                            {propertyName,value }
                        };

                }
            }
        };
    }

    public class ClientsProviderHelper : IProviderHelper<Client>
    {
        public IEnumerable<string> SearchProperties => new string[]
        {
            "Name",
            "Email",
            "Gender",
            "Age"
        };

        public Dictionary<string, string> SearchDataProviderKeyValues => new Dictionary<string, string>()
        {
            {"Email","Email" },
            {"Gender","Gender" }
        };

        public Func<string, object, Dictionary<string, object>> PropertiesFactory =>
           (propertyName, value) =>
           {
               if (SearchDataProviderKeyValues.TryGetValue(propertyName, out string propertyDataName))
               {
                   return new Dictionary<string, object>() {
                    {propertyDataName,value }
                };
               }
               else
               {
                   switch (propertyName)
                   {
                       case "Name":
                           return ProviderChangerServiceHelpers.NameSearch(value);
                       case "Age":
                           return ProviderChangerServiceHelpers.AgeSearch(value);
                       default:
                           return new()
                        {
                            {propertyName,value }
                        };

                   }
               }
           };

    }
}
