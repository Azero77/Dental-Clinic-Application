using DentalClinicApp.Models;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.ViewModels
{
    public class ClientProfileViewModel : ViewModelBase
    {
        private int _id;
        private Client? client;
        public Client? Client
        {
            get
            {
                return client;
            }
            set
            {
                client = value;
                OnPropertyChanged(nameof(Client));
            }
        }
        private bool _isLoading = false;
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

        public IProvider<Client> ClientProvider { get; }

        public ClientProfileViewModel(IProvider<Client> clientProvider,int id)
        {
            ClientProvider = clientProvider;
            _id = id;
        }

        public async Task Load()
        {
            IsLoading = true;
            Client = await ClientProvider.GetItem(_id);
            IsLoading = false;

        }
        public static ClientProfileViewModel LoadClientProfileViewModel(
            IProvider<Client> provider,
            int id)
        {
            ClientProfileViewModel viewModel = new(provider,id);
            viewModel.Load().ConfigureAwait(false);
            return viewModel;
        }
    }
}
