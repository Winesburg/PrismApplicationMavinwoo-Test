using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismApplicationMavinwoo_Test.core.DataAccess;
using PrismApplicationMavinwoo_Test.core.Models;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Module.ViewModels
{
    public class InventoryDialogViewModel : BindableBase, IDialogAware
    {
        private IDataRepository _dataRepository;
        private string _item = "";
        private string _reorderLimit = "";
        private string? _deliveryDate = "";
        private string? _onOrder = "";
        private string _inStock = "";
        private int _displayInventoryIndex = 0;
        private int _displayAddIndex = 2;
        private int _displayDeleteIndex = 0;
        private int _displayUpdateIndex = 0;
        //private int delDateFormat;
        private string? _selection;
        private string _propertySelection;
        private string _newPropertyValue;
        private InventoryAddDialogModel _selectedInventory;
        private InventoryAddDialogModel? _delInv;
        private ObservableCollection<InventoryAddDialogModel> _inventoryData;
        private ObservableCollection<InventoryAddDialogModel> _invItems;
        private ObservableCollection<InventoryAddDialogModel> _listOfInv;
        public string Item
        {
            get => _item;
            set
            {
                SetProperty(ref _item, value);
                RaisePropertyChanged(nameof(Item));
                AddInventoryCommand.RaiseCanExecuteChanged();
            }
        }
        public string ReorderLimit
        {
            get => _reorderLimit;
            set
            {
                SetProperty(ref _reorderLimit, value);
                RaisePropertyChanged(nameof(ReorderLimit));
                AddInventoryCommand.RaiseCanExecuteChanged();
            }
        }
        public string? DeliveryDate
        {
            get => _deliveryDate;
            set
            {
                SetProperty(ref _deliveryDate, value);
                RaisePropertyChanged(nameof(DeliveryDate));
            }
        }
        public string? OnOrder
        {
            get => _onOrder;
            set
            {
                SetProperty(ref _onOrder, value);
                RaisePropertyChanged(nameof(OnOrder));
            }
        }
        public string InStock
        {
            get => _inStock;
            set
            {
                SetProperty(ref _inStock, value);
                RaisePropertyChanged(nameof(_inStock));
                AddInventoryCommand.RaiseCanExecuteChanged();
            }
        }
        public int DisplayInventoryIndex { get => _displayInventoryIndex; set { SetProperty(ref _displayInventoryIndex, value); } }
        public int DisplayAddIndex { get => _displayAddIndex; set { SetProperty(ref _displayAddIndex, value); } }
        public int DisplayDeleteIndex { get => _displayDeleteIndex; set { SetProperty(ref _displayDeleteIndex, value); } }
        public int DisplayUpdateIndex { get => _displayUpdateIndex; set { SetProperty(ref _displayUpdateIndex, value); } }
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
        public string PropertySelection
        {
            get => _propertySelection;
            set
            {
                SetProperty(ref _propertySelection, value);
                RaisePropertyChanged(nameof(PropertySelection));
                UpdateInvCommand.RaiseCanExecuteChanged();
            }
        }
        public string NewPropertyValue
        {
            get => _newPropertyValue;
            set
            {
                SetProperty(ref _newPropertyValue, value);
                RaisePropertyChanged(nameof(_newPropertyValue));
            }
        }
        public InventoryAddDialogModel SelectedInventory
        {
            get => _selectedInventory;
            set
            {
                SetProperty(ref _selectedInventory, value);
                RaisePropertyChanged(nameof(SelectedInventory));
                EditOptionsCommand.RaiseCanExecuteChanged();
            }
        }
        public InventoryAddDialogModel? DelInv
        {
            get => _delInv;
            set
            {
                SetProperty(ref _delInv, value);
                RaisePropertyChanged(nameof(DelInv));
            }
        }
        //  Inventory Data Display
        public ObservableCollection<InventoryAddDialogModel> InventoryData { get => _inventoryData; set { SetProperty(ref _inventoryData, value); } }
        public ObservableCollection<InventoryAddDialogModel> InvItems
        {
            get => _invItems;
            set
            {
                SetProperty(ref _invItems, value);
                RaisePropertyChanged(nameof(InvItems));
            }
        }
        public ObservableCollection<InventoryAddDialogModel> ListOfInv { get => _listOfInv; set { SetProperty(ref _listOfInv, value); } }
        public DelegateCommand DisplaySelectedCommand {  get; private set; }
        public DelegateCommand EditOptionsCommand { get; private set; }
        public DelegateCommand UpdateInvCommand { get; private set; }
        public DelegateCommand AddInventoryCommand { get; private set; }
        public DelegateCommand ClearInventoryCommand { get; private set; }
        public DelegateCommand DelInvLineCommand {  get; private set; }

        public InventoryDialogViewModel(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
            _dataRepository = dataRepository;
            DisplaySelectedCommand = new DelegateCommand(DisplaySelected, CanClickSelection);
            EditOptionsCommand = new DelegateCommand(DisplayEditOptions, CanClickInventory);
            UpdateInvCommand = new DelegateCommand(UpdateInv, CanSubmit);
            AddInventoryCommand = new DelegateCommand(AddInventory, CanAddInv);
            DelInvLineCommand = new DelegateCommand(DeleteInventoryLine);
            ClearInventoryCommand = new DelegateCommand(ClearForm);
            InventoryData = new ObservableCollection<InventoryAddDialogModel>();
            InvItems = new ObservableCollection<InventoryAddDialogModel>();
            ListOfInv = new ObservableCollection<InventoryAddDialogModel>();
            GenerateInvList();
        }

        private void DeleteInventoryLine()
        {
            if (DelInv != null)
            { 
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete:    {DelInv.Item}", "Confirmation", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        _dataRepository.DeleteInvLine(DelInv.Item);
                        DelInv = null;
                        GetInventoryData();
                        break;
                    case MessageBoxResult.No:
                        DelInv = null;
                        break;
                }
            }
            else if (DelInv == null)
            {
                MessageBox.Show("Must select an inventory line", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void GenerateInvList()
        {
            ListOfInv.Clear();
            ListOfInv.AddRange(_dataRepository.GetInventory());
        }

        private void ClearForm()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you would like to clear this form?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch(result)
            {
                case MessageBoxResult.Yes:
                    Item = "";
                    DeliveryDate = "";
                    InStock = "";
                    OnOrder = "";
                    ReorderLimit = "";
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        public void AddInventory()
        {
            try
            {
                //  Limits set based on limits defined within database
                if(Item.Length >= 50 || InStock.Length >= 10 || OnOrder.Length >=10 ||  ReorderLimit.Length >= 10)
                {
                    MessageBox.Show("Input is too large", "", MessageBoxButton.OK);
                }
                else { 
                    if (Item.Length > 0 && InStock.Length > 0 && ReorderLimit.Length > 0)
                    {
                        if (DeliveryDate.Length == 0 && OnOrder.Length == 0)
                        {
                            _dataRepository.AddInventoryNull(Item, InStock, ReorderLimit);
                            Item = "";
                            DeliveryDate = "";
                            InStock = "";
                            OnOrder = "";
                            ReorderLimit = "";
                        }
                        else if (DeliveryDate.Length > 0 && OnOrder.Length > 0)
                        {
                            DateTime date = DateTime.Parse(DeliveryDate);
                            _dataRepository.AddInventory(Item, InStock, OnOrder, date, ReorderLimit);
                            Item = "";
                            DeliveryDate = "";
                            InStock = "";
                            OnOrder = "";
                            ReorderLimit = "";
                        }
                        else
                        {
                            MessageBox.Show("Both  'On Order'  and  'Delivery Date'  must be completed", "Improper Use", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Input: Make sure input fields are complete!", "Improper Use", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Invalid Input", "Improper Use", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateInv()
        {
            string column = PropertySelection.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            string item = SelectedInventory.Item; 
            string? deldateformat = NewPropertyValue == null ? null : NewPropertyValue.Replace(" ", "");
            try
            { 
                if (column == "On_Order")
                {
                    if (NewPropertyValue == "null" || NewPropertyValue == "" || NewPropertyValue == "0" || NewPropertyValue == null || NewPropertyValue.Length == 0 || deldateformat == null)
                    {
                        _dataRepository.UpdateNullInventory(column, item);
                        DisplayEditOptions();
                    }
                    else
                    {
                        _dataRepository.UpdateInventory(column, NewPropertyValue, item);
                        DisplayEditOptions();
                    }
                }
                if (column == "Delivery_Date")
                {
                   
                    if (NewPropertyValue == "null" || NewPropertyValue == "" || NewPropertyValue == "0" || NewPropertyValue == null || NewPropertyValue.Length == 0 || deldateformat == null)
                    {
                        _dataRepository.UpdateNullInventory(column, item);
                        DisplayEditOptions();
                    }
                    else if (NewPropertyValue.Length > 0)
                    {
                        try
                        {
                            string deliveryDate = DateTime.Parse(NewPropertyValue).ToString("yyyy'-'MM'-'dd");
                            _dataRepository.UpdateInventory(column, deliveryDate, item);
                            DisplayEditOptions();

                        }
                        catch
                        {
                            MessageBox.Show("Day, Month, and Year must be separated \n\n " +
                                            "with either a froward slash  ( / )  or  a hyphen  ( - ) \n\n " + 
                                            "Examples: \n\n 01-22-2024 \n\n 01/22/2024", "Improper Format");
                        }
                    }
                }
                if(NewPropertyValue != null)
                { 
                    if (column == "In_Stock" || column == "Reorder_Limit")
                    { 
                        _dataRepository.UpdateInventory(column, NewPropertyValue, item);
                        DisplayEditOptions();
                    }
                }
                if (NewPropertyValue == null)
                {
                    if (column == "In_Stock" || column == "Reorder_Limit")
                    {
                        MessageBox.Show("No input given", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Invalid Input", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DisplayEditOptions()
        {
            if (SelectedInventory != null)
            {
            InvItems.Clear();
            InvItems.AddRange(_dataRepository.GetSelectedInvItem(SelectedInventory.Item));
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
                    GetInventoryData();
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

        private bool CanSubmit()
        {
            if (SelectedInventory !=null && PropertySelection != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CanAddInv()
        {
            if (Item.Length > 0 && InStock.Length > 0 && ReorderLimit.Length > 0)
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
            return;
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            return;
        }
    }
}
