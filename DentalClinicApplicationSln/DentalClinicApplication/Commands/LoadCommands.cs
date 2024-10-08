using DentalClinicApp.Models;
using DentalClinicApplication.Services.DataProvider;
using DentalClinicApplication.Stores;
using DentalClinicApplication.ViewModels;
using DentalClinicApplication.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    public class LoadCommand<T> : AsyncCommandBase
    {
        public CollectionViewModelBase<T> CollectionViewModelBase { get; }
        public LoadCommand(CollectionViewModelBase<T> collectionViewModelBase)
        {
            CollectionViewModelBase = collectionViewModelBase;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                await CollectionViewModelBase.LoadViewModel();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
