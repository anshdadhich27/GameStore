using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Text.RegularExpressions;

namespace GameStore.Models.DataProvider
{
    public class GamePlatformRepository : IGamePlatformRepository
    {
        private const string IsIdgbPlatform_ExistsSP = "GamePlatform_IsIdgbPlatform_Exists";
        private const string Add_New_From_IdgbSP = "GamePlatform_Add_New_From_Idgb";
        private const string Add_NewSP = "GamePlatforms_Add_New";
        private const string UpdateSP = "GamePlatforms_Update";
        private const string DeleteByIdSP = "GamePlatforms_DeleteById";
        private const string GetPaginatedSP = "GamePlatform_GetPaginated";
        private const string Search_And_GetPaginatedSP = "GamePlatform_Search_And_GetPaginated";
        private const string GetByUrlSP = "GamePlatforms_GetByUrl";
        private const string GetTopNSP = "GamePlatforms_GetTopN";
        private const string Get_Last_IdgbIdSP = "SELECT TOP(1) Igdb_Id FROM GamePlatformsTbl ORDER BY Igdb_Id DESC";
        private const string Get_Supported_N_PlatformsSP = "SELECT TOP({0}) Id, Name, NameUrl, Igdb_Id, LogoUrl, Active FROM GamePlatformsTbl Where Id IN ({1}) AND Active=1  ORDER BY Position ASC;";
        private const string Get_ALL_ActiveSP = "GamePlatforms_Get_ALL_Active";
        private const string GetByIdSP = "GamePlatforms_GetById";


        public async Task<GamePlatform> GetById(int Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<GamePlatform>(GetByIdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<GamePlatform>> Get_ALL_ActiveAsync()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<GamePlatform>(Get_ALL_ActiveSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<GamePlatform>> Get_Supported_N_Platforms(int top, string Ids)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<GamePlatform>(string.Format(Get_Supported_N_PlatformsSP, top, Ids), commandType: CommandType.Text);
            }
        }

        public async Task<long> Get_Last_IdgbId()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<long>(Get_Last_IdgbIdSP, commandType: CommandType.Text);
            }
        }

        public async Task<DbResult> IsIdgbPlatform_ExistsAsync(long id)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(IsIdgbPlatform_ExistsSP, new { id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<GamePlatform>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Add_New_From_IdgbAsync(GamePlatform obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Add_New_From_IdgbSP, new { obj.Name, obj.NameUrl, obj.Alternative_name, obj.Games, obj.Generation, obj.Igdb_Id, obj.PageContent, obj.Website }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<GamePlatform>(); }
                }
            }
            return result;
        }


        public async Task<IEnumerable<GamePlatform>> GetTopNAsync(int N)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<GamePlatform>(GetTopNSP, new { N }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<GamePlatform> GetByUrlAsync(string Url)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<GamePlatform>(GetByUrlSP, new { Url }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<PaginatedEntity<GamePlatform>> Search_And_GetPaginatedAsync(SearchQuery obj)
        {
            var list = new PaginatedEntity<GamePlatform>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_And_GetPaginatedSP, new { obj.offset, obj.rows, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<GamePlatform>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }
        public async Task<PaginatedEntity<GamePlatformExport>> GetGamePlatform_By_Custom_QueryAsync(string query)
        {
            var list = new PaginatedEntity<GamePlatformExport>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(query, commandType: CommandType.Text))
                {
                    list.PagedSet = multiple.Read<GamePlatformExport>();
                }
            }
            return list;
        }
        
        public async Task<PaginatedEntity<GamePlatform>> GetPaginatedAsync(long offset, int rows)
        {
            var list = new PaginatedEntity<GamePlatform>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { offset, rows }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<GamePlatform>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<DbResult> DeleteByIdAsync(int Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<DbResult>(DeleteByIdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<DbResult> Add_NewAsync(GamePlatform obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Add_NewSP, new { obj.Code, obj.LogoUrl, obj.Banner, obj.Name, obj.NameUrl, obj.Active, obj.MetaDescription, obj.MetaKeyword, obj.PageContent, obj.PageTitle, obj.Position }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<GamePlatform>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> UpdateAsync(GamePlatform obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(UpdateSP, new { obj.Code, obj.LogoUrl, obj.Banner, obj.Id, obj.Name, obj.NameUrl, obj.Active, obj.MetaDescription, obj.MetaKeyword, obj.PageContent, obj.PageTitle, obj.Position }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<GamePlatform>(); }
                }
            }
            return result;
        }
    }
}
