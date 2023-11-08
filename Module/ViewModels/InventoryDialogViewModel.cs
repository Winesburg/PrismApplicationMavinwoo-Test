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
        private ObservableCollection<InventoryAddDialogModel> _inventoryData;
        private ObservableCollection<string> _test;
        private string? _selectedInventory;

        public ObservableCollection<string> Test2 
        { 
            get => _test2;
            set
            { 
                SetProperty(ref _test2, value); 
                RaisePropertyChanged(nameof(Test2));
                EditOptionsCommand.RaiseCanExecuteChanged();
            }
        }


        private string? _selectTest;
        private ObservableCollection<string> _test2;
        private ObservableCollection<InventoryItemModel> _inventoryItems;

        public string? SelectTest
        {
            get => _selectTest;
            set 
            { 
                SetProperty(ref _selectTest, value); 
            }
        }

        public string Item { get => _item; set => _item = value; }
        public int InStock { get => _inStock; set => _inStock = value; }
        public int? OnOrder { get => _onOrder; set => _onOrder = value; }
        public DateTime? DeliveryDate { get => _deliveryDate; set => _deliveryDate = value; }
        public int ReorderLimit { get => _reorderLimit; set => _reorderLimit = value; }
        public int DisplayInventoryIndex { get => _displayInventoryIndex; set { SetProperty(ref _displayInventoryIndex, value); } }
        public int DisplayAddIndex { get => _displayAddIndex; set { SetProperty(ref _displayAddIndex, value); } }
        public int DisplayDeleteIndex { get => _displayDeleteIndex; set { SetProperty(ref _displayDeleteIndex, value); } }
        public int DisplayUpdateIndex { get => _displayUpdateIndex; set { SetProperty(ref _displayUpdateIndex, value); } }
        public ObservableCollection<InventoryItemModel> InventoryItems { get => _inventoryItems; set { SetProperty(ref _inventoryItems, value); } }
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
        public ObservableCollection<InventoryAddDialogModel> InventoryData { get => _inventoryData; set { SetProperty(ref _inventoryData, value); } }
        public ObservableCollection<string> Test { get => _test; set { SetProperty(ref _test, value); }  }
        public DelegateCommand DisplaySelectedCommand {  get; private set; }
        public DelegateCommand EditOptionsCommand { get; private set; }

        public InventoryDialogViewModel(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
            DisplaySelectedCommand = new DelegateCommand(DisplaySelected, CanClickSelection);
            EditOptionsCommand = new DelegateCommand(DisplayEditOptions, CanClickInventory);
            InventoryData = new ObservableCollection<InventoryAddDialogModel>();
            InventoryItems = new ObservableCollection<InventoryItemModel>();
            Test= new ObservableCollection<string>();
            Test2 = new ObservableCollection<string>();
        }
        
        public void GetInvColumns()
        {
            Test.Clear();
            Test.AddRange(_dataRepository.GetColumns());
        }

        public void GetInvItems()
        {
            if (InventoryItems.Any())
            {
                return;
            }

            var customers = _dataRepository.GetInventoryItems();

            if (customers is not null)
            {
                foreach(var customer in customers)
                {
                    InventoryItems.Add(customer);
                }
            }
        }

        public void DisplayEditOptions()
        {
            switch (SelectedInventory)
            {
                case "System.Windows.Controls.ComboBoxItem: Sunglasses":
                    SelectTest = "This is a test";
                    break;
                case "System.Windows.Controls.ComboBoxItem: Spoon":
                    SelectTest = "This is a test2";
                    break;
                case "System.Windows.Controls.ComboBoxItem: Flashlight":
                    SelectTest = "This is a test3";
                    break;
                case "System.Windows.Controls.ComboBoxItem: Hat":
                    SelectTest = "This is a test4";
                    break;
                case "System.Windows.Controls.ComboBoxItem: Backpack":
                    SelectTest = "This is a test5";
                    break;
                case "System.Windows.Controls.ComboBoxItem: Gloves":
                    SelectTest = "This is a test6";
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
                    GetInvItems();
                    GetInvColumns();
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
        //private void CloseDialog()
        //{
        //    var result = ButtonResult.OK;

        //    var p = new DialogParameters();
        //    p.Add("myParam", "The dialog was closed by the user");

        //    RequestClose.Invoke(new DialogResult(result, p));
        //}
        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            //Message = parameters.GetValue<string>("message");
        }
    }
}
