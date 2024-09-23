using AutoMapper.Configuration.Conventions;
using DentalClinicApp.ViewModels;
using DentalClinicApplication.Services;
using DentalClinicApplication.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.ComponentsViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        public MessageStore MessageStore { get; }
        public string Message => MessageStore.CurrentMessage;
        public MessageType MessageType => MessageStore.CurrentMessageType;

        public bool HasMessage => !string.IsNullOrEmpty(Message);

        public MessageViewModel(MessageStore messageStore)
        {
            MessageStore = messageStore;
            MessageStore.CurrentMessageChanged += OnCurrentMessageChanged;
            MessageStore.CurrentMessageTypeChanged += OnCurrentMessageTypeChanged;
        }

        private void OnCurrentMessageTypeChanged()
        {
            OnPropertyChanged(nameof(MessageType));
            OnPropertyChanged(nameof(HasMessage));
        }

        private void OnCurrentMessageChanged()
        {
            OnPropertyChanged(nameof(Message));
        }
    }
}
