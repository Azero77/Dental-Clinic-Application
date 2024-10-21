using DentalClinicApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Stores
{
    public class ModalNavigationStore
    {
		private ViewModelBase? _currentModal = null;
		public ViewModelBase? CurrentModal
		{
			get
			{
				return _currentModal;
			}
			set
			{
				_currentModal = value;
				OnCurrentModalChanged();
			}
		}
		public bool IsOpen => CurrentModal is not null;
		public void Close()
		{
			CurrentModal = null;
		}

		public event Action? CurrentModalChanged;
		public void OnCurrentModalChanged()
		{
			CurrentModalChanged?.Invoke();
		}

	}
}
