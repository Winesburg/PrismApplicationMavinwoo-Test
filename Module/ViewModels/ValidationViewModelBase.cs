using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Module.ViewModels
{
    public class ValidationViewModelBase : BindableBase, INotifyDataErrorInfo
    {
        // Made readonly because it was initialized directly
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new();
        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            return propertyName is not null && _errorsByPropertyName.ContainsKey(propertyName)
            ? _errorsByPropertyName[propertyName]
            : Enumerable.Empty<string>();
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs args) 
        {
            // '?' checks if null
            ErrorsChanged?.Invoke(this, args);
        }

        protected void AddError(string error, string propertyName)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName)) 
            {
                _errorsByPropertyName[propertyName] = new List<string>();
            }
            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
                RaisePropertyChanged(nameof(HasErrors));
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
                RaisePropertyChanged(nameof(HasErrors));
            }
        }
    }
}
