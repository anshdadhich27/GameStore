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
    public class Guest_User_Repository : IGuest_User_Repository
    {
        private const string Add_NewSP = "Guest_User_Add_New";
        private const string DeleteSP = "Guest_User_Delete";
        private const string CheckSP = "Guest_User_Check";


        public async Task<long> Add_New_Async(Guest_User obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteScalarAsync<long>(Add_NewSP, new { obj.Cart_Id, obj.Email, obj.OTP }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> Check_Async(Guest_User obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteScalarAsync<bool>(CheckSP, new { obj.Cart_Id, obj.Email, obj.OTP }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Delete_Async(Guest_User obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(DeleteSP, new { obj.Cart_Id, obj.Email, obj.OTP }, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
