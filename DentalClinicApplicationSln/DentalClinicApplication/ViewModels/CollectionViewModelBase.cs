﻿using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.Services;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    public abstract class CollectionViewModelBase<T> : SearchCollectionViewModel<T>
    {
        public CollectionViewModelBase(IProvider<T> collectionProvider
			) : base(collectionProvider)
        {
            CollectionProvider = collectionProvider;
        }
        private IEnumerable<T> _collection = Enumerable.Empty<T>();
		public IEnumerable<T> Collection
		{
			get => _collection;
			set
			{
				_collection = value;
				OnCollectionChanged();
			}
		}

        private void OnCollectionChanged()
        {
			CollectionChagned?.Invoke();
        }
		public event Action? CollectionChagned;

        public IProvider<T> CollectionProvider { get; set; }

        private bool _isLoading;
		public bool IsLoading
		{
			get
			{
				return _isLoading;
			}
			set
			{
				_isLoading = value;
				OnPropertyChanged(nameof(IsLoading));
			}
		}
        public abstract Task LoadViewModel();

        /// <summary>
        /// Take the view model and load the collection from the provider
        /// </summary>
        /// <param name="collectionViewModelBase"></param>
        /// <returns></returns>
        public static CollectionViewModelBase<T> LoadCollectionViewModel(
			CollectionViewModelBase<T> collectionViewModelBase
			)
		{
			ICommand LoadCommand =
				new LoadCommand<T>(collectionViewModelBase);
			LoadCommand.Execute(null);
			return collectionViewModelBase;
		}
	}

	/// <summary>
	/// View Model Base Class For Searching With CollectionViewModel base
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class SearchCollectionViewModel<T>
		:ViewModelBase
	{
        public ProviderChangerService<T> ProviderChangerService { get; }
        public SearchCollectionViewModel(
            IProvider<T> collectionProvider
            )
        {
            ProviderChangerService = new ProviderChangerService<T>(collectionProvider,OnProviderChanged);
        }
        public abstract Task OnProviderChanged();

		//method for generating properties and values search
		public abstract Dictionary<string, object> SearchMapper(string property, object value);

    }
}
