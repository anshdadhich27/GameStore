using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class Common_Name_UrlRepository : ICommon_Name_UrlRepository
    {
        private const string Add_NewSP = "Common_Name_Url_Add_New";
        private const string Update_By_IdSP = "Common_Name_Url_Update_By_Id";
        private const string Delete_By_IdSP = "Common_Name_Url_Delete_By_Id";
        private const string Get_AllSP = "Common_Name_Url_Get_All";
        private const string Get_All_By_TypeSP = "Common_Name_Url_Get_All_By_Type";


        public async Task<IEnumerable<Common_Name_Url>> Get_All_By_Type(string type)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Common_Name_Url>(Get_All_By_TypeSP, new { type }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Common_Name_Url>> Get_All_Async()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Common_Name_Url>(Get_AllSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> Delete_By_Id(int Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstAsync<bool>(Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> Add_New(Common_Name_Url obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                return await con.QueryFirstAsync<bool>(Add_NewSP, new { obj.Type, obj.Active, obj.Name, obj.NameUrl, obj.Logo }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> Update_By_Id(Common_Name_Url obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                return await con.QueryFirstAsync<bool>(Update_By_IdSP, new { obj.Type, obj.Active, obj.Id, obj.Name, obj.NameUrl, obj.Logo }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
