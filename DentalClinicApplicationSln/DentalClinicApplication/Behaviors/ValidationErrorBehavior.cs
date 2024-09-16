using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DentalClinicApplication.Behaviors
{
    public class ValidationErrorBehavior : Behavior<FrameworkElement>
    {
        // Attach the behavior to the element and hook into the Validation.Error event
        protected override void OnAttached()
        {
            base.OnAttached();
            Validation.AddErrorHandler(AssociatedObject, OnValidationError!);
        }

        // Detach the behavior from the element and remove the event handler
        protected override void OnDetaching()
        {
            base.OnDetaching();
            Validation.RemoveErrorHandler(AssociatedObject, OnValidationError!);
        }

        // Event handler for Validation.Error
        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            // Call the method in your ViewModel, passing the event args
            if (Command != null && Command.CanExecute(e))
            {
                Command.Execute(e);
            }
        }

        // Define a Command property to bind to the ViewModel
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(System.Windows.Input.ICommand), typeof(ValidationErrorBehavior));

        public System.Windows.Input.ICommand Command
        {
            get { return (System.Windows.Input.ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
    }
}
