using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Deal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public decimal Price { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }        
        public DateTime? Added_On { get; set; }

        public IList<Deal_Product> ProductList { get; set; }

        public Deal()
        {
            ProductList = new List<Deal_Product>();
        }
    }

    public class Deal_Product
    {
        public int SNo { get; set; }
        public int Deal_Id { get; set; }
        public long Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_Type { get; set; }
        public int Product_Type_Id { get; set; }
        public string Product_Type_Name { get; set; }
        public string Product_Link { get; set; }
        public int Quantity { get; set; }
    }

    public class Deal_DTO
    {
        public IEnumerable<Deal> Deals { get; set; }
        public IEnumerable<Deal_Product> Deal_Products { get; set; }

        public Deal_DTO()
        {
            Deals = new List<Deal>();
            Deal_Products = new List<Deal_Product>();
        }
    }
}
