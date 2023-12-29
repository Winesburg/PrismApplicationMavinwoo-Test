using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismApplicationMavinwoo_Test.core.Models
{
    //  Not currently in use -- for future use when utilize IEventAggregator
    public class OrderLinesDialogModel
    {
        public string Item { get; set; }
        public decimal Price { get; set; }
        public int Units_Sold { get; set; }
    }
}
