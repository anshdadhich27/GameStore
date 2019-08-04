using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GameStore.Models.Interface;
using GameStore.Models.Entity;
using System.Data;
using Dapper;

namespace GameStore.Models.DataProvider
{
    public class SearchRepository : ISearchRepository
    {
        private const string Search_By_querySP = "Search_By_query";
        private const string Search_By_query_for_sellSP = "Search_By_query_for_sell";


        public async Task<DbResult> Search_By_query(string query)
        {
            DbResult result = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_By_querySP, new { query }, commandType: CommandType.StoredProcedure))
                {
                    result = multiple.ReadFirstOrDefault<DbResult>();
                    if (result != null && result.Success)
                    {
                        var _data = new SearchResult();
                        _data.GameList = multiple.Read<Game_Search>();
                        _data.GamePlatforms = multiple.Read<GamePlatform>();
                        _data.Accessories = multiple.Read<Product>();
                        _data.Consoles = multiple.Read<Product>();
                        _data.Blogs = multiple.Read<Blog>();
                        result.Data = _data;
                    }
                }
            }
            return result;
        }

        public async Task<Sell_Product> Search_By_query_for_sell(Sell_Query obj)
        {
            var _data = new Sell_Product();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Search_By_query_for_sellSP, new { obj.SearchTxt,obj.ProductType }, commandType: CommandType.StoredProcedure))
                {
                    if (obj.ProductType == Constant.GAME)
                    {
                        _data.Games = multiple.Read<Game_Search>();
                    }
                    else if (obj.ProductType == Constant.ACCESSORIES)
                    {
                        _data.Accessories = multiple.Read<Product>();
                    }
                    else
                    {
                        _data.Consoles = multiple.Read<Product>();
                    }
                }
            }
            return _data;
        }


        public async Task<SearchResult> Search_With_Custom_query(string query_for_game, string query_for_accessories, string query_for_console,string MaxPriceQuery)
        {
            var result = new SearchResult();
            using (var con = DbHelper.GetSqlConnection())
            {
                result.GameList = await con.QueryAsync<Game_Search>(query_for_game, commandType: CommandType.Text);
                result.Consoles = await con.QueryAsync<Product>(query_for_console, commandType: CommandType.Text);
                result.Accessories = await con.QueryAsync<Product>(query_for_accessories, commandType: CommandType.Text);
                using (var multiple = await con.QueryMultipleAsync(MaxPriceQuery, commandType: CommandType.Text))
                {
                    result.MaxPrice = multiple.Read<decimal>().SingleOrDefault();
                }
            }
            return result;
        }
    }
}
