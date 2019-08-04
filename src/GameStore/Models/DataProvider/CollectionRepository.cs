using System.Threading.Tasks;

using GameStore.Models.Interface;
using GameStore.Models.Entity;
using System.Data;
using Dapper;
using System.Text.RegularExpressions;
using System.Linq;

namespace GameStore.Models.DataProvider
{
    public class CollectionRepository : ICollectionRepository
    {
        private const string IsIgdbCollection_ExistsSP = "Collection_IsIgdbCollection_Exists";
        private const string Add_New_IgdbCollectionSP = "Collection_Add_New_IgdbCollection";
        private const string Add_NewSP = "Collection_Add_New";
        private const string DeleteByIdSP = "Collection_DeleteById";
        private const string UpdateSP = "Collection_Update";
        private const string GetPaginatedSP = "Collection_GetPaginated";
        private const string Search_And_GetPaginatedSP = "Collection_Search_And_GetPaginated";
        private const string Get_Last_IdgbIdSP = "SELECT TOP(1) Igdb_Id FROM CollectionTbl ORDER BY Igdb_Id DESC";

        public async Task<long> Get_Last_IdgbId()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<long>(Get_Last_IdgbIdSP, commandType: CommandType.Text);
            }
        }

        public async Task<PaginatedEntity<Collection>> Search_And_GetPaginatedAsync(SearchQuery obj)
        {
            var list = new PaginatedEntity<Collection>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_And_GetPaginatedSP, new { obj.offset, obj.rows, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Collection>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<PaginatedEntity<Collection>> GetPaginatedAsync(long offset, int rows)
        {
            var list = new PaginatedEntity<Collection>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { offset, rows }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Collection>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }


        public async Task<DbResult> UpdateAsync(Collection obj)
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
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Collection>(); }
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


        public async Task<DbResult> Add_NewAsync(Collection obj)
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
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Collection>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Add_New_IgdbCollectionAsync(Collection obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Add_New_IgdbCollectionSP, new { obj.Games, obj.Name, obj.NameUrl, obj.Igdb_Id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Collection>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> IsIgdbCollection_ExistsAsync(long id)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(IsIgdbCollection_ExistsSP, new { id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Collection>(); }
                }
            }
            return result;
        }
    }
}
