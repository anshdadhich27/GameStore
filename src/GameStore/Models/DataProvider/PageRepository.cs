using GameStore.Models.Interface;
using GameStore.Models.Entity;
using System.Data;
using Dapper;
using System.Text.RegularExpressions;
using System.Linq;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class PageRepository : IPageRepository
    {
        private const string Add_NewSP = "Pages_Add_new";
        private const string Update_By_IdSP = "Pages_Update_By_Id";
        private const string Update_Status_IdSP = "Pages_Update_Status_Id";
        private const string Delete_By_IdSP = "Pages_Delete_By_Id";
        private const string Get_By_IdSP = "Pages_Get_By_Id";
        private const string Get_By_UrlSP = "Pages_Get_By_Url";
        private const string Search_And_GetPaginatedSP = "Pages_Search_And_GetPaginated";
        private const string GetPaginatedSP = "Pages_GetPaginated";

        public async Task<PaginatedEntity<PageContent>> GetPaginatedAsync(long offset, int rows)
        {
            var list = new PaginatedEntity<PageContent>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { offset, rows }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<PageContent>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }


        public async Task<PaginatedEntity<PageContent>> Search_And_GetPaginatedAsync(SearchQuery obj)
        {
            var list = new PaginatedEntity<PageContent>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_And_GetPaginatedSP, new { obj.offset, obj.rows, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<PageContent>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }


        public async Task<PageContent> Get_By_Url(string URL)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<PageContent>(Get_By_UrlSP, new { URL }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<PageContent> Get_By_Id(int Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<PageContent>(Get_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Delete_By_Id(int Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Update_Status_Id(PageContent obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Update_Status_IdSP, new { obj.Active, obj.Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Update_By_Id(PageContent obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Update_By_IdSP, new { obj.ShowMenu, obj.Name, obj.Title, obj.Id, obj.MetaKeywords, obj.MetaDescription, obj.HeaderContent, obj.BodyContent }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Add_New(PageContent obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                obj.URL = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.URL = Regex.Replace(obj.URL, @"(?<=(-))\1+", "").ToLower();
                obj.URL = obj.URL.EndsWith("-") ? obj.URL.Remove(obj.URL.Length - 1, 1) : obj.URL;
                return await con.ExecuteAsync(Add_NewSP, new { obj.ShowMenu, obj.Name, obj.Title, obj.URL, obj.MetaKeywords, obj.MetaDescription, obj.HeaderContent, obj.BodyContent }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
