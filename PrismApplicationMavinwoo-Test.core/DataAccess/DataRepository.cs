using Dapper;
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
    }
}
