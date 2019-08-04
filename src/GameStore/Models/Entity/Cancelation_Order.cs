using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Cancelation_Order
    {
        public string Id { get; set; }
        public string SKU { get; set; }
        public string Status { get; set; }
        public string Tracking_Id { get; set; }
        public string Shipment_Id { get; set; }
        public DateTime? Resuest_Date { get; set; }
        //public decimal Refundable_Amount { get; set; }


        public long Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_Type { get; set; }
        public int Product_TypeId { get; set; }
        public string Product_TypeName { get; set; }
        public int Quantity { get; set; }
        public string PageUrl { get; set; }
        public string ImageUrl { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentType { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }
}
