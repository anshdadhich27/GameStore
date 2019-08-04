using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class WishListRepository : IWishListRepository
    {
        private const string Add_NewSP = "WishList_Add_New";
        private const string Get_All_By_User_IdSP = "WishList_Get_All_By_User_Id";
        private const string Delete_By_IdSP = "WishList_Delete_By_Id";


        public async Task<int> Delete_By_Id(long Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<WishList_DTO> Get_All_By_User_Id(long UserId)
        {
            var data = new WishList_DTO();
            using(var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Get_All_By_User_IdSP, new { UserId }, commandType: CommandType.StoredProcedure))
                {
                    data.GameList = multiple.Read<WishList>();
                    data.ConsoleList = multiple.Read<WishList>();
                    data.AccessoriesList = multiple.Read<WishList>();
                }
            }
            return data;
        }

        public async Task<bool> Add_New(WishList obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteScalarAsync<bool>(Add_NewSP, new { obj.Price, obj.SKU, obj.ProductType_Id, obj.Product_Id, obj.UserId, obj.WishType }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
