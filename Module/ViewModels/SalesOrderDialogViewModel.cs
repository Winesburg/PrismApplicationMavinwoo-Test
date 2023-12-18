using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismApplicationMavinwoo_Test.core.DataAccess;
using PrismApplicationMavinwoo_Test.core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Module.ViewModels
{
    public class SalesOrderDialogViewModel : BindableBase, IDialogAware, INotifyDataErrorInfo
    {
        private IDataRepository _dataRepository;
        private IDialogService _dialogService;
        private ObservableCollection<InventoryAddDialogModel> _listOfInv;
        private InventoryAddDialogModel _selItem;
        private ObservableCollection<CompletedSalesOrderModel> _salesOrder;
        private string? _itemDateConn;
        private string _itemSalespersonConn;
        private string _itemCustomerConn;
        private decimal? _itemPriceConn;
        private int? _itemQuantityConn;
        private ObservableCollection<string> _salesHeader_Date;
        private ObservableCollection<string> _salesHeader_Salesperson;
        private ObservableCollection<string> _salesHeader_Customer;
        private int _addSalesTrigger;


        public int AddSalesTrigger 
        { 
            get => _addSalesTrigger;
            set 
            { 
                SetProperty(ref _addSalesTrigger, value);
                RaisePropertyChanged(nameof(_addSalesTrigger));
                SubmitSalesOrderCommand.RaiseCanExecuteChanged();
                ClearSalesOrderCommand.RaiseCanExecuteChanged();
                DeleteOrderLineCommand.RaiseCanExecuteChanged();
            } 
        }
        public string? ItemDateConn { get => _itemDateConn; set { SetProperty(ref _itemDateConn, value); } }
        public string ItemSalespersonConn { get => _itemSalespersonConn; set { SetProperty(ref _itemSalespersonConn, value); } }
        public string ItemCustomerConn { get => _itemCustomerConn; set { SetProperty(ref _itemCustomerConn, value); } }
        public decimal? ItemPriceConn 
        { 
            get => _itemPriceConn; 
            set 
            { 
                SetProperty(ref _itemPriceConn, value);
                RaisePropertyChanged(nameof(_itemPriceConn));
                AddSalesLineCommand.RaiseCanExecuteChanged();
            } 
        }
        public int? ItemQuantityConn 
        { 
            get => _itemQuantityConn; 
            set 
            { 
                SetProperty(ref _itemQuantityConn, value);
                RaisePropertyChanged(nameof(_itemQuantityConn));
                AddSalesLineCommand.RaiseCanExecuteChanged();
            } 
        }

        public ObservableCollection<string> SalesHeader_Date 
        { 
            get => _salesHeader_Date; 
            set 
            {
                SetProperty(ref _salesHeader_Date, value);
                RaisePropertyChanged(nameof(SalesHeader_Date));
            }
        }

        public ObservableCollection<string> SalesHeader_Salesperson { get => _salesHeader_Salesperson; set { SetProperty(ref _salesHeader_Salesperson, value); } }
        public ObservableCollection<string> SalesHeader_Customer { get => _salesHeader_Customer; set { SetProperty(ref _salesHeader_Customer, value); } }
        public ObservableCollection<CompletedSalesOrderModel> SalesOrder 
        { 
            get => _salesOrder; 
            set 
            { 
                SetProperty(ref _salesOrder, value);
                RaisePropertyChanged(nameof(SalesOrder));
            } 
        }
        public InventoryAddDialogModel SelItem { get => _selItem; set { SetProperty(ref _selItem, value); } }
        public ObservableCollection<InventoryAddDialogModel> ListOfInv { get => _listOfInv; set { SetProperty(ref _listOfInv, value); } }
        public DelegateCommand NewCustomerCommand { get; set; }
        public DelegateCommand TestCommand { get; set; }
        public DelegateCommand AddSalesLineCommand { get; set; }
        public DelegateCommand OpenSalespersonCommand { get; set; }
        public DelegateCommand SubmitSalesOrderCommand { get; set; }
        public DelegateCommand DeleteOrderLineCommand {  get; set; }
        public DelegateCommand ClearSalesOrderCommand { get; set; }

        public SalesOrderDialogViewModel(IDataRepository dataRepository, IDialogService dialogService)
        {
            _dataRepository = dataRepository;
            _dialogService = dialogService;
            ListOfInv = new ObservableCollection<InventoryAddDialogModel>();
            SalesOrder = new ObservableCollection<CompletedSalesOrderModel>();
            SalesHeader_Date = new ObservableCollection<string>();
            SalesHeader_Salesperson = new ObservableCollection<string>();
            SalesHeader_Customer = new ObservableCollection<string>();
            NewCustomerCommand = new DelegateCommand(ShowAddCustomerDialog);
            AddSalesLineCommand = new DelegateCommand(AddSalesLine);
            OpenSalespersonCommand = new DelegateCommand(ShowAddSalespersonDialog);
            SubmitSalesOrderCommand = new DelegateCommand(SubmitSalesOrder, CanClickSalesOrder);
            ClearSalesOrderCommand = new DelegateCommand(ClearSalesOrder, CanClickSalesOrder);
            DeleteOrderLineCommand = new DelegateCommand(DeleteOrderLine, CanClickSalesOrder);
            _propertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
            GenerateInvList();
            FormatInputs();
        }

        private void ClearSalesOrder()
        {
            SalesHeader_Date.Clear();
            SalesHeader_Customer.Clear();
            SalesHeader_Salesperson.Clear();
            SalesOrder.Clear();
            AddSalesTrigger = 0;
            CanClickSalesOrder();
        }

        private void FormatInputs()
        {
            if (ItemPriceConn == 0)
            {
                ItemPriceConn = null;
            }
            if (ItemQuantityConn == 0)
            {
                ItemQuantityConn = null;
            }
            
        }

        private void AddSalesLine()
        {
                if (ItemDateConn != null && ItemPriceConn != null && ItemQuantityConn != null && SelItem != null)
                {
                    string item_date = ItemDateConn.Replace(" 12:00:00 AM", "");
                    string ItemSalesperson = ItemSalespersonConn.Replace("System.Windows.Controls.ComboBoxItem: ", "");
                    string item_customer = ItemCustomerConn.Replace("System.Windows.Controls.ComboBoxItem: ", "");
                    string item_item = SelItem.Item;
                    decimal? item_price = ItemPriceConn;
                    int? item_quantity = ItemQuantityConn;
                    SalesOrder.Add(new CompletedSalesOrderModel(item_date, ItemSalesperson, item_customer, item_item, item_price, item_quantity));

                    if(SalesOrder.Count > 1)
                    {
                        for (int i = 1;  i < SalesOrder.Count; i++)
                        {
                            if (SalesOrder[i].Item == SalesOrder[0].Item && SalesOrder[i].Price == SalesOrder[0].Price && SalesOrder[i].Quantity == SalesOrder[0].Quantity && SalesOrder[i].Salesperson == SalesOrder[0].Salesperson)
                            {
                                SalesOrder.RemoveAt(0);
                                MessageBox.Show("Cannot enter identical information");
                            }
                        }
                    }

                    if (SalesHeader_Date.Count == 0 && SalesHeader_Customer.Count == 0 && SalesHeader_Salesperson.Count == 0)
                    {
                        SalesHeader_Date.Add(SalesOrder[0].Date_Sold);
                        SalesHeader_Customer.Add(SalesOrder[0].Customer);
                        SalesHeader_Salesperson.Add(SalesOrder[0].Salesperson);
                    }

                    AddSalesTrigger = 1;
                    ItemPriceConn = null;
                    ItemQuantityConn = null;
                    SelItem = null;



                }
                else if (ItemDateConn == null || ItemPriceConn == null || ItemQuantityConn == null || SelItem == null)
                {
                    MessageBox.Show("Invalid Input: Make sure all input fields are complete!");
                }
        }

        private void DeleteOrderLine()
        {
            for (int i = 0; i < SalesOrder.Count; i++)
            {
                if (SalesOrder[i].IsClicked == true)
                {
                    SalesOrder.RemoveAt(i);
                }
            }
            if (SalesOrder.Count == 0)
            {
                SalesOrder.Clear();
                CanClickSalesOrder();
            }

        }

        // Figure this out later
        private bool CanClickSalesOrder()
        {
            if(SalesOrder.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void SubmitSalesOrder()
        {
            SalesHeader_Date.Clear();
            SalesHeader_Customer.Clear();
            SalesHeader_Salesperson.Clear();
            AddSalesTrigger = 0;
            if (SalesOrder.Count > 0)
            {
                List<string> IncorrectQuantity = new List<string>();
                string display = "";
                for (int i = 0; i < SalesOrder.Count; i++)
                {
                    int currentStock = _dataRepository.GetCurrentStock(SalesOrder[i].Item);
                    
                    if(currentStock < SalesOrder[i].Quantity)
                    {
                        IncorrectQuantity.Add(SalesOrder[i].Item);
                    }
                    else
                    {
                        _dataRepository.SetStock((currentStock - SalesOrder[i].Quantity), SalesOrder[i].Item);
                    }
                }
                if (IncorrectQuantity.Count > 0)
                {
                    for (int i = 0; i < IncorrectQuantity.Count; i++)
                    {
                        display += ($"{IncorrectQuantity[i]} \n");
                    }
                    MessageBoxResult result =  MessageBox.Show($"The following items do not have enough inventory in stock to fill order:\n " +
                        $"Would you like to automatically reduce order amount to the number available? \n {display}", "", MessageBoxButton.YesNoCancel);
                    switch(result)
                    {
                        case MessageBoxResult.Yes:
                            MessageBox.Show("Yes selected");
                            for (int i = 0; i < SalesOrder.Count; i++)
                            {
                                for (int j = 0; j < IncorrectQuantity.Count; j++)
                                {
                                    if (IncorrectQuantity[j] == SalesOrder[i].Item)
                                    {
                                        SalesOrder[i].Quantity = _dataRepository.GetCurrentStock(SalesOrder[i].Item);
                                    }
                                }
                            }
                            break;
                        case MessageBoxResult.No:
                            MessageBoxResult result2 = MessageBox.Show("Would you like to process the Sale as it is?", "", MessageBoxButton.YesNoCancel);
                            switch (result2)
                            {
                                case MessageBoxResult.Yes:
                                    MessageBox.Show("An order reminder email has been send to 'Dutty@Example.com' ");
                                    break;
                            }
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                    }
                    IncorrectQuantity.Clear();
                }
                else if (IncorrectQuantity.Count == 0)
                {
                    SalesOrder.Clear();
                }
            }
        }

        public void GenerateInvList()
        {
            ListOfInv.Clear();
            ListOfInv.AddRange(_dataRepository.GetInventory());
        }

        private void ShowAddCustomerDialog()
        {
            var p = new DialogParameters();
         

            _dialogService.ShowDialog("AddDialogView", p, result =>
            {
                
            });
        }

        private void ShowAddSalespersonDialog()
        {
            var p = new DialogParameters();


            _dialogService.ShowDialog("AddSalespersonDialogView", p, result =>
            {

            });
        }

        public string Title => "Sales Order";

       

        public event Action<IDialogResult> RequestClose;



        private readonly Dictionary<string, List<string>> _propertyNameToErrorsDictionary;
        public bool HasErrors => _propertyNameToErrorsDictionary.Any();
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public IEnumerable GetErrors(string? propertyName)
        {
            return _propertyNameToErrorsDictionary.GetValueOrDefault(propertyName, new List<string>());
        }


        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
           
        }
    }
}
