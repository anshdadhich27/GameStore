using Dapper;
using System.Data;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class AdsRepository : IAdsRepository
    {
        private const string Add_NewSP = "Ads_Add_New";
        private const string Update_By_IdSP = "Ads_Update_By_Id";
        private const string Delete_By_IdSP = "Ads_Delete_By_Id";
        private const string Get_AllSP = "Ads_Get_All";
        private const string Get_By_Page_NameSP = "Ads_Get_By_Page_Name";


        public async Task<Ads> Get_By_Page_Name(string PageName)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Ads>(Get_By_Page_NameSP, new { PageName }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Ads>> Get_All()
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Ads>(Get_AllSP, commandType: CommandType.StoredProcedure);
            }
        }


        public async Task<int> Delete_By_Id(int Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Update_By_Id(Ads obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Update_By_IdSP, new { obj.Id, obj.PageName, obj.AdScript, obj.Active }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Ads> Add_New(Ads obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Ads>(Add_NewSP, new { obj.Active, obj.AdScript, obj.PageName }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
