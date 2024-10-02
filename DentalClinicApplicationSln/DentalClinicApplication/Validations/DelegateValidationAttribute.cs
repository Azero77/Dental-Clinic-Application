using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Validations
{
    public class DelegateValidationAttribute : ValidationAttribute
    {
        private readonly string _methodName;
        private readonly string _errorMessage;

        public DelegateValidationAttribute(string errorMessage, string methodName)
        {
            _methodName = methodName;
            _errorMessage = errorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Get the object instance that contains the validation method
            var instance = validationContext.ObjectInstance;
            // Use reflection to find the method on the instance
            var methodInfo = instance.GetType().GetMethod(_methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (methodInfo == null)
            {
                throw new InvalidOperationException($"Method {_methodName} not found on {instance.GetType()}");
            }
            var result = methodInfo.Invoke(instance, new object[] { value });

            if (result is bool isValid && isValid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(_errorMessage);
        }
    }

}

