using DentalClinicApplication.Services.DataProvider;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#pragma warning  disable CS4014
namespace DentalClinicApplication.VirtualizationCollections
{
    public class VirtualizationCollection<T>
        : INotifyCollectionChanged,
        INotifyPropertyChanged,
        IEnumerable<T>
    {
        public VirtualizationCollection(IVirtualizationItemsProvider<T> itemsProvider, int pageSize, int pageTimeout)
        {
            ItemsProvider = itemsProvider;
            PageSize = pageSize;
            PageTimeout = pageTimeout;
        }


        public VirtualizationCollection(IVirtualizationItemsProvider<T> itemsProvider, int pageSize)
        {
            ItemsProvider = itemsProvider;
            PageSize = pageSize;

        }
        public VirtualizationCollection(IVirtualizationItemsProvider<T> itemsProvider)
        {
            ItemsProvider = itemsProvider;

        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            CollectionChanged?.Invoke(this,new NotifyCollectionChangedEventArgs(action));
        }
        public void OnCollectionReset()
        {
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (!_pages.ContainsKey(CurrentPageIndex))
                yield break;
            
            for (int i = 0; i < _pages[CurrentPageIndex].Count; i++)
            {
                yield return _pages[CurrentPageIndex][i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region properties
        public IVirtualizationItemsProvider<T> ItemsProvider { get; private set; }
        public async Task ChangeProvider(IVirtualizationItemsProvider<T> newProvider)
        {
            ItemsProvider = newProvider;
            CurrentPageIndex = 0;
            _pages = new();
            _pagesTimeout = new();
            await Load();
        }
        private int _pageSize = 20;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
                OnPropertyChanged(nameof(PagesCount));
                OnPropertyChanged(nameof(PageSize));
                PageSizeChanged();
            }
        }
        private int _pageTimeout = 2000;
        public int PageTimeout
        {
            get
            {
                return _pageTimeout;
            }
            set
            {
                _pageTimeout = value;
                OnPropertyChanged(nameof(PageTimeout));
            }
        }
        private int _count = 0;
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                OnPropertyChanged(nameof(PagesCount));
                OnPropertyChanged(nameof(Count));
            }
        }
        private int _currentPageIndex = 0;
        public int CurrentPageIndex
        {
            get
            {
                return _currentPageIndex;
            }
            set
            {
                _currentPageIndex = value;
                OnPropertyChanged(nameof(CurrentPageIndex));
            }
        }

        public int PagesCount => (Count % PageSize == 0) ? Count / PageSize : (Count / PageSize) + 1;
        Dictionary<int, IList<T>> _pages = new();
        Dictionary<int, DateTime> _pagesTimeout = new();

        #endregion

        #region Virtualization Logic
        public async Task RenderPage(int pageIndex)
        {
            await Task.Delay(1000);
            if (!_pages.ContainsKey(pageIndex))
            {
                _pages[pageIndex] = null;
                _pagesTimeout.Add(pageIndex, DateTime.Now);
            }
            else
            {
                _pagesTimeout[pageIndex] = DateTime.Now;
            }
            int quoetint = (Count % PageSize == 0) ? 0 : 1;
            if (pageIndex != (Count / PageSize) + quoetint)
                LoadPage(pageIndex + 1).ConfigureAwait(false);
            else if (pageIndex != 0)
                LoadPage(pageIndex - 1).ConfigureAwait(false);
            await LoadPage(pageIndex);
            CleanUpPages();

        }

        public async Task LoadCount()
        {
            Count = await ItemsProvider.FetchCount();
            OnPropertyChanged(nameof(PagesCount));
        }

        public async Task LoadPage(int pageIndex)
        {
            PopulatePage(pageIndex, await ItemsProvider.FetchRange(pageIndex * PageSize,
                PageSize));
        }

        private void PopulatePage(int pageIndex, IList<T> page)
        {
            if (_pages.ContainsKey(pageIndex))
            {
                _pages[pageIndex] = page;
            }
        }

        private void CleanUpPages()
        {
            foreach (int pageIndex in _pagesTimeout.Keys)
            {
                if (
                    pageIndex != 0 &&
                    (_pagesTimeout[pageIndex] - DateTime.Now).TotalMilliseconds > PageTimeout)
                {
                    _pages.Remove(pageIndex);
                    _pagesTimeout.Remove(pageIndex);
                }
            }
        }

        #endregion

        #region Virtualization View
        public async Task MoveToPage(int pageIndex)
        {
            CurrentPageIndex = pageIndex;
            await RenderPage(pageIndex);
            OnCollectionReset();
        }
        public async Task MoveToPage(MoveValue moveValue)
        {
            if (moveValue == MoveValue.Next)
                await MoveToPage(CurrentPageIndex + 1);
            else
                await MoveToPage(CurrentPageIndex - 1);
        }

        public enum MoveValue
        {
            Next,
            Previous,
            Undefined
        }
        public bool CanMoveToPage(int newPageNumber, MoveValue moveValue = MoveValue.Undefined)
        {

            if (moveValue == MoveValue.Next)
            {
                return newPageNumber < PagesCount - 1;
            }
            else if (moveValue == MoveValue.Previous)
            {
                return newPageNumber > 0;
            }
            return true;
        }
        #endregion
        #region LazyLoading
        public async Task Load()
        {
            await LoadCount();
            await RenderPage(CurrentPageIndex);
            OnPropertyChanged(string.Empty);
            OnCollectionReset();
        }

        public async Task PageSizeChanged()
        {
            if (CurrentPageIndex > PagesCount)
                CurrentPageIndex = 0;
            await RenderPage(CurrentPageIndex);
            OnCollectionReset();
        }
        #endregion
    }
}
#pragma warning restore
