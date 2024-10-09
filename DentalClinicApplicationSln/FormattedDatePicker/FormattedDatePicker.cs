using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace FormattedDatePicker
{
    public class FormattedDatePicker : DatePicker
    {
        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.Register("Format", typeof(string), typeof(FormattedDatePicker),
                new PropertyMetadata("dd/MM/yyyy", OnFormatChanged));

        static FormattedDatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FormattedDatePicker),
                new FrameworkPropertyMetadata(typeof(FormattedDatePicker)));
        }

        public FormattedDatePicker()
        {
            this.SelectedDateChanged += CustomDatePicker_SelectedDateChanged;
        }

        private void CustomDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Format the selected date using the applied format
            ApplyCustomDateFormat();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ApplyCustomDateFormat();
        }

        private static void OnFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = d as FormattedDatePicker;
            datePicker?.ApplyCustomDateFormat();
        }

        private void ApplyCustomDateFormat()
        {
            if (this.SelectedDate.HasValue)
            {
                // Bind the DatePickerTextBox to the SelectedDate and apply the desired format
                var binding = new Binding("SelectedDate")
                {
                    Source = this,
                    StringFormat = this.Format,
                    ConverterCulture = CultureInfo.InvariantCulture
                };

                var datePickerTextBox = GetTemplateChild("PART_TextBox") as DatePickerTextBox;
                if (datePickerTextBox != null)
                {
                    BindingOperations.SetBinding(datePickerTextBox, TextBox.TextProperty, binding);
                }
            }
        }
    }
}
