using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Virtualization.Providers;
#pragma warning  disable CS4014
namespace Virtualization.VirtualizationCollections
{
    public class VirtualizationCollection<T>
        : INotifyCollectionChanged,
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

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < PageSize; i++)
            {
                yield return _pages[CurrentPageIndex][i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region properties
        public IVirtualizationItemsProvider<T> ItemsProvider {get;}
        public int PageSize { get; } = 20;
        public int PageTimeout { get; } = 2000;
        public int Count { get; set; } = -1;
        public int CurrentPageIndex { get; set; } = 0;

        Dictionary<int, IList<T>> _pages = new();
        Dictionary<int, DateTime> _pagesTimeout = new();

        #endregion

        #region Virtualization Logic
        public async Task RenderPage(int pageIndex)
        {
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
        public void MoveToPage(int pageIndex)
        {
            CurrentPageIndex = pageIndex;
        }
        public void MoveToPage(MoveValue moveValue)
        {
            if (moveValue == MoveValue.Next)
                CurrentPageIndex = CurrentPageIndex + 1;
            CurrentPageIndex = CurrentPageIndex - 1;
        }

        public enum MoveValue
        {
            Next,
            Previous
        }
        #endregion
        
    }
}
#pragma warning restore
