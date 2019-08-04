using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameStore.Models;
using GameStore.Services;
using GameStore.Models.Interface;
using GameStore.Models.Entity;
using System.IO;

namespace GameStore.Controllers
{
    [Route("Reports")]
    [Authorize(ActiveAuthenticationSchemes = Constant.Admin_Auth_Scheme)]
    public class ReportsController : BaseController
    {
        private FetchrApi _fetchr = null;
        private readonly IEmailSender _emailSender = null;
        private readonly IAddressRepository _addr_repo = null;
        private readonly IShopping_Cart_Repository _shop_repo = null;
        private readonly IPaymentSettingRepository _pay_repo = null;
        private readonly ICreditRepository _credit_repo = null;
        private readonly IOrder_Repository _order_repo = null;
        public ReportsController(IShopping_Cart_Repository shop_repo,
            IPaymentSettingRepository pay_repo,
            IEmailSender emailSender,
            IAddressRepository addr_repo,
            ICreditRepository credit_repo,
            IOrder_Repository order_repo)
        {
            _credit_repo = credit_repo;
            _pay_repo = pay_repo;
            _addr_repo = addr_repo;
            _emailSender = emailSender;
            _shop_repo = shop_repo;
            _order_repo = order_repo;
            _fetchr = new FetchrApi();
        }


        [HttpPost("Get-Orders")]
        public async Task<JsonResult> Get_Orders([FromBody] Order_Filter obj)
        {
            #region where_clouse
            string where_clouse = string.Empty;
            if (!string.IsNullOrEmpty(obj.PaymentType) && obj.PaymentType != "all")
            {
                where_clouse += string.Format(" AND t1.PaymentType='{0}' ", obj.PaymentType);
            }

            if (!string.IsNullOrEmpty(obj.CartType) && obj.CartType != "all")
            {
                where_clouse += string.Format(" AND t1.CartType='{0}' ", obj.CartType);
            }

            if (!string.IsNullOrEmpty(obj.Product_Category) && obj.Product_Category != "ALL")
            {
                where_clouse += string.Format(" AND t4.Product_Type='{0}' ", obj.Product_Category);
            }

            if (!string.IsNullOrEmpty(obj.Product_SKU))
            {
                where_clouse += string.Format(" AND t4.SKU='{0}' ", obj.Product_SKU);
            }

            if (!string.IsNullOrEmpty(obj.OrderNumber))
            {
                where_clouse += string.Format(" AND t1.Id='{0}' ", obj.OrderNumber);
            }

            if (obj.Product_SubCategoryId.HasValue)
            {
                where_clouse += string.Format(" AND t4.Product_TypeId='{0}' ", obj.Product_SubCategoryId.Value);
            }

            if (obj.Platform_Id.HasValue)
            {
                where_clouse += string.Format(" AND t4.Product_TypeId='{0}' ", obj.Platform_Id.Value);
            }

            if (obj.BeginValue.HasValue)
            {
                where_clouse += string.Format(" AND t1.Total_Sum >= {0} ", obj.BeginValue.Value);
            }

            if (obj.EndValue.HasValue)
            {
                where_clouse += string.Format(" AND t1.Total_Sum <= {0} ", obj.EndValue.Value);
            }

            if (obj.StartDate.HasValue)
            {
                where_clouse += string.Format(" AND CAST(t1.OrderDate as DATE) >= CAST('{0}' as DATE) ", obj.StartDate.Value.ToString("MM/dd/yyyy"));
            }

            if (obj.EndDate.HasValue)
            {
                where_clouse += string.Format(" AND CAST(t1.OrderDate as DATE) <= CAST('{0}' as DATE) ", obj.EndDate.Value.ToString("MM/dd/yyyy"));
            }
            #endregion

            #region Query Builder
            string query = string.Empty;
            query += " SELECT COUNT(t1.Id) as Total FROM [Shopping_Cart_Tracking_Tbl] t1 ";
            query += " LEFT JOIN Shopping_Cart_Items_Tbl t4 on t1.Id=t4.Tracking_Id ";
            query += " WHERE t1.[Status] != 'Ready' AND t1.IsDeleted=0 ";
            query += where_clouse;
            query += "; ";


            query += " SELECT DISTINCT(t1.Id), t1.*, t2.Email, CONCAT(t2.FirstName, ' ', t2.LastName) as Name, ";
            query += " CONCAT(t3.First_Name, ' ', t3.Last_Name, '<br/>', t3.Address_Line, ' ', t3.Area, '<br/>', t3.City, ' ', t3.[State], '<br/>', t3.Country, ' ', t3.ZipCode, '<br/>Mobile: ', t3.Mobile) as 'Address' ";
            query += " FROM [Shopping_Cart_Tracking_Tbl] t1 ";
            query += " LEFT JOIN UsersTbl t2 on t1.UserId=t2.Id ";
            query += " LEFT JOIN AddressTbl t3 on t1.Shipping_Address_Id=t3.Id ";
            query += " LEFT JOIN Shopping_Cart_Items_Tbl t4 on t1.Id=t4.Tracking_Id ";
            query += " WHERE t1.[Status] != 'Ready' AND t1.IsDeleted=0 ";
            query += where_clouse;
            query += " ORDER by t1.OrderDate DESC ";
            query += string.Format(" OFFSET {0} ROWS ", obj.offset);
            query += string.Format(" FETCH NEXT {0} ROWS ONLY ", obj.rows);
            query += "; ";
            #endregion

            var result = await _shop_repo.GetPaginated_By_Custom_QueryAsync(query);
            return Json(result);
        }

        [HttpGet("Delete-Order/{id}")]
        public async Task<JsonResult> Delete_Order(string id)
        {
            var result = await _shop_repo.Shopping_Cart_Tracking_Delete_By_Id(id);
            return Json(result);
        }

        [HttpGet("Unblock-Credits/{id}")]
        public async Task<JsonResult> Unblock_Credits_By_TransactionId(string id)
        {
            var result = await _credit_repo.Unblock_By_Transaction_Id(id);
            return Json(result);
        }

        [HttpGet("Cancel-Order/{id}")]
        public async Task<JsonResult> Cancel_Order(string id)
        {
            var result = await _shop_repo.Cancel_Order(id);
            return Json(result);
        }

        [HttpGet("Order-Items/{id}")]
        public async Task<JsonResult> Order_Items(string id)
        {
            var result = await _shop_repo.Shopping_Cart_Item_Get_By_Id(id);
            return Json(result);
        }

        [HttpGet("Confirm-COD-Payments/{id}")]
        public async Task<JsonResult> Update_COD_Payment_Status_By_Id(string id)
        {
            var result = await _shop_repo.Shopping_Cart_Tracking_Update_COD_Payment_Status_By_Id(id);
            return Json(result);
        }

        [HttpGet("Awb-Bills/{id}")]
        public async Task<JsonResult> Get_Awb_Bills(string id)
        {
            var awb = new Awb();
            awb.Search_Value = await _shop_repo.Get_Shipping_Ids_By_Tracking_Id(id);
            var result = await _fetchr.Get_Awb_Bills(awb);
            return Json(result);
        }

        [HttpGet("Awb-Bill/{id}")]
        public async Task<JsonResult> Get_Awb_Bill(string id)
        {
            var awb = new Awb();
            awb.Search_Value = new List<string>() { id };
            var result = await _fetchr.Get_Awb_Bills(awb);
            return Json(result);
        }

        [HttpGet("Invoice/{id}")]
        public async Task<FileResult> Get_Invoice(string id)
        {
            var cart = await _shop_repo.Shopping_Cart_Tracking_Get_By_Id(id);
            var addr = await _addr_repo.Get_By_Id(cart.Shipping_Address_Id);
            addr.Mobile = string.IsNullOrEmpty(addr.Mobile) ? string.Empty : addr.Mobile.StartsWith("+") ? addr.Mobile : string.Format("+{0}{1}", addr.ISDCode, addr.PhoneNumber);
            cart.ShippingAddress = addr;

            var invoice_template = _emailSender.GetMailTemplate(cart, "invoice.cshtml");
            var file = Utility.Create_Invoice(invoice_template, cart.Id);
            return File(file, "application/pdf", string.Format("{0}.pdf", id));
        }


        [HttpPost("Create-Shipment-Order")]
        public async Task<JsonResult> Create_Shipment_Order([FromBody] Shipment obj)
        {
            bool shipment_status = false;
            var result = new Models.DataProvider.DbResult();
            var cart = await _shop_repo.Shopping_Cart_Tracking_Get_By_Id(obj.Cart_Id);
            if (!cart.Status.Equals("Pending")) { result.Message = "Order already created."; return Json(result); }

            var addr = await _addr_repo.Get_By_Id(cart.Shipping_Address_Id);
            cart.ShippingAddress = addr;
            string mobile = string.IsNullOrEmpty(addr.Mobile) ? string.Empty : addr.Mobile.StartsWith("+") ? addr.Mobile : string.Format("+{0}{1}", addr.ISDCode, addr.PhoneNumber);

            if (cart.CartType == "Shopping")
            {
                var dorship = new Dorship(cart.Id, obj.PaymentType);
                var order_rec = new Receiver_Data(addr.Full_Name, mobile, cart.Email, addr.Country, addr.State, addr.City, addr.Address_Line, addr.ZipCode);
                order_rec.Street_Two = addr.Address_Line2; order_rec.Street_Three = addr.Address_Line3;
                dorship.Receiver_Data = order_rec;

                var packages = new List<Package_Data>();

                foreach (var x in cart.ShopingCart)
                {
                    decimal price = (obj.PaymentType == "cc") ? 0 : x.TotalPrice;
                    var package = new Package_Data(x.SKU, price, x.Product_Name);
                    packages.Add(package);
                }
                dorship.Package_Data = packages;
                var shipment = await _fetchr.Create_Order_For_Shipment(dorship);

                if (shipment.Success)
                {
                    if (shipment.Data.Success)
                    {
                        result.Message = "Order created successfully.";
                        result.Success = shipment_status = true;
                        foreach (var x in shipment.Data.Dorship.Package_Data)
                        {
                            var abc = _shop_repo.Update_Shipment_Id(x.Tracking_No, shipment.Data.Dorship.Order_Reference, x.Package_Reference);
                            abc.Wait();
                        }
                        var ord = await _shop_repo.Shopping_Cart_Tracking_Update_Order_Status(obj.Cart_Id, shipment_status, obj.PaymentType, "Order Created");

                        var invoice_template = _emailSender.GetMailTemplate(cart, "invoice.cshtml");
                        var file = Utility.Create_Invoice(invoice_template, cart.Id);
                        var invoice_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resources", "Invoice", string.Format("{0}.pdf", cart.Id));
                        var html_template = _emailSender.GetMailTemplate(cart, "order-confirmation.cshtml");
                        var mailResult = await _emailSender.Send_Email_With_Attachement(cart.Name, cart.Email, "Order Confirmation", html_template, invoice_path);


                    }
                }
                else
                {
                    result.Message = shipment.Content;
                    await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(string.Format("Error: Create_Order_For_Shipment, Id: {0}", obj.Cart_Id)), shipment.Content));
                }
            }

            if (cart.CartType == "Selling")
            {
                var pickup = new PickUp();
                var pickup_data = new PickUp_Data { Email = cart.Email, Phone_Number = mobile, Name = addr.Full_Name };
                pickup_data.Address = string.Format("{0}, {1}", addr.Address_Line, addr.Area);
                pickup_data.City = addr.City;
                pickup_data.Country = addr.Country;
                pickup_data.Order_Reference = cart.Id;
                foreach (var item in cart.SellingCart)
                {
                    pickup_data.Product_Items.Add(new PickUp_Product { Product_Description = string.Format("{0}, {1} ({2})", item.Product_Name, item.Product_TypeName, item.Product_Type) });
                }
                pickup.Data.Add(pickup_data);

                var shipment = await _fetchr.Create_PickUp_Order(pickup);
                if (shipment.Success)
                {
                    if (shipment.Data.Success)
                    {
                        var resp = shipment.Data;
                        result.Message = "Order created successfully.";
                        result.Success = shipment_status = resp.Success;
                        if (shipment_status)
                        {
                            var abc = _shop_repo.Update_Shipment_Id_Pickup(resp.Tracking_Id, resp.Order_Reference);
                            abc.Wait();
                            var payment = await _pay_repo.Get();
                            await _credit_repo.Add_New(new Credit { Amount = cart.Total_Sum * payment.Credit_Ratio, IsBlocked = true, Trancaction_Id = obj.Cart_Id, Transaction_Type = "CREDIT", Transaction_For = "Selling", UserId = cart.UserId, Comments = string.Format("Added Credit for Order #{0}", obj.Cart_Id) });
                            var ord = await _shop_repo.Shopping_Cart_Tracking_Update_Order_Status(obj.Cart_Id, shipment_status, obj.PaymentType, "Order Created");

                            var invoice_template = _emailSender.GetMailTemplate(cart, "invoice.cshtml");
                            var file = Utility.Create_Invoice(invoice_template, cart.Id);
                            var invoice_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resources", "Invoice", string.Format("{0}.pdf", cart.Id));
                            var html_template = _emailSender.GetMailTemplate(cart, "order-confirmation.cshtml");
                            var mailResult = await _emailSender.Send_Email_With_Attachement(cart.Name, cart.Email, "Order Confirmation", html_template, invoice_path);
                        }
                    }
                }
                else
                {
                    result.Message = shipment.Content;
                    await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(string.Format("Error: Create_Order_For_Shipment, Id: {0}", obj.Cart_Id)), shipment.Content));
                }
            }
            return Json(result);
        }

        [HttpPost("download-csv-shoppingorder")]
        public async Task<IActionResult> Get_CSV_Of_shoppingorder([FromForm] Order_Filter obj)
        {
            #region where_clouse
            string where_clouse = string.Empty;
            if (!string.IsNullOrEmpty(obj.PaymentType) && obj.PaymentType != "all")
            {
                where_clouse += string.Format(" AND t1.PaymentType='{0}' ", obj.PaymentType);
            }

            if (!string.IsNullOrEmpty(obj.CartType) && obj.CartType != "all")
            {
                where_clouse += string.Format(" AND t1.CartType='{0}' ", obj.CartType);
            }

            if (!string.IsNullOrEmpty(obj.Product_Category) && obj.Product_Category != "ALL")
            {
                where_clouse += string.Format(" AND t4.Product_Type='{0}' ", obj.Product_Category);
            }

            if (!string.IsNullOrEmpty(obj.Product_SKU))
            {
                where_clouse += string.Format(" AND t4.SKU='{0}' ", obj.Product_SKU);
            }

            if (!string.IsNullOrEmpty(obj.OrderNumber))
            {
                where_clouse += string.Format(" AND t1.Id='{0}' ", obj.OrderNumber);
            }

            if (obj.Product_SubCategoryId.HasValue)
            {
                where_clouse += string.Format(" AND t4.Product_TypeId='{0}' ", obj.Product_SubCategoryId.Value);
            }

            if (obj.Platform_Id.HasValue)
            {
                where_clouse += string.Format(" AND t4.Product_TypeId='{0}' ", obj.Platform_Id.Value);
            }

            if (obj.BeginValue.HasValue)
            {
                where_clouse += string.Format(" AND t1.Total_Sum >= {0} ", obj.BeginValue.Value);
            }

            if (obj.EndValue.HasValue)
            {
                where_clouse += string.Format(" AND t1.Total_Sum <= {0} ", obj.EndValue.Value);
            }

            if (obj.StartDate.HasValue)
            {
                where_clouse += string.Format(" AND CAST(t1.OrderDate as DATE) >= CAST('{0}' as DATE) ", obj.StartDate.Value.ToString("MM/dd/yyyy"));
            }

            if (obj.EndDate.HasValue)
            {
                where_clouse += string.Format(" AND CAST(t1.OrderDate as DATE) <= CAST('{0}' as DATE) ", obj.EndDate.Value.ToString("MM/dd/yyyy"));
            }
            #endregion

            #region Query Builder
            string query = string.Empty;
            query += "  SELECT DISTINCT  t1.Id,CONCAT(t2.FirstName, ' ', t2.LastName) as Name,Total_Item,Total_Price,Tax_Rate,Status,Total_Sum,Tax_Amount,ShippingCharge,";
            query += " CONCAT(t3.First_Name, ' ', t3.Last_Name, ', ', t3.Address_Line, ' ', t3.Area, ', ', t3.City, ' ', t3.[State], ', ', t3.Country, ' ', t3.ZipCode, ', Mobile: ', t3.Mobile) as 'Address' ";
            query += " ,Payment_Status,Auth_Status,Auth_StatusTxt,Auth_Code,Auth_Message,Transaction_Id,Transaction_Date,case when OrderPlaced = 1  then 'Yes' else 'No' end as OrderPlaced,CONVERT(char(10), OrderDate, 126) as [OrderDate],PaymentType,CartType, ";
            query += " case when IsDeleted = 1 then 'Yes' else 'No' end as IsDeleted,Total_Deduction,CreditUsed,PromoCode,Email ";
            query += "  FROM[Shopping_Cart_Tracking_Tbl] t1 ";
            query += " LEFT JOIN UsersTbl t2 on t1.UserId = t2.Id ";
            query += " LEFT JOIN AddressTbl t3 on t1.Shipping_Address_Id = t3.Id ";
            query += " LEFT JOIN Shopping_Cart_Items_Tbl t4 on t1.Id = t4.Tracking_Id ";
            query += " WHERE t1.[Status] != 'Ready' AND t1.IsDeleted=0  ";

            query += where_clouse;
            //query += " ORDER by t1.OrderDate DESC ";
            #endregion

            var result = await _shop_repo.GetShoppingorder_By_Custom_QueryAsync(query);
            var data = Utility.ExportExcel(result.PagedSet.ToList(), "ShoppingOrder");
            return File(data, "text/xlsx", $"ShoppingOrder.xlsx");
        }

        [HttpGet("Orders")]
        public IActionResult Shopping_Order()
        {
            return View();
        }

        [HttpGet("Selling-Order")]
        public IActionResult Selling_Order()
        {
            return View();
        }

        [HttpGet("Order-Cancelation")]
        public IActionResult Order_Cancelation()
        {
            return View();
        }

        [HttpPost("Get-Order-Cancelation")]
        public async Task<JsonResult> Get_Order_Canceleation_List([FromBody] Order_Filter obj)
        {
            #region where_clouse
            string where_clouse = string.Empty;
            if (!string.IsNullOrEmpty(obj.PaymentType) && obj.PaymentType != "all")
            {
                where_clouse += string.Format(" AND t3.PaymentType='{0}' ", obj.PaymentType);
            }
            

            if (!string.IsNullOrEmpty(obj.Product_Category) && obj.Product_Category != "ALL")
            {
                where_clouse += string.Format(" AND t2.Product_Type='{0}' ", obj.Product_Category);
            }

            if (!string.IsNullOrEmpty(obj.Product_SKU))
            {
                where_clouse += string.Format(" AND t1.SKU='{0}' ", obj.Product_SKU);
            }

            if (!string.IsNullOrEmpty(obj.OrderNumber))
            {
                where_clouse += string.Format(" AND t1.Tracking_Id='{0}' ", obj.OrderNumber);
            }

            if (obj.Product_SubCategoryId.HasValue)
            {
                where_clouse += string.Format(" AND t2.Product_TypeId='{0}' ", obj.Product_SubCategoryId.Value);
            }

            if (obj.Platform_Id.HasValue)
            {
                where_clouse += string.Format(" AND t2.Product_TypeId='{0}' ", obj.Platform_Id.Value);
            }

            if (obj.BeginValue.HasValue)
            {
                where_clouse += string.Format(" AND t2.TotalPrice >= {0} ", obj.BeginValue.Value);
            }

            if (obj.EndValue.HasValue)
            {
                where_clouse += string.Format(" AND t2.TotalPrice <= {0} ", obj.EndValue.Value);
            }

            if (obj.StartDate.HasValue)
            {
                where_clouse += string.Format(" AND CAST(t1.Resuest_Date as DATE) >= CAST('{0}' as DATE) ", obj.StartDate.Value.ToString("MM/dd/yyyy"));
            }

            if (obj.EndDate.HasValue)
            {
                where_clouse += string.Format(" AND CAST(t1.Resuest_Date as DATE) <= CAST('{0}' as DATE) ", obj.EndDate.Value.ToString("MM/dd/yyyy"));
            }
            #endregion

            #region Query Builder
            string query = string.Empty;
            query += " SELECT COUNT(t1.Id) as Total FROM Cancelation_Request_Tbl t1 ";
            query += " LEFT JOIN Shopping_Cart_Items_Tbl t2 on t1.SKU=t2.SKU AND t1.Tracking_Id=t2.Tracking_Id ";
            query += " LEFT JOIN Shopping_Cart_Tracking_Tbl t3 on t1.Tracking_Id=t3.Id ";
            query += " LEFT JOIN UsersTbl t4 on t3.UserId=t4.Id ";
            query += " WHERE t1.Id IS NOT NULL  ";
            query += where_clouse;
            query += "; ";


            query += " SELECT DISTINCT t1.Id, t1.SKU, t1.[Status], t1.Tracking_Id, t1.Resuest_Date,  ";
            query += " t2.Product_Id, t2.Product_Name, t2.Product_Type, t2.Product_TypeId, t2.Product_TypeName, ";
            query += " t2.Quantity, t2.PageUrl, t2.ImageUrl, t2.TotalPrice, t3.PaymentType, ";
            query += " CONCAT(t4.FirstName, ' ', t4.LastName) as Name, t4.Email  ";
            query += " FROM Cancelation_Request_Tbl t1 ";
            query += " LEFT JOIN Shopping_Cart_Items_Tbl t2 on t1.SKU=t2.SKU AND t1.Tracking_Id=t2.Tracking_Id ";
            query += " LEFT JOIN Shopping_Cart_Tracking_Tbl t3 on t1.Tracking_Id=t3.Id ";
            query += " LEFT JOIN UsersTbl t4 on t3.UserId=t4.Id ";
            query += " WHERE t1.Id IS NOT NULL  ";
            query += where_clouse;
            query += " ORDER by t1.Resuest_Date DESC ";
            query += string.Format(" OFFSET {0} ROWS ", obj.offset);
            query += string.Format(" FETCH NEXT {0} ROWS ONLY ", obj.rows);
            query += "; ";
            #endregion

            var result = await _order_repo.Get_Cancelation_Orders_By_Query(query);
            return Json(result);
        }


        [HttpPost("Export-Order-Cancelation")]
        public async Task<FileResult> Export_Order_Canceleation_List([FromForm] Order_Filter obj)
        {
            #region where_clouse
            string where_clouse = string.Empty;
            if (!string.IsNullOrEmpty(obj.PaymentType) && obj.PaymentType != "all")
            {
                where_clouse += string.Format(" AND t3.PaymentType='{0}' ", obj.PaymentType);
            }


            if (!string.IsNullOrEmpty(obj.Product_Category) && obj.Product_Category != "ALL")
            {
                where_clouse += string.Format(" AND t2.Product_Type='{0}' ", obj.Product_Category);
            }

            if (!string.IsNullOrEmpty(obj.Product_SKU))
            {
                where_clouse += string.Format(" AND t1.SKU='{0}' ", obj.Product_SKU);
            }

            if (!string.IsNullOrEmpty(obj.OrderNumber))
            {
                where_clouse += string.Format(" AND t1.Tracking_Id='{0}' ", obj.OrderNumber);
            }

            if (obj.Product_SubCategoryId.HasValue)
            {
                where_clouse += string.Format(" AND t2.Product_TypeId='{0}' ", obj.Product_SubCategoryId.Value);
            }

            if (obj.Platform_Id.HasValue)
            {
                where_clouse += string.Format(" AND t2.Product_TypeId='{0}' ", obj.Platform_Id.Value);
            }

            if (obj.BeginValue.HasValue)
            {
                where_clouse += string.Format(" AND t2.TotalPrice >= {0} ", obj.BeginValue.Value);
            }

            if (obj.EndValue.HasValue)
            {
                where_clouse += string.Format(" AND t2.TotalPrice <= {0} ", obj.EndValue.Value);
            }

            if (obj.StartDate.HasValue)
            {
                where_clouse += string.Format(" AND CAST(t1.Resuest_Date as DATE) >= CAST('{0}' as DATE) ", obj.StartDate.Value.ToString("MM/dd/yyyy"));
            }

            if (obj.EndDate.HasValue)
            {
                where_clouse += string.Format(" AND CAST(t1.Resuest_Date as DATE) <= CAST('{0}' as DATE) ", obj.EndDate.Value.ToString("MM/dd/yyyy"));
            }
            #endregion

            #region Query Builder
            string query = string.Empty;
            query += " SELECT COUNT(t1.Id) as Total FROM Cancelation_Request_Tbl t1 ";
            query += " LEFT JOIN Shopping_Cart_Items_Tbl t2 on t1.SKU=t2.SKU AND t1.Tracking_Id=t2.Tracking_Id ";
            query += " LEFT JOIN Shopping_Cart_Tracking_Tbl t3 on t1.Tracking_Id=t3.Id ";
            query += " LEFT JOIN UsersTbl t4 on t3.UserId=t4.Id ";
            query += " WHERE t1.Id IS NOT NULL  ";
            query += where_clouse;
            query += "; ";


            query += " SELECT DISTINCT t1.Id, t1.SKU, t1.[Status], t1.Tracking_Id, t1.Resuest_Date,  ";
            query += " t2.Product_Id, t2.Product_Name, t2.Product_Type, t2.Product_TypeId, t2.Product_TypeName, ";
            query += " t2.Quantity, t2.PageUrl, t2.ImageUrl, t2.TotalPrice, t3.PaymentType, ";
            query += " CONCAT(t4.FirstName, ' ', t4.LastName) as Name, t4.Email  ";
            query += " FROM Cancelation_Request_Tbl t1 ";
            query += " LEFT JOIN Shopping_Cart_Items_Tbl t2 on t1.SKU=t2.SKU AND t1.Tracking_Id=t2.Tracking_Id ";
            query += " LEFT JOIN Shopping_Cart_Tracking_Tbl t3 on t1.Tracking_Id=t3.Id ";
            query += " LEFT JOIN UsersTbl t4 on t3.UserId=t4.Id ";
            query += " WHERE t1.Id IS NOT NULL  ";
            query += where_clouse;
            query += " ORDER by t1.Resuest_Date DESC ";
            //query += string.Format(" OFFSET {0} ROWS ", obj.offset);
            //query += string.Format(" FETCH NEXT {0} ROWS ONLY ", obj.rows);
            query += "; ";
            #endregion

            var result = await _order_repo.Get_Cancelation_Orders_By_Query(query);
            var data = Utility.ExportExcel(result.PagedSet.ToList(), "CanceledOrder");
            return File(data, "text/xlsx", $"CanceledOrder.xlsx");
        }

    }
}
