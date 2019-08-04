using GameStore.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Models.Entity;
using System.Data;
using Dapper;
using System.Text.RegularExpressions;

namespace GameStore.Models.DataProvider
{
    public class GenereRepository : IGenereRepository
    {
        private const string Add_NewSP = "Genere_Add_New";
        private const string DeleteByIdSP = "Genere_DeleteById";
        private const string UpdateSP = "Genere_Update";
        private const string GetPaginatedSP = "Genere_GetPaginated";
        private const string Search_And_GetPaginatedSP = "Genere_Search_And_GetPaginated";
        private const string GetByUrlSP = "Genere_GetByUrl";
        private const string GetTopNSP = "Genere_GetTopN";
        private const string IsIdgb_Genere_ExistsSP = "Genere_IsIdgb_Genere_Exists";
        private const string Add_New_From_IdgbSP = "Genere_Add_New_From_Idgb";
        private const string Get_Last_IdgbIdSP = "SELECT TOP(1) Igdb_Id FROM GenereTbl ORDER BY Igdb_Id DESC";
        private const string Get_Supported_N_GenereSP = "SELECT TOP({0}) Id, Name, NameUrl, Igdb_Id FROM GenereTbl Where Id IN ({1});";
        private const string Get_ALL_ActiveSP = "Genere_Get_ALL_Active";

        public async Task<IEnumerable<Genere>> Get_ALL_ActiveAsync()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Genere>(Get_ALL_ActiveSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Genere>> Get_Supported_N_Genere(int top, string Ids)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Genere>(string.Format(Get_Supported_N_GenereSP, top, Ids), commandType: CommandType.Text);
            }
        }

        public async Task<long> Get_Last_IdgbId()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<long>(Get_Last_IdgbIdSP, commandType: CommandType.Text);
            }
        }

        public async Task<DbResult> IsIdgb_Genere_ExistsAsync(long id)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(IsIdgb_Genere_ExistsSP, new { id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Genere>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Add_New_From_IdgbAsync(Genere obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Add_New_From_IdgbSP, new { obj.Games, obj.Name, obj.NameUrl, obj.Igdb_Id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Genere>(); }
                }
            }
            return result;
        }

        public async Task<IEnumerable<Genere>> GetTopNAsync(int N)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Genere>(GetTopNSP, new { N }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Genere> GetByUrlAsync(string Url)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Genere>(GetByUrlSP, new { Url }, commandType: CommandType.StoredProcedure);
            }
        }


        public async Task<PaginatedEntity<Genere>> Search_And_GetPaginatedAsync(SearchQuery obj)
        {
            var list = new PaginatedEntity<Genere>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_And_GetPaginatedSP, new { obj.offset, obj.rows, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Genere>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }
        public async Task<PaginatedEntity<Genere_CSV>> GetGenere_By_Custom_QueryAsync(string query)
        {
            var list = new PaginatedEntity<Genere_CSV>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(query, commandType: CommandType.Text))
                {
                    list.PagedSet = multiple.Read<Genere_CSV>();
                }
            }
            return list;
        }
        public async Task<PaginatedEntity<Genere>> GetPaginatedAsync(long offset, int rows)
        {
            var list = new PaginatedEntity<Genere>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { offset, rows }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Genere>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }



        public async Task<DbResult> DeleteByIdAsync(int Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<DbResult>(DeleteByIdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }


        public async Task<DbResult> Add_NewAsync(Genere obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(Add_NewSP, new { obj.Name, obj.NameUrl, obj.Active }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Genere>(); }
                }
            }
            return result;
        }


        public async Task<DbResult> UpdateAsync(Genere obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                using (var multiple = await con.QueryMultipleAsync(UpdateSP, new { obj.Id, obj.Name, obj.NameUrl, obj.Active }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Genere>(); }
                }
            }
            return result;
        }
    }
}
