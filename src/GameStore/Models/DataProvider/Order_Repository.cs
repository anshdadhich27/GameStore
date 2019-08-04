using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

namespace GameStore.Models.DataProvider
{
    public class Order_Repository : IOrder_Repository
    {
        private const string Cancelation_Request_Add_NewSP = "Cancelation_Request_Add_New";
        private const string Get_Order_Cancelation_ListSP = "Get_Order_Cancelation_List";


        public async Task<PaginatedEntity<Cancelation_Order>> Get_Cancelation_Orders_By_Query(string query)
        {
            var list = new PaginatedEntity<Cancelation_Order>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(query, commandType: CommandType.Text))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Cancelation_Order>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<PaginatedEntity<Cancelation_Order>> Get_Order_Cancelation_List(long offset, int rows)
        {
            var list = new PaginatedEntity<Cancelation_Order>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Get_Order_Cancelation_ListSP, new { offset, rows }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Cancelation_Order>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<int> Add_New_Cancelation_Request(Cancelation_Order obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                obj.Id = DbHelper.GetUniqueKey(); obj.Status = "Cancelation Request";
                return await con.ExecuteAsync(Cancelation_Request_Add_NewSP, new { obj.Id, obj.Shipment_Id, obj.SKU, obj.Status, obj.Tracking_Id }, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
