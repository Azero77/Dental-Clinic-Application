using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DentalClinicApplication.ValidationRules
{
    public class BindingErrorValidationRule : ValidationRule
    {
        public Type BindingType { get; set; } = typeof(Int32);
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                //Attempt to convert the value back to the target type
                Convert.ChangeType(value, BindingType);
                return ValidationResult.ValidResult;
            }
            catch (InvalidCastException ex)
            {
                return new ValidationResult(false, $"Invalid value. Error: {ex.Message}");
            }
            catch (FormatException ex)
            {
                return new ValidationResult(false, $"Format error. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, $"Error: {ex.Message}");
            }
        }
    }

}
