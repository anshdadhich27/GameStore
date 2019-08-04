using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Data;
using Dapper;
using System.Text.RegularExpressions;

namespace GameStore.Models.DataProvider
{
    public class ProductRepository : IProductRepository
    {
        private const string Add_NewSP = "Products_Add_New";
        private const string Delete_By_IdSP = "Products_Delete_By_Id";
        private const string Get_By_IdSP = "Products_Get_By_Id";
        private const string Get_By_Url_TypeSP = "Products_Get_By_Url_Type";
        private const string Update_By_IdSP = "Products_Update_By_Id";
        private const string GetPaginatedSP = "Products_GetPaginated";
        private const string GetPaginated_By_TypeIdSP = "Products_GetPaginated_By_TypeId";
        private const string Get_By_Url_Type_ConditionSP = "Products_Get_By_Url_Type_Condition";
        private const string Get_Details_By_SKUSP = "Products_Get_Details_By_SKU";

        public async Task<Product> Get_Details_By_SKU(string SKU)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Product>(Get_Details_By_SKUSP, new { SKU }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<PaginatedEntity<Product>> GetPaginated_By_Custom_QueryAsync(string query)
        {
            var list = new PaginatedEntity<Product>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(query, commandType: CommandType.Text))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Product>();
                    list.NumResult = list.PagedSet.Count();
                    list.MaxPrice = multiple.Read<decimal>().SingleOrDefault();
                }
            }
            return list;
        }

        public async Task<PaginatedEntity<Product>> GetPaginated_By_TypeId(SearchQuery obj)
        {
            var list = new PaginatedEntity<Product>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginated_By_TypeIdSP, new { obj.offset, obj.rows, obj.searchTxt, obj.TypeId }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<int>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Product>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<PaginatedEntity<Product>> GetPaginated(SearchQuery obj)
        {
            var list = new PaginatedEntity<Product>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { obj.offset, obj.rows, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<int>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Product>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }


        public async Task<Product> Get_By_Url_Type_Condition(string URL, string ProductType, string Condition)
        {
            var obj = new Product();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Get_By_Url_Type_ConditionSP, new { URL, ProductType, Condition }, commandType: CommandType.StoredProcedure))
                {
                    obj = multiple.Read<Product>().FirstOrDefault();
                    if (obj != null) {
                        obj.Reviews = multiple.Read<Review>();
                        obj.ImageList = multiple.Read<Images>();
                    }
                }
            }
            return obj;
        }

        public async Task<Product> Get_By_Url_Type(string URL, string ProductType)
        {
            var obj = new Product();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Get_By_Url_TypeSP, new { URL, ProductType }, commandType: CommandType.StoredProcedure))
                {
                    obj = multiple.Read<Product>().FirstOrDefault();
                    if (obj != null) { obj.Reviews = multiple.Read<Review>(); }
                }
            }
            return obj;
        }

        public async Task<Product> Get_By_Id(long Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Product>(Get_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }


        public async Task<bool> Delete_By_Id(long Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteScalarAsync<bool>(Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<long> Add_New(Product obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                return await con.ExecuteScalarAsync<long>(Add_NewSP, new { obj.Added_On, obj.Barcode, obj.Vendor_Id, obj.PreOrder, obj.Buying_Price, obj.Selling_Price, obj.Original_Price, obj.IsBestSelling, obj.Available_To_Buy, obj.Available_To_Sell, obj.Popularity, obj.ProductTypeId, obj.Description, obj.Developer, obj.Condition_Id, obj.Name, obj.NameUrl, obj.PEGI_Rating,  obj.ProductType, obj.Publisher, obj.Quantity, obj.SortInfo, obj.CategoryCode, obj.Platform_Code }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> Update_By_Id(Product obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                return await con.ExecuteScalarAsync<bool>(Update_By_IdSP, new { obj.Added_On, obj.Barcode, obj.Vendor_Id, obj.PreOrder, obj.Buying_Price, obj.Selling_Price, obj.Original_Price, obj.IsBestSelling, obj.Available_To_Buy, obj.Available_To_Sell, obj.Popularity, obj.ProductTypeId, obj.Id, obj.Description, obj.Developer, obj.Condition_Id, obj.Name, obj.NameUrl, obj.PEGI_Rating, obj.ProductType, obj.Publisher, obj.Quantity, obj.SortInfo, obj.CategoryCode, obj.Platform_Code }, commandType: CommandType.StoredProcedure);
            }
        }


    }
}
