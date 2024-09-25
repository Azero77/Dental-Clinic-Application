using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using DentalClinicApplication.Services.DataProvider;
using DentalClinicApplication.Stores;
using DentalClinicApplication.VirtualizationCollections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
        public ChangeMode ChangeMode { get; }

        public ProviderChangerService(IEnumerable<T> collection,
            IProvider<T> currentProvider,
            ChangeMode changeMode)
        {
            Collection = collection;
            CurrentProvider = currentProvider;
            ChangeMode = changeMode;
        }

        public IEnumerable<T> Collection { get; set; }


        public async Task Change(string propertyName,object? value)
        {
            string sql = ChangeMode == ChangeMode.Search ?
                WhereClauseGenerator(propertyName, value) :
                OrderByGenerator(propertyName);
            IProvider<T> newProvider = GenerateProvider(sql);
            if (Collection is VirtualizationCollection<T> virtualizationCollection)
            {
                await virtualizationCollection.ChangeProvider((IVirtualizationItemsProvider<T>)newProvider);
                return;
            }
            Collection = await newProvider.GetItems();
        }

        private string WhereClauseGenerator(string propertyName,object? value)
        {
            //see the propertyType
            //depending on the propertyType we set the sql string
            PropertyInfo? prop = typeof(T).GetProperty(propertyName);
            if (prop is null)
                throw new InvalidDataException("The Type is not Found");
            Type propType = prop.PropertyType;
            string sql = "WHERE ";
            switch (propType)
            {
                case Type t when t == typeof(int):
                    sql += $"{propertyName} = {value.ToString()}";
                    break;
                case Type t when t == typeof(string):
                    sql += $"{propertyName} LIKE '%{value.ToString()}%'";
                    break;
                case Type t when t == typeof(DateTime):
                    sql += $"{propertyName} = {value.ToString()}";
                    break;
            }
            return sql;

        }
        private string OrderByGenerator(string propertyName)
        {
            return $"ORDER BY {propertyName}";
        }

        private IProvider<T> GenerateProvider(string clause)
        {
            return ChangeMode == ChangeMode.Search ?
             CurrentProvider.ChangeProvider(clause, null) : 
             CurrentProvider.ChangeProvider(null, clause);
        }


    }
    public enum ChangeMode
    {
        Search = 0,
        Order = 1
    }
}
