using Dapper;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.DataProvider
{
    public class Shopping_Cart_Repository : IShopping_Cart_Repository
    {
        private const string Add_New_Shopping_Cart_ItemSP = "Shopping_Cart_Items_Add_New";
        private const string Shopping_Cart_Item_Get_By_IdSP = "Shopping_Cart_Items_Get_By_Id";
        private const string Shopping_Cart_Item_Delete_By_IdSP = "Shopping_Cart_Items_Delete_By_Id";
        private const string Shopping_Cart_Items_Update_Shipment_IdSP = "Shopping_Cart_Items_Update_Shipment_Id";
        private const string Shopping_Cart_Items_Update_Shipment_Id_PickupSP = "Shopping_Cart_Items_Update_Shipment_Id_Pickup";
        private const string Shopping_Cart_Items_Get_Shipping_Ids_By_Tracking_IdSP = "Shopping_Cart_Items_Get_Shipping_Ids_By_Tracking_Id";

        private const string Add_New_Shopping_Cart_TrackingSP = "Shopping_Cart_Tracking_Add_New";
        private const string Shopping_Cart_Tracking_Update_By_IdSP = "Shopping_Cart_Tracking_Update_By_Id";
        private const string Shopping_Cart_Tracking_Get_By_IdSP = "Shopping_Cart_Tracking_Get_By_Id";
        private const string Shopping_Cart_Tracking_Get_By_UserIdSP = "Shopping_Cart_Tracking_Get_By_UserId";
        private const string Shopping_Cart_Tracking_Delete_By_IdSP = "Shopping_Cart_Tracking_Delete_By_Id";
        private const string Shopping_Cart_Tracking_Update_Address_IdSP = "Shopping_Cart_Tracking_Update_Address_Id";
        private const string Shopping_Cart_Tracking_Update_Payment_Status_By_IdSP = "Shopping_Cart_Tracking_Update_Payment_Status_By_Id";
        private const string Shopping_Cart_Tracking_Update_Total_Price_IdSP = "Shopping_Cart_Tracking_Update_Total_Price_Id";
        private const string Shopping_Cart_Tracking_Update_Order_StatusSP = "Shopping_Cart_Tracking_Update_Order_Status";

        private const string Get_Order_List_By_UserIdSP = "User_Get_Order_List_By_UserId";
        private const string ShipmentAlerts_Add_NewSP = "ShipmentAlerts_Add_New";
        private const string Selling_Cart_Tracking_Get_By_UserIdSP = "Selling_Cart_Tracking_Get_By_UserId";
        private const string Cancel_OrderSP = "Shopping_Cart_Tracking_Cancel_Order";
        private const string Shopping_Cart_Items_Get_By_Shipment_IdSP = "Shopping_Cart_Items_Get_By_Shipment_Id";
        private const string Shopping_Cart_Tracking_Update_COD_Payment_Status_By_IdSP = "Shopping_Cart_Tracking_Update_COD_Payment_Status_By_Id";


        public async Task<int> Shopping_Cart_Tracking_Update_COD_Payment_Status_By_Id(string Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Shopping_Cart_Tracking_Update_COD_Payment_Status_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<CartItem> Shopping_Cart_Items_Get_By_Shipment_Id(string Shipment_Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryFirstOrDefaultAsync<CartItem>(Shopping_Cart_Items_Get_By_Shipment_IdSP, new { Shipment_Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Cancel_Order(string Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Cancel_OrderSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<string>> Get_Shipping_Ids_By_Tracking_Id(string Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<string>(Shopping_Cart_Items_Get_Shipping_Ids_By_Tracking_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<PaginatedEntity<Cart_DTO>> GetPaginated_By_Custom_QueryAsync(string query)
        {
            var list = new PaginatedEntity<Cart_DTO>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(query, commandType: CommandType.Text))
                {
                    list.TotalCount = multiple.Read<long>().SingleOrDefault();
                    list.PagedSet = multiple.Read<Cart_DTO>();
                    list.NumResult = list.PagedSet.Count();
                }
            }
            return list;
        }


        public async Task<int> Shopping_Cart_Tracking_Update_Order_Status(string Id, bool OrderPlaced, string PaymentType, string Status)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Shopping_Cart_Tracking_Update_Order_StatusSP, new { Id, OrderPlaced, PaymentType, Status }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> ShipmentAlerts_Add_New(Shipment_Alert obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(ShipmentAlerts_Add_NewSP, new { obj.Address, obj.Channel, obj.Client_Id, obj.Comments, obj.Date, obj.Latitude, obj.Longitude, obj.Reason, obj.Scheduling_Status_Code, obj.Scheduling_Status_Name, obj.Status, obj.Status_Code, obj.Status_Name, obj.Timeslot, obj.Tracking_Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Cart_DTO>> Get_Order_List_By_UserId(long Id)
        {
            IEnumerable<Cart_DTO> list = null;
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Get_Order_List_By_UserIdSP, new { Id }, commandType: CommandType.StoredProcedure))
                {
                    list = multiple.Read<Cart_DTO>();
                    var items = multiple.Read<CartItem>();
                    var addressList = multiple.Read<Address>();
                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            item.ShippingAddress = addressList.Where(x => x.Id == item.Shipping_Address_Id).FirstOrDefault();                            
                            if (item.CartType == "Shopping") { item.ShopingCart = items.Where(x => x.Tracking_Id == item.Id).ToList(); }
                            else { item.SellingCart = items.Where(x => x.Tracking_Id == item.Id).ToList(); }
                        }
                    }
                }                
            }
            return list;
        }


        public async Task<int> Shopping_Cart_Tracking_Update_Payment_Status_By_Id(string Id, string Status, string Auth_Status, string Auth_Code, string Auth_StatusTxt, string Auth_Message, string Transaction_Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Shopping_Cart_Tracking_Update_Payment_Status_By_IdSP, new { Id, Status, Auth_Status, Auth_Code, Auth_StatusTxt, Auth_Message, Transaction_Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> Shopping_Cart_Tracking_Update_Address_Id(Cart_DTO obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteScalarAsync<string>(Shopping_Cart_Tracking_Update_Address_IdSP, new { obj.Id, obj.UserId, obj.Billing_Address_Id, obj.Shipping_Address_Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> Shopping_Cart_Tracking_Update_By_Id(Cart_DTO obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteScalarAsync<string>(Shopping_Cart_Tracking_Update_By_IdSP, new { obj.Id, obj.UserId, obj.Total_Item, obj.Total_Price, obj.Tax_Rate, obj.Status, obj.Total_Sum, obj.Tax_Amount, obj.ShippingCharge }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> Shopping_Cart_Tracking_Update_Total_Price_Id(Cart_DTO obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteScalarAsync<string>(Shopping_Cart_Tracking_Update_Total_Price_IdSP, new { obj.PromoCode, obj.CreditUsed, obj.Total_Deduction, obj.Id, obj.UserId, obj.Total_Item, obj.Total_Price, obj.Tax_Rate, obj.Status, obj.Total_Sum, obj.Tax_Amount, obj.ShippingCharge }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Shopping_Cart_Tracking_Delete_By_Id(string Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Shopping_Cart_Tracking_Delete_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Cart_DTO> Shopping_Cart_Tracking_Get_By_Id(string Id)
        {
            var obj = new Cart_DTO();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Shopping_Cart_Tracking_Get_By_IdSP, new { Id }, commandType: CommandType.StoredProcedure))
                {
                    obj = multiple.ReadFirstOrDefault<Cart_DTO>();
                    if (obj != null)
                    {
                        var data = multiple.Read<CartItem>();
                        if (data != null)
                        {
                            if (obj.CartType == "Shopping") { obj.ShopingCart = data.ToList(); }
                            else { obj.SellingCart = data.ToList(); }
                        }
                    }
                }
            }
            return obj;
        }

        public async Task<Cart_DTO> Shopping_Cart_Tracking_Get_By_UserId(long UserId)
        {
            var obj = new Cart_DTO();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Shopping_Cart_Tracking_Get_By_UserIdSP, new { UserId }, commandType: CommandType.StoredProcedure))
                {
                    obj = multiple.ReadFirstOrDefault<Cart_DTO>();
                    if (obj != null)
                    {
                        var data = multiple.Read<CartItem>();
                        if (data != null) { obj.ShopingCart = data.ToList(); }
                    }
                }
            }
            return obj;
        }

        public async Task<Cart_DTO> Selling_Cart_Tracking_Get_By_UserId(long UserId)
        {
            var obj = new Cart_DTO();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(Selling_Cart_Tracking_Get_By_UserIdSP, new { UserId }, commandType: CommandType.StoredProcedure))
                {
                    obj = multiple.ReadFirstOrDefault<Cart_DTO>();
                    if (obj != null)
                    {
                        var data = multiple.Read<CartItem>();
                        if (data != null) { obj.SellingCart = data.ToList(); }
                    }
                }
            }
            return obj;
        }

        public async Task<string> Add_New_Shopping_Cart_Tracking(Cart_DTO obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                obj.Id = DbHelper.GetUniqueKey();
                return await con.ExecuteScalarAsync<string>(Add_New_Shopping_Cart_TrackingSP, new { obj.Total_Deduction, obj.CartType, obj.Id, obj.UserId, obj.Total_Item, obj.Total_Price, obj.Tax_Rate, obj.Status, obj.Total_Sum, obj.Tax_Amount, obj.ShippingCharge }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Shopping_Cart_Item_Delete_By_Id(string Tracking_Id)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Shopping_Cart_Item_Delete_By_IdSP, new { Tracking_Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<CartItem>> Shopping_Cart_Item_Get_By_Id(string Tracking_Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.QueryAsync<CartItem>(Shopping_Cart_Item_Get_By_IdSP, new { Tracking_Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Update_Shipment_Id(string Shipment_Id, string Tracking_Id, string SKU)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Shopping_Cart_Items_Update_Shipment_IdSP, new { Shipment_Id, Tracking_Id, SKU }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Update_Shipment_Id_Pickup(string Shipment_Id, string Tracking_Id)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteAsync(Shopping_Cart_Items_Update_Shipment_Id_PickupSP, new { Shipment_Id, Tracking_Id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Add_New_Shopping_Cart_Item(CartItem obj)
        {
            using(var con = DbHelper.GetSqlConnection())
            {
                obj.SKU = DbHelper.GetUniqueKey();
                return await con.ExecuteAsync(Add_New_Shopping_Cart_ItemSP, new { obj.Cancelation_Request_Submitted, obj.Condition, obj.BoxValue, obj.ManualValue, obj.Deduction, obj.ShippingStatus, obj.Shipment_Id, obj.Added_On, obj.Tracking_Id, obj.SKU, obj.Product_Id, obj.Product_Name, obj.Product_Type, obj.Product_TypeId, obj.Product_TypeName, obj.Quantity, obj.Price, obj.ImageUrl, obj.PageUrl, obj.TotalPrice }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> Add_New_Shopping_Cart_Items(List<CartItem> list)
        {
            int result = 0;
            using (var con = DbHelper.GetSqlConnection())
            {
                var trans = con.BeginTransaction();
                result = await con.ExecuteAsync(Add_New_Shopping_Cart_ItemSP, list, trans, commandType: CommandType.StoredProcedure);
                trans.Commit();
            }
            return result;
        }

        
            public async Task<PaginatedEntity<ShoppingorderListExport>> GetShoppingorder_By_Custom_QueryAsync(string query)
        {
            var list = new PaginatedEntity<ShoppingorderListExport>();
            using (var con = DbHelper.GetSqlConnection())
            {
                using (var multiple = await con.QueryMultipleAsync(query, commandType: CommandType.Text))
                {
                    list.PagedSet = multiple.Read<ShoppingorderListExport>();
                }
            }
            return list;
        }
    }
}
