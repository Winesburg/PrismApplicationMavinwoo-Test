using Prism.Commands;
using Prism.Mvvm;
using PrismApplicationMavinwoo_Test.core.DataAccess;
using PrismApplicationMavinwoo_Test.core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Module.ViewModels
{

    internal class DataViewModel : BindableBase
    {
        // "_dataRepository" field is declared as an IDataRepository
        private IDataRepository _dataRepository;

        // "_title" field is declared as a "List" of type "OrderInfoModel"
        private List<OrderInfoModel> _title;

        // "Title" property is declared and the "getter" and "setter" are both created
        public List<OrderInfoModel> Title { 
            get => _title; 
            set { 
                SetProperty(ref _title, value);
               // SelectedData.RaiseCanExecuteChanged();
            } }

        //private DateTime _testbox = new DateTime(1998,04,30);

        //  The original setter read: set { _testbox = value } 
        //  Needed to use SetProperty() for DelegateCommand to work
        //public DateTime Testbox 
        //{ 
        //    get { return _testbox; } 
        //    set { SetProperty(ref _testbox, value); } 
        //}

        private DateTime _date_Start;
        private DateTime _date_End;
        private List<OrderInfoModel> _filterData;
        private ObservableCollection<OrderInfoModel> _filterD;
        private ObservableCollection<OrderInfoModel> _searchD;
        private ObservableCollection<SalespersonModel> _selectedD;
        private string _searchData;
        private string _keyword;
        private string _selection;
        private int _displayChart;
        private bool _displaySalesperson = false;


        public int test = 0;
        public bool DisplaySalesperson { get => _displaySalesperson; set => _displaySalesperson = value; }
        public int DisplayChart { get => _displayChart; set => _displayChart = value; }
        public string SearchData
        {
            get { return _searchData; }
            set { SetProperty(ref _searchData, value); }
        }
        public string Keyword
        {
            get { return _keyword; }
            set { _keyword = value; }
        }
        public string Selection { get => _selection; set { SetProperty(ref _selection, value); } }
        public DateTime Date_Start { get => _date_Start; set { SetProperty(ref _date_Start, value); } }
        public DateTime Date_End { get => _date_End; set { SetProperty(ref _date_End, value); } }
        public List<OrderInfoModel> FilterData { get => _filterData; set { SetProperty(ref _filterData, value); } }
        public DelegateCommand SelectedData {  get; private set; }
        public DelegateCommand FilterDataResults { get; private set; }
        public DelegateCommand SearchDataResults {  get; private set; }
        public ObservableCollection<OrderInfoModel> FilterD { get => _filterD; set { SetProperty(ref _filterD, value); } }

        public ObservableCollection<OrderInfoModel> SearchD {  get => _searchD; set { SetProperty(ref _searchD, value); } }
        public ObservableCollection<SalespersonModel> SelectedD { get => _selectedD; set { SetProperty(ref _selectedD, value); } }



        // DataViewModel Constructor
        public DataViewModel(IDataRepository dataRepository)
        {
            
            // DataViewModel parameter "dataRepository" assigned to private field "_dataRepository"
            _dataRepository = dataRepository;
            
            // Title is now initialized with an instance of "List<OrderInfoModel"
            Title = new List<OrderInfoModel>();

            // "GetData()" clears anything within Title, then adds the info retrieved from GetData() query in _dataRepository
            //GetData();

            GetData();


            //TestClick = new DelegateCommand(Click, CanClick);
            SelectedData = new DelegateCommand(SelectDataGrid, CanClick);
            SearchDataResults = new DelegateCommand(Search, CanClick);
            FilterDataResults = new DelegateCommand(Filter, CanClick);
            Date_Start = DateTime.Now;
            Date_End = DateTime.Now;
            FilterData = new List<OrderInfoModel>();
            FilterD = new ObservableCollection<OrderInfoModel>();
            SearchD = new ObservableCollection<OrderInfoModel>();
            SelectedD = new ObservableCollection<SalespersonModel>();
            test = DisplayChart;

        }

        private void SelectDataGrid()
        {
            if (DisplaySalesperson == false)
            {
                SelectedD.Clear();
                SelectedD.AddRange(_dataRepository.SelectSalesperson());
                SelectedD.ToList();
                DisplayChart = 2; 
                DisplaySalesperson = true;
            }
        }

        private void GetData()
        {
            Title.Clear();
            Title.AddRange(_dataRepository.GetData());
        }

        public void Filter()
        {
            FilterD.Clear();
            FilterD.AddRange(_dataRepository.FilterData(Date_Start, Date_End));
            Title = FilterD.ToList();
        }

        private void Search()
        {
            SearchD.Clear();
            SearchD.AddRange(_dataRepository.SearchData(Keyword));
            Title = SearchD.ToList();
        }

        private void Select()
        {
            SelectedD.Clear();
            SelectedD.AddRange(_dataRepository.SelectSalesperson());
            SelectedD.ToList();
        }
        private bool CanClick()
        {
            return true;
        }
    }
}
