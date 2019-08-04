using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class WishList
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string WishType { get; set; }
        public long Product_Id { get; set; }
        public int ProductType_Id { get; set; }
        public DateTime AddedOn { get; set; }

        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string ImageUrl { get; set; }
        public string ProductName { get; set; }
        public string ProductLink { get; set; }
        public string ProductTypeName { get; set; }
        
    }

    public class WishList_DTO
    {
        public IEnumerable<WishList> GameList { get; set; }
        public IEnumerable<WishList> ConsoleList { get; set; }
        public IEnumerable<WishList> AccessoriesList { get; set; }

        public WishList_DTO()
        {
            GameList = new List<WishList>();
            ConsoleList = new List<WishList>();
            AccessoriesList = new List<WishList>();
        }
    }
}
