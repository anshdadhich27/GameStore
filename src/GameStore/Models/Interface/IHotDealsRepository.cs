using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IHotDealsRepository
    {

        Task<HotDeals> Search_With_Custom_query(string query_for_game, string query_for_accessories, string query_for_console, string MaxPriceQuery);
    }
}
