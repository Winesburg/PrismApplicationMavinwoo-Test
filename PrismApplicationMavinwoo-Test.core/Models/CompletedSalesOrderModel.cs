﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace PrismApplicationMavinwoo_Test.core.Models
{
    public class CompletedSalesOrderModel
    {
        //public int ID { get; }
        public string Date_Sold { get; set; }
        public string Salesperson { get; set; }
        public string Customer { get; set; }
        public string Item { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public decimal? Total {  get; set; }
        public bool IsClicked { get; set; }
        public string ReturnItem()
        {
            return Item;
        }

        public CompletedSalesOrderModel(string DS, string SP, string C, string I, decimal? P, int? Q)
        {
            //ID = I_D;
            Date_Sold = DS;
            Salesperson = SP;
            Customer = C;
            Item = I;
            Price = P;
            Quantity = Q;
            Total = (decimal)(Price * Quantity);
        }
    }
}
