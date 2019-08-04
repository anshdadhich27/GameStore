using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class CartItem
    {
        public string SKU { get; set; } = string.Empty;
        public string Tracking_Id { get; set; } = string.Empty;
        public string Shipment_Id { get; set; } = string.Empty;
        public string ShippingStatus { get; set; } = string.Empty;
        public long Product_Id { get; set; } 
        public string Product_Name { get; set; } = string.Empty;
        public string Product_Type { get; set; } = string.Empty;
        public int Product_TypeId { get; set; }
        public string Product_TypeName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Deduction { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string PageUrl { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }        
        public DateTime Added_On { get; set; } = DateTime.Now;

        public int BoxValue { get; set; }
        public int Condition { get; set; }
        public int ManualValue { get; set; }

        public bool Cancelation_Request_Submitted { get; set; }
    }

    public class Cart_DTO
    {
        public string Id { get; set; } = string.Empty;
        public long UserId { get; set; }        
        public int Tax_Rate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int Total_Item { get; set; }
        public bool OrderPlaced { get; set; }
        public decimal Total_Sum { get; set; }
        public decimal Tax_Amount { get; set; }
        public decimal Total_Price { get; set; }
        public decimal ShippingCharge { get; set; }
        public decimal Total_Deduction { get; set; }
        public string Payment_Status { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public string CartType { get; set; } = string.Empty;
        public long Billing_Address_Id { get; set; }
        public long Shipping_Address_Id { get; set; }
        public DateTime Added_On { get; set; } = DateTime.Now;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public IList<CartItem> ShopingCart { get; set; }
        public IList<CartItem> SellingCart { get; set; }

        public Address ShippingAddress { get; set; }

        public decimal Credit_Ratio { get; set; }
        public decimal CreditUsed { get; set; }
        public string PromoCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public string Auth_Status { get; set; }
        public string Auth_StatusTxt { get; set; }
        public string Auth_Code { get; set; }
        public string Auth_Message { get; set; }
        public string Transaction_Id { get; set; }
        public string Transaction_Date { get; set; }

        public Cart_DTO()
        {
            ShopingCart = new List<CartItem>();
            SellingCart = new List<CartItem>();
            ShippingAddress = new Address();
        }
    }

    public class Shipment_Notification
    {
        public CartItem Cart { get; set; }
        public Shipment_Alert Shipment_Alert { get; set; }

        public Shipment_Notification()
        {
            Cart = new CartItem();
            Shipment_Alert = new Shipment_Alert();
        }
    }

    public class Cart_Details
    {        
        public long TimeStamp { get; set; }
        public string Signature { get; set; } = string.Empty;

        public string ReturnSignature { get; set; } = string.Empty;
        public string SuccessUrl { get; set; } = string.Empty;
        public string CanceledUrl { get; set; } = string.Empty;
        public string DecliendUrl { get; set; } = string.Empty;
        public string CallBackUrl { get; set; } = string.Empty;


        public User User { get; set; }
        public Cart_DTO Cart { get; set; }
        public Address Billing { get; set; }
        public Address Shipping { get; set; }
        
        public Cart_Details()
        {
            User = new User();
            Cart = new Cart_DTO();
            Billing = new Address();
            Shipping = new Address();

        }

        public Cart_Details(Cart_DTO obj)
        {
            Cart = obj;
            User = new User();            
            Billing = new Address();
            Shipping = new Address();
        }
    }
    public class ShoppingorderListExport
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Total_Item { get; set; }
        public string Total_Price { get; set; }
        public string Tax_Rate { get; set; }
        public string Status { get; set; }
        public string Total_Sum { get; set; }
        public string Tax_Amount { get; set; }
        public string ShippingCharge { get; set; }
        public string Address { get; set; }
        public string Payment_Status { get; set; }
        public string Auth_Status { get; set; }
        public string Auth_StatusTxt { get; set; }
        public string Auth_Code { get; set; }
        public string Auth_Message { get; set; }
        public string Transaction_Id { get; set; }
        public string Transaction_Date { get; set; }
        public string OrderPlaced { get; set; }
        public string OrderDate { get; set; }
        public string PaymentType { get; set; }
        public string CartType { get; set; }
        public string IsDeleted { get; set; }
        public string Total_Deduction { get; set; }
        public string CreditUsed { get; set; }
        public string PromoCode { get; set; }
        public string Email { get; set; }

    }
}
