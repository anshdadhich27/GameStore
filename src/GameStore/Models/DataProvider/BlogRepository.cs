using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class BlogRepository : IBlogRepository
    {
        private const string Add_NewSP = "Blog_Add_New";
        private const string Delete_By_IdSP = "Blog_Delete_By_Id";
        private const string Update_By_IdSP = "Blog_Update_By_Id";
        private const string Get_By_LinkSP = "Blog_Get_By_Link";
        private const string Get_By_IdSP = "Blog_Get_By_Id";
        private const string GetPaginatedSP = "Blog_GetPaginated";
        private const string Search_And_GetPaginatedSP = "Blog_Search_And_GetPaginated";
        private const string GetTopNSP = "Blog_GetTopN";
        private const string GetTopN_EditorsPicSP = "Blog_GetTopN_EditorsPic";
        private const string GetTopN_NewsSP = "Blog_GetTopN_News";

        public async Task<IEnumerable<Blog>> GetTopN_News(int N)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Blog>(GetTopN_NewsSP, new { N }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Blog>> GetTopN_EditorsPic(int N)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Blog>(GetTopN_EditorsPicSP, new { N }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Blog>> GetTopNAsync(int N)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Blog>(GetTopNSP, new { N }, commandType: CommandType.StoredProcedure);
            }
        }


        public async Task<PaginatedEntity<Blog>> Search_And_GetPaginatedAsync(SearchQuery obj)
        {
            var list = new PaginatedEntity<Blog>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_And_GetPaginatedSP, new { obj.offset, obj.rows, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Blog>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<PaginatedEntity<Blog>> GetPaginatedAsync(long offset, int rows)
        {
            var list = new PaginatedEntity<Blog>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { offset, rows }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Blog>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<Blog> Get_By_Id(int Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Blog>(Get_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Blog> Get_By_Link(string Link)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Blog>(Get_By_LinkSP, new { Link }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> Delete_By_Id(int Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteScalarAsync<bool>(Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> Add_New(Blog obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.Link = Regex.Replace(obj.Title.Trim(), "[^a-zA-Z0-9]", "-");
                obj.Link = Regex.Replace(obj.Link, @"(?<=(-))\1+", "").ToLower();
                obj.Link = obj.Link.EndsWith("-") ? obj.Link.Remove(obj.Link.Length - 1, 1) : obj.Link;
                return await con.ExecuteScalarAsync<bool>(Add_NewSP, new { obj.SortDescription, obj.IsNews, obj.EditorsPic, obj.Title, obj.Link, obj.Active, obj.Author, obj.Banner, obj.BlogContent, obj.Category_Id, obj.MetaDescription, obj.MetaKeyword }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> Update_By_Id(Blog obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.Link = Regex.Replace(obj.Title.Trim(), "[^a-zA-Z0-9]", "-");
                obj.Link = Regex.Replace(obj.Link, @"(?<=(-))\1+", "").ToLower();
                obj.Link = obj.Link.EndsWith("-") ? obj.Link.Remove(obj.Link.Length - 1, 1) : obj.Link;
                return await con.ExecuteScalarAsync<bool>(Update_By_IdSP, new { obj.SortDescription, obj.IsNews, obj.EditorsPic, obj.Id, obj.Title, obj.Link, obj.Active, obj.Author, obj.Banner, obj.BlogContent, obj.Category_Id, obj.MetaDescription, obj.MetaKeyword }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
