using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace GameStore.Models.DataProvider
{
    public class VideoRepository : IVideoRepository
    {
        private const string Add_NewSP = "Videos_Add_New";
        private const string Remove_By_IdSP = "Videos_Remove_By_Id";
        private const string Add_or_UpdateSP = "Videos_Add_or_Update";
        private const string Get_By_ReferenceId_Video_OfSP = "Videos_Get_By_ReferenceId_Video_Of";


        public async Task<int> Remove_By_Id_Async(long Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Remove_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }


        public async Task<int> Add_or_Update_Multiple(IEnumerable<Video> list)
        {
            int result = 0;
            using (var con = DbHelper.GetSqlConnection())
            {
                var trans = con.BeginTransaction();
                result = await con.ExecuteAsync(Add_or_UpdateSP, list, trans, commandType: CommandType.StoredProcedure);
                trans.Commit();
            }
            return result;
        }

        public async Task<DbResult> Videos_Get_By_ReferenceId_Video_Of(long ReferenceId, string Video_of)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Get_By_ReferenceId_Video_OfSP, new { Video_of, ReferenceId }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.Read<Video>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Add_NewAsync(Video obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Add_NewSP, new { obj.Reference_Id, obj.Name, obj.Snapshot, obj.Video_Id, obj.Video_of }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Video>(); }
                }
            }
            return result;
        }
    }
}
