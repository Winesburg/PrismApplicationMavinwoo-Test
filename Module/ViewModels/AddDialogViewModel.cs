using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismApplicationMavinwoo_Test.core.DataAccess;
using PrismApplicationMavinwoo_Test.core.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Module.ViewModels
{
    public class AddDialogViewModel : BindableBase, IDialogAware
    {
        private IDataRepository _dataRepository;
        private string _message;
        private string _name;
        private string _address;
        private string _city;
        private string _state;
        private string _zip;
        private string _phone;
        private ObservableCollection<CustomerAddDialogModel> _addCust;
        public string Name { get => _name; set => _name = value; }
        public string Address { get => _address; set => _address = value; }
        public string City { get => _city; set => _city = value; }
        public string State { get => _state; set => _state = value; }
        public string Zip { get => _zip; set => _zip = value; }
        public string Phone { get => _phone; set => _phone = value; }
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        public DelegateCommand CloseDialogCommand { get; }
        public DelegateCommand AddCustomerCommand { get; }
        public ObservableCollection<CustomerAddDialogModel> AddCust
        {
            get { return _addCust; }
            set { SetProperty(ref _addCust, value); }
        }
        public AddDialogViewModel(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
            CloseDialogCommand = new DelegateCommand(CloseDialog);
            AddCustomerCommand = new DelegateCommand(AddCustomer);

        }

        private void AddCustomer()
        {
            // Add data validation
            if (Name != null && Address != null && City != null && State != null && Zip != null && Phone != null) 
            { 
                if (Phone.Length > 10 && Zip.Length > 5)
                { 
                    _dataRepository.AddCustomers(Name, Address, City, State, Zip, Phone);
                }
            }
            else
            {
                MessageBox.Show("Invalid Input", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseDialog()
        {
            var result = ButtonResult.OK;

            var p = new DialogParameters();
            p.Add("myParam", "The dialog was closed by the user");

            RequestClose.Invoke(new DialogResult(result, p));
        }
        
        public string Title => "Edit Customer";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("message");
        }
    }
}
