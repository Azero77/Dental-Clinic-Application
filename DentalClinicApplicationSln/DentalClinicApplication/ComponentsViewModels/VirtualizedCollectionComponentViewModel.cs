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
        public VirtualizationCollection<T> _collection;
        
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
            _collection.CollectionChanged += OnCollectionChanged;
            CollectionStore = collectionStore;
            if(CollectionStore is not null)
                CollectionStore.CollectionChanged += OnCollectionReset;
        }

        #region events
        public void OnCollectionReset()
        {
            if (_collection is not null)
            {
                _collection.CollectionChanged += OnCollectionChanged;
                _collection.PropertyChanged += OnPropertyChanged;
                ReloadButtons();
            }
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PagesIndexers));
            OnPropertyChanged(e?.PropertyName ?? string.Empty);
        }

        private void OnCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_collection is null)
                return;
            ReloadButtons();
        }

        private void ReloadButtons()
        {
            Move = new AsyncRelayCommand<int>
                ((p) => MoveCommandDelegate(p),
                (p) => _collection!.CanMoveToPage(p));

            MoveNext = new AsyncRelayCommand<int>((p) => MoveCommandDelegate(VirtualizationCollection<T>.MoveValue.Next),
                (p) => _collection!.CanMoveToPage(p));
            MovePrevious = new AsyncRelayCommand<int>(
               (p) => MoveCommandDelegate(VirtualizationCollection<T>.MoveValue.Previous),
                (p) => _collection!.CanMoveToPage(p, VirtualizationCollection<T>.MoveValue.Previous));
            SearchCommand = new SearchCommand<T>(
                new ProviderChangerService<T>(this, _collection!.ItemsProvider,ChangeMode.Search),this);
            OrderCommand = new SearchCommand<T>(
                new ProviderChangerService<T>(this,_collection.ItemsProvider,ChangeMode.Order),this);
            ReloadPropertyChanged();
        }

        private async Task MoveCommandDelegate(
            VirtualizationCollection<T>.MoveValue moveValue)
        {
            IsLoading = true;
            await _collection!.MoveToPage(moveValue);
            IsLoading = false;
        }
        private async Task MoveCommandDelegate(
            int pageNumber)
        {
            IsLoading = true;
            await _collection!.MoveToPage(pageNumber);
            IsLoading = false;
        }
        #endregion

        private void ReloadPropertyChanged()
        {
            OnPropertyChanged(nameof(Collection));
            OnPropertyChanged(nameof(Move));
            OnPropertyChanged(nameof(MoveNext));
            OnPropertyChanged(nameof(MovePrevious));
            OnPropertyChanged(nameof(CurrentPageIndex));
        }
        #region commands
        public ICommand? Move { get; set; }
        public ICommand? MoveNext { get; set; }
        public ICommand? MovePrevious { get; set; }
        public ICommand? SearchCommand { get; set; }
        public ICommand? OrderCommand { get; set; }

        private int? _currentPageIndex;
        public int? CurrentPageIndex
        {
            get => _collection?.CurrentPageIndex ?? _currentPageIndex;
            set
            {
                if (_collection != null)
                {
                    // Update _collection's CurrentPageIndex
                    _collection.CurrentPageIndex = value ?? _collection.CurrentPageIndex;
                }
                _currentPageIndex = value;
                OnPropertyChanged(nameof(CurrentPageIndex));
            }
        }
        private int? _pageSize;
        public int? PageSize
        {
            get => _collection?.PageSize ?? _pageSize;
            set
            {
                if (_collection != null)
                {
                    // Update _collection's PageSize if needed
                    _collection.PageSize = value ?? _collection.PageSize;
                }
                _pageSize = value;
                OnPropertyChanged(nameof(PageSize));
            }
        }
        public int? PagesCount => _collection?.PagesCount;
        public IEnumerable<string> Properties => typeof(T).GetProperties().Select(p => p.Name);
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


        public static VirtualizedCollectionComponentViewModel<T> LoadVirtualizedCollectionComponentViewModel<TVM>(
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
