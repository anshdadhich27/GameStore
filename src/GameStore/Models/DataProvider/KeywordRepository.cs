using GameStore.Models.Interface;
using GameStore.Models.Entity;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace GameStore.Models.DataProvider
{
    public class KeywordRepository : IKeywordRepository
    {
        private const string Add_New_IgdbKeywordSP = "Keyword_Add_New_IgdbKeyword";
        private const string IsIgdbKeywords_ExistsSP = "Keyword_IsIgdbKeywords_Exists";
        private const string Add_NewSP = "Keywords_Add_New";
        private const string DeleteByIdSP = "Keywords_DeleteById";
        private const string UpdateSP = "Keywords_Update";
        private const string GetPaginatedSP = "Keywords_GetPaginated";
        private const string Search_And_GetPaginatedSP = "Keywords_Search_And_GetPaginated";
        private const string Get_Last_IdgbIdSP = "SELECT TOP(1) Igdb_Id FROM KeywordsTbl ORDER BY Igdb_Id DESC";
        private const string Get_ALL_ActiveSP = "Keywords_Get_ALL_Active";


        public async Task<IEnumerable<Keyword>> Get_ALL_ActiveAsync()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Keyword>(Get_ALL_ActiveSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<long> Get_Last_IdgbId()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<long>(Get_Last_IdgbIdSP, commandType: CommandType.Text);
            }
        }

        public async Task<PaginatedEntity<Keyword>> Search_And_GetPaginatedAsync(SearchQuery obj)
        {
            var list = new PaginatedEntity<Keyword>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_And_GetPaginatedSP, new { obj.offset, obj.rows, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Keyword>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<PaginatedEntity<Keyword>> GetPaginatedAsync(long offset, int rows)
        {
            var list = new PaginatedEntity<Keyword>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { offset, rows }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Keyword>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }
        
        public async Task<DbResult> UpdateAsync(Keyword obj)
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
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Keyword>(); }
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

        public async Task<DbResult> Add_NewAsync(Keyword obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Add_NewSP, new { obj.Name, obj.NameUrl }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Keyword>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Add_New_IgdbKeywordAsync(Keyword obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Add_New_IgdbKeywordSP, new { obj.Games, obj.Name, obj.NameUrl, obj.Igdb_Id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Keyword>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> IsIgdbKeywords_ExistsAsync(long id)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(IsIgdbKeywords_ExistsSP, new { id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Keyword>(); }
                }
            }
            return result;
        }
    }
}
