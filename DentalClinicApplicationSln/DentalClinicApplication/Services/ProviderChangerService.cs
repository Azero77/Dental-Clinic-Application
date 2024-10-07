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
        public CollectionViewModelBase<T> CollectionViewModelBase { get; }
        public IProvider<T> CurrentProvider { get; set; }
        public ChangeMode ChangeMode { get; }
        public IEnumerable<T> Collection => CollectionViewModelBase.Collection;


        public ProviderChangerService(CollectionViewModelBase<T> collectionViewModelBase,
            IProvider<T> currentProvider,
            ChangeMode changeMode)
        {
            CollectionViewModelBase = collectionViewModelBase;
            CurrentProvider = currentProvider;
            ChangeMode = changeMode;
        }



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
            CurrentProvider = newProvider;
            CollectionViewModelBase.CollectionProvider = newProvider;
            CollectionViewModelBase.Collection = await newProvider.GetItems();
        }

        protected virtual PropertyInfo? SearchProperty(string propertyName)
        {
            return typeof(T).GetProperty(propertyName);
        }
        private string WhereClauseGenerator(string propertyName,object? value)
        {
            //see the propertyType
            //depending on the propertyType we set the sql string
            PropertyInfo? prop = SearchProperty(propertyName);
            if (prop is null)
                throw new InvalidDataException("The Type is not Found");
            Type propType = prop.PropertyType;
            string sql = "WHERE ";
            if (value is null)
            {
                throw new InvalidDataException();
            }
            switch (propType)
            {
                case Type t when t == typeof(int):
                    sql += $"{propertyName} = {value.ToString()}";
                    break;
                case Type t when t == typeof(string):
                    sql += $"{propertyName} LIKE '%{value.ToString()}%'";
                    break;
                case Type t when t == typeof(DateTime):
                    DateTime dateTime = DateTime.Parse(value.ToString());
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

        private IProvider<T> GenerateProvider(string clause)
        {
            return ChangeMode == ChangeMode.Search ?
             CurrentProvider.ChangeProvider(clause,null) : 
             CurrentProvider.ChangeProvider(null, clause);
        }


    }

    public class ProviderChangerService<T, TJOIN>
        : ProviderChangerService<T>
    {
        public ProviderChangerService(CollectionViewModelBase<T> collectionViewModelBase, IProvider<T> currentProvider, ChangeMode changeMode) : base(collectionViewModelBase, currentProvider, changeMode)
        {
        }
        protected override PropertyInfo? SearchProperty(string propertyName)
        {
            return typeof(T).GetProperty(propertyName) ??
                typeof(TJOIN).GetProperty(propertyName);
        }
    }

    public enum ChangeMode
    {
        Search = 0,
        Order = 1
    }
}
