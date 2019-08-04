using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class CreditRepository : ICreditRepository
    {
        private const string Add_NewSP = "Credits_Add_New";
        private const string Get_logs_By_User_IdSP = "Credit_Get_logs_By_User_Id";
        private const string Unblock_By_Transaction_IdSP = "Credits_Unblock_By_Transaction_Id";

        public async Task<int> Unblock_By_Transaction_Id(string Trancaction_Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Unblock_By_Transaction_IdSP, new { Trancaction_Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Credit>> Get_logs_By_User_Id(long UserId)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Credit>(Get_logs_By_User_IdSP, new { UserId }, commandType: CommandType.StoredProcedure);
            }
        }


        public async Task<int> Add_New(Credit obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {                
                return await con.ExecuteAsync(Add_NewSP, new { obj.Transaction_For, obj.IsBlocked, obj.UserId, obj.Amount, obj.Comments, obj.Trancaction_Id, obj.Transaction_Type }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
