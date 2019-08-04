using GameStore.Models.Interface;
using GameStore.Models.Entity;
using System.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class ContactRepository : IContactRepository
    {
        private const string Add_NewSP = "Contact_Add_New";
        private const string Verify_EmailSP = "Contact_Verify_Email";
        private const string Delete_By_IdSP = "Contact_Delete_By_Id";

        public async Task<int> Delete_By_Id(long Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> Verify_Email(long Id, int OTP)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteScalarAsync<bool>(Verify_EmailSP, new { Id, OTP }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Contact> Add_New(Contact obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Contact>(Add_NewSP, new { obj.Name, obj.Email, obj.Phone_No, obj.Contact_For, obj.Message, obj.OTP }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
