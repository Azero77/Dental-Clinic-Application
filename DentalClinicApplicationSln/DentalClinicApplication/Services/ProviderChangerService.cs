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
        public (string? whereClause,string? orderClause) sqlGenerator(string propertyName,object? value)
        {
            if (value is not null)
            {
                //then it will be a where clause
                return (WhereClauseGenerator(propertyName, value), null);
            }
            return (null, OrderByGenerator(propertyName));
        }

        protected virtual PropertyInfo? SearchProperty(string propertyName)
        {
            return typeof(T).GetProperty(propertyName);
        }
        private string WhereClauseGenerator(string propertyName,object value)
        {
            //see the propertyType
            //depending on the propertyType we set the sql string
            PropertyInfo? prop = SearchProperty(propertyName);
            if (prop is null)
                throw new InvalidDataException("The Type is not Found");
            Type propType = prop.PropertyType;

            //parse the value, if there is error => invalidCastException
            try
            {
                value = Convert.ChangeType(value, propType);
            }
            catch (FormatException e)
            {
                throw;
            }
            string sql = "WHERE ";
            switch (propType)
            {
                case Type t when t == typeof(int):
                    sql += $"{propertyName} = {value}";
                    break;
                case Type t when t == typeof(string):
                    sql += $"{propertyName} LIKE '%{value}%'";
                    break;
                case Type t when t == typeof(DateTime):
                    DateTime dateTime = DateTime.Parse(value!.ToString()!);
                    string sqlDate = dateTime.ToString("yyyy-MM-dd");
                    sql += $"DATE({propertyName}) = DATE('{sqlDate}')";
                    break;
            }
            return sql;

        }
        

        private string OrderByGenerator(string propertyName)
        {
            return $"ORDER BY {propertyName}";
        }

        public void ChangeProvider(string propertyName,object? value)
        {
            (string? whereClause,string? orderClause) = sqlGenerator(propertyName, value);
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

}
