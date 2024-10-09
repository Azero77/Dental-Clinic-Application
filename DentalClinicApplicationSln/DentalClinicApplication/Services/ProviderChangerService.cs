using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using DentalClinicApplication.Services.DataProvider;
using DentalClinicApplication.Stores;
using DentalClinicApplication.ViewModels;
using DentalClinicApplication.VirtualizationCollections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace DentalClinicApplication.Services
{
    
    /// <summary>
    /// Class For Changing the provider For the Collection Store COllection
    /// </summary>
    /// <typeparam name="T">type of Item to provide</typeparam>


    public class ProviderChangerService<T>
    {
        public IProvider<T> CurrentProvider { get; }
        public event Func<Task> ProviderChanged;

        public ProviderChangerService(
            IProvider<T> CurrentProvider,
            Func<Task> OnProviderChanged)
        {
            this.CurrentProvider = CurrentProvider;
            ProviderChanged += OnProviderChanged;
        }



        /// <summary>
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value">if not present then it will be an order clause</param>
        public (string? whereClause,string? orderClause) sqlGenerator(Dictionary<string,object> keyValuePairs)
        {
            bool isOrder = keyValuePairs.Count == 1 && keyValuePairs.First().Value == null;

            if (isOrder)
                return (null, OrderByGenerator(keyValuePairs.First().Key));
            return (WhereClauseGenerator(keyValuePairs), null);
        }

        protected virtual PropertyInfo? SearchProperty(string propertyName)
        {
            return typeof(T).GetProperty(propertyName);
        }

        private static string sqlPropertyGenerator(string propertyName, object value, Type propType)
        {
            try
            {
                value = Convert.ChangeType(value, propType);
            }
            catch (Exception e)
            {
                throw;
            }
            switch (propType)
            {
                case Type t when t == typeof(int):
                    return $"{propertyName} = {value}";
                case Type t when t == typeof(string):
                    return $"{propertyName} LIKE '%{value}%'";
                case Type t when t == typeof(DateTime):
                    DateTime dateTime = DateTime.Parse(value!.ToString()!);
                    string sqlDate = dateTime.ToString("yyyy-MM-dd");
                    return $"DATE({propertyName}) = DATE('{sqlDate}')";
                default:
                    return string.Empty;
            }

        }

        /// <summary>
        /// For Multiple Search Like Name (FirstName and LastName)
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        private string WhereClauseGenerator(Dictionary<string, object> keyValuePairs)
        {
            string sql = "WHERE ";
            List<string> whereClauses = new();
            foreach (string propertyName in keyValuePairs.Keys)
            {
                PropertyInfo? prop = SearchProperty(propertyName);
                if (prop is null)
                    throw new InvalidDataException("The Type is not Found");
                Type propType = prop.PropertyType;
                whereClauses.Add(sqlPropertyGenerator(propertyName, keyValuePairs[propertyName], propType!));
            }
            sql += string.Join(" AND ",whereClauses);
            return sql;
        }

        private string OrderByGenerator(string propertyName)
        {
            return $"ORDER BY {propertyName}";
        }

        public void ChangeProvider(Dictionary<string,object> keyValuePairs)
        {
            (string? whereClause,string? orderClause) = sqlGenerator(keyValuePairs);
            CurrentProvider.ChangeProvider(whereClause, orderClause);
            ProviderChanged?.Invoke();
        }

        public static Dictionary<Type, string> TypeRepresentaion = new()
        {
            {typeof(int),"Integer" },
            {typeof(string), "Text" },
            {typeof(DateTime), "Date" }
        };
        
    }

    public class ProviderChangerService<T, TJOIN>
        : ProviderChangerService<T>
    {
        public ProviderChangerService(IProvider<T> CurrentProvider, Func<Task> OnProviderChanged) : base(CurrentProvider, OnProviderChanged)
        {
        }

        protected override PropertyInfo? SearchProperty(string propertyName)
        {
            return typeof(T).GetProperty(propertyName) ??
                typeof(TJOIN).GetProperty(propertyName);
        }
    }
    public class ProviderChangerServiceHelpers
    {
        public static Dictionary<string, object> NameSearch(object value)
        {
            if (value is string stringValue)
            {
                Dictionary<string, object> result = new();
                string[] NameArray = stringValue.Split(" ");
                string firstName = NameArray[0];
                result.Add("FirstName", firstName);
                if (NameArray.Length > 1)
                {
                    string lastName = NameArray[1];
                    result.Add("LastName", lastName);
                }
                return result;
            }
            throw new InvalidCastException();
        }
    }

}
