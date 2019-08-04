using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class GameRepository : IGameRepository
    {
        private const string Add_NewGame_From_Idgb_ApiSP = "Games_Add_NewGame_From_Idgb_Api";
        private const string IsIdgbGame_ExistsSP = "Games_IsIdgbGame_Exists";
        private const string Add_New_Release_Date_From_IGDbSP = "Games_Release_Date_Add_New_From_IGDb";
        private const string Get_Last_IdgbIdSP = "SELECT TOP(1) Igdb_Id FROM GamesTbl ORDER BY Igdb_Id DESC";
        private const string GetPaginated_By_PlatformIdSP = "Games_GetPaginated_By_PlatformId";
        private const string Get_ALL_Witch_Has_No_Cover_ImageSP = "Games_Get_ALL_Witch_Has_No_Cover_Image";
        private const string Get_ALL_Which_Has_No_VideosSP = "Games_Get_ALL_Which_Has_No_Videos";
        private const string Get_Details_By_UrlSP = "Games_Get_Details_By_Url";
        private const string GetPaginated_By_PlatformId_GenreIdSP = "Games_GetPaginated_By_PlatformId_GenreId";
        private const string GetPaginated_By_Custom_QuerySP = "Games_GetPaginated_By_Custom_Query";
        private const string Update_Game_DetailsSP = "Games_Update_Game_Details";
        private const string Add_NewGame_ManuallySP = "Games_Add_NewGame_Manually";
        private const string Get_Details_By_IdSP = "Games_Get_Details_By_Id";
        private const string Get_Details_By_SKUSP = "Games_Get_Details_By_SKU";
        private const string DeleteByIdSP = "Games_DeleteById";
        private const string GetPaginated_By_PlatformId_From_ViewSP = "Games_GetPaginated_By_PlatformId_From_View";
        private const string GetPaginated_By_PlatformId_GenreId_From_ViewSP = "Games_GetPaginated_By_PlatformId_GenreId_From_View";
        private const string Get_Details_By_Url_PlatformSP = "Games_Get_Details_By_Url_Platform";
        private const string Get_Details_By_Url_Platform_n_ConditionSP = "Games_Get_Details_By_Url_Platform_n_Condition";
        private const string GetPaginated_By_PlatformId_ConditionId_From_ViewSP = "Games_GetPaginated_By_PlatformId_ConditionId_From_View";
        private const string GetPaginated_By_PlatformId_GenreId_ConditionId_From_ViewSP = "Games_GetPaginated_By_PlatformId_GenreId_ConditionId_From_View";

        public async Task<PaginatedEntity<Game>> GetPaginated_By_PlatformId_GenreId_From_View(long offset, int rows, long PlatformId, long GenreId)
        {
            var list = new PaginatedEntity<Game>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginated_By_PlatformId_GenreId_From_ViewSP, new { offset, rows, PlatformId, GenreId }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Game>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<PaginatedEntity<Game>> GetPaginated_By_PlatformId_GenreId_ConditionId_From_View(long offset, int rows, long PlatformId, long GenreId, int ConditionId)
        {
            var list = new PaginatedEntity<Game>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginated_By_PlatformId_GenreId_ConditionId_From_ViewSP, new { offset, rows, PlatformId, GenreId, ConditionId }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Game>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<Game_DTO> Get_Details_By_Url_Platform(string NameUrl, string PlatformUrl)
        {
            var result = new Game_DTO();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Get_Details_By_Url_PlatformSP, new { NameUrl, PlatformUrl }, commandType: CommandType.StoredProcedure))
                {
                    result.Game = multiple.ReadFirstOrDefault<Game>();
                    result.ImageList = multiple.Read<Images>();
                    result.ReleaseDates = multiple.Read<Game_Release_Date>();
                    result.Reviews = multiple.Read<Review>();
                    result.VideoList = multiple.Read<Video>();
                }
            }
            return result;
        }

        public async Task<Game_DTO> Get_Details_By_Url_Platform_n_Condition(string NameUrl, string PlatformUrl, int Condition_Id)
        {
            var result = new Game_DTO();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Get_Details_By_Url_Platform_n_ConditionSP, new { NameUrl, PlatformUrl, Condition_Id }, commandType: CommandType.StoredProcedure))
                {
                    result.Game = multiple.ReadFirstOrDefault<Game>();
                    result.ImageList = multiple.Read<Images>();
                    result.ReleaseDates = multiple.Read<Game_Release_Date>();
                    result.Reviews = multiple.Read<Review>();
                    result.VideoList = multiple.Read<Video>();
                }
            }
            return result;
        }

        public async Task<PaginatedEntity<Game>> GetPaginated_By_PlatformId_From_View(long offset, int rows, long PlatformId)
        {
            var list = new PaginatedEntity<Game>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginated_By_PlatformId_From_ViewSP, new { offset, rows, PlatformId }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Game>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<PaginatedEntity<Game>> GetPaginated_By_PlatformId_ConditionId_From_View(long offset, int rows, long PlatformId, int ConditionId)
        {
            var list = new PaginatedEntity<Game>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginated_By_PlatformId_ConditionId_From_ViewSP, new { offset, rows, PlatformId, ConditionId }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Game>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<DbResult> DeleteByIdAsync(long Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<DbResult>(DeleteByIdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Game> Get_Details_By_SKU(string SKU)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Game>(Get_Details_By_SKUSP, new { SKU }, commandType: CommandType.StoredProcedure);
            }
        }


        public async Task<Game_DTO> Get_Details_By_Id(long id)
        {
            var result = new Game_DTO();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Get_Details_By_IdSP, new { id }, commandType: CommandType.StoredProcedure))
                {
                    result.Game = multiple.ReadFirstOrDefault<Game>();
                    result.ImageList = multiple.Read<Images>();
                    result.ReleaseDates = multiple.Read<Game_Release_Date>();
                    result.Reviews = multiple.Read<Review>();
                    result.VideoList = multiple.Read<Video>();
                    result.GamePlatformMapings = multiple.Read<GamePlatformMaping>();
                }
            }
            return result;
        }

        public async Task<DbResult> Add_NewGame_ManuallyAsync(Game obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Add_NewGame_ManuallySP, new { obj.Conditions, obj.Barcode, obj.Vendor_Id, obj.PreOrder, obj.Active, obj.PEGI_Rating, obj.PEGI_Synopsis, obj.ESRB_Rating, obj.ESRB_Synopsis, obj.Platforms, obj.Popularity, obj.First_release_date, obj.Genres, obj.Name, obj.NameUrl, obj.Storyline, obj.Summary, obj.Game_modes, obj.Keywords }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Game>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Update_Game_DetailsAsync(Game obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Update_Game_DetailsSP, new { obj.Barcode, obj.Vendor_Id, obj.PreOrder, obj.Active, obj.Id, obj.PEGI_Rating, obj.PEGI_Synopsis, obj.ESRB_Rating, obj.ESRB_Synopsis, obj.Platforms, obj.Popularity, obj.First_release_date, obj.Genres, obj.Name, obj.NameUrl, obj.Storyline, obj.Summary, obj.Game_modes, obj.Keywords }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Game>(); }
                }
            }
            return result;
        }

        public async Task<PaginatedEntity<Game>> GetPaginated_By_Custom_QueryAsync(string query)
        {
            var list = new PaginatedEntity<Game>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(query, commandType: CommandType.Text))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Game>();
                    list.NumResult = list.PagedSet.Count();
                    list.MaxPrice = multiple.Read<decimal>().SingleOrDefault();
                }
            }
            return list;
        }
        public async Task<PaginatedEntity<Game>> GetPaginated_By_Custom_master_QueryAsync(string query)
        {
            var list = new PaginatedEntity<Game>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(query, commandType: CommandType.Text))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Game>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }
        public async Task<PaginatedEntity<GameListExport>> GetGame_By_Custom_QueryAsync(string query)
        {
            var list = new PaginatedEntity<GameListExport>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(query, commandType: CommandType.Text))
                {
                    list.PagedSet = multiple.Read<GameListExport>();
                }
            }
            return list;
        }
        public async Task<PaginatedEntity<Game>> GetPaginated_By_PlatformId_GenreId(long offset, int rows, long PlatformId, long GenreId)
        {
            var list = new PaginatedEntity<Game>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginated_By_PlatformId_GenreIdSP, new { offset, rows, PlatformId, GenreId }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Game>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<IEnumerable<Game>> Get_ALL_Which_Has_No_VideosAsync()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Game>(Get_ALL_Which_Has_No_VideosSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Game_DTO> Get_Details_By_Url(string NameUrl)
        {
            var result = new Game_DTO();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Get_Details_By_UrlSP, new { NameUrl }, commandType: CommandType.StoredProcedure))
                {
                    result.Game = multiple.ReadFirstOrDefault<Game>();
                    result.ImageList = multiple.Read<Images>();
                    result.ReleaseDates = multiple.Read<Game_Release_Date>();
                    result.Reviews = multiple.Read<Review>();
                    result.VideoList = multiple.Read<Video>();
                }
            }
            return result;
        }

        public async Task<IEnumerable<Game>> Get_ALL_Witch_Has_No_Cover_Image()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Game>(Get_ALL_Witch_Has_No_Cover_ImageSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<PaginatedEntity<Game>> GetPaginated_By_PlatformId(long offset, int rows, long PlatformId)
        {
            var list = new PaginatedEntity<Game>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginated_By_PlatformIdSP, new { offset, rows, PlatformId }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Game>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<long> Get_Last_IdgbId()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<long>(Get_Last_IdgbIdSP, commandType: CommandType.Text);
            }
        }

        public async Task<DbResult> Add_New_Release_Date_From_IGDbAsync(Game_Release_Date obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Add_New_Release_Date_From_IGDbSP, new { obj.Category, obj.Game_Id, obj.IGdb_Date, obj.Platform_Id, obj.Region, obj.Release_On }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Game_Release_Date>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Add_NewGame_From_Idgb_ApiAsync(Game obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Add_NewGame_From_Idgb_ApiSP, new { obj.PEGI_Rating, obj.PEGI_Synopsis, obj.ESRB_Rating, obj.ESRB_Synopsis, obj.Platforms, obj.Popularity, obj.First_release_date, obj.Genres, obj.Name, obj.NameUrl, obj.Publishers, obj.Storyline, obj.Summary, obj.Igdb_Id, obj.Collection, obj.Category, obj.Rating, obj.RatingCount, obj.SimilarGames, obj.Tags, obj.Developers, obj.Game_modes, obj.Keywords, obj.Themes }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Game>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> IsIdgbGame_ExistsAsync(long id)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(IsIdgbGame_ExistsSP, new { id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Game>(); }
                }
            }
            return result;
        }
    }
}
