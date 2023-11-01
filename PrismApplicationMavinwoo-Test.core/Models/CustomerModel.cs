using System;

namespace PrismApplicationMavinwoo_Test.core.Models
{
    public class CustomerModel
    {
        //public int ID { get; set; }
        public int Order_No { get; set; }
        public DateTime Date_Sold { get; set; }
        public int Salesperson { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }



        //public int ID { get => _iD; set => _iD = value; }
        //public int OrderNo { get => _orderNo; set => _orderNo = value; }
        //public DateTime DateSold { get => _dateSold; set => _dateSold = value; }
        //public int Salesperson { get => _salesperson; set => _salesperson = value; }
        //public string Customer { get => _customer; set => _customer = value; }
        //public decimal Price { get => _price; set => _price = value; }
    }
}
