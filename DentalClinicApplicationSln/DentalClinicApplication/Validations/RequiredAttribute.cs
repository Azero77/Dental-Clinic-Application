using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Validations
{
    /// <summary>
    /// Checks if the value is null or empty (if string)
    /// </summary>
    public class RequiredAttribute : ValidationAttribute
    {

        public RequiredAttribute(string errorMessage) : base(errorMessage)
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
