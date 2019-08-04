using GameStore.Models.Interface;
using GameStore.Models.Entity;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;

namespace GameStore.Models.DataProvider
{
    public class GameEngineRepository : IGameEngineRepository
    {
        private const string IsIdgb_GameEngine_ExistsSP = "Game_Engine_IsIdgb_GameEngine_Exists";
        private const string Add_New_IdgbGame_EngineSP = "Game_Engine_Add_New_IdgbGame_Engine";
        private const string Add_NewSP = "Game_Engines_Add_New";
        private const string DeleteByIdSP = "Game_Engine_DeleteById";
        private const string UpdateSP = "Game_Engine_Update";
        private const string GetPaginatedSP = "Game_Engine_GetPaginated";
        private const string Search_And_GetPaginatedSP = "Game_Engine_Search_And_GetPaginated";
        private const string Get_Last_IdgbIdSP = "SELECT TOP(1) Igdb_Id FROM Game_EnginesTbl ORDER BY Igdb_Id DESC";

        public async Task<long> Get_Last_IdgbId()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<long>(Get_Last_IdgbIdSP, commandType: CommandType.Text);
            }
        }

        public async Task<PaginatedEntity<GameEngine>> Search_And_GetPaginatedAsync(SearchQuery obj)
        {
            var list = new PaginatedEntity<GameEngine>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_And_GetPaginatedSP, new { obj.offset, obj.rows, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<GameEngine>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<PaginatedEntity<GameEngine>> GetPaginatedAsync(long offset, int rows)
        {
            var list = new PaginatedEntity<GameEngine>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { offset, rows }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<GameEngine>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<DbResult> UpdateAsync(GameEngine obj)
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
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<GameEngine>(); }
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
        
        public async Task<DbResult> Add_NewAsync(GameEngine obj)
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
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<GameEngine>(); }
                }
            }
            return result;
        }
        
        public async Task<DbResult> IsIdgb_GameEngine_ExistsAsync(long id)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(IsIdgb_GameEngine_ExistsSP, new { id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<GameEngine>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Add_New_IdgbGame_EngineAsync(GameEngine obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Add_New_IdgbGame_EngineSP, new { obj.Games, obj.Name, obj.NameUrl, obj.Igdb_Id, obj.Companies, obj.Platforms }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<GameEngine>(); }
                }
            }
            return result;
        }
    }
}
