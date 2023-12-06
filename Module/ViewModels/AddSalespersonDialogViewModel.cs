using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismApplicationMavinwoo_Test.core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.ViewModels
{
    public class AddSalespersonDialogViewModel : BindableBase, IDialogAware
    {
        private IDataRepository _dataRepository;
        private IDialogService _dialogService;
        

        public AddSalespersonDialogViewModel(IDataRepository dataRepository, IDialogService dialogService) 
        {
            _dataRepository = dataRepository;
            _dialogService = dialogService;
            
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
            var result = ButtonResult.OK;

            var p = new DialogParameters();
            p.Add("myParam", "The dialog was closed by the user");

            RequestClose.Invoke(new DialogResult(result, p));
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            //Message = parameters.GetValue<string>("message");
        }
    }
}
