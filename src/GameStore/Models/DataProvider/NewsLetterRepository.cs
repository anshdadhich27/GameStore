using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Models.Entity;
using System.Data;
using Dapper;
using GameStore.Models.Interface;

namespace GameStore.Models.DataProvider
{
    public class NewsLetterRepository : INewsLetterRepository
    {
        private const string NewsLetter_Subscriber_Add_NewSP = "NewsLetter_Subscriber_Add_New";
        private const string NewsLetter_Subscriber_VerifySP = "NewsLetter_Subscriber_Verify";
        private const string Search_And_GetPaginatedSP = "NewsLetter_Search_And_GetPaginated";
        private const string GetPaginatedSP = "NewsLetter_GetPaginated";
        private const string Delete_By_IdSP = "NewsLetter_Delete_By_Id";

        public async Task<int> Delete_By_Id(int Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<PaginatedEntity<NewsLetter>> GetPaginatedAsync(long offset, int rows)
        {
            var list = new PaginatedEntity<NewsLetter>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { offset, rows }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<NewsLetter>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }


        public async Task<PaginatedEntity<NewsLetter>> Search_And_GetPaginatedAsync(SearchQuery obj)
        {
            var list = new PaginatedEntity<NewsLetter>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_And_GetPaginatedSP, new { obj.offset, obj.rows, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<NewsLetter>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<DbResult> NewsLetter_Subscriber_Verify(string Security_Key)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<DbResult>(NewsLetter_Subscriber_VerifySP, new { Security_Key }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<DbResult> NewsLetter_Subscriber_Add_New(NewsLetter obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<DbResult>(NewsLetter_Subscriber_Add_NewSP, new { obj.Email, obj.Name, obj.FirstName, obj.LastName, obj.Security_Key }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
