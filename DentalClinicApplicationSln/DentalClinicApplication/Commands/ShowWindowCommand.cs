using DentalClinicApp.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DentalClinicApplication.Commands
{
    public class ShowWindowCommand<TWindow>
        : CommandBase
        where TWindow : Window
    {
        public Func<object?,TWindow> _windowFactory;

        public ShowWindowCommand(Func<object?, TWindow> windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public override void Execute(object? parameter)
        {
            Window window = _windowFactory(parameter);
            window.Show();
        }
    }
}
