using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    public abstract class CollectionViewModelBase<T> : ViewModelBase
    {
        public CollectionViewModelBase(IProvider<T> collectionProvider)
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

        public IProvider<T> CollectionProvider { get; }

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
}
