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
        public List<OrderInfoModel> SelectData(string selection);
    }
    public class DataRepository : IDataRepository
    {

        public List<OrderInfoModel> GetData()
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                return Conn.Query<OrderInfoModel>(" select * from Sales_Order ").AsList();
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

        public List<OrderInfoModel> SelectData(string selection)
        {
            using (MySqlConnection Conn = new MySqlConnection(SqlHelper.ConMySQL))
            {
                List<OrderInfoModel> P = Conn.Query<OrderInfoModel>(" select * from '" + selection + "'").AsList();
                return P;
            }
        }
    }
}
