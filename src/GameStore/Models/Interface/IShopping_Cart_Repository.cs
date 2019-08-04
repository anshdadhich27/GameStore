using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IShopping_Cart_Repository
    {
        Task<int> Add_New_Shopping_Cart_Item(CartItem obj);
        Task<int> Shopping_Cart_Item_Delete_By_Id(string Tracking_Id);
        Task<IEnumerable<CartItem>> Shopping_Cart_Item_Get_By_Id(string Tracking_Id);
        Task<int> Update_Shipment_Id_Pickup(string Shipment_Id, string Tracking_Id);
        Task<CartItem> Shopping_Cart_Items_Get_By_Shipment_Id(string Shipment_Id);

        Task<int> Cancel_Order(string Id);


        Task<IEnumerable<Cart_DTO>> Get_Order_List_By_UserId(long Id);
        Task<int> Shopping_Cart_Tracking_Delete_By_Id(string Id);
        Task<Cart_DTO> Shopping_Cart_Tracking_Get_By_Id(string Id);
        Task<string> Add_New_Shopping_Cart_Tracking(Cart_DTO obj);
        Task<string> Shopping_Cart_Tracking_Update_By_Id(Cart_DTO obj);
        Task<Cart_DTO> Shopping_Cart_Tracking_Get_By_UserId(long UserId);
        Task<int> Add_New_Shopping_Cart_Items(List<CartItem> list);
        Task<string> Shopping_Cart_Tracking_Update_Address_Id(Cart_DTO obj);
        Task<int> Shopping_Cart_Tracking_Update_Payment_Status_By_Id(string Id, string Status, string Auth_Status, string Auth_Code, string Auth_StatusTxt, string Auth_Message, string Transaction_Id);
        Task<string> Shopping_Cart_Tracking_Update_Total_Price_Id(Cart_DTO obj);
        Task<int> Shopping_Cart_Tracking_Update_Order_Status(string Id, bool OrderPlaced, string PaymentType, string Status);

        Task<int> ShipmentAlerts_Add_New(Shipment_Alert obj);
        Task<int> Update_Shipment_Id(string Shipment_Id, string Tracking_Id, string SKU);
        Task<PaginatedEntity<Cart_DTO>> GetPaginated_By_Custom_QueryAsync(string query);

        Task<IEnumerable<string>> Get_Shipping_Ids_By_Tracking_Id(string Id);
        Task<Cart_DTO> Selling_Cart_Tracking_Get_By_UserId(long UserId);

        Task<int> Shopping_Cart_Tracking_Update_COD_Payment_Status_By_Id(string Id);
        Task<PaginatedEntity<ShoppingorderListExport>> GetShoppingorder_By_Custom_QueryAsync(string query);
        

    }
}
