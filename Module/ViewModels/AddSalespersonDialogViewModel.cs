using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismApplicationMavinwoo_Test.core.DataAccess;
using System;
using System.Windows;

namespace Module.ViewModels
{

    // Defines the logic for Dialog View that adds a new Salesperson
    public class AddSalespersonDialogViewModel : BindableBase, IDialogAware
    {
        private IDataRepository _dataRepository;

        private string? _name;
        private string? _state;
        private string? _commission;

        public string? Name
        {
            get { return _name; }
            set 
            { 
                SetProperty( ref _name, value);
                RaisePropertyChanged( nameof(Name) );
                AddSalespersonCommand.RaiseCanExecuteChanged();
                
            }
        }

        public string? State
        {
            get { return _state; }
            set 
            { 
                SetProperty(ref _state, value);
                RaisePropertyChanged( nameof(State) );
                AddSalespersonCommand.RaiseCanExecuteChanged();
            }
        }

        public string? Commission
        {
            get { return _commission; }
            set 
            { 
                SetProperty(ref _commission, value); 
                RaisePropertyChanged( nameof(Commission) );
                AddSalespersonCommand.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand AddSalespersonCommand { get; set; }


        public AddSalespersonDialogViewModel(IDataRepository dataRepository) 
        {
            _dataRepository = dataRepository;
            AddSalespersonCommand = new DelegateCommand(AddSalesperson, CanAddSalesperson);
        }

        public void AddSalesperson()
        {
            if(Name != null && State != null && Commission != null)
            {
                decimal commConv = Convert.ToDecimal(Commission);
                if (commConv > 1)
                {
                    MessageBox.Show("Commission must be a decimal representation of a percentage! Example:     '5%'     =     '0.05' ");
                }
                else
                {
                    _dataRepository.AddSalesperson(Name, State, commConv);
                    Name = "";
                    State = "";
                    Commission = "";
                }
            }
        }

        public bool CanAddSalesperson()
        {
            if (Name != null && State != null && Commission != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string Title => "Add Salesperson";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            return;
        }

        private void CloseDialog()
        {
            return;
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            return;
        }
    }
}
