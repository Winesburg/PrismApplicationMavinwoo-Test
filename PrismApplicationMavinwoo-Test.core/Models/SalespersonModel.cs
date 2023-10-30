using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismApplicationMavinwoo_Test.core.Models
{
    public class SalespersonModel
    {
        private int _iD;
        private int _orderNo;
        private DateTime _dateSold;
        private string _name;
        private string _customer;
        private decimal _price;

        public int ID { get => _iD; set => _iD = value; }
        public int OrderNo { get => _orderNo; set => _orderNo = value; }
        public DateTime Date_Sold { get => _dateSold; set => _dateSold = value; }
        public string Name { get => _name; set => _name = value; }
        public string Customer { get => _customer; set => _customer = value; }
        public decimal Price { get => _price; set => _price = value; }
    }
}
