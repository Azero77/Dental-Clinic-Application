using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Stores
{
    public class MessageStore
    {
		private string _currentMessage = string.Empty;
		public string CurrentMessage
		{
			get
			{
				return _currentMessage;
			}
			set
			{
				_currentMessage = value;
				OnCurrentMessageChanged();
			}
		}

		private MessageType _currentMessageType;
		public MessageType CurrentMessageType
		{
			get
			{
				return _currentMessageType;
			}
			set
			{
				_currentMessageType = value;
				OnCurrentMessageTypeChanged();
			}
		}
		public void ClearMessage()
		{
			CurrentMessage = string.Empty;
		}

		public event Action? CurrentMessageChanged;
		public void OnCurrentMessageChanged()
		{
			CurrentMessageChanged?.Invoke();
		}

        public event Action? CurrentMessageTypeChanged;
        public void OnCurrentMessageTypeChanged()
        {
            CurrentMessageTypeChanged?.Invoke();
        }

    }

	public enum MessageType
	{
		Status = 0,
		Error = 1
	}
}
