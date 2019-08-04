using System.Threading.Tasks;
using GameStore.Models.Interface;
using GameStore.Models.Entity;
using System.Data;
using Dapper;
using System.Text.RegularExpressions;
using System.Linq;

namespace GameStore.Models.DataProvider
{
    public class PlatformRepository : IPlatformRepository
    {
        private const string IsIdgbPlatform_ExistsSP = "Platform_IsIdgbPlatform_Exists";
        private const string Add_New_IdgbPlatformSP = "Platform_Add_New_IdgbPlatform";
        private const string Add_NewSP = "Platform_Add_New";
        private const string DeleteByIdSP = "Platform_DeleteById";
        private const string UpdateSP = "Platform_Update";
        private const string GetPaginatedSP = "Platform_GetPaginated";
        private const string Search_And_GetPaginatedSP = "Platform_Search_And_GetPaginated";
        private const string Get_Last_IdgbIdSP = "SELECT TOP(1) Igdb_Id FROM PlatformTbl ORDER BY Igdb_Id DESC";

        public async Task<long> Get_Last_IdgbId()
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<long>(Get_Last_IdgbIdSP, commandType: CommandType.Text);
            }
        }

        public async Task<PaginatedEntity<Platform>> Search_And_GetPaginatedAsync(SearchQuery obj)
        {
            var list = new PaginatedEntity<Platform>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_And_GetPaginatedSP, new { obj.offset, obj.rows, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Platform>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<PaginatedEntity<Platform>> GetPaginatedAsync(long offset, int rows)
        {
            var list = new PaginatedEntity<Platform>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { offset, rows }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Platform>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<DbResult> UpdateAsync(Platform obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(UpdateSP, new { obj.Games, obj.Id, obj.Igdb_Id, obj.Name, obj.NameUrl }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Platform>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> DeleteByIdAsync(long Id)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(DeleteByIdSP, new { Id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                }
            }
            return result;
        }
        
        public async Task<DbResult> Add_NewAsync(Platform obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Add_NewSP, new { obj.Name, obj.NameUrl, obj.Website, obj.Summary }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Platform>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Add_New_IdgbPlatformAsync(Platform obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Add_New_IdgbPlatformSP, new { obj.Name, obj.NameUrl, obj.Alternative_name, obj.Games, obj.Generation, obj.Igdb_Id, obj.Summary, obj.Website }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Platform>(); }
                }
            }
            return result;
        }


        public async Task<DbResult> IsIdgbPlatform_ExistsAsync(long id)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(IsIdgbPlatform_ExistsSP, new { id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Platform>(); }
                }
            }
            return result;
        }
    }
}
