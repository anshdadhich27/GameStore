using GameStore.Models.Interface;
using GameStore.Models.Entity;
using System.Data;
using Dapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GameStore.Models.DataProvider
{
    public class ReviewRepository : IReviewRepository
    {
        private const string Add_New_IgdbReviewSP = "Review_Add_New_IgdbReview";
        private const string Add_New_ReviewSP = "Review_Add_New_Review";
        private const string IsIgdbReview_ExistsSP = "Review_IsIgdbReview_Exists";
        private const string GetPaginatedSP = "Review_GetPaginated";
        private const string Delete_By_IdSP = "Review_Delete_By_Id";
        private const string Update_By_IdSP = "Review_Update_By_Id";

        public async Task<DbResult> Update_By_Id(Review obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.Link = Regex.Replace(obj.Title.Trim(), "[^a-zA-Z0-9]", "-");
                obj.Link = Regex.Replace(obj.Link, @"(?<=(-))\1+", "").ToLower();
                obj.Link = obj.Link.EndsWith("-") ? obj.Link.Remove(obj.Link.Length - 1, 1) : obj.Link;
                using (var multiple = await con.QueryMultipleAsync(Update_By_IdSP, new { obj.Id, obj.Rating, obj.Conclusion, obj.Content, obj.Introduction, obj.Likes, obj.Negative_Points, obj.Positive_Points, obj.Video, obj.Views, obj.Title, obj.Link }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Review>(); }
                }
            }
            return result;
        }

        public async Task<int> Delete_By_Id(long Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<PaginatedEntity<Review>> GetPaginated(int rows, long offset)
        {
            var list = new PaginatedEntity<Review>();
            using(var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { rows, offset }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Review>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<DbResult> Add_New_IgdbReviewAsync(Review obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.Link = Regex.Replace(obj.Title.Trim(), "[^a-zA-Z0-9]", "-");
                obj.Link = Regex.Replace(obj.Link, @"(?<=(-))\1+", "").ToLower();
                obj.Link = obj.Link.EndsWith("-") ? obj.Link.Remove(obj.Link.Length - 1, 1) : obj.Link;
                using (var multiple = await con.QueryMultipleAsync(Add_New_IgdbReviewSP, new { obj.Rating, obj.Category, obj.Conclusion, obj.Content, obj.Introduction, obj.Likes, obj.Negative_Points, obj.Platform_Id, obj.Positive_Points, obj.Rating_Category, obj.UserId, obj.UserName, obj.Video, obj.Views, obj.Reference_Id, obj.Title, obj.Link, obj.Igdb_Id, obj.ReviewFor }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Review>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Add_New_Review(Review obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.Link = Regex.Replace(obj.Title.Trim(), "[^a-zA-Z0-9]", "-");
                obj.Link = Regex.Replace(obj.Link, @"(?<=(-))\1+", "").ToLower();
                obj.Link = obj.Link.EndsWith("-") ? obj.Link.Remove(obj.Link.Length - 1, 1) : obj.Link;
                using (var multiple = await con.QueryMultipleAsync(Add_New_ReviewSP, new { obj.Rating, obj.Category, obj.Conclusion, obj.Content, obj.Introduction, obj.Likes, obj.Negative_Points, obj.Platform_Id, obj.Positive_Points, obj.Rating_Category, obj.UserId, obj.UserName, obj.Video, obj.Views, obj.Reference_Id, obj.Title, obj.Link, obj.Igdb_Id, obj.ReviewFor }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Review>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> IsIgdbReview_ExistsAsync(long id)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(IsIgdbReview_ExistsSP, new { id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Review>(); }
                }
            }
            return result;
        }
    }
}
