using Module.Events;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismApplicationMavinwoo_Test.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.ViewModels
{
    public class OrderLinesDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IEventAggregator _eventAggregator;
        private List<OrderLinesDialogModel> _orderLineDisplay;

        public List<OrderLinesDialogModel> OrderLineDisplay { get => _orderLineDisplay; set { SetProperty(ref _orderLineDisplay, value); } }

        public OrderLinesDialogViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OrderLinesViewEvent>().Subscribe(GetOrderLines);
            OrderLineDisplay = new List<OrderLinesDialogModel>();
        }

        private void GetOrderLines(List<OrderLinesDialogModel> OrderLines)
        {
            OrderLineDisplay = OrderLines;
        }


        public string Title => "OrderLineDetails";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            return;
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            return;
        }
    }
}
