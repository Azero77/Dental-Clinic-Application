using HeaderedTextBox.Behaviors;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HeaderedTextBox
{
    public class HeaderedTextBox : TextBox
    {


        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(HeaderedTextBox), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ValidationErrorCommandProperty =
           DependencyProperty.Register("ValidationErrorCommand", typeof(ICommand), typeof(HeaderedTextBox), new PropertyMetadata(null, OnValidationErrorCommandChanged));

        public ICommand ValidationErrorCommand
        {
            get { return (ICommand)GetValue(ValidationErrorCommandProperty); }
            set { SetValue(ValidationErrorCommandProperty, value); }
        }


        static HeaderedTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderedTextBox), new FrameworkPropertyMetadata(typeof(HeaderedTextBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            AttachValidationBehavior();
        }

        // When the ValidationErrorCommand changes, update the behavior
        private static void OnValidationErrorCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as HeaderedTextBox;
            control?.AttachValidationBehavior();
        }

        // Method to attach the behavior programmatically
        private void AttachValidationBehavior()
        {
            if (ValidationErrorCommand != null)
            {
                var behaviors = Interaction.GetBehaviors(this);

                // Remove existing behavior if already added
                var existingBehavior = behaviors.OfType<ValidationErrorBehavior>().FirstOrDefault();
                if (existingBehavior != null)
                {
                    behaviors.Remove(existingBehavior);
                }

                // Add new behavior
                behaviors.Add(new ValidationErrorBehavior { Command = ValidationErrorCommand });
            }
        }
    }
}
