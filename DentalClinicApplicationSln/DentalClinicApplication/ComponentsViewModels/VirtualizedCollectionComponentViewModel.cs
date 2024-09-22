using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.VirtualizationCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DentalClinicApplication.ComponentsViewModels
{
    public class VirtualizedCollectionComponentViewModel<T> : ViewModelBase
    {
        private VirtualizationCollection<T>? _collection;
        public IEnumerable? Collection
        {
            get => _collection;
            set
            {
                if (value is not VirtualizationCollection<T> d)
                {
                    return;
                }
                _collection = (VirtualizationCollection<T>?)value;
                OnCollectionReset();

            }
        }

        private void OnCollectionReset()
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
            Move = new RelayCommand<int>
                (async (p) => await _collection!.MoveToPage(p),
                (p) => _collection!.CanMoveToPage(p));

            MoveNext = new RelayCommand<int>(async (n) => await _collection!.MoveToPage(VirtualizationCollection<T>.MoveValue.Next),
                                             (p) => _collection!.CanMoveToPage(p, VirtualizationCollection<T>.MoveValue.Next));
            MovePrevious = new RelayCommand<int>(
                async (n) => await _collection!.MoveToPage(VirtualizationCollection<T>.MoveValue.Previous),
                (p) => _collection!.CanMoveToPage(p, VirtualizationCollection<T>.MoveValue.Previous));
            SearchCommand = new SearchCommand<T>(
                new Services.ProviderChangerService<T>(_collection!,_collection!.ItemsProvider));
            ReloadPropertyChanged();
        }

        private void ReloadPropertyChanged()
        {
            OnPropertyChanged(nameof(Collection));
            OnPropertyChanged(nameof(Move));
            OnPropertyChanged(nameof(MoveNext));
            OnPropertyChanged(nameof(MovePrevious));
            OnPropertyChanged(nameof(CurrentPageIndex));
        }

        public ICommand? Move { get; set; }
        public ICommand? MoveNext { get; set; }
        public ICommand? MovePrevious { get; set; }
        public ICommand? SearchCommand { get; set; }
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
        public static VirtualizedCollectionComponentViewModel<T> GetVirtualizedCollectionComponentViewModel(
            IEnumerable collection)
        {
            
            VirtualizedCollectionComponentViewModel<T> vm = new();
            vm.Collection = collection;
            
            return vm;
        }
        
    }
}
