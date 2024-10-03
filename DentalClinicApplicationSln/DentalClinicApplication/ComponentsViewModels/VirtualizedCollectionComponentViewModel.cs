using DentalClinicApp.Commands;
using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.Services;
using DentalClinicApplication.Stores;
using DentalClinicApplication.ViewModels;
using DentalClinicApplication.VirtualizationCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace DentalClinicApplication.ComponentsViewModels
{
    public class VirtualizedCollectionComponentViewModel<T> : CollectionViewModelBase<T>
    {
        private VirtualizationCollection<T> _collection;
        
        //maybe null if the collection does not have to be stored
        public ICollectionStore<T>? CollectionStore { get; private set; }
        public Task Load()
        {
            return _collection.Load();
        }

        public VirtualizedCollectionComponentViewModel(
            VirtualizationCollection<T> collection,
            ICollectionStore<T>? collectionStore = null)
            : base(collection.ItemsProvider)
        {
            _collection = collection;
            Collection = _collection;
            CollectionStore = collectionStore;
            _collection.CollectionChanged += _collection_CollectionChanged;
            _collection.PropertyChanged += OnPropertyChanged;
            /*if (CollectionStore is not null)
                CollectionStore.CollectionChanged += OnCollectionReset;*/
            Move = new VirtualizationCollectionMoveCommand<T>(collection);
            MoveNext = new VirtualizationCollectionMoveCommand<T>(collection, moveValue: MoveValue.Next);
            MovePrevious = new VirtualizationCollectionMoveCommand<T>(collection, moveValue: MoveValue.Previous);
            SearchCommand = new SearchCommand<T>(
                new ProviderChangerService<T>(this, _collection!.ItemsProvider, ChangeMode.Search), this);
            OrderCommand = new SearchCommand<T>(
                new ProviderChangerService<T>(this, _collection.ItemsProvider, ChangeMode.Order), this);
        }



        #region events

        private void _collection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(string.Empty);
        }
        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e?.PropertyName ?? string.Empty);
        }

        #endregion

        
        #region commands
        public ICommand? Move { get; set; }
        public ICommand? MoveNext { get; set; }
        public ICommand? MovePrevious { get; set; }
        public ICommand? SearchCommand { get; set; }
        public ICommand? OrderCommand { get; set; }

        public int CurrentPageIndex => _collection.CurrentPageIndex;
        public int PageSize
        {
            get => _collection.PageSize;
            set
            {
                _collection.PageSize = value;
            }
        }
        public int? PagesCount => _collection?.PagesCount;
        public IEnumerable<string> Properties => typeof(T).GetProperties().Select(p => p.Name).Where(n => !n.Contains("Id"));
        public string? FirstProperty => Properties.FirstOrDefault();
        public List<int> PagesIndexers =>
            MakePageIndexers();
        #endregion

        private List<int> MakePageIndexers()
        {
            List<int> result = new();
            for (int i = 0; i < PagesCount; i++)
            {
                result.Add(i);
            }
            return result;
        }

        //initializing a new virtualization view model since there is no constuctor 
        // can be used to pass the view model when view is made
        /// <summary>
        /// Factory Method For Corresponding ViewModel
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>


        public static VirtualizedCollectionComponentViewModel<T> LoadVirtualizedCollectionComponentViewModel(
            VirtualizationCollection<T> collection,
            ICollectionStore<T>? collectionStore = null
            )
        {
            VirtualizedCollectionComponentViewModel<T> vm = new(collection,collectionStore);
            return (VirtualizedCollectionComponentViewModel<T>) LoadCollectionViewModel(vm);
        }

        public override Task LoadViewModel()
        {
            if (CollectionStore is not null)
                return CollectionStore.Load();
            return _collection?.Load() ?? Task.CompletedTask;
        }
    }
    public class VirtualizedClientsComponentViewModel : VirtualizedCollectionComponentViewModel<Client>
    {
        public VirtualizedClientsComponentViewModel(VirtualizationCollection<Client> virtualizedCollectionComponentViewModel)
            : base(virtualizedCollectionComponentViewModel)
        {

        }
    }
}
