using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;

namespace GameStore.Models.DataProvider
{
    public class VendorRepository : IVendorRepository
    {
        private const string Add_NewSP = "Vendor_Add_New";
        private const string Update_By_IdSP = "Vendor_Update_By_Id";
        private const string Get_By_IdSP = "Vendor_Get_By_Id";
        private const string Get_AllSP = "Vendor_Get_All";
        private const string Delete_By_IdSP = "Vendor_Delete_By_Id";
        private const string GetPaginatedSP = "Vendor_GetPaginated";
        private const string Search_And_GetPaginatedSP = "Vendor_Search_And_GetPaginated";


        public async Task<PaginatedEntity<Vendor>> Search_And_GetPaginated(SearchQuery obj)
        {
            var list = new PaginatedEntity<Vendor>();
            using(var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_And_GetPaginatedSP, new { obj.offset, obj.rows, obj.searchTxt }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Vendor>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<PaginatedEntity<Vendor>> GetPaginated(long offset, int rows)
        {
            var list = new PaginatedEntity<Vendor>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(GetPaginatedSP, new { offset, rows }, commandType: CommandType.StoredProcedure))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Vendor>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }

        public async Task<int> Delete_By_Id(int Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Vendor> Get_By_Id(int Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Vendor>(Get_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Vendor>> Get_All()
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Vendor>(Get_AllSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Vendor> Add_New(Vendor obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                return await con.QueryFirstOrDefaultAsync<Vendor>(Add_NewSP, new { obj.Comments, obj.NameUrl, obj.Address, obj.City, obj.Country, obj.Email, obj.ISDCode, obj.Name, obj.PhoneNumber, obj.State, obj.ZipCode }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Vendor> Update_By_Id(Vendor obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                obj.NameUrl = Regex.Replace(obj.Name.Trim(), "[^a-zA-Z0-9]", "-");
                obj.NameUrl = Regex.Replace(obj.NameUrl, @"(?<=(-))\1+", "").ToLower();
                obj.NameUrl = obj.NameUrl.EndsWith("-") ? obj.NameUrl.Remove(obj.NameUrl.Length - 1, 1) : obj.NameUrl;
                return await con.QueryFirstOrDefaultAsync<Vendor>(Update_By_IdSP, new { obj.Comments, obj.NameUrl, obj.Id, obj.Address, obj.City, obj.Country, obj.Email, obj.ISDCode, obj.Name, obj.PhoneNumber, obj.State, obj.ZipCode }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
