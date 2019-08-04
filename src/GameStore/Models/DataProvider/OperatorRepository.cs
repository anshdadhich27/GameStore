using GameStore.Models.Interface;
using System.Threading.Tasks;
using GameStore.Models.Entity;
using Dapper;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace GameStore.Models.DataProvider
{
    public class OperatorRepository : IOperatorRepository
    {
        private const string Add_NewSP = "Operator_Add_New";
        private const string Get_By_CredentialsSP = "Operator_Get_By_Credentials";
        private const string Get_By_AllSP = "Operator_Get_By_All";
        private const string Update_By_IdSP = "Operator_Update_By_Id";


        public async Task<int> Update_By_Id(User obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Update_By_IdSP, new { obj.FirstName, obj.LastName, obj.Password, obj.Email, obj.Id, obj.Active }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<User>> Get_All()
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<User>(Get_By_AllSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<User> Get_By_Credentials(string Email, string Password)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                Password = EncryptionHelper.Encrypt(Password);
                return await con.QueryFirstOrDefaultAsync<User>(Get_By_CredentialsSP, new { Email, Password }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> Add_New(User obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                obj.UserKey = DbHelper.GetUniqueKey(20);
                obj.SecurityCode = DbHelper.GetUniqueKey(20);
                obj.Password = EncryptionHelper.Encrypt(obj.Password);
                return await con.ExecuteScalarAsync<bool>(Add_NewSP, new { obj.Email, obj.FirstName, obj.LastName, obj.Password, obj.SecurityCode, obj.UserKey }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
