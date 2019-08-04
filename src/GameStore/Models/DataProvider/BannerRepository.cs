using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class BannerRepository : IBannerRepository
    {
        private const string Add_newSP = "Banner_Add_new";
        private const string Update_By_IdSP = "Banner_Update_By_Id";
        private const string Delete_By_IdSP = "Banner_Delete_By_Id";
        private const string Get_AllSP = "Banner_Get_All";
        private const string Get_All_ActiveSP = "Banner_Get_All_Active";

        public async Task<IEnumerable<Banner>> Get_All_Active()
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Banner>(Get_All_ActiveSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Banner>> Get_All()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Banner>(Get_AllSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Banner> Add_new(Banner obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Banner>(Add_newSP, new { obj.Active, obj.ImageUrl, obj.Link, obj.Title }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Update_By_Id(Banner obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Update_By_IdSP, new { obj.Id, obj.Active, obj.ImageUrl, obj.Link, obj.Title }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Delete_By_Id(int Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
