using Dapper;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using PrismApplicationMavinwoo_Test.core.Models;
using System;
using System.Collections.Generic;

namespace PrismApplicationMavinwoo_Test.core.DataAccess
{
    public interface IDataRepository
    {
        public List<OrderInfoModel> GetData();
        public List<OrderInfoModel> FilterSale(DateTime start, DateTime end);
        public List<SalespersonModel> FilterSalesperson(DateTime start, DateTime end);
        public List<CustomerModel> FilterCustomer(DateTime start, DateTime end);
        public List<OrderInfoModel> SearchOrder(string keyword);
        public List<SalespersonModel> SearchSalesperson(string keyword);
        public List<CustomerModel> SearchCustomer(string keyword);
        public List<SalespersonModel> SelectSalesperson();
        public List<CustomerModel> SelectCustomers();
        public List<CustomerAddDialogModel> AddCustomers(string value1, string value2, string value3, string value4, string value5, string value6);
        public List<InventoryAddDialogModel> AddInventory(string value1, string value2, string? value3, DateTime value4, string value5);
        public List<InventoryAddDialogModel> AddInventoryNull(string value1, string value2, string value3);
        public List<InventoryAddDialogModel> GetInventory();
        public List<InventoryAddDialogModel> GetSelectedInvItem(string input);
        public List<InventoryAddDialogModel> GetItemValue(string input);
        public List<InventoryAddDialogModel> GetStockValue(string input);
        public List<InventoryAddDialogModel> GetOnOrderValue(string input);
        public List<InventoryAddDialogModel> GetDeliveryDateValue(string input);
        public List<InventoryAddDialogModel> GetReorderLimitValue(string input);
        public List<InventoryAddDialogModel> UpdateInventory(string x, string i, string v);
        public List<InventoryAddDialogModel> UpdateNullInventory(string x, string v);
        public List<InventoryAddDialogModel> UpdateDelDateInv(string x, DateTime delDate, string v);

    }
    public class DataRepository : IDataRepository
    {

        public List<OrderInfoModel> GetData()
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                return Conn.Query<OrderInfoModel>(" select Order_No, Date_Sold, Salesperson, Customer, Price from Sales_Order ").AsList() ;
            }
        }
        public List<OrderInfoModel> FilterSale(DateTime start, DateTime end)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<OrderInfoModel> P = Conn.Query<OrderInfoModel>(" select * from Sales_Order where date_sold between '" + start.ToString("yyyy'-'MM'-'dd") + "' AND '" + end.ToString("yyyy'-'MM'-'dd") + "'").AsList();
                return P;
            }
        }
        public List<SalespersonModel> FilterSalesperson(DateTime start, DateTime end)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<SalespersonModel> P = Conn.Query<SalespersonModel>(" SELECT Sales_Order.Order_No, Sales_Order.Date_Sold, Salespersons.Name, Sales_Order.Customer, Sales_Order.Price FROM Sales_Order Left JOIN Salespersons ON Sales_Order.Salesperson = Salespersons.ID where date_sold between '" + start.ToString("yyyy'-'MM'-'dd") + "' AND '" + end.ToString("yyyy'-'MM'-'dd") + "'").AsList();
                return P;
            }
        }
        public List<CustomerModel> FilterCustomer(DateTime start, DateTime end)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<CustomerModel> P = Conn.Query<CustomerModel>(" SELECT Sales_Order.Order_No, Sales_Order.Date_Sold, Sales_Order.Salesperson, Customer.Name, Sales_Order.Price FROM Sales_Order Left JOIN Customer ON Sales_Order.Customer = Customer.Customer where date_sold between '" + start.ToString("yyyy'-'MM'-'dd") + "' AND '" + end.ToString("yyyy'-'MM'-'dd") + "'").AsList();
                return P;
            }
        }

        public List<OrderInfoModel> SearchOrder(string keyword)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<OrderInfoModel> P = Conn.Query<OrderInfoModel>(" select Order_No, Date_Sold, Salesperson, Customer, Price from Sales_Order where Order_No like '%" + keyword + "%' OR Date_Sold LIKE '%" + keyword + "%' OR Salesperson LIKE '%" + keyword + "%' OR Customer LIKE '%" + keyword + "%' OR Price LIKE '%" + keyword + "%' ").AsList();
                return P;
            }
        }
        public List<SalespersonModel> SearchSalesperson(string keyword)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<SalespersonModel> P = Conn.Query<SalespersonModel>(" SELECT Sales_Order.Order_No, Sales_Order.Date_Sold, Salespersons.Name, Sales_Order.Customer, Sales_Order.Price FROM Sales_Order Left JOIN Salespersons ON Sales_Order.Salesperson = Salespersons.ID where Sales_Order.Order_No like '%" + keyword + "%' OR Sales_Order.Date_Sold LIKE '%" + keyword + "%' OR Salespersons.Name LIKE '%" + keyword + "%' OR Sales_Order.Customer LIKE '%" + keyword + "%' OR Sales_Order.Price LIKE '%" + keyword + "%' ").AsList();
                return P;
            }
        }
        public List<CustomerModel> SearchCustomer(string keyword)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<CustomerModel> P = Conn.Query<CustomerModel>(" SELECT Sales_Order.Order_No, Sales_Order.Date_Sold, Sales_Order.Salesperson, Customer.Name, Sales_Order.Price FROM Sales_Order Left JOIN Customer ON Sales_Order.Customer = Customer.Customer where Sales_Order.Order_No like '%" + keyword + "%' OR Sales_Order.Date_Sold LIKE '%" + keyword + "%' OR Sales_Order.Salesperson LIKE '%" + keyword + "%' OR Customer.Name LIKE '%" + keyword + "%' OR Sales_Order.Price LIKE '%" + keyword + "%' ").AsList();
                return P;
            }
        }

        public List<SalespersonModel> SelectSalesperson()
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<SalespersonModel> P = Conn.Query<SalespersonModel>(" SELECT Sales_Order.Order_No, Sales_Order.Date_Sold, Salespersons.Name, Sales_Order.Customer, Sales_Order.Price FROM Sales_Order Left JOIN Salespersons ON Sales_Order.Salesperson = Salespersons.ID ").AsList();
                return P;
            }
        }

        public List<CustomerModel> SelectCustomers()
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<CustomerModel> P = Conn.Query<CustomerModel>(" SELECT Sales_Order.Order_No, Sales_Order.Date_Sold, Sales_Order.Salesperson, Customer.Name, Sales_Order.Price FROM Sales_Order Left JOIN Customer ON Sales_Order.Customer = Customer.Customer ").AsList();
                return P;
            }
        }
        public List<CustomerAddDialogModel> AddCustomers(string value1, string value2, string value3, string value4, string value5, string value6)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<CustomerAddDialogModel> P = Conn.Query<CustomerAddDialogModel>(" INSERT INTO Customer (Name, Address, City, State, Zip, Phone)  VALUES ('" + value1 + "', '" + value2 + "', '" + value3 + "', '" + value4 + "', '" + value5 + "', '" + value6 + "') ").AsList();
                return P;
            }
        }
        

        public List<InventoryAddDialogModel> AddInventory(string value1, string value2, string? value3, DateTime value4, string value5)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<InventoryAddDialogModel> P = Conn.Query<InventoryAddDialogModel>(" INSERT INTO Inventory (Item, In_Stock, On_Order, Delivery_Date, Reorder_Limit)  VALUES ('" + value1 + "', '" + value2 + "', '" + value3 + "', '" + value4.ToString("yyyy'-'MM'-'dd") + "', '" + value5 + "') ").AsList();
                return P;
            }
        }
        public List<InventoryAddDialogModel> AddInventoryNull(string value1, string value2, string value3)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<InventoryAddDialogModel> P = Conn.Query<InventoryAddDialogModel>(" INSERT INTO Inventory (Item, In_Stock, Reorder_Limit)  VALUES ('" + value1 + "', '" + value2 + "', '" + value3 + "') ").AsList();
                return P;
            }
        }

        public List<InventoryAddDialogModel> GetInventory()
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<InventoryAddDialogModel> P = Conn.Query<InventoryAddDialogModel>(" SELECT * from Inventory ").AsList();
                return P;
            }
        }

        public List<InventoryAddDialogModel> GetSelectedInvItem(string input)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<InventoryAddDialogModel> P = Conn.Query<InventoryAddDialogModel>(" SELECT Item, In_Stock, On_Order, Delivery_Date, Reorder_Limit from Inventory where Item = '" + input + "'  ").AsList();
                return P;
            }
        }


        public List<InventoryAddDialogModel> GetItemValue(string input)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<InventoryAddDialogModel> P = Conn.Query<InventoryAddDialogModel>(" SELECT Item FROM Inventory WHERE Item = '" + input + "'  ").AsList();
                return P;
            }
        }
        public List<InventoryAddDialogModel> GetStockValue(string input)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<InventoryAddDialogModel> P = Conn.Query<InventoryAddDialogModel>(" SELECT In_Stock FROM Inventory WHERE Item = '" + input + "'  ").AsList();
                return P;
            }
        }
        public List<InventoryAddDialogModel> GetOnOrderValue(string input)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<InventoryAddDialogModel> P = Conn.Query<InventoryAddDialogModel>(" SELECT On_Order FROM Inventory WHERE Item = '" + input + "'  ").AsList();
                return P;
            }
        }
        public List<InventoryAddDialogModel> GetDeliveryDateValue(string input)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<InventoryAddDialogModel> P = Conn.Query<InventoryAddDialogModel>(" SELECT Delivery_Date FROM Inventory WHERE Item = '" + input + "'  ").AsList();
                return P;
            }
        }
        public List<InventoryAddDialogModel> GetReorderLimitValue(string input)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<InventoryAddDialogModel> P = Conn.Query<InventoryAddDialogModel>(" SELECT Reorder_Limit FROM Inventory WHERE Item = '" + input + "'  ").AsList();
                return P;
            }
        }
        public List<InventoryAddDialogModel> UpdateInventory(string x, string i, string v)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<InventoryAddDialogModel> P = Conn.Query<InventoryAddDialogModel>(" UPDATE Inventory SET " + x + " = '" + i + "' WHERE Item = '" + v + "' ").AsList();
                return P;
            }
        }
        public List<InventoryAddDialogModel> UpdateNullInventory(string x, string v)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<InventoryAddDialogModel> P = Conn.Query<InventoryAddDialogModel>(" UPDATE Inventory SET " + x + " = null WHERE Item = '" + v + "' ").AsList();
                return P;
            }
        }
        public List<InventoryAddDialogModel> UpdateDelDateInv (string x, DateTime delDate, string v)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<InventoryAddDialogModel> P = Conn.Query<InventoryAddDialogModel>(" UPDATE Inventory SET " + x + " = '" + delDate.ToString("yyyy'-'MM'-'dd") + "' WHERE Item = '" + v + "' ").AsList();
                return P;
            }
        }
    }
}