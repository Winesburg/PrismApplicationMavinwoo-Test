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
using System.Net.Mail;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using Prism.Events;
using PrismApplicationMavinwoo_Test.core.Events;

namespace Module.ViewModels
{
    public class SalesOrderDialogViewModel : BindableBase, IDialogAware, INotifyDataErrorInfo
    {
        private IDataRepository _dataRepository;
        private IDialogService _dialogService;
        private IEventAggregator _eventAggregator;
        private string? _itemDateConn;
        private string _itemSalespersonConn;
        private string _itemCustomerConn;
        private decimal? _itemPriceConn;
        private int? _itemQuantityConn;
        private int _addSalesTrigger;
        private decimal? _amountDue = 0;
        private InventoryAddDialogModel _selItem;
        private ObservableCollection<string> _salesHeader_Date;
        private ObservableCollection<string> _salesHeader_Salesperson;
        private ObservableCollection<string> _salesHeader_Customer;
        private ObservableCollection<InventoryAddDialogModel> _listOfInv;
        private ObservableCollection<CompletedSalesOrderModel> _salesOrder;

        //private Dictionary<string, int> _customers;

        //public Dictionary<string, int> Customers
        //{
        //    get => _customers;
        //    set
        //    {
        //        SetProperty(ref _customers, value);
        //    }
        //}
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
        public decimal? AmountDue
        {
            get => _amountDue;
            set
            {
                SetProperty(ref _amountDue, value);
                RaisePropertyChanged(nameof(AmountDue));
            }
        }
        public InventoryAddDialogModel SelItem { get => _selItem; set { SetProperty(ref _selItem, value); } }
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
        public ObservableCollection<InventoryAddDialogModel> ListOfInv { get => _listOfInv; set { SetProperty(ref _listOfInv, value); } }
        public ObservableCollection<CompletedSalesOrderModel> SalesOrder 
        { 
            get => _salesOrder; 
            set 
            { 
                //SetProperty(ref _salesOrder, value);
                //RaisePropertyChanged(nameof(SalesOrder));
                
                if (_salesOrder != value)
                {
                    _salesOrder = value;                           
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SalesOrder)));
                }
            } 
        }
        public DelegateCommand NewCustomerCommand { get; set; }
        public DelegateCommand TestCommand { get; set; }
        public DelegateCommand AddSalesLineCommand { get; set; }
        public DelegateCommand OpenSalespersonCommand { get; set; }
        public DelegateCommand SubmitSalesOrderCommand { get; set; }
        public DelegateCommand DeleteOrderLineCommand {  get; set; }
        public DelegateCommand ClearSalesOrderCommand { get; set; }

        public SalesOrderDialogViewModel(IDataRepository dataRepository, IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _dataRepository = dataRepository;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
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

        private void UpdateSalesOrder()
        {
            string test = "This works";
            _eventAggregator.GetEvent<SalesOrderUpdateEvent>().Publish(test);

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
                AmountDue += SalesOrder[SalesOrder.Count-1].Total;
               
                    if (SalesOrder.Count > 1)
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
                    AmountDue -= SalesOrder[i].Total;
                    SalesOrder.RemoveAt(i);
                }
            }
            if (SalesOrder.Count == 0)
            {
                SalesOrder.Clear();
                CanClickSalesOrder();
            }

        }
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
                DateTime item_date = Convert.ToDateTime(ItemDateConn.Replace(" 12:00:00 AM", ""));
                string display = "";
                List<string> IncorrectQuantity = new List<string>();

                // Initial loop that iterates through the entire Sales Order
                for (int i = 0; i < SalesOrder.Count; i++)
                {
                    //  Checks if ordered quantity of any item is greater than quantity in inventory
                    int currentStock = _dataRepository.GetCurrentStock(SalesOrder[i].Item);

                    if (currentStock < SalesOrder[i].Quantity)
                    {
                        IncorrectQuantity.Add(SalesOrder[i].Item);
                    }
                }
                
                if (IncorrectQuantity.Count > 0)
                {
                    for (int i = 0; i < IncorrectQuantity.Count; i++)
                    {
                        display += ($"{IncorrectQuantity[i]} \n");
                    }
                    MessageBoxResult result = MessageBox.Show($"The following items do not have enough inventory in stock to fill order:\n\n{display} \n " +
                        $"Would you like to automatically reduce order amount to the number available?", "", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            // Loops through Sales Order and IncorrectQuantity to update Inventory table
                            for (int i = 0; i < SalesOrder.Count; i++)
                            {
                                for (int j = 0; j < IncorrectQuantity.Count; j++)
                                {
                                    if (IncorrectQuantity[j] == SalesOrder[i].Item)
                                    {
                                        SalesOrder[i].Quantity = _dataRepository.GetCurrentStock(SalesOrder[i].Item);
                                    }
                                }
                                _dataRepository.SetStock(SalesOrder[i].Quantity, SalesOrder[i].Item);
                            }
                            UpdateOrderLineTable();
                            _dataRepository.UpdateSalesOrder(FindSalesOrderNumber() + 1, item_date, FindSaleSalesperson(), FindSaleCustomer(), (decimal)AmountDue);
                            SalesOrder.Clear();
                            break;
                        case MessageBoxResult.No:
                            MessageBoxResult result2 = MessageBox.Show("Would you like to process the Sale as it is?", "", MessageBoxButton.YesNo);
                            switch (result2)
                            {
                                case MessageBoxResult.Yes:

                                    ///////// Sets up and sends auto email
                                    string senderEmail = "11kurtzd@gmail.com";
                                    string senderPassword = "rrjc ocml wthc xzjj";
                                    string recipientEmail = "11kurtzd@gmail.com";

                                    string smtpServer = "smtp.gmail.com";
                                    int smtpPort = 587;

                                    // Create a new MailMessage
                                    MailMessage mail = new MailMessage(senderEmail, recipientEmail);
                                    mail.Subject = "Test Email";
                                    string htmlBody = $"<p>The inventory of the following item(s) should be checked.</p>" +
                                                      $"<p>The quantity sold surpassed the quantity in stock in Order Number: <b>{FindSalesOrderNumber()}</b></p><br>" +
                                                      $"<p>{display}</p>";
                                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");
                                    mail.AlternateViews.Add(htmlView);


                                    // Configure the SMTP client
                                    SmtpClient smtpClient = new SmtpClient(smtpServer);
                                    smtpClient.Port = smtpPort;
                                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                                    smtpClient.EnableSsl = true; // Enable SSL for secure connections

                                    try
                                    {
                                        // Send the email
                                        smtpClient.Send(mail);
                                        MessageBox.Show("An order reminder email has been send to 'Email@Example.com' ");
                                        // Update Database
                                        UpdateOrderLineTable();
                                        _dataRepository.UpdateSalesOrder(FindSalesOrderNumber() + 1, item_date, FindSaleSalesperson(), FindSaleCustomer(), (decimal)AmountDue);
                                        for (int i = 0; i < SalesOrder.Count; i++)
                                        {
                                            int currentStock = _dataRepository.GetCurrentStock(SalesOrder[i].Item);
                                            _dataRepository.SetStock(currentStock - SalesOrder[i].Quantity, SalesOrder[i].Item);
                                        }
                                        SalesOrder.Clear();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Error: {ex.Message}");
                                    }
                                    break;
                                case MessageBoxResult.No:
                                    break;
                            }
                            break;
                    }
                    IncorrectQuantity.Clear();
                }
                else if (IncorrectQuantity.Count == 0)
                {
                    //  Updates Inventory Table
                    
                    for (int i = 0; i < SalesOrder.Count; i++)
                    {
                        int currentStock = _dataRepository.GetCurrentStock(SalesOrder[i].Item);
                        _dataRepository.SetStock((currentStock - SalesOrder[i].Quantity), SalesOrder[i].Item);
                    }
                    UpdateOrderLineTable();
                    _dataRepository.UpdateSalesOrder(FindSalesOrderNumber() + 1, item_date, FindSaleSalesperson(), FindSaleCustomer(), (decimal)AmountDue);
                    SalesOrder.Clear();
                }

            }






           
        }

        private int FindSalesOrderNumber()
        {
            int id = _dataRepository.GetSalesOrderNo();
            return id;
        }
        private int FindSaleCustomer()
        {
            List<CustomerAddDialogModel> listOfCustomers = _dataRepository.GetCustomerForSale();
            string replacementCustomer = ItemCustomerConn.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            for (int x = 0; x < listOfCustomers.Count; x++)
            {
                if (listOfCustomers[x].Name == replacementCustomer)
                {
                    return listOfCustomers[x].Customer;
                }
            }
            return 0;
        }

        private int FindSaleSalesperson()
        {
            List<Salesperson> listOfCustomers = _dataRepository.GetSalespersonForSale();
            string replacementSalesperson = ItemSalespersonConn.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            for (int x = 0; x < listOfCustomers.Count; x++)
            {
                if (listOfCustomers[x].Name == replacementSalesperson)
                {
                    return listOfCustomers[x].Id;
                }
            }
            return 0;
        }

        private void UpdateOrderLineTable()
        {
            int orderNumber = FindSalesOrderNumber() + 1;
            for (int i = 0; i < SalesOrder.Count; i++)
            {
                _dataRepository.UpdateOrderLines(SalesOrder[i].Item, (decimal)SalesOrder[i].Price, (int)SalesOrder[i].Quantity, orderNumber);
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
            UpdateSalesOrder();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            return;
        }
    }
}
