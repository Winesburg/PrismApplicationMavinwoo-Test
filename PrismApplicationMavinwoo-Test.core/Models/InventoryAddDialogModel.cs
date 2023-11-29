using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismApplicationMavinwoo_Test.core.Models
{
    public class InventoryAddDialogModel
    {
        //public int ID {  get; set; }
        public string Item { get; set ; }
        public int In_Stock { get; set; }
        public int? On_Order { get; set; }
        public DateTime? Delivery_Date { get; set; }
        public int Reorder_Limit { get; set; }
        public string ReturnItem()
        {
            return Item;
        }
    }
}
