using Dapper;
using MySql.Data.MySqlClient;
using PrismApplicationMavinwoo_Test.core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PrismApplicationMavinwoo_Test.core.DataAccess
{
    public interface IDataRepository
    {
        public List<OrderInfoModel> GetData();
        public List<OrderInfoModel> FilterData(DateTime start, DateTime end);

        public List<OrderInfoModel> SearchData(string keyword);
        public List<SalespersonModel> SelectSalesperson();
    }
    public class DataRepository : IDataRepository
    {

        public List<OrderInfoModel> GetData()
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                return Conn.Query<OrderInfoModel>(" select * from Sales_Order ").AsList() ;
            }
        }
        public List<OrderInfoModel> FilterData(DateTime start, DateTime end)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<OrderInfoModel> P = Conn.Query<OrderInfoModel>(" select * from Sales_Order where date_sold between '" + start.ToString("yyyy'-'MM'-'dd") + "' AND '" + end.ToString("yyyy'-'MM'-'dd") + "'").AsList();
                return P;
            }
        }

        public List<OrderInfoModel> SearchData(string keyword)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<OrderInfoModel> P = Conn.Query<OrderInfoModel>(" select * from Sales_Order where ID like '" + keyword + "' ").AsList();
                return P;
            }
        }

        public List<SalespersonModel> SelectSalesperson()
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<SalespersonModel> P = Conn.Query<SalespersonModel>(" SELECT Sales_Order.ID, Sales_Order.Order_No, Sales_Order.Date_Sold, Salespersons.Name, Sales_Order.Customer, Sales_Order.Price FROM Sales_Order Left JOIN Salespersons ON Sales_Order.Salesperson = Salespersons.ID ").AsList();
                return P;
            }
        }
    }
}
