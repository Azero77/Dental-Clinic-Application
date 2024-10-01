using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Validations
{
    public class RangeAttribute : ValidationAttribute
    {
        private readonly int minValue;
        private readonly int maxValue;
        public RangeAttribute(int minValue, int maxValue,
            string errorMessage) : base(errorMessage)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public RangeAttribute(int minValue,
            int maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            ErrorMessage = $"Value should be in range between {minValue} and {maxValue}";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            int number;
            if (!int.TryParse(value?.ToString(), out number))
            {
                return new ValidationResult("Value Should be a number");
            }
            if (number <= maxValue && number >= minValue)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
    }

    public class DateTimeRangeAttribue : ValidationAttribute
    {
        private readonly DateTime minValue;
        private readonly DateTime maxValue;
        public DateTimeRangeAttribue(DateTime minValue, DateTime maxValue,
            string errorMessage) : base(errorMessage)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public DateTimeRangeAttribue(DateTime minValue,
            DateTime maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            ErrorMessage = $"Value should be in range between {minValue} and {maxValue}";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DateTime number;
            if (!DateTime.TryParse(value?.ToString(), out number))
            {
                return new ValidationResult("Value Should be a number");
            }
            if (number <= maxValue && number >= minValue)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}
