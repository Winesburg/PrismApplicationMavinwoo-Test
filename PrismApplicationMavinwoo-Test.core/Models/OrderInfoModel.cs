using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismApplicationMavinwoo_Test.core.Models
{
    public class OrderInfoModel
    {
        private int iD;
        private int order_No;
        private DateTime date_Sold;
        private int salesperson;
        private int customer;
        private int price;

        public int ID { get => iD; set => iD = value; }
        public int Order_No { get => order_No; set => order_No = value; }
        public DateTime Date_Sold { get => date_Sold; set => date_Sold = value; }
        public int Salesperson { get => salesperson; set => salesperson = value; }
        public int Customer { get => customer; set => customer = value; }
        public int Price { get => price; set => price = value; }

    }
}
