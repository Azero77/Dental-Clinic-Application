using DentalClinicApp.ViewModels;
using DentalClinicApplication.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace DentalClinicApplication.ViewModels
{
    public abstract class ErrorViewModelBase : ViewModelBase,INotifyDataErrorInfo
    {
        public ErrorViewModelBase()
        {
			ValidationErrorCommand = new RelayCommand<ValidationErrorEventArgs>(OnValidatoinError);
        }

        public bool HasErrors => _errors.Count > 0;
        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return _errors.Values.Select(p => p).ToList();
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName]!;
            return Enumerable.Empty<string>();
        }
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public Dictionary<string, List<string>?> _errors = new();
        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        public void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors.Add(propertyName, new List<string>());
            _errors[propertyName]?.Add(error);
            OnErrorsChanged(propertyName);
        }

        public void ClearErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                _errors.Remove(propertyName);
            OnErrorsChanged(propertyName);

        }


        protected bool Validate(string propertyName)
        {
            //get attributes
            //validate each one
            PropertyInfo? propertyInfo = this.GetType().GetProperty(propertyName);
            IEnumerable<ValidationAttribute>? attributes =
                propertyInfo?
                .GetCustomAttributes<ValidationAttribute>(true);
            if (attributes is not null)
            {
                ClearErrors(propertyInfo!.Name);
                foreach (ValidationAttribute validationAttribute in attributes)
                {
                    System.ComponentModel.DataAnnotations.ValidationResult? result = validationAttribute.GetValidationResult(propertyInfo!.GetValue(this), 
                        new ValidationContext(this));
                    if (result != System.ComponentModel.DataAnnotations.ValidationResult.Success)
                    {
                        string errorMessage = result?.ErrorMessage
                                ?? $"{propertyInfo.Name} is not valid";
                        
                        AddError(propertyInfo!.Name, errorMessage);
                        return false;
                    }
                }
                
            }
            return true;
                
        }
        public bool IsModelValid => !HasErrors
            && !GetType().GetProperties().Where(p => p.Name != "IsModelValid").Any(p => p.GetValue(this) is null
            || string.IsNullOrEmpty(p.GetValue(this)?.ToString()));

        public override void OnPropertyChanged(string propertyName)
        {
            Validate(propertyName);
            base.OnPropertyChanged(propertyName);
            base.OnPropertyChanged(nameof(HasErrors));
            base.OnPropertyChanged(nameof(IsModelValid));
        }

        /// <summary>
        /// Banded To Behavior in the view and listens to the view Validation.Error event
        /// through built in behavior
        /// </summary>

        public ICommand ValidationErrorCommand { get; }
        public void OnValidatoinError(ValidationErrorEventArgs validationErrorEventArgs)
        {
           if (validationErrorEventArgs.Action == ValidationErrorEventAction.Added)
            {
                BindingExpression? Binding =
               validationErrorEventArgs.Error.BindingInError as BindingExpression;

                if (Binding is not null)
                {
                    string propertyName = Binding.ResolvedSourcePropertyName;
                    if (_errors.ContainsKey(propertyName)
                        && _errors[propertyName]?[0] == validationErrorEventArgs.Error.ErrorContent.ToString())
                    {
                        return;
                    }
                    else
                    {
                        validationErrorEventArgs.Error.ErrorContent = null;
                        string errorMessage = $"{propertyName} is not valid";
                        AddError(propertyName, errorMessage);
                    }
                }
            }
        }
    }
}
