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
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace Module.ViewModels
{
    public class InventoryDialogViewModel : BindableBase, IDialogAware
    {
        private IDataRepository _dataRepository;
        private string _item;
        private int _reorderLimit;
        private DateTime? _deliveryDate;
        private int? _onOrder;
        private int _inStock;
        private int _displayInventoryIndex = 0;
        private int _displayAddIndex = 2;
        private int _displayDeleteIndex = 0;
        private int _displayUpdateIndex = 0;
        private string? _selection;
        private string _propertySelection;
        private string _invDisplayPath;
        private string _newPropertyValue;
        private string? _selectedInventory;
        private ObservableCollection<InventoryAddDialogModel> _inventoryData;
        private ObservableCollection<InventoryAddDialogModel> _invItems;
        private ObservableCollection<InventoryAddDialogModel> _currentPropertyValue;

        public string Item { get => _item; set => _item = value; }
        public int InStock { get => _inStock; set => _inStock = value; }
        public int? OnOrder { get => _onOrder; set => _onOrder = value; }
        public DateTime? DeliveryDate { get => _deliveryDate; set => _deliveryDate = value; }
        public int ReorderLimit { get => _reorderLimit; set => _reorderLimit = value; }
        public int DisplayInventoryIndex { get => _displayInventoryIndex; set { SetProperty(ref _displayInventoryIndex, value); } }
        public int DisplayAddIndex { get => _displayAddIndex; set { SetProperty(ref _displayAddIndex, value); } }
        public int DisplayDeleteIndex { get => _displayDeleteIndex; set { SetProperty(ref _displayDeleteIndex, value); } }
        public int DisplayUpdateIndex { get => _displayUpdateIndex; set { SetProperty(ref _displayUpdateIndex, value); } }
        public string NewPropertyValue { get => _newPropertyValue; set { SetProperty(ref _newPropertyValue, value); } }
        public string? SelectedInventory
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
            }
        }


        public ObservableCollection<InventoryAddDialogModel> CurrentPropertyValue
        {
            get => _currentPropertyValue;
            set
            {
                SetProperty(ref _currentPropertyValue, value);
                RaisePropertyChanged(nameof(CurrentPropertyValue));
                InventoryPropertyCommand.RaiseCanExecuteChanged();
            }
        }

        //  Inventory Data Display
        public ObservableCollection<InventoryAddDialogModel> InventoryData { get => _inventoryData; set { SetProperty(ref _inventoryData, value); } }
        public DelegateCommand DisplaySelectedCommand {  get; private set; }
        public DelegateCommand EditOptionsCommand { get; private set; }
        public DelegateCommand InventoryPropertyCommand { get; private set; }
        public DelegateCommand UpdateInvCommand { get; private set; }

        public InventoryDialogViewModel(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
            DisplaySelectedCommand = new DelegateCommand(DisplaySelected, CanClickSelection);
            EditOptionsCommand = new DelegateCommand(DisplayEditOptions, CanClickInventory);
            InventoryPropertyCommand = new DelegateCommand(PropertySelectionFunc);
            UpdateInvCommand = new DelegateCommand(UpdateInv);
            InventoryData = new ObservableCollection<InventoryAddDialogModel>();
            InvItems = new ObservableCollection<InventoryAddDialogModel>();
            CurrentPropertyValue = new ObservableCollection<InventoryAddDialogModel>();
        }

        public void UpdateInv()
        {
            _dataRepository.UpdateInventory();
        }
        
        public void PropertySelectionFunc()
        {
            string value1 = PropertySelection.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            string value2 = SelectedInventory.Replace("System.Windows.Controls.ComboBoxItem: ", "");

            switch (value1)
            {
                case "Item":
                    CurrentPropertyValue.Clear();
                    CurrentPropertyValue.AddRange(_dataRepository.GetItemValue(value2));
                    InvDisplayPath = "Item";
                    break;

                case "In_Stock":
                    CurrentPropertyValue.Clear();
                    CurrentPropertyValue.AddRange(_dataRepository.GetStockValue(value2));
                    InvDisplayPath = "In_Stock";
                    break;
                case "On_Order":
                    CurrentPropertyValue.Clear();
                    CurrentPropertyValue.AddRange(_dataRepository.GetOnOrderValue(value2));
                    InvDisplayPath = "On_Order";
                    break;
                case "Delivery_Date":
                    CurrentPropertyValue.Clear();
                    CurrentPropertyValue.AddRange(_dataRepository.GetDeliveryDateValue(value2));
                    InvDisplayPath = "Delivery_Date";
                    break;
                case "Reorder_Limit":
                    CurrentPropertyValue.Clear();
                    CurrentPropertyValue.AddRange(_dataRepository.GetReorderLimitValue(value2));
                    InvDisplayPath = "Reorder_Limit";
                    break;
            }
        }

        public void DisplayEditOptions()
        {
            string replacement = SelectedInventory.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            switch (replacement)
            {
                case "Sunglasses":
                    InvItems.Clear();
                    InvItems.AddRange(_dataRepository.GetSelectedInvItem(replacement));
                    
                    break;
                case "Spoon":
                    InvItems.Clear();
                    InvItems.AddRange(_dataRepository.GetSelectedInvItem(replacement));
                    SelectedInventory = null;
                    break;
                case "Flashlight":
                    InvItems.Clear();
                    InvItems.AddRange(_dataRepository.GetSelectedInvItem(replacement));
                    SelectedInventory = null;
                    break;
                case "Hat":
                    InvItems.Clear();
                    InvItems.AddRange(_dataRepository.GetSelectedInvItem(replacement));
                    SelectedInventory = null;
                    break;
                case "Backpack":
                    InvItems.Clear();
                    InvItems.AddRange(_dataRepository.GetSelectedInvItem(replacement));
                    SelectedInventory = null;
                    break;
                case "Gloves":
                    InvItems.Clear();
                    InvItems.AddRange(_dataRepository.GetSelectedInvItem(replacement));
                    SelectedInventory = null;
                    break;
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
