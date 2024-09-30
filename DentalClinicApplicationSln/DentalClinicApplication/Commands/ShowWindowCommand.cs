using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DentalClinicApplication.Commands
{
    public class ShowWindowCommand<TWindow>
        : AsyncCommandBase
        where TWindow : Window
    {
        public Func<object?, Task<TWindow>> _windowFactory;

        public ShowWindowCommand(Func<object?, Task<TWindow>> windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            Window window = await _windowFactory(parameter);
            window.Show();
        }
    }
}
