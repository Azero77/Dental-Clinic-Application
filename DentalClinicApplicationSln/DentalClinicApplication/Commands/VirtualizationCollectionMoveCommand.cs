using DentalClinicApplication.ViewModels;
using DentalClinicApplication.VirtualizationCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DentalClinicApplication.Commands
{
    public class VirtualizationCollectionMoveCommand<T>
        : AsyncCommandBase, IDisposable
    {
        private readonly MoveValue _moveValue;

        private readonly VirtualizationCollection<T> _collection;
        public VirtualizationCollectionMoveCommand(
            CollectionViewModelBase<T> collectionViewModel,
            VirtualizationCollection<T> collection,
            MoveValue moveValue = MoveValue.Undefined)
        {
            CollectionViewModel = collectionViewModel;
            _collection = collection;
            _moveValue = moveValue;
            _collection.PageChanged += OnCanExecuteChanged;
        }

        public CollectionViewModelBase<T> CollectionViewModel { get; }

        public override bool CanExecute(object? parameter)
        {
            if (_moveValue == MoveValue.Undefined)
            {
                int pageNumber;
                int.TryParse(parameter?.ToString(), out pageNumber);
                return _collection.CanMoveToPage(pageNumber)
                    && base.CanExecute(null);
            }
            return _collection.CanMoveToPage(_collection.CurrentPageIndex, _moveValue)
                && base.CanExecute(null);
        }

        public void Dispose()
        {
            _collection.PageChanged -= OnCanExecuteChanged;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            int pageNumber;
            CollectionViewModel.IsLoading = true;
            if (int.TryParse(parameter?.ToString(), out pageNumber) && _moveValue == MoveValue.Undefined)
            {
                await _collection.MoveToPage(pageNumber);

            }
            else
            {
                await _collection.MoveToPage(_moveValue);
            }
            CollectionViewModel.IsLoading = false;
            OnCanExecuteChanged();
        }
    }
}
