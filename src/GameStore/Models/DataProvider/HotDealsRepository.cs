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
    public class HotDealsRepository : IHotDealsRepository
    {
        public async Task<HotDeals> Search_With_Custom_query(string query_for_game, string query_for_accessories, string query_for_console,string MaxPriceQuery)
        {
            var result = new HotDeals();
            var game_list = new PaginatedEntity<Game_Search>();
            var console_list = new PaginatedEntity<Product>();
            var accessories_list = new PaginatedEntity<Product>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(query_for_game, commandType: CommandType.Text))
                {

                    game_list.TotalCount = multiple.Read<int>().SingleOrDefault();
                    game_list.PagedSet = multiple.Read<Game_Search>();
                    game_list.NumResult = game_list.PagedSet.Count();
                }
                using (var multiple1 = await con.QueryMultipleAsync(query_for_console, commandType: CommandType.Text))
                {

                    console_list.TotalCount = multiple1.Read<int>().SingleOrDefault();
                    console_list.PagedSet = multiple1.Read<Product>();
                    console_list.NumResult = game_list.PagedSet.Count();
                }
                using (var multiple2 = await con.QueryMultipleAsync(query_for_accessories, commandType: CommandType.Text))
                {

                    accessories_list.TotalCount = multiple2.Read<int>().SingleOrDefault();
                    accessories_list.PagedSet = multiple2.Read<Product>();
                    accessories_list.NumResult = game_list.PagedSet.Count();
                }
                result.GameList = game_list.PagedSet;
                result.Consoles = console_list.PagedSet;
                result.Accessories = accessories_list.PagedSet;
                if (game_list != null) { result.TotalCounts +=  game_list.TotalCount; }
                if (console_list != null) { result.TotalCounts += console_list.TotalCount; }
                if (accessories_list != null) { result.TotalCounts += accessories_list.TotalCount; }

                if (game_list != null) { result.NumResult += game_list.NumResult; }
                if (console_list != null) { result.NumResult += console_list.NumResult; }
                if (accessories_list != null) { result.NumResult += accessories_list.NumResult; }
                using (var multiple = await con.QueryMultipleAsync(MaxPriceQuery, commandType: CommandType.Text))
                {
                    result.MaxPrice = multiple.Read<decimal>().SingleOrDefault();
                }
            }
            return result;
        }
    }
}
