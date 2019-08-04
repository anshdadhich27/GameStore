using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class DealRepository : IDealRepository
    {
        private const string Add_newSP = "Deal_Add_new";
        private const string Update_By_IdSP = "Deal_Update_By_Id";
        private const string Delete_By_IdSP = "Deal_Delete_By_Id";
        private const string Product_Add_NewSP = "Deal_Product_Add_New";
        private const string Product_Delete_By_IdSP = "Deal_Product_Delete_By_Id";
        private const string Get_By_IdSP = "Deal_Get_By_Id";
        private const string GetPaginatedSP = "Deal_GetPaginated";
        private const string Get_Product_By_IdSP = "Deal_Product_Get_By_Id";
        private const string Search_And_GetPaginatedSP = "Deal_Search_And_GetPaginated";


        public async Task<IEnumerable<Deal_Product>> Get_Product_By_Id(int Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Deal_Product>(Get_Product_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<PaginatedEntity<Deal>> GetPaginated(int rows, long offset)
        {
            var list = new PaginatedEntity<Deal>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { rows, offset }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Deal>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<PaginatedEntity<Deal>> Search_And_GetPaginated(SearchQuery obj)
        {
            var list = new PaginatedEntity<Deal>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_And_GetPaginatedSP, new { obj.rows, obj.offset, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Deal>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<Deal> Get_By_Id(int Id)
        {
            Deal deal = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Get_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure))
                {
                    deal = multiple.Read<Deal>().FirstOrDefault();
                    if (deal != null) { deal.ProductList = multiple.Read<Deal_Product>().ToList(); }
                }
            }
            return deal;
        }

        public async Task<int> Delete_By_Id(int Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Deal> Add_new(Deal obj)
        {
            Deal deal = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                deal = await con.QueryFirstOrDefaultAsync<Deal>(Add_newSP, new { obj.Active, obj.End_Date, obj.Name, obj.Price, obj.Start_Date }, commandType: CommandType.StoredProcedure);
                if (deal != null)
                {
                    if (obj.ProductList.Count > 0)
                    {
                        foreach (var x in obj.ProductList) { x.Deal_Id = deal.Id; }
                        var trans = con.BeginTransaction();
                        await con.ExecuteAsync(Product_Add_NewSP, obj.ProductList, trans, commandType: CommandType.StoredProcedure);
                        trans.Commit();
                    }                    
                }
            }
            return deal;
        }

        public async Task<Deal> Update_By_Id(Deal obj)
        {
            Deal deal = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                con.Execute(Product_Delete_By_IdSP, new { obj.Id }, commandType: CommandType.StoredProcedure);
                deal = await con.QueryFirstOrDefaultAsync<Deal>(Update_By_IdSP, new { obj.Id, obj.Active, obj.End_Date, obj.Name, obj.Price, obj.Start_Date }, commandType: CommandType.StoredProcedure);
                if (deal != null)
                {
                    if (obj.ProductList.Count > 0)
                    {
                        foreach (var x in obj.ProductList) { x.Deal_Id = deal.Id; }
                        var trans = con.BeginTransaction();
                        con.Execute(Product_Add_NewSP, obj.ProductList, trans, commandType: CommandType.StoredProcedure);
                        trans.Commit();
                    }
                }
            }
            return deal;
        }
    }
}
