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
    public class InventoryDialogViewModel : BindableBase, IDialogAware
    {
        private IDataRepository _dataRepository;
        private string _item;
        private int? _reorderLimit;
        private string? _deliveryDate;
        private int? _onOrder;
        private int? _inStock;
        private int _displayInventoryIndex = 0;
        private int _displayAddIndex = 2;
        private int _displayDeleteIndex = 0;
        private int _displayUpdateIndex = 0;
        private string? _selection;
        private string _propertySelection;
        private string _invDisplayPath;
        private string _newPropertyValue;
        private InventoryAddDialogModel _selectedInventory;
        private ObservableCollection<InventoryAddDialogModel> _inventoryData;
        private ObservableCollection<InventoryAddDialogModel> _invItems;
        private ObservableCollection<InventoryAddDialogModel> _currentPropertyValue;
        private ObservableCollection<InventoryAddDialogModel> _listOfInv;
        private int delDateFormat;

        public int DelDateFormat 
        { 
            get => delDateFormat;
            set
            { 
                SetProperty(ref delDateFormat, value); 
                RaisePropertyChanged(nameof(DelDateFormat));
            } 
        }
        public string Item 
        { 
            get => _item;
            set
            {
                SetProperty(ref _item, value);
                RaisePropertyChanged(nameof(Item));
            }
        }
        public int? InStock
        {
            get => _inStock;
            set
            {
                SetProperty(ref _inStock, value);
            }
        }
        public int? OnOrder 
        { 
            get => _onOrder;
            set 
            { 
                SetProperty(ref _onOrder, value);
                RaisePropertyChanged(nameof(OnOrder));
            } 
        }
        public string? DeliveryDate 
        { 
            get => _deliveryDate;
            set
            {
                SetProperty(ref _deliveryDate, value);
                RaisePropertyChanged(nameof(DeliveryDate));
                //RaisePropertyChanged($"Delivery Date: {value}");
            } 
        }
        public int? ReorderLimit
        {
            get => _reorderLimit;
            set
            {
                SetProperty(ref _reorderLimit, value);
                RaisePropertyChanged(nameof(ReorderLimit));
            }
        }
        public int DisplayInventoryIndex { get => _displayInventoryIndex; set { SetProperty(ref _displayInventoryIndex, value); } }
        public int DisplayAddIndex { get => _displayAddIndex; set { SetProperty(ref _displayAddIndex, value); } }
        public int DisplayDeleteIndex { get => _displayDeleteIndex; set { SetProperty(ref _displayDeleteIndex, value); } }
        public int DisplayUpdateIndex { get => _displayUpdateIndex; set { SetProperty(ref _displayUpdateIndex, value); } }
        public string NewPropertyValue { get => _newPropertyValue; set { SetProperty(ref _newPropertyValue, value); } }
        public InventoryAddDialogModel? SelectedInventory
        {
            get => _selectedInventory;
            set
            {
                SetProperty( ref _selectedInventory, value);
                RaisePropertyChanged(nameof(SelectedInventory));
                EditOptionsCommand.RaiseCanExecuteChanged();
            }
        }
        public string? Selection 
        { 
            get => _selection; 
            set 
            { 
                SetProperty(ref _selection, value);
                RaisePropertyChanged(nameof(Selection));
                DisplaySelectedCommand.RaiseCanExecuteChanged();
            } 
        }
        public string InvDisplayPath
        {
            get => _invDisplayPath;
            set
            {
                SetProperty(ref _invDisplayPath, value);
            }
        }
        public string PropertySelection
        {
            get => _propertySelection;
            set
            {
                SetProperty(ref _propertySelection, value);
            }
        }
        public ObservableCollection<InventoryAddDialogModel> InvItems
        {
            get => _invItems;
            set
            {
                SetProperty(ref _invItems, value);
                RaisePropertyChanged(nameof(InvItems));
                //UpdateInvCommand.RaiseCanExecuteChanged();
            }
        }


        public ObservableCollection<InventoryAddDialogModel> CurrentPropertyValue
        {
            get => _currentPropertyValue;
            set
            {
                SetProperty(ref _currentPropertyValue, value);
                RaisePropertyChanged(nameof(CurrentPropertyValue));
                //InventoryPropertyCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<InventoryAddDialogModel> ListOfInv { get => _listOfInv; set { SetProperty(ref _listOfInv, value); } }
        //  Inventory Data Display
        public ObservableCollection<InventoryAddDialogModel> InventoryData { get => _inventoryData; set { SetProperty(ref _inventoryData, value); } }
        public DelegateCommand DisplaySelectedCommand {  get; private set; }
        public DelegateCommand EditOptionsCommand { get; private set; }
        public DelegateCommand InventoryPropertyCommand { get; private set; }
        public DelegateCommand UpdateInvCommand { get; private set; }
        public DelegateCommand AddInventoryCommand { get; private set; }

        public InventoryDialogViewModel(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
            //DeliveryDate = DateTime.Now;
            OnOrder = 0;
            DisplaySelectedCommand = new DelegateCommand(DisplaySelected, CanClickSelection);
            EditOptionsCommand = new DelegateCommand(DisplayEditOptions, CanClickInventory);
            //InventoryPropertyCommand = new DelegateCommand(PropertySelectionFunc);
            //UpdateInvCommand = new DelegateCommand(UpdateInv);
            AddInventoryCommand = new DelegateCommand(AddInventory);
            InventoryData = new ObservableCollection<InventoryAddDialogModel>();
            InvItems = new ObservableCollection<InventoryAddDialogModel>();
            ListOfInv = new ObservableCollection<InventoryAddDialogModel>();
            CurrentPropertyValue = new ObservableCollection<InventoryAddDialogModel>();
            GenerateInvList();
            FormatInputs();
        }

        public void FormatInputs()
        {
            InStock = null;
            OnOrder = null;
            ReorderLimit = null;
        }

        public void GenerateInvList()
        {
            ListOfInv.Clear();
            ListOfInv.AddRange(_dataRepository.GetInventory());
        }

        public void AddInventory()
        {
            if (Item != null && InStock != null  && ReorderLimit != null)
            {

                if (DeliveryDate == null)
                {
                    _dataRepository.AddInventoryNull(Item, (int)InStock, (int)ReorderLimit);
                    Item = "";
                    DeliveryDate = "";
                    InStock = null;
                    OnOrder = null;
                    ReorderLimit = null;
                }

            }
            else if (Item != null && InStock != null && OnOrder != null && DeliveryDate != null && ReorderLimit != null)
            {
                if (DeliveryDate != null )
                {
                    DateTime date = DateTime.Parse(DeliveryDate);
                    _dataRepository.AddInventory(Item, (int)InStock, OnOrder, date, (int)ReorderLimit);
                    Item = "";
                    DeliveryDate = "";
                    InStock = null;
                    OnOrder = null;
                    ReorderLimit = null;
                }
            }
            else if (Item == null || InStock == null || ReorderLimit == null)
            {
                MessageBox.Show("Invalid Input: Make sure all input fields are complete!");
            }
            
        }

        //public void UpdateInv()
        //{
        //    string value1 = PropertySelection.Replace("System.Windows.Controls.ComboBoxItem: ", "");
        //    //string value2 = SelectedInventory.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            
        //    if (value1 == "On_Order" || value1 == "Delivery_Date")
        //    { 
        //        if (NewPropertyValue == null)
        //        {
        //            _dataRepository.UpdateNullInventory(value1, "null", value2);
        //        }
        //        else
        //        {
        //            _dataRepository.UpdateInventory(value1, NewPropertyValue, value2);
        //        }
        //    }
        //    else 
        //    { 
        //        if (NewPropertyValue == null)
        //        {
        //            return;
        //        }
        //        else
        //        {
        //        _dataRepository.UpdateInventory(value1, NewPropertyValue, value2);
        //        }
        //    }
        //}
        //public void PropertySelectionFunc()
        //{
        //    string value1 = PropertySelection.Replace("System.Windows.Controls.ComboBoxItem: ", "");
        //    //string value2 = SelectedInventory.Replace("System.Windows.Controls.ComboBoxItem: ", "");

        //    switch (value1)
        //    {
        //        case "Item":
        //            CurrentPropertyValue.Clear();
        //            CurrentPropertyValue.AddRange(_dataRepository.GetItemValue(value2));
        //            InvDisplayPath = "Item";
        //            break;

        //        case "In_Stock":
        //            CurrentPropertyValue.Clear();
        //            CurrentPropertyValue.AddRange(_dataRepository.GetStockValue(value2));
        //            InvDisplayPath = "In_Stock";
        //            break;
        //        case "On_Order":
        //            CurrentPropertyValue.Clear();
        //            CurrentPropertyValue.AddRange(_dataRepository.GetOnOrderValue(value2));
        //            InvDisplayPath = "On_Order";
        //            break;
        //        case "Delivery_Date":
        //            CurrentPropertyValue.Clear();
        //            CurrentPropertyValue.AddRange(_dataRepository.GetDeliveryDateValue(value2));
        //            InvDisplayPath = "Delivery_Date";
        //            break;
        //        case "Reorder_Limit":
        //            CurrentPropertyValue.Clear();
        //            CurrentPropertyValue.AddRange(_dataRepository.GetReorderLimitValue(value2));
        //            InvDisplayPath = "Reorder_Limit";
        //            break;
        //    }
        //}

        public void DisplayEditOptions()
        {
            if (SelectedInventory != null)
            { 
            InvItems.Clear();
            InvItems.AddRange(_dataRepository.GetSelectedInvItem(SelectedInventory.Item));
            SelectedInventory = null;
            }
        }

        public void DisplaySelected()
        {
            switch (Selection)
            {
                case "System.Windows.Controls.ComboBoxItem: Add":
                    DisplayInventoryIndex = 0;
                    DisplayDeleteIndex = 0;
                    DisplayUpdateIndex = 0;
                    DisplayAddIndex = 2;
                    Selection = null;
                    break;
                case "System.Windows.Controls.ComboBoxItem: Delete":
                    DisplayInventoryIndex = 0;
                    DisplayDeleteIndex = 2;
                    DisplayUpdateIndex = 0;
                    DisplayAddIndex = 0;
                    Selection = null;
                    break;
                case "System.Windows.Controls.ComboBoxItem: Update":
                    DisplayInventoryIndex = 0;
                    DisplayDeleteIndex = 0;
                    DisplayUpdateIndex = 2;
                    DisplayAddIndex = 0;
                    Selection = null;
                    break;
                case "System.Windows.Controls.ComboBoxItem: Display":
                    DisplayInventoryIndex = 2;
                    DisplayDeleteIndex = 0;
                    DisplayUpdateIndex = 0;
                    DisplayAddIndex = 0;
                    GetInventoryData();
                    Selection = null;
                    break;

            }
        }
        public void GetInventoryData()
        {
            InventoryData.Clear();
            InventoryData.AddRange(_dataRepository.GetInventory());
            InventoryData.ToList();
        }
        
        private bool CanClickSelection()
        {
            if (Selection != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CanClickInventory()
        {
            if (SelectedInventory != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string Title => "Edit Inventory";

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
