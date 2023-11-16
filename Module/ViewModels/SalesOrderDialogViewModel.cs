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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        public SalesOrderDialogViewModel(IDataRepository dataRepository, IDialogService dialogService)
        {
            _dataRepository = dataRepository;
            _dialogService = dialogService;
            ListOfInv = new ObservableCollection<InventoryAddDialogModel>();
            SelItemCollection = new ObservableCollection<InventoryAddDialogModel>();
            ItemName = new ObservableCollection<string>();
            SalesOrder = new ObservableCollection<CompletedSalesOrderModel>();
            SalesOrderDisplay = new ObservableCollection<string>();
            


            NewCustomerCommand = new DelegateCommand(ShowAddCustomerDialog);
            AddSalesLineCommand = new DelegateCommand(AddSalesLine);

            GenerateInvList();

            // This was the test functionality for dynamically adding inv items and selecting them
            //TestCommand = new DelegateCommand(TestTest);

        }

        private void AddSalesLine()
        {
            DateTime item_date = new DateTime(2023, 11, 25);
            string item_salesperson = "Dustin Kurtz";
            string item_customer = "Mountain Outfitters";
            string item_item = "Sunglasses";
            decimal item_price = 80.00m;
            int item_quantity = 3;
            SalesOrder.Add(new CompletedSalesOrderModel(item_date, item_salesperson, item_customer, item_item, item_price, item_quantity));
            List<string> strings  = new List<string>();
            strings.AddRange(SalesOrder.Select(s => s.Item).ToList());
            strings.AddRange(SalesOrder.Select(t => t.Salesperson).ToList());
            strings.AddRange(SalesOrder.Select(u => u.Price.ToString()).ToList());
            strings.AddRange(SalesOrder.Select(w => w.Date_Sold.ToString()).ToList());
            strings.AddRange(SalesOrder.Select(c => c.Quantity.ToString()).ToList());
            strings.AddRange(SalesOrder.Select(e => e.Customer.ToString()).ToList());

            //Array<string> test2 = new Array<string>();
            //    SalesOrder.Select(s => s.Item).ToList();

            SalesOrderDisplay.Add($"{strings[0]} {strings[1]}");
            SalesOrderDisplay.Add(strings[1]);
            SalesOrderDisplay.Add(strings[2]);
            SalesOrderDisplay.Add(strings[3]);
            SalesOrderDisplay.Add(strings[4]);
            SalesOrderDisplay.Add(strings[5]);

            
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
