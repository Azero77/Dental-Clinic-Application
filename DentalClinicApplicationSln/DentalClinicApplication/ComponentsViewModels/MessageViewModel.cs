using AutoMapper.Configuration.Conventions;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using DentalClinicApplication.Services;
using DentalClinicApplication.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace DentalClinicApplication.ComponentsViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        public MessageStore MessageStore { get; }
        public string Message => MessageStore.CurrentMessage;
        public MessageType MessageType => MessageStore.CurrentMessageType;
        public bool HasMessage => !string.IsNullOrEmpty(Message);
        public ICommand CloseCommand { get; }
        public MessageViewModel(MessageStore messageStore)
        {
            MessageStore = messageStore;
            MessageStore.CurrentMessageChanged += OnCurrentMessageChanged;
            MessageStore.CurrentMessageTypeChanged += OnCurrentMessageTypeChanged;
            CloseCommand = new RelayCommand<object>(
                (o) => MessageStore.ClearMessage()
                );
        }

        private void OnCurrentMessageTypeChanged()
        {
            OnPropertyChanged(nameof(MessageType));
        }

        private void OnCurrentMessageChanged()
        {
            OnPropertyChanged(nameof(Message));
        }
    }
}
