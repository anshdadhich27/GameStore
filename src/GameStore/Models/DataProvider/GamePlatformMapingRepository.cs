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
    public class GamePlatformMapingRepository : IGamePlatformMapingRepository
    {
        private const string Add_NewSP = "Game_Platform_Mapings_Add_New";
        private const string Remove_MapingSP = "Game_Platform_Mapings_Remove";
        private const string Get_By_Game_IdSP = "Game_Platform_Mapings_Get_By_Game_Id";
        private const string Remove_Maping_By_Sku_SP = "Game_Mapings_Remove";

        public async Task<int> Remove_Maping_By_Sku(string sku)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Remove_Maping_By_Sku_SP, new { sku }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Remove_Maping_Async(long Game_Id, int Platform_Id, int Condition_Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Remove_MapingSP, new { Game_Id, Platform_Id, Condition_Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<GamePlatformMaping>> Get_By_Game_Id(long Game_Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<GamePlatformMaping>(Get_By_Game_IdSP, new { Game_Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Add_New_Maping(GamePlatformMaping obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Add_NewSP, new { obj.IsBestSelling, obj.Available_To_Buy, obj.Available_To_Sell, obj.Buying_Price, obj.Selling_Price, obj.Original_Price, obj.SKU, obj.Platform_Name, obj.Quantity, obj.Game_Id, obj.Platform_Id, obj.SNo, obj.Condition, obj.Condition_Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Add_New_Mapings(IEnumerable<GamePlatformMaping> list)
        {
            int result = 0;
            using (var con = DbHelper.GetSqlConnection())
            {
                var trans = con.BeginTransaction();
                result = await con.ExecuteAsync(Add_NewSP, list, trans, commandType: CommandType.StoredProcedure);
                trans.Commit();
            }
            return result;
        }
    }
}
