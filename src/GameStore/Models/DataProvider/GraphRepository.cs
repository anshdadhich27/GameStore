using GameStore.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Models.Entity;
using System.Data;
using Dapper;

namespace GameStore.Models.DataProvider
{
    public class GraphRepository : IGraphRepository
    {
        private const string Get_Selling_Count_By_Product_TypeSP = "Graph_Get_Selling_Count_By_Product_Type";
        private const string Get_Buying_Count_By_Product_TypeSP = "Graph_Get_Buying_Count_By_Product_Type";
        private const string Get_Pending_Order_CountSP = "Graph_Get_Pending_Order_Count";
        private const string Get_Todays_Order_CountSP = "Graph_Get_Todays_Order_Count";

        public DateWiseGraph Get_Order_Count_By_Month(string cartType, int month, int year)
        {           
            var obj = new DateWiseGraph();
            using(var con = DbHelper.GetSqlConnection())
            {
                var dates = con.Query<DateTime>(string.Format("SELECT [Dates] from Get_All_Days_Of_Month({0}, {1})", month, year), commandType: CommandType.Text).ToList();
                foreach(var x in dates) { obj.labels.Add(x.ToString("dd MMM yyyy")); }
                var productTypes = con.Query<string>("SELECT DISTINCT Product_Type FROM [Shopping_Cart_Items_Tbl]", commandType: CommandType.Text);
                if (productTypes != null)
                {
                    var dataSet = new List<Data_Item>();
                    foreach(var x in productTypes)
                    {
                        var dataItem = new Data_Item { label = x };
                        dataItem.data = con.Query<int>(string.Format("SELECT ISNULL(t2.[Count], 0) as 'data' from Get_All_Days_Of_Month({0}, {1}) t1 LEFT JOIN Get_DateWise_Order_Count('{2}', '{3}') t2 on t1.[Dates]=t2.[Date]", month, year, cartType, x), commandType: CommandType.Text).ToList();
                        dataSet.Add(dataItem);
                    }
                    obj.datasets = dataSet;
                }
            }
            return obj;
        }

        public async Task<IEnumerable<Total_Count>> Get_Selling_Count_By_Product_Type()
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Total_Count>(Get_Selling_Count_By_Product_TypeSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Total_Count>> Get_Buying_Count_By_Product_Type()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Total_Count>(Get_Buying_Count_By_Product_TypeSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Total_Count>> Get_Pending_Order_Count()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Total_Count>(Get_Pending_Order_CountSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Total_Count>> Get_Todays_Order_Count()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Total_Count>(Get_Todays_Order_CountSP, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
