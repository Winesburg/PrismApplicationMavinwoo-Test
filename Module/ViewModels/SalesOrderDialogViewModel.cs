using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismApplicationMavinwoo_Test.core.DataAccess;
using PrismApplicationMavinwoo_Test.core.Models;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Module.ViewModels
{
    public class SalesOrderDialogViewModel : BindableBase, IDialogAware
    {
        private IDataRepository _dataRepository;
        private IDialogService _dialogService;
        private ObservableCollection<InventoryAddDialogModel> _listOfInv;
        private ObservableCollection<string> _itemName;
        private InventoryAddDialogModel _selItem;
        private ObservableCollection<InventoryAddDialogModel> _selItemCollection;
        private ObservableCollection<CompletedSalesOrderModel> _selOrderLineCollection;
        private ObservableCollection<CompletedSalesOrderModel> _salesOrder;
        private ObservableCollection<string> _salesOrderDisplay;
        private DateTime _itemDateConn;
        private string _itemSalespersonConn;
        private string _itemCustomerConn;
        private string _itemNameConn;
        private decimal? _itemPriceConn;
        private int? _itemQuantityConn;
        private string _selectedItemName;
        private string _itemSalesperson;
        private ObservableCollection<string> _salesHeader_Date;
        private ObservableCollection<string> _salesHeader_Salesperson;
        private ObservableCollection<string> _salesHeader_Customer;
        private int _addSalesTrigger;
        private List<string> _strings;
        private string _selOrderLine;
        private int _salesOrderID = 1;
        private ObservableCollection<CompletedSalesOrderModel> _orderLineView;
        private string _dateSold;
        private string _customer;
        private string _item;
        private decimal _price;
        private int _quantity;

        public int AddSalesTrigger 
        { 
            get => _addSalesTrigger;
            set 
            { 
                SetProperty(ref _addSalesTrigger, value);
                RaisePropertyChanged(nameof(_addSalesTrigger));
                SubmitSalesOrderCommand.RaiseCanExecuteChanged();
            } 
        }
        public int SalesOrderID { get => _salesOrderID; set { SetProperty(ref _salesOrderID, value); } }
        public string DateSold { get => _dateSold; set { SetProperty(ref _dateSold, value); } }
        public string ItemSalesperson 
        { 
            get => _itemSalesperson; 
            set 
            { 
                SetProperty(ref _itemSalesperson, value);
                RaisePropertyChanged(nameof(_itemSalesperson));
                AddSalesLineCommand.RaiseCanExecuteChanged();
            }
        }
        public string Customer { get => _customer; set { SetProperty(ref _customer, value); } }
        public string Item { get => _item; set { SetProperty(ref _item, value); }  }
        public decimal Price { get => _price; set { SetProperty(ref _price, value); } }
        public int Quantity { get => _quantity; set { SetProperty(ref _quantity, value); } }

        public string SelectedItemName { get => _selectedItemName; set { SetProperty(ref _selectedItemName, value); } }
        public DateTime ItemDateConn { get => _itemDateConn; set { SetProperty(ref _itemDateConn, value); } }
        public string ItemSalespersonConn { get => _itemSalespersonConn; set { SetProperty(ref _itemSalespersonConn, value); } }
        public string ItemCustomerConn { get => _itemCustomerConn; set { SetProperty(ref _itemCustomerConn, value);
            } }
        public string ItemNameConn 
        { 
            get => _itemNameConn; 
            set 
            { 
                SetProperty(ref _itemNameConn, value);
                RaisePropertyChanged(nameof(_itemNameConn));
                AddSalesLineCommand.RaiseCanExecuteChanged();
            } 
        }
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
        public ObservableCollection<string> SalesOrderDisplay 
        { 
            get => _salesOrderDisplay; 
            set 
            { 
                SetProperty(ref _salesOrderDisplay, value);
                RaisePropertyChanged(nameof(SalesOrderDisplay));
            }
        }
        public ObservableCollection<CompletedSalesOrderModel> SalesOrder { get => _salesOrder; set { SetProperty(ref _salesOrder, value); } }
        public ObservableCollection<CompletedSalesOrderModel> OrderLineView { get => _orderLineView; set { SetProperty(ref _orderLineView, value); } }
        public ObservableCollection<InventoryAddDialogModel> SelItemCollection { get => _selItemCollection; set { SetProperty(ref _selItemCollection, value); } }
        public ObservableCollection<CompletedSalesOrderModel> SelOrderLineCollection { get => _selOrderLineCollection; set { SetProperty(ref _selOrderLineCollection, value); } }
        public InventoryAddDialogModel SelItem { get => _selItem; set { SetProperty(ref _selItem, value); } }
        public string SelOrderLine 
        { 
            get => _selOrderLine; 
            set 
            {
                SetProperty(ref _selOrderLine, value);
                RaisePropertyChanged(nameof(_selOrderLine));
                DeleteOrderLineCommand.RaiseCanExecuteChanged();
            } 
        }
        public ObservableCollection<string> ItemName 
        { 
            get => _itemName; 
            set 
            {
                SetProperty(ref _itemName, value);
            } 
        }
        public List<string> strings { get => _strings; set => _strings = value; }
        public ObservableCollection<InventoryAddDialogModel> ListOfInv { get => _listOfInv; set { SetProperty(ref _listOfInv, value); } }
        public DelegateCommand NewCustomerCommand { get; set; }
        public DelegateCommand TestCommand { get; set; }
        public DelegateCommand AddSalesLineCommand { get; set; }
        public DelegateCommand OpenSalespersonCommand { get; set; }
        public DelegateCommand SubmitSalesOrderCommand { get; set; }
        public DelegateCommand DeleteOrderLineCommand {  get; set; }

        public SalesOrderDialogViewModel(IDataRepository dataRepository, IDialogService dialogService)
        {
            _dataRepository = dataRepository;
            _dialogService = dialogService;
            ListOfInv = new ObservableCollection<InventoryAddDialogModel>();
            SelItemCollection = new ObservableCollection<InventoryAddDialogModel>();
            SelOrderLineCollection = new ObservableCollection<CompletedSalesOrderModel>();
            ItemName = new ObservableCollection<string>();
            SalesOrder = new ObservableCollection<CompletedSalesOrderModel>();
            OrderLineView = new ObservableCollection<CompletedSalesOrderModel>(); 
            SalesOrderDisplay = new ObservableCollection<string>();
            SalesHeader_Date = new ObservableCollection<string>();
            SalesHeader_Salesperson = new ObservableCollection<string>();
            SalesHeader_Customer = new ObservableCollection<string>();
            ItemDateConn = DateTime.Now;
            NewCustomerCommand = new DelegateCommand(ShowAddCustomerDialog);
            AddSalesLineCommand = new DelegateCommand(AddSalesLine);
            OpenSalespersonCommand = new DelegateCommand(ShowAddSalespersonDialog);
            SubmitSalesOrderCommand = new DelegateCommand(SubmitSalesOrder, CanSubmitSalesOrder);
            DeleteOrderLineCommand = new DelegateCommand(DeleteOrderLine, CanDelete);
            strings = new List<string>();
            //SelOrderLine = new ObservableCollection<CompletedSalesOrderModel>();
            //SelOrderLine = new CompletedSalesOrderModel(SalesOrderID, DateSold, ItemSalesperson, Customer, Item, Price, Quantity);
            GenerateInvList();
            FormatInputs();
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
            
            
                if (ItemPriceConn != null && ItemQuantityConn != null && SelItem != null)
                {
                    string item_date = ItemDateConn.Date.ToString().Replace(" 12:00:00 AM", "");
                    string ItemSalesperson = ItemSalespersonConn.Replace("System.Windows.Controls.ComboBoxItem: ", "");
                    string item_customer = ItemCustomerConn.Replace("System.Windows.Controls.ComboBoxItem: ", "");
                    string item_item = SelItem.Item;
                    decimal? item_price = ItemPriceConn;
                    int? item_quantity = ItemQuantityConn;
                    SalesOrder.Add(new CompletedSalesOrderModel(SalesOrderID, item_date, ItemSalesperson, item_customer, item_item, item_price, item_quantity));
                    OrderLineView.Add(SalesOrder[SalesOrder.Count - 1]);
                    strings.AddRange(SalesOrder.Select(w => w.Date_Sold.ToString()).ToList());
                    strings.AddRange(SalesOrder.Select(t => t.Salesperson).ToList());
                    strings.AddRange(SalesOrder.Select(e => e.Customer.ToString()).ToList());
                    strings.AddRange(SalesOrder.Select(s => s.Item).ToList());
                    strings.AddRange(SalesOrder.Select(u => u.Price.ToString()).ToList());
                    strings.AddRange(SalesOrder.Select(c => c.Quantity.ToString()).ToList());


                    if (SalesHeader_Date.Count == 0 && SalesHeader_Customer.Count == 0)
                    {
                        SalesHeader_Date.Add($"{strings[0]}");
                        SalesHeader_Customer.Add($"{strings[2]}");
                    }
                    SalesOrderDisplay.Add($"{strings[1]}                              {strings[3]}      " +
                        $"                        {strings[4]}                              {strings[5]}");

                    AddSalesTrigger = 1;


                    SalesOrder.Clear();
                    strings.Clear();
                    ItemPriceConn = null;
                    ItemQuantityConn = null;
                    SelItem = null;
                }
                else if (ItemPriceConn == null || ItemQuantityConn == null || SelItem == null)
                {
                    MessageBox.Show("Invalid Input: Make sure all input fields are complete!");
                }

        }
        private void DeleteOrderLine()
        {
            for ( int i = 0; i < SalesOrderDisplay.Count; i++)
            if(SalesOrderDisplay.Contains(SelOrderLine))
            {
                SalesOrderDisplay.RemoveAt(i);
            }
            if(SalesOrderDisplay.Count() == 0)
            {
                AddSalesTrigger = 0;
            }
        }
        private bool CanDelete()
        {
            if (SelOrderLine == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //SalesOrderDisplay.Clear();
        //strings.Clear();
        //SalesHeader_Date.Clear();
        //SalesHeader_Customer.Clear();

        private bool CanSubmitSalesOrder()
        {
            if(AddSalesTrigger > 0)
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
            SalesOrderDisplay.Clear();
            SalesHeader_Date.Clear();
            SalesHeader_Customer.Clear();
            AddSalesTrigger = 0;
            SalesOrderID = 1;
        }
        private void TestTest()
        {
            //List<string> Sunglasses = new List<string>();
            //Sunglasses.Add("Sunglasses");
            

            //if (SelItem == null)
            //{
            //    return;
            //}
            //else
            //{
            //    SelItemCollection.Add(SelItem);
            //    List<string> testtesttest = SelItemCollection.Select(x => x.Item).ToList();

            //    if (testtesttest[0] == "Spoon")
            //    {
            //        ItemName.Add(testtesttest[0]);
            //    }
            //}
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
