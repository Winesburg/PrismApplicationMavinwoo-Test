using Module.Events;
using Module.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismApplicationMavinwoo_Test.core.DataAccess;
using PrismApplicationMavinwoo_Test.core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Module.ViewModels
{
    // Defines the logic for the Main Application View -- 'Data.xaml'
    public class DataViewModel : BindableBase
    {
        private IDataRepository _dataRepository;
        private IDialogService _dialogService;
        private IEventAggregator _eventAggregator;
        private DateTime _date_Start;
        private DateTime _date_End;
        private List<OrderLinesDialogModel> _returnedOrderLines;
        private OrderInfoModel _orderNumberView;
        private List<OrderInfoModel> _title;
        private List<OrderInfoModel> _filterData;
        private ObservableCollection<OrderInfoModel> _filterD;
        private ObservableCollection<OrderInfoModel> _searchD;
        private ObservableCollection<SalespersonModel> _selectedSalespersons;
        private ObservableCollection<CustomerModel> _selectedCustomers;
        private int _displaySalesperson;
        private int _displayCustomer;
        private string _keyword;
        private string _messageReceived;
        private string _selection;
        private int _displayOrder;
        
        public int DisplayOrder { get => _displayOrder; set { SetProperty(ref _displayOrder, value); } }
        public List<OrderLinesDialogModel> ReturnedOrderLines { get => _returnedOrderLines; set { SetProperty(ref _returnedOrderLines, value); } }
        public OrderInfoModel OrderNumberView { get => _orderNumberView; set { SetProperty(ref _orderNumberView, value); } }
        public DateTime Date_Start { get => _date_Start; set { SetProperty(ref _date_Start, value); } }
        public DateTime Date_End { get => _date_End; set { SetProperty(ref _date_End, value); } }
        public List<OrderInfoModel> Title
        {
            get => _title;
            set
            {
                //  Needed to use SetProperty() for DelegateCommand to work
                SetProperty(ref _title, value);
                RaisePropertyChanged(nameof(Title));
            }
        }
        public List<OrderInfoModel> FilterData { get => _filterData; set { SetProperty(ref _filterData, value); } }
        public ObservableCollection<OrderInfoModel> FilterD { get => _filterD; set { SetProperty(ref _filterD, value); } }

        public ObservableCollection<OrderInfoModel> SearchD { get => _searchD; set { SetProperty(ref _searchD, value); } }
        public ObservableCollection<SalespersonModel> SelectedSalespersons { get => _selectedSalespersons; set { SetProperty(ref _selectedSalespersons, value); } }
        public ObservableCollection<CustomerModel> SelectedCustomers
        {
            get => _selectedCustomers; set
            {
                SetProperty(ref _selectedCustomers, value);
            }
        }
        public int DisplaySalesperson 
        {
            get { return _displaySalesperson; }
            set
            {
                SetProperty(ref _displaySalesperson, value);
            }
        }
        public int DisplayCustomer
        {
            get { return _displayCustomer; }
            set
            {
                SetProperty(ref _displayCustomer, value);
            }
        }
        public string Keyword
        {
            get { return _keyword; }
            set { _keyword = value; }
        }
        public string MessageReceived { get => _messageReceived; set { SetProperty(ref _messageReceived, value); } }
        public string Selection 
        { 
            get => _selection; 
            set 
            { 
                SetProperty(ref _selection, value);

            } 
        }
        public DelegateCommand SelectedData {  get; private set; }
        public DelegateCommand FilterDataResults { get; private set; }
        public DelegateCommand SearchDataResults {  get; private set; }
        public DelegateCommand ShowDialogCommand { get; private set; }
        public DelegateCommand EditInventoryCommand { get; private set; }
        public DelegateCommand SalesOrderCommand {  get; private set; }
        public DelegateCommand DoubleClickCommand { get; }
        public DelegateCommand ReturnCommand { get; private set; }
        



        // DataViewModel Constructor
        public DataViewModel(IDataRepository dataRepository, IDialogService dialogService, IEventAggregator eventAggregator)
        {
            // DataViewModel parameter "dataRepository" assigned to private field "_dataRepository"
            _dataRepository = dataRepository;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            // Title is now initialized with an instance of "List<OrderInfoModel"
            Title = new List<OrderInfoModel>();
            SelectedData = new DelegateCommand(SelectDataGrid, CanClick);
            ReturnCommand = new DelegateCommand(ReturnFromOrderView);
            SearchDataResults = new DelegateCommand(Search, CanClick);
            FilterDataResults = new DelegateCommand(Filter, CanClick);
            ShowDialogCommand = new DelegateCommand(ShowDialog, CanClick);
            EditInventoryCommand = new DelegateCommand(ShowInventoryDialog, CanClick);
            SalesOrderCommand = new DelegateCommand(ShowSalesDialog, CanClick);
            DoubleClickCommand = new DelegateCommand(OnDoubleClick);
            Date_Start = DateTime.Now;
            Date_End = DateTime.Now;
            FilterData = new List<OrderInfoModel>();
            FilterD = new ObservableCollection<OrderInfoModel>();
            SearchD = new ObservableCollection<OrderInfoModel>();
            SelectedSalespersons = new ObservableCollection<SalespersonModel>();
            SelectedCustomers = new ObservableCollection<CustomerModel>();
            GetData();
        }
        private void ReturnFromOrderView()
        {
            DisplayOrder = 0;
        } 
        private void OnDoubleClick()
        {

            //  Find a way to communicate information to dialog window to display order lines in graph
            ReturnedOrderLines = _dataRepository.ViewOrderDetails(OrderNumberView.Order_No);
            DisplayOrder = 5;
            //_eventAggregator.GetEvent<OrderLinesViewEvent>().Publish(ReturnedOrderLines);
            //Title = _dataRepository.ViewOrderDetails(OrderNumberView.Order_No);
            //ShowOrderLinesDialog();
        }
        private void GetData()
        {
            Title.Clear();
            Title.AddRange(_dataRepository.GetData());
        }

        //  Selects which data to display -- either joins Salespersons or Customer tables with Sales_Order Table
        private void SelectDataGrid()
        {
            if (Selection == "System.Windows.Controls.ComboBoxItem: Salespersons")
            {
                DisplayCustomer = 0;
                SelectedSalespersons.Clear();
                SelectedSalespersons.AddRange(_dataRepository.SelectSalesperson());
                SelectedSalespersons.ToList();
                DisplaySalesperson = 2;
            }
            if (Selection == "System.Windows.Controls.ComboBoxItem: Customer")
            {
                DisplaySalesperson = 0;
                SelectedCustomers.Clear();
                SelectedCustomers.AddRange(_dataRepository.SelectCustomers());
                SelectedCustomers.ToList();
                DisplayCustomer = 2;
            }
            if(Selection == "System.Windows.Controls.ComboBoxItem: Refresh")
            {
                DisplayCustomer = 0;
                DisplaySalesperson = 0;
                GetData();
                Title.ToList();
            }
        }

        //  Filters Data based on entered dates
        public void Filter()
        {
            if (Date_Start > Date_End)
            {
                MessageBox.Show("Invalid Filter Date Range");
            }
            else
            {
                switch (Selection)
                {
                    case null:
                        FilterD.Clear();
                        FilterD.AddRange(_dataRepository.FilterSale(Date_Start, Date_End));
                        Title = FilterD.ToList();
                        break;
                    case "System.Windows.Controls.ComboBoxItem: Salespersons":
                        SelectedSalespersons.Clear();
                        SelectedSalespersons.AddRange(_dataRepository.FilterSalesperson(Date_Start, Date_End));
                        SelectedSalespersons.ToList();
                        break;
                    case "System.Windows.Controls.ComboBoxItem: Customer":
                        SelectedCustomers.Clear();
                        SelectedCustomers.AddRange(_dataRepository.FilterCustomer(Date_Start, Date_End));
                        SelectedCustomers.ToList();
                        break;
                    default:
                        FilterD.Clear();
                        FilterD.AddRange(_dataRepository.FilterSale(Date_Start, Date_End));
                        Title = FilterD.ToList();
                        break;
                }
            }
        }

        private void Search()
        {
            if (Keyword != null)
            {
                //  Checks for special characters
                if
                    (Keyword.Contains("%") == true || Keyword.Contains(".") == true || Keyword.Contains(",") == true ||
                    Keyword.Contains("/") == true || Keyword.Contains("?") == true || Keyword.Contains("+") == true ||
                    Keyword.Contains("=") == true || Keyword.Contains("-") == true || Keyword.Contains("_") == true ||
                    Keyword.Contains("(") == true || Keyword.Contains(")") == true || Keyword.Contains("*") == true ||
                    Keyword.Contains("&") == true || Keyword.Contains("$") == true || Keyword.Contains("!") == true ||
                    Keyword.Contains("<") == true || Keyword.Contains(">") == true || Keyword.Contains("`") == true ||
                    Keyword.Contains("~") == true || Keyword.Contains("'") == true)
                {
                    MessageBox.Show("Invalid input: Does not accept special characters/punctuation");
                }
                else
                {
                    switch (Selection)
                    {
                        case null:
                            SearchD.Clear();
                            SearchD.AddRange(_dataRepository.SearchOrder(Keyword));
                            Title = SearchD.ToList();
                            break;
                        case "System.Windows.Controls.ComboBoxItem: Salespersons":
                            DisplayCustomer = 0;
                            SelectedSalespersons.Clear();
                            SelectedSalespersons.AddRange(_dataRepository.SearchSalesperson(Keyword));
                            SelectedSalespersons.ToList();
                            DisplaySalesperson = 2;
                            break;
                        case "System.Windows.Controls.ComboBoxItem: Customer":
                            DisplaySalesperson = 0;
                            SelectedCustomers.Clear();
                            SelectedCustomers.AddRange(_dataRepository.SearchCustomer(Keyword));
                            SelectedCustomers.ToList();
                            DisplayCustomer = 2;
                            break;
                        default:
                            SearchD.Clear();
                            SearchD.AddRange(_dataRepository.SearchOrder(Keyword));
                            Title = SearchD.ToList();
                            break;
                    }
                }
            }
        }
        private bool CanClick()
        {
            return true;
        }
        private void ShowSalesDialog()
        {
            var p = new DialogParameters();
            _dialogService.ShowDialog("SalesOrderDialogView", p, result =>
            {
                GetData();
            });
        }

        //   Should be used in future to implement event aggregation
        //private void ShowOrderLinesDialog()
        //{
        //    var p = new DialogParameters();
        //    _dialogService.ShowDialog("OrderLinesDialogView", p, result =>
        //    { });
        //}
        private void ShowDialog()
        {
            var p = new DialogParameters();
            _dialogService.ShowDialog("AddDialogView", p, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    MessageReceived = result.Parameters.GetValue<string>("myParam");
                }
                else
                {
                    MessageReceived = "Okay button not clicked";
                }
            });
        }

        private void ShowInventoryDialog()
        {
            var p = new DialogParameters();

            _dialogService.ShowDialog("InventoryDialogView", p, result =>
            {
                return;
            });
        }
        async Task SalesOrderCheck()
        {
            while (true)
            {
                GetData();
                await Task.Delay(5000);
            }
            
        }
    }
}
