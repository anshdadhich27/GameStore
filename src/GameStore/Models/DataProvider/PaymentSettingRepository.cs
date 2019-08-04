using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Data;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class PaymentSettingRepository : IPaymentSettingRepository
    {
        private const string GetSP = "PaymentSettings_Get";
        private const string UpdateSP = "PaymentSettings_Update";

        public async Task<int> Update(PaymentSetting obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(UpdateSP, new { obj.Id, obj.Tax, obj.ShippingCharge, obj.Credit_Ratio }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<PaymentSetting> Get()
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<PaymentSetting>(GetSP, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
