using DentalClinicApp.Commands;
using System;
using System.Threading.Tasks;

namespace DentalClinicApplication.Commands
{
    public abstract class AsyncCommandBase : CommandBase
    {
        private Action<Exception>? _onException = null;
        public AsyncCommandBase()
        {
            
        }
        public AsyncCommandBase(Action<Exception> onException)
        {
            _onException = onException;
        }
        private bool _isExecuting;
        private bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                _isExecuting = value;
                OnCanExecuteChanged();
            }
        }
        public override bool CanExecute(object? parameter)
        {
            return !IsExecuting && base.CanExecute(parameter);
        }
        public override async void Execute(object? parameter)
        {
            IsExecuting = true;
            try
            {
                await ExecuteAsync(parameter);

            }
            catch (Exception e)
            {
                if (_onException is not null)
                {
                    _onException.Invoke(e);
                }
                else
                    throw e;
            }
            IsExecuting = false;
        }

        public abstract Task ExecuteAsync(object? parameter);
    }
}