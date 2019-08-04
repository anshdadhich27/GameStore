using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class HotDeals
    {
        public IEnumerable<Blog> Blogs { get; set; }        
        public IEnumerable<Product> Consoles { get; set; }
        public IEnumerable<Product> Accessories { get; set; }
        public IEnumerable<Game_Search> GameList { get; set; }
        public IEnumerable<GamePlatform> GamePlatforms { get; set; }
        public long TotalCounts { get; set; }
        public long NumResult { get; set; }
        public decimal? MaxPrice { get; set; }
        public HotDeals()
        {
            Blogs = new List<Blog>();            
            Consoles = new List<Product>();
            Accessories = new List<Product>();
            GameList = new List<Game_Search>();
            GamePlatforms = new List<GamePlatform>();
            MaxPrice = 0;
        }
    }

   
}
