using System.Threading.Tasks;

using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace GameStore.Models.DataProvider
{
    public class ImageRepository : IImageRepository
    {
        private const string Add_New_ImageSP = "Image_Add_New_Image";
        private const string Remove_By_UrlSP = "Images_Remove_By_Url";
        private const string Add_New_ImagesSP = "Image_Add_New_Images";
        private const string Delete_Image_ByIdSP = "Image_Delete_Image_ById";
        private const string Delete_Image_By_ReferenceIdSP = "Image_Delete_Image_By_ReferenceId";
        private const string Get_Image_By_ReferenceId_ImageType_ImageOfSP = "Image_Get_Image_By_ReferenceId_ImageType_ImageOf";
        private const string Get_Image_By_ReferenceId_ImageOfSP = "Image_Get_Image_By_ReferenceId_ImageOf";


        public async Task<int> Remove_By_Url_Async(string Url)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Remove_By_UrlSP, new { Url }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<DbResult> Get_Image_By_ReferenceId_ImageOfAsync(long ReferenceId, string ImageOf)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Get_Image_By_ReferenceId_ImageOfSP, new { ImageOf, ReferenceId }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.Read<Images>(); }
                }
            }
            return result;
        }
        public async Task<PaginatedEntity<ConsoleListExport>> GetConsole_By_Custom_QueryAsync(string query)
        {
            var list = new PaginatedEntity<ConsoleListExport>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(query, commandType: CommandType.Text))
                {
                    list.PagedSet = multiple.Read<ConsoleListExport>();
                }
            }
            return list;
        }
        public async Task<DbResult> Get_Image_By_ReferenceId_ImageType_ImageOfAsync(long ReferenceId, string ImageType, string ImageOf)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Get_Image_By_ReferenceId_ImageType_ImageOfSP, new { ImageOf, ImageType, ReferenceId }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.Read<Images>(); }
                }
            }
            return result;
        }

        public async Task<int> Add_New_ImagesAsync(IEnumerable<Images> list)
        {
            int result = 0;
            using (var con = DbHelper.GetSqlConnection())
            {
                var trans = con.BeginTransaction();
                result = await con.ExecuteAsync(Add_New_ImagesSP, list, trans, commandType: CommandType.StoredProcedure);
                trans.Commit();
            }
            return result;
        }

        public async Task<DbResult> Add_New_ImageAsync(Images obj)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Add_New_ImageSP, new { obj.ImageCategory, obj.Url, obj.Height, obj.Width, obj.ImageOf, obj.ImageType, obj.Reference_Id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success) { result.Data = multiple.ReadFirstOrDefault<Images>(); }
                }
            }
            return result;
        }

        public async Task<DbResult> Delete_Image_ByIdAsync(long Id)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Delete_Image_ByIdSP, new { Id }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                }
            }
            return result;
        }

        public async Task<DbResult> Delete_Image_By_ReferenceIdAsync(long ReferenceId)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Delete_Image_By_ReferenceIdSP, new { ReferenceId }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                }
            }
            return result;
        }
    }
    public class ConsoleListExport
    {
        public string Name { get; set; }
        public string PlatformName { get; set; }
        public string Rating { get; set; }
        public string RatingCount { get; set; }
        public string Popularity { get; set; }
        public string PreOrder { get; set; }
        public string Buying_Price { get; set; }
        public string Selling_Price { get; set; }
        public string Original_Price { get; set; }
        public string IsBestSelling { get; set; }
        public string Available_To_Buy { get; set; }
        public string Available_To_Sell { get; set; }
        public string Quantity { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
    }
}
