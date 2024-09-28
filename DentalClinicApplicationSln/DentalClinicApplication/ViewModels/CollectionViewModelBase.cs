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
		public IEnumerable<T> Collection { get; set; } = Enumerable.Empty<T>();
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
