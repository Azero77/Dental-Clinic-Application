using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    public class AsyncRelayCommand<T> : AsyncCommandBase
    {
        private Func<T?, bool> _canExecute;
        private Func<T?,Task> _execute;

        public AsyncRelayCommand(
            Func<T?,Task> execute,
            Func<T?, bool> canExecute
            )
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public override bool CanExecute(object? parameter)
        {
            return (_canExecute is null || _canExecute(parameter is null ? default(T) : (T)parameter) )
                && base.CanExecute(parameter);
            
        }
        public override Task ExecuteAsync(object? parameter)
        {
            return _execute(parameter is not null ? (T) parameter : default);
        }
    }
}
