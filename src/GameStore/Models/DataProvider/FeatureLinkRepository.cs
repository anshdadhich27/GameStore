using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class FeatureLinkRepository : IFeatureLinkRepository
    {
        private const string Add_NewSP = "FeatureLinks_Add_New";
        private const string Delete_By_ReferenceSP = "FeatureLinks_Delete_By_Reference";
        private const string Get_AllSP = "FeatureLinks_Get_All";


        public async Task<IEnumerable<FeatureLink>> Get_All()
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<FeatureLink>(Get_AllSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Delete_By_Reference(long Reference_Id, string Reference_Name)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Delete_By_ReferenceSP, new { Reference_Id, Reference_Name }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Add_New(IList<FeatureLink> list)
        {
            int result = 0;
            using(var con = DbHelper.GetSqlConnection())
            {
                var obj = list.FirstOrDefault();
                if (obj != null)
                {
                    var abc = Delete_By_Reference(obj.Reference_Id, obj.Reference_Name);
                    abc.Wait();
                }
                var trans = con.BeginTransaction();
                result = await con.ExecuteAsync(Add_NewSP, list, trans, commandType: CommandType.StoredProcedure);
                trans.Commit();
            }
            return result;
        }
    }
}
