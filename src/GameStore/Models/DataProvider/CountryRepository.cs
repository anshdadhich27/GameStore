using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class CountryRepository : ICountryRepository
    {
        private const string Get_AllSP = "Country_Get_All";

        public async Task<IEnumerable<Country>> Get_All()
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<Country>(Get_AllSP, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
