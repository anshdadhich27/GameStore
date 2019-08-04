using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class AddressRepository : IAddressRepository
    {
        private const string Add_New_SP = "Address_Add_New";
        private const string Get_By_UserId_SP = "Address_Get_By_UserId";
        private const string Get_By_Id_SP = "Address_Get_By_Id";
        private const string Delete_By_Id_SP = "Address_Delete_By_Id";


        public async Task<Address> Get_By_Id(long Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Address>(Get_By_Id_SP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Delete_By_Id(long Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Delete_By_Id_SP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Address>> Get_By_UserId(long UserId)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Address>(Get_By_UserId_SP, new { UserId }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Add_New(Address obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Add_New_SP, new { obj.ISDCode, obj.PhoneNumber, obj.IP, obj.Email, obj.Title, obj.Address_Line2, obj.Address_Line3, obj.Full_Name, obj.Complete_Address, obj.Address_Line, obj.Address_Type, obj.Area, obj.City, obj.Country, obj.First_Name, obj.IsDefault, obj.Last_Name, obj.Mobile, obj.State, obj.UserId, obj.ZipCode }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Add_Multiple_address(IEnumerable<Address> list)
        {
            int result = 0;
            using (var con = DbHelper.GetSqlConnection())
            {
                var trans = con.BeginTransaction();
                result = await con.ExecuteAsync(Add_New_SP, list, trans, commandType: CommandType.StoredProcedure);
                trans.Commit();
            }
            return result;
        }
    }
}
