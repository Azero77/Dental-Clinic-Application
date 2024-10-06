using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PlaceHolderTextControl
{
    public class PlaceHolderTextBox : TextBox
    {


        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Placeholder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(PlaceHolderTextBox), new PropertyMetadata(string.Empty));




        // Using a DependencyProperty as the backing store for IsEmpty.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey IsEmptyPropertyKey =
            DependencyProperty.RegisterReadOnly("IsEmpty", typeof(bool), typeof(PlaceHolderTextBox), new PropertyMetadata(true));

        public static readonly DependencyProperty IsEmptyProperty = IsEmptyPropertyKey.DependencyProperty;

        public bool IsEmpty
        {
            get { return (bool)GetValue(IsEmptyProperty); }
            private set { SetValue(IsEmptyPropertyKey, value); }
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            IsEmpty = string.IsNullOrEmpty(Text);

        }
        static PlaceHolderTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceHolderTextBox),
                new FrameworkPropertyMetadata(typeof(PlaceHolderTextBox)));
        }

    }
}
