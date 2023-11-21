using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismApplicationMavinwoo_Test.core.DataAccess;
using PrismApplicationMavinwoo_Test.core.Models;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

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
        private ObservableCollection<CompletedSalesOrderModel> _salesOrder;
        private ObservableCollection<string> _salesOrderDisplay;
        private DateTime _itemDateConn;
        private string _itemSalespersonConn;
        private string _itemCustomerConn;
        private string _itemNameConn;
        private decimal _itemPriceConn;
        private int _itemQuantityConn;
        private string _selectedItemName;
        private string _itemSalesperson;
        private ObservableCollection<string> _salesHeader_Date;
        private ObservableCollection<string> _salesHeader_Salesperson;
        private ObservableCollection<string> _salesHeader_Customer;
        private string testTest1;

        public string testTest { get => testTest1; set => testTest1 = value; }
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
        public string SelectedItemName { get => _selectedItemName; set { SetProperty(ref _selectedItemName, value); } }
        public DateTime ItemDateConn { get => _itemDateConn; set { SetProperty(ref _itemDateConn, value); } }
        public string ItemSalespersonConn { get => _itemSalespersonConn; set { SetProperty(ref _itemSalespersonConn, value); } }
        public string ItemCustomerConn { get => _itemCustomerConn; set { SetProperty(ref _itemCustomerConn, value);
            } }
        public string ItemNameConn { get => _itemNameConn; set { SetProperty(ref _itemNameConn, value); } }
        public decimal ItemPriceConn { get => _itemPriceConn; set { SetProperty(ref _itemPriceConn, value); } }
        public int ItemQuantityConn { get => _itemQuantityConn; set { SetProperty(ref _itemQuantityConn, value); } }

        public ObservableCollection<string> SalesHeader_Date { get => _salesHeader_Date; set { SetProperty(ref _salesHeader_Date, value); } }
        public ObservableCollection<string> SalesHeader_Salesperson { get => _salesHeader_Salesperson; set { SetProperty(ref _salesHeader_Salesperson, value); } }
        public ObservableCollection<string> SalesHeader_Customer { get => _salesHeader_Customer; set { SetProperty(ref _salesHeader_Customer, value); } }
        public ObservableCollection<string> SalesOrderDisplay { get => _salesOrderDisplay; set { SetProperty(ref _salesOrderDisplay, value); } }
        public ObservableCollection<CompletedSalesOrderModel> SalesOrder { get => _salesOrder; set { SetProperty(ref _salesOrder, value); } }
        public ObservableCollection<InventoryAddDialogModel> SelItemCollection { get => _selItemCollection; set { SetProperty(ref _selItemCollection, value); } }
        public InventoryAddDialogModel SelItem { get => _selItem; set { SetProperty(ref _selItem, value); } }
        public ObservableCollection<string> ItemName 
        { 
            get => _itemName; 
            set 
            {
                SetProperty(ref _itemName, value);
                //RaisePropertyChanged(nameof(_itemName));
                //TestCommand.RaiseCanExecuteChanged();
            } 
        }
        public ObservableCollection<InventoryAddDialogModel> ListOfInv { get => _listOfInv; set { SetProperty(ref _listOfInv, value); } }
        public DelegateCommand NewCustomerCommand { get; set; }
        public DelegateCommand TestCommand { get; set; }
        public DelegateCommand AddSalesLineCommand { get; set; }
        public DelegateCommand OpenSalespersonCommand { get; set; }

        public SalesOrderDialogViewModel(IDataRepository dataRepository, IDialogService dialogService)
        {
            _dataRepository = dataRepository;
            _dialogService = dialogService;
            ListOfInv = new ObservableCollection<InventoryAddDialogModel>();
            SelItemCollection = new ObservableCollection<InventoryAddDialogModel>();
            ItemName = new ObservableCollection<string>();
            SalesOrder = new ObservableCollection<CompletedSalesOrderModel>();
            SalesOrderDisplay = new ObservableCollection<string>();
            SalesHeader_Date = new ObservableCollection<string>();
            SalesHeader_Salesperson = new ObservableCollection<string>();
            SalesHeader_Customer = new ObservableCollection<string>();
            ItemDateConn = DateTime.Now;

            System.Windows.Controls.Image test = new System.Windows.Controls.Image();
            test.Width = 10;
            BitmapImage test1 = new BitmapImage();
            test1.BeginInit();
            test1.UriSource = new Uri(@"C:\Users\dustin\source\repos\PrismApplicationMavinwoo-Test3\PrismApplicationMavinwoo-Test\Resources\delete.png");
            test1.DecodePixelWidth = 10;
            test1.EndInit();
            test.Source = test1;



            NewCustomerCommand = new DelegateCommand(ShowAddCustomerDialog);
            AddSalesLineCommand = new DelegateCommand(AddSalesLine);
            OpenSalespersonCommand = new DelegateCommand(ShowAddSalespersonDialog);

            GenerateInvList();

            // This was the test functionality for dynamically adding inv items and selecting them
            //TestCommand = new DelegateCommand(TestTest);

        }

        private void AddSalesLine()
        {
            string item_date = ItemDateConn.Date.ToString().Replace(" 12:00:00 AM", "");
            string ItemSalesperson = ItemSalespersonConn.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            string item_customer = ItemCustomerConn.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            string item_item = SelItem.Item;
            decimal item_price = ItemPriceConn;
            int item_quantity = ItemQuantityConn;
            SalesOrder.Add(new CompletedSalesOrderModel(item_date, ItemSalesperson, item_customer, item_item, item_price, item_quantity));
            List<string> strings  = new List<string>();
            strings.AddRange(SalesOrder.Select(w => w.Date_Sold.ToString()).ToList());
            strings.AddRange(SalesOrder.Select(t => t.Salesperson).ToList());
            strings.AddRange(SalesOrder.Select(e => e.Customer.ToString()).ToList());
            strings.AddRange(SalesOrder.Select(s => s.Item).ToList());
            strings.AddRange(SalesOrder.Select(u => u.Price.ToString()).ToList());
            strings.AddRange(SalesOrder.Select(c => c.Quantity.ToString()).ToList());

            SalesHeader_Date.Add($"{strings[0]}");
            SalesHeader_Salesperson.Add($"{strings[1]}");
            SalesHeader_Customer.Add($"{strings[2]}");

            //Array<string> test2 = new Array<string>();
            //    SalesOrder.Select(s => s.Item).ToList();

            //SalesOrderDisplay.Add("Date Time        Salesperson           Customer                    Item    Price   Quantity");
            SalesOrderDisplay.Add($"{strings[1]}          {strings[3]}          {strings[4]}          {strings[5]}");






            strings.Clear();
            SalesOrder.Clear();

            // Need to add counter to determine logic for header vs order lines


        }

        private void TestTest()
        {
            //if (SelItem != null)
            //{
            //    ItemName.AddRange(SelItem);
            //}

            List<string> Sunglasses = new List<string>();
            Sunglasses.Add("Sunglasses");
            

            if (SelItem == null)
            {
                return;
            }
            else
            {
                SelItemCollection.Add(SelItem);
                List<string> testtesttest = SelItemCollection.Select(x => x.Item).ToList();

                if (testtesttest[0] == "Spoon")
                {
                    ItemName.Add(testtesttest[0]);



                    //Works perfectly
                    //List<string> y = ListOfInv.Select(x => x.Item).ToList();
                    //ItemName.Add(y[0]);

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
