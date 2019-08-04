using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Dorship
    {
        [JsonProperty("order_reference")]
        public string Order_Reference { get; set; }

        [JsonProperty("payment_type")]
        public string Payment_Type { get; set; }

        [JsonProperty("consolidate_orders")]
        public bool Consolidate_Orders { get; set; }

        [JsonProperty("is_express_delivery")]
        public bool Is_Express_Delivery { get; set; }

        [JsonProperty("receiver_data")]
        public Receiver_Data Receiver_Data { get; set; }

        [JsonProperty("package_data")]
        public IList<Package_Data> Package_Data { get; set; }


        public Dorship()
        {
            Consolidate_Orders = false;
            Is_Express_Delivery = true;
            Receiver_Data = new Receiver_Data();
            Package_Data = new List<Package_Data>();
        }

        public Dorship(string order_reference, string payment_type)
        {
            Consolidate_Orders = false;
            Is_Express_Delivery = true;
            Payment_Type = payment_type;
            Order_Reference = order_reference;
            Receiver_Data = new Receiver_Data();
            Package_Data = new List<Package_Data>();
        }
    }
    
    public class Receiver_Data
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonProperty("Alternate_Phone")]
        public string alternate_phone { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("country")]
        public string Country { get; set; } = string.Empty;

        [JsonProperty("state")]
        public string State { get; set; } = string.Empty;

        [JsonProperty("city")]
        public string City { get; set; } = string.Empty;

        [JsonProperty("street_one")]
        public string Street_One { get; set; } = string.Empty;

        [JsonProperty("street_two")]
        public string Street_Two { get; set; } = string.Empty;

        [JsonProperty("street_three")]
        public string Street_Three { get; set; } = string.Empty;

        [JsonProperty("postal_code")]
        public string Postal_Code { get; set; } = string.Empty;

        [JsonProperty("instructions")]
        public string Instructions { get; set; } = string.Empty;

        public Receiver_Data() { }

        public Receiver_Data(string name, string phone, string email, string country, string state, string city, string street_one, string zip_code)
        {
            Name = name; Phone = phone; Email = email; Country = country; State = state; City = city; Street_One = street_one; Postal_Code = zip_code;
        }

        
    }

    public class Package_Data
    {
        [JsonProperty("pickup_address_id")]
        public string Pickup_Address_Id { get; set; } = string.Empty;

        [JsonProperty("package_reference")]
        public string Package_Reference { get; set; } = string.Empty;
        
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("price")]
        public decimal Price { get; set; } 

        [JsonProperty("order_value")]
        public decimal Order_Value { get; set; } 

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("bag_count")]
        public int Bag_Count { get; set; }

        [JsonProperty("weight")]
        public string Weight { get; set; } = string.Empty;

        [JsonProperty("width")]
        public string Width { get; set; } = string.Empty;

        [JsonProperty("height")]
        public string Height { get; set; } = string.Empty;

        [JsonProperty("depth")]
        public string Depth { get; set; } = string.Empty;

        [JsonProperty("dimension_unit")]
        public string Dimension_Unit { get; set; } = string.Empty;

        [JsonProperty("dimension_weight")]
        public string Dimension_Weight { get; set; } = string.Empty;

        public Package_Data()
        {
            Bag_Count = 1;
            Type = "fragile";
            Pickup_Address_Id = Constant.FetchrLocationId;
        }

        public Package_Data(string package_reference, decimal price, string description)
        {
            Bag_Count = 1;
            Type = "fragile";
            Price = price;
            Order_Value = price;
            Description = description;
            Package_Reference = package_reference;
            Pickup_Address_Id = Constant.FetchrLocationId;
        }
    }

    public class Dorship_Response
    {
        [JsonProperty("order_reference")]
        public string Order_Reference { get; set; }
        

        [JsonProperty("package_data")]
        public IList<Package_Data_Receive> Package_Data { get; set; }


        public Dorship_Response()
        {
            Package_Data = new List<Package_Data_Receive>();
        }
        
    }

    public class Package_Data_Receive
    {

        [JsonProperty("package_reference")]
        public string Package_Reference { get; set; } = string.Empty;

        [JsonProperty("tracking_no")]
        public string Tracking_No { get; set; } = string.Empty;
        
    }

    public class ShipmentLog
    {
        [JsonProperty("source")]
        public string Source { get; set; } = string.Empty;

        [JsonProperty("status_name")]
        public string Status_Name { get; set; } = string.Empty;

        [JsonProperty("status_code")]
        public string Status_Code { get; set; } = string.Empty;

        [JsonProperty("status_description")]
        public string Status_Description { get; set; } = string.Empty;

        [JsonProperty("status_date")]
        public string Status_Date { get; set; } = string.Empty;

        [JsonProperty("status_date_local")]
        public string Status_Date_Local { get; set; } = string.Empty;
    }

    public class OrderInfo
    {
        [JsonProperty("tracking_no")]
        public string Tracking_No { get; set; } = string.Empty;

        [JsonProperty("so_number")]
        public string SO_Number { get; set; } = string.Empty;

        [JsonProperty("client_ref")]
        public string Client_Ref { get; set; } = string.Empty;
    }

    public class ShipmentStatus
    {
        [JsonProperty("tracking_information")]
        public ShipmentLog ShipmentLog { get; set; }

        [JsonProperty("order_information")]
        public OrderInfo OrderInfo { get; set; }

        public ShipmentStatus()
        {
            OrderInfo = new OrderInfo();
            ShipmentLog = new ShipmentLog();
        }
    }

    public class ShipmentHistory
    {
        [JsonProperty("tracking_information")]
        public IList<ShipmentLog> ShipmentLogs { get; set; }

        [JsonProperty("order_information")]
        public OrderInfo OrderInfo { get; set; }

        public ShipmentHistory()
        {
            OrderInfo = new OrderInfo();
            ShipmentLogs = new List<ShipmentLog>();
        }
    }

    public class Shipment_Order_Response
    {
        [JsonProperty("data")]
        public Dorship_Response Dorship { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        public Shipment_Order_Response()
        {
            Dorship = new Dorship_Response();
        }
    }

    public class Shipment_Alert
    {
        [JsonProperty("tracking_id")]
        public string Tracking_Id { get; set; } = string.Empty;

        [JsonProperty("status_code")]
        public string Status_Code { get; set; } = string.Empty;

        [JsonProperty("status_name")]
        public string Status_Name { get; set; } = string.Empty;

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("client_id")]
        public string Client_Id { get; set; } = string.Empty;

        [JsonProperty("scheduling_status_code")]
        public string Scheduling_Status_Code { get; set; } = string.Empty;

        [JsonProperty("scheduling_status_name")]
        public string Scheduling_Status_Name { get; set; } = string.Empty;

        [JsonProperty("reason")]
        public string Reason { get; set; } = string.Empty;

        [JsonProperty("channel")]
        public string Channel { get; set; } = string.Empty;

        [JsonProperty("latitude")]
        public string Latitude { get; set; } = string.Empty;

        [JsonProperty("longitude")]
        public string Longitude { get; set; } = string.Empty;

        [JsonProperty("timeslot")]
        public string Timeslot { get; set; } = string.Empty;

        [JsonProperty("address")]
        public string Address { get; set; } = string.Empty;

        [JsonProperty("comments")]
        public string Comments { get; set; } = string.Empty;
    }

    public class Shipment
    {
        public string Cart_Id { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public bool PaymentStatus { get; set; }
    }


    public class PickUp
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("data")]
        public IList<PickUp_Data> Data { get; set; }
        
        public PickUp()
        {
            Method = "pickup_orders";
            Data = new List<PickUp_Data>();
            UserName = Constant.FetchrUserName;
            Password = Constant.FetchrPassword;
        }
    }

    public class PickUp_Data
    {
        [JsonProperty("order_reference")]
        public string Order_Reference { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone_number")]
        public string Phone_Number { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("payment_type")]
        public string Payment_Type { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("product_items")]
        public IList<PickUp_Product> Product_Items { get; set; }


        public PickUp_Data()
        {
            Amount = 0;
            Payment_Type = "cod";
            Product_Items = new List<PickUp_Product>();
        }

        public PickUp_Data(string order_reference)
        {
            Amount = 0;
            Payment_Type = "cod";
            Order_Reference = order_reference;
            Product_Items = new List<PickUp_Product>();
        }

    }

    public class PickUp_Product
    {
        [JsonProperty("product_description")]
        public string Product_Description { get; set; }

        [JsonProperty("product_qty")]
        public int Product_Qty { get; set; }

        public PickUp_Product()
        {
            Product_Qty = 1;
            Product_Description = string.Empty;
        }
    }

    public class PickUp_Response
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("order_reference")]
        public string Order_Reference { get; set; }

        [JsonProperty("tracking_id")]
        public string Tracking_Id { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class Awb
    {
        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("type")]
        public string Bill_Type { get; set; }

        [JsonProperty("search_key")]
        public string Search_Key { get; set; }

        [JsonProperty("search_value")]
        public IEnumerable<string> Search_Value { get; set; }

        [JsonProperty("start_date")]
        public string Start_Date { get; set; }

        [JsonProperty("end_date")]
        public string End_Date { get; set; }

        public Awb()
        {
            Format = "pdf";
            End_Date = null;
            Start_Date = null;
            Bill_Type = "standard";
            Search_Key = "tracking_no";
            Search_Value = new List<string>();
        }
    }

    public class Awb_Bill
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }

    
}
