using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class SearchQuery
    {
        public long offset { get; set; }
        public int rows { get; set; }
        public string searchTxt { get; set; }
        public int TypeId { get; set; }
    }

    public class Order_Filter
    {
        public long offset { get; set; }
        public int rows { get; set; }
        public string CartType { get; set; }
        public string PaymentType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Product_Category { get; set; }
        public int? Product_SubCategoryId { get; set; }
        public int? Platform_Id { get; set; }
        public decimal? BeginValue { get; set; }
        public decimal? EndValue { get; set; }
        public string Product_SKU { get; set; }
        public string OrderNumber { get; set; }
    }


    public class Sell_Query
    {        
        public string ProductType { get; set; }
        public string SearchTxt { get; set; }
    }

    public class Sell_Product
    {
        public IEnumerable<Game_Search> Games { get; set; }
        public IEnumerable<Product> Consoles { get; set; }
        public IEnumerable<Product> Accessories { get; set; }

        public Sell_Product()
        {
            Games = new List<Game_Search>();
            Consoles = new List<Product>();
            Accessories = new List<Product>();
        }
    }
}
