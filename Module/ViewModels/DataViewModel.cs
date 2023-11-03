using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismApplicationMavinwoo_Test.core.DataAccess;
using PrismApplicationMavinwoo_Test.core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Module.ViewModels
{

    internal class DataViewModel : BindableBase
    {
        // "_dataRepository" field is declared as an IDataRepository
        private IDataRepository _dataRepository;
        private IDialogService _dialogService;

        // "_title" field is declared as a "List" of type "OrderInfoModel"
        private List<OrderInfoModel> _title;
        private DateTime _date_Start;
        private DateTime _date_End;
        private List<OrderInfoModel> _filterData;
        private ObservableCollection<OrderInfoModel> _filterD;
        private ObservableCollection<OrderInfoModel> _searchD;
        private ObservableCollection<SalespersonModel> _selectedSalespersons;
        private ObservableCollection<CustomerModel> _selectedCustomers;
        private string _keyword;
        private string _selection;
        private int _displaySalesperson;
        private int _displayCustomer;
        private string _messageReceived;

        public string MessageReceived {  get => _messageReceived; set { SetProperty(ref _messageReceived, value); } }


        // "Title" property is declared and the "getter" and "setter" are both created
        public List<OrderInfoModel> Title
        {
            get => _title;
            //  The original setter read: set { _testbox = value } 
            //  Needed to use SetProperty() for DelegateCommand to work
            set
            {
                SetProperty(ref _title, value);
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
        public string Selection { get => _selection; 
            set 
            { 
                SetProperty(ref _selection, value);

            } }
        public DateTime Date_Start { get => _date_Start; set { SetProperty(ref _date_Start, value); } }
        public DateTime Date_End { get => _date_End; set { SetProperty(ref _date_End, value); } }
        public List<OrderInfoModel> FilterData { get => _filterData; set { SetProperty(ref _filterData, value); } }
        public DelegateCommand SelectedData {  get; private set; }
        public DelegateCommand FilterDataResults { get; private set; }
        public DelegateCommand SearchDataResults {  get; private set; }
        public DelegateCommand ShowDialogCommand { get; private set; }
        public ObservableCollection<OrderInfoModel> FilterD { get => _filterD; set { SetProperty(ref _filterD, value); } }

        public ObservableCollection<OrderInfoModel> SearchD {  get => _searchD; set { SetProperty(ref _searchD, value); } }
        public ObservableCollection<SalespersonModel> SelectedSalespersons { get => _selectedSalespersons; set { SetProperty(ref _selectedSalespersons, value); } }
        public ObservableCollection<CustomerModel> SelectedCustomers { get => _selectedCustomers; set { SetProperty(ref _selectedCustomers, value); } }



        // DataViewModel Constructor
        public DataViewModel(IDataRepository dataRepository, IDialogService dialogService)
        {
            _dialogService = dialogService;
            // DataViewModel parameter "dataRepository" assigned to private field "_dataRepository"
            _dataRepository = dataRepository;
            
            // Title is now initialized with an instance of "List<OrderInfoModel"
            Title = new List<OrderInfoModel>();

            // "GetData()" clears anything within Title, then adds the info retrieved from GetData() query in _dataRepository
            //GetData();
            GetData();


            SelectedData = new DelegateCommand(SelectDataGrid, CanClick);
            SearchDataResults = new DelegateCommand(Search, CanClick);
            FilterDataResults = new DelegateCommand(Filter, CanClick);
            ShowDialogCommand = new DelegateCommand(ShowDialog, CanClick);
            Date_Start = DateTime.Now;
            Date_End = DateTime.Now;
            FilterData = new List<OrderInfoModel>();
            FilterD = new ObservableCollection<OrderInfoModel>();
            SearchD = new ObservableCollection<OrderInfoModel>();
            SelectedSalespersons = new ObservableCollection<SalespersonModel>();
            SelectedCustomers = new ObservableCollection<CustomerModel>();
        }

        private void ShowDialog()
        {
            var p = new DialogParameters();
            p.Add("message", "This is a test message.");

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
            } );
        }
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
            if(Selection == "System.Windows.Controls.ComboBoxItem: Reset")
            {
                DisplayCustomer = 0;
                DisplaySalesperson = 0;
                Title.Clear();
                Title.AddRange( _dataRepository.GetData());
                Title.ToList();
            }
        }

        private void GetData()
        {
            Title.Clear();
            Title.AddRange(_dataRepository.GetData());
        }

        public void Filter()
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

        private void Search()
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
        private bool CanClick()
        {
            return true;
        }
    }
}
