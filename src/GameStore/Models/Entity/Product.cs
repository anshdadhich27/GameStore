using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string NameUrl { get; set; }
        public string Platform { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }        
        public int PEGI_Rating { get; set; }
        public double Popularity { get; set; }
        public string SortInfo { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public string CategoryCode { get; set; }
        public string Platform_Code { get; set; }

        public int RatingCount { get; set; }
        public double Rating { get; set; }
        public double RatingValue { get; set; }
        
        public int Quantity { get; set; }
        public DateTime Added_On { get; set; } = DateTime.Now;

        public string Condition { get; set; }
        public int Condition_Id { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }        
        public string ProductTypeName { get; set; }

        public decimal Buying_Price { get; set; }
        public decimal Selling_Price { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal Original_Price { get; set; }

        public bool PreOrder { get; set; }
        public bool IsBestSelling { get; set; }
        public bool Available_To_Buy { get; set; }
        public bool Available_To_Sell { get; set; }

        public string ImageUrl { get; set; }
        public string SKU { get; set; }

        public int Vendor_Id { get; set; }
        public string Vendor_Name { get; set; }

        public IEnumerable<Images> ImageList { get; set; }
        public IEnumerable<Review> Reviews { get; set; }

        public string PageUrl { get { return string.Format("/{0}/{1}/{2}", ProductType, NameUrl, Condition).ToLower(); } }

        public Product()
        {
            ImageList = new List<Images>();
            Reviews = new List<Review>();
        }
    }
}
