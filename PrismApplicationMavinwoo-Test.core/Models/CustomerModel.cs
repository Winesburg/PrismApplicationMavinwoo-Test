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
    }
}
