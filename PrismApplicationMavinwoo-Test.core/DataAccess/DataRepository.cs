using Dapper;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using MySqlX.XDevAPI.Relational;
using PrismApplicationMavinwoo_Test.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

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
        public List<CustomerAddDialogModel> AddSalesperson(string value1, string value2, decimal value3);
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
        public List<InventoryAddDialogModel> DeleteInvLine(string v);
        public int GetCurrentStock(string v);
        public int SetStock(int? v, string x);
        public List<CustomerAddDialogModel> GetCustomerForSale();
        public List<Salesperson> GetSalespersonForSale();
        public int GetSalesOrderNo();
        public void UpdateSalesOrder(int orderNo, DateTime dateSold, int salesPerson, int customer, decimal Price);
        public void UpdateOrderLines(string item, decimal price, int unitSold, int orderNo);

    }
    public class DataRepository : IDataRepository
    {

        public List<OrderInfoModel> GetData()
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                return Conn.Query<OrderInfoModel>(" select Order_No, Date_Sold, Salesperson, Customer, Price from Sales_Order ").AsList();
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

        public List<CustomerAddDialogModel> AddSalesperson(string value1, string value2, decimal value3)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<CustomerAddDialogModel> P = Conn.Query<CustomerAddDialogModel>(" INSERT INTO Salespersons (Name, State, Commission)  VALUES ('" + value1 + "', '" + value2 + "', '" + value3 + "') ").AsList();
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
        public List<InventoryAddDialogModel> DeleteInvLine(string v)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<InventoryAddDialogModel> P = Conn.Query<InventoryAddDialogModel>(" Delete from Inventory WHERE Item = '" + v + "' ").AsList();
                return P;
            }
        }
        public int GetCurrentStock(string v)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                string sql = " Select In_Stock from Inventory WHERE Item = @value1 ";
                MySqlCommand cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("@value1", v);
                int result;

                try
                {
                    Conn.Open();
                    // Use ExecuteScalar for queries that return a single value
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }

                return result;
            }
        }

        public int SetStock(int? v, string x)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                string sql = " UPDATE Inventory SET In_Stock = @value1 WHERE Item = @value2 ";
                MySqlCommand cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("@value1", v);
                cmd.Parameters.AddWithValue("@value2", x);
                int result;

                try
                {
                    Conn.Open();
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }

                return result;
            }
        }

        public List<CustomerAddDialogModel> GetCustomerForSale()
        {
            using (MySqlConnection connection = new MySqlConnection(SqlHelper.ConMySQL))
            {
                connection.Open();
                string query = "Select Customer, Name from Customer";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                List<CustomerAddDialogModel> P = connection.Query<CustomerAddDialogModel>(query).ToList();
                return P;
            }
        }

        public List<Salesperson> GetSalespersonForSale()
        {
            using (MySqlConnection connection = new MySqlConnection(SqlHelper.ConMySQL))
            {
                connection.Open();
                string query = "Select ID, Name from Salespersons";
                List<Salesperson> P = connection.Query<Salesperson>(query).ToList();
                return P;
            }
        }

        public int GetSalesOrderNo()
        {
            using(MySqlConnection connection = new MySqlConnection(SqlHelper.ConMySQL))
            {
                string query = "Select MAX(Order_No) from Sales_Order";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                int result;

                try
                {
                    connection.Open();
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
                return result;

            }
        }
        //public int GetCustomerCount()
        //{
        //    using (MySqlConnection connection = new MySqlConnection(SqlHelper.ConMySQL))
        //    {
        //        string query = "Select Count(Customer) From Customer";
        //        MySqlCommand cmd = new MySqlCommand(query, connection);
        //        int result;

        //        try
        //        {
        //            connection.Open();
        //            result = Convert.ToInt32(cmd.ExecuteScalar());
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //            throw;
        //        }
        //        return result;
        //    }
        //}

        //public int GetSalespersonCount()
        //{
        //    using (MySqlConnection connection = new MySqlConnection(SqlHelper.ConMySQL))
        //    {
        //        string query = "Select Count(Customer) From Customer";
        //        MySqlCommand cmd = new MySqlCommand(query, connection);
        //        int result;

        //        try
        //        {
        //            connection.Open();
        //            result = Convert.ToInt32(cmd.ExecuteScalar());
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //            throw;
        //        }
        //        return result;
        //    }
        //}
        public void UpdateSalesOrder(int orderNo, DateTime dateSold, int salesPerson, int customer, decimal Price)
        {
            using (MySqlConnection connection = new MySqlConnection(SqlHelper.ConMySQL))
            {
                string query = "Insert Into Sales_Order(Order_No, Date_Sold, Salesperson, Customer, Price) Value(@value1, @value2, @value3, @value4, @value5)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@value1", orderNo);
                cmd.Parameters.AddWithValue("@value2", dateSold);
                cmd.Parameters.AddWithValue("@value3", salesPerson);
                cmd.Parameters.AddWithValue("@value4", customer);
                cmd.Parameters.AddWithValue("@value5", Price);

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
        }
        public void UpdateOrderLines(string item, decimal price, int unitSold, int orderNo)
        {
            using (MySqlConnection connection = new MySqlConnection(SqlHelper.ConMySQL))
            {
                string query = "Insert Into Order_Lines (Item, Price, Units_Sold, Order_No) Value (@value1, @value2, @value3, @value4)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@value1", item);
                cmd.Parameters.AddWithValue("@value2", price);
                cmd.Parameters.AddWithValue("@value3", unitSold);
                cmd.Parameters.AddWithValue("@value4", orderNo);

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
        }
    }
}