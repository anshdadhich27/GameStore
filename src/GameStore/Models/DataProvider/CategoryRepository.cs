using GameStore.Models.Interface;
using GameStore.Models.Entity;
using System.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class CategoryRepository : ICategoryRepository
    {
        private const string Add_NewSP = "Category_Add_New";
        private const string Delete_By_IdSP = "Category_Delete_By_Id";
        private const string Update_By_IdSP = "Category_Update_By_Id";
        private const string Get_AllSP = "Category_Get_All";

        public async Task<IEnumerable<Category>> Get_All_Async()
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Category>(Get_AllSP, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> Update_By_Id(Category obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteScalarAsync<bool>(Update_By_IdSP, new { obj.Id, obj.Name, obj.Platform_Ids }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> Delete_By_Id(int Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteScalarAsync<bool>(Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> Add_New_Async(Category obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteScalarAsync<bool>(Add_NewSP, new { obj.Name, obj.Platform_Ids }, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
