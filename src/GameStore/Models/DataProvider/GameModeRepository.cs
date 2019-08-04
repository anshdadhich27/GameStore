using System.Threading.Tasks;

using GameStore.Models.Interface;
using GameStore.Models.Entity;
using System.Data;
using Dapper;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace GameStore.Models.DataProvider
{
    public class GameModeRepository : IGameModeRepository
    {
        private const string IsIdgb_Game_Mode_ExistsSP = "Game_Mode_IsIdgb_Game_Mode_Exists";
        private const string Add_New_IdgbGame_ModeSP = "Game_Mode_Add_New_IdgbGame_Mode";
        private const string GetPaginatedSP = "GameMode_GetPaginated";
        private const string Add_NewSP = "Game_Mode_Add_New";
        private const string DeleteByIdSP = "Game_Mode_DeleteById";
        private const string UpdateSP = "Game_Mode_Update";
        private const string Search_And_GetPaginatedSP = "GameMode_Search_And_GetPaginated";
        private const string Get_Last_IdgbIdSP = "SELECT TOP(1) Igdb_Id FROM GameModeTbl ORDER BY Igdb_Id DESC";
        private const string Get_ALL_ActiveSP = "GameMode_Get_ALL_Active";


        public async Task<IEnumerable<GameMode>> Get_ALL_ActiveAsync()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<GameMode>(Get_ALL_ActiveSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<long> Get_Last_IdgbId()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<long>(Get_Last_IdgbIdSP, commandType: CommandType.Text);
            }
        }

        public async Task<PaginatedEntity<GameMode>> Search_And_GetPaginatedAsync(SearchQuery obj)
        {
            var list = new PaginatedEntity<GameMode>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_And_GetPaginatedSP, new { obj.offset, obj.rows, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<GameMode>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }


        public async Task<DbResult> UpdateAsync(GameMode obj)
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
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<GameMode>(); }
                }
            }
            return result;
        }

        public async Task<PaginatedEntity<GameMode>> GetPaginatedAsync(long offset, int rows)
        {
            var list = new PaginatedEntity<GameMode>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { offset, rows }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<GameMode>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<DbResult> Add_New_IdgbGame_ModeAsync(GameMode obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Add_New_IdgbGame_ModeSP, new { obj.Games, obj.Name, obj.NameUrl, obj.Igdb_Id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<GameMode>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Add_NewAsync(GameMode obj)
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
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<GameMode>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> IsIdgb_Game_Mode_ExistsAsync(long id)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(IsIdgb_Game_Mode_ExistsSP, new { id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<GameMode>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> DeleteByIdAsync(long ID)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(DeleteByIdSP, new { ID }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                }
            }
            return result;
        }
    }
}
