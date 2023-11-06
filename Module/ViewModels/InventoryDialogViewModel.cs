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
    public class InventoryDialogViewModel : BindableBase, IDialogAware
    {
        private IDataRepository _dataRepository;
        private string _item;
        private int _reorderLimit;
        private DateTime? _deliveryDate;
        private int? _onOrder;
        private int _inStock;

        public string Item { get => _item; set => _item = value; }
        public int InStock { get => _inStock; set => _inStock = value; }
        public int? OnOrder { get => _onOrder; set => _onOrder = value; }
        public DateTime? DeliveryDate { get => _deliveryDate; set => _deliveryDate = value; }
        public int ReorderLimit { get => _reorderLimit; set => _reorderLimit = value; }

        //private string _message;
        //public string Message
        //{
        //    get { return _message; }
        //    set { SetProperty(ref _message, value); }
        //}

        public InventoryDialogViewModel(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;

        }


        public string Title => "Edit Inventory";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }
        //private void CloseDialog()
        //{
        //    var result = ButtonResult.OK;

        //    var p = new DialogParameters();
        //    p.Add("myParam", "The dialog was closed by the user");

        //    RequestClose.Invoke(new DialogResult(result, p));
        //}
        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            //Message = parameters.GetValue<string>("message");
        }
    }
}
