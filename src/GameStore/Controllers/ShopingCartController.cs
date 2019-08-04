using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameStore.Models.Interface;
using GameStore.Models.Entity;
using GameStore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GameStore.Services;
using GameStore.Models.DataProvider;
using System.Text.RegularExpressions;

namespace GameStore.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = Constant.Auth_Scheme)]
    public class ShopingCartController : BaseController
    {
        private FetchrApi _fetchr = null;
        private readonly IPageRepository _page_repo = null;
        private readonly IGamePlatformRepository _gp_repo = null;
        private readonly IProductRepository _prod_repo = null;
        private readonly IGameRepository _game_repo = null;
        private readonly ICommon_Name_UrlRepository _common_repo = null;
        private readonly IPaymentSettingRepository _pay_repo = null;
        private readonly IShopping_Cart_Repository _shop_repo = null;
        private readonly IAddressRepository _addr_repo = null;
        private readonly IReviewRepository _review_repo = null;
        private readonly IWishListRepository _wish_repo = null;
        private readonly ISearchRepository _search_repo = null;
        private readonly ICreditRepository _credit_repo = null;
        private readonly IEmailSender _emailSender = null;
        private readonly IUserRepository _user_repo = null;
        private readonly ISmsSender _sms_sender = null;

        public ShopingCartController(IPageRepository page_repo,
            IGamePlatformRepository gp_repo,
            IProductRepository prod_repo,
            IGameRepository game_repo,
            IUserRepository user_repo,
            ICommon_Name_UrlRepository common_repo,
            IPaymentSettingRepository pay_repo,
            IShopping_Cart_Repository shop_repo,
            IAddressRepository addr_repo,
            IReviewRepository review_repo,
            IWishListRepository wish_repo,
            ISearchRepository search_repo,
            ICreditRepository credit_repo,
            IEmailSender emailSender,
            ISmsSender sms_sender)
        {
            _page_repo = page_repo;
            _gp_repo = gp_repo;
            _prod_repo = prod_repo;
            _game_repo = game_repo;
            _common_repo = common_repo;
            _pay_repo = pay_repo;
            _shop_repo = shop_repo;
            _addr_repo = addr_repo;
            _review_repo = review_repo;
            _wish_repo = wish_repo;
            _search_repo = search_repo;
            _credit_repo = credit_repo;
            _emailSender = emailSender;
            _user_repo = user_repo;
            _sms_sender = sms_sender;
            _fetchr = new FetchrApi();
        }

        [AllowAnonymous]
        [HttpGet("Sell-Yours")]
        public async Task<IActionResult> Sell_Yours()
        {
            var pageContent = await _page_repo.Get_By_Url("sell-yours");
            if (pageContent == null) { return NotFound(); }
            ViewBag.Payments = await _pay_repo.Get();
            return View(pageContent);
        }


        [AllowAnonymous]
        [HttpPost("Sell-Yours/Search")]
        public async Task<JsonResult> Seach_Products([FromBody] Sell_Query obj)
        {
            obj.SearchTxt = Regex.Replace(obj.SearchTxt, "[^a-zA-Z0-9 ]", "");
            var result = await _search_repo.Search_By_query_for_sell(obj);
            return Json(result);
        }


        [HttpPost("Add-To-WishList")]
        public async Task<JsonResult> Add_To_WishList([FromBody] WishList obj)
        {
            long userId = Convert.ToInt64(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            obj.UserId = userId;
            var result = await _wish_repo.Add_New(obj);
            return Json(result);
        }

        [HttpPost("Submit-Review")]
        public async Task<PartialViewResult> Submit_Review(Review obj)
        {
            string msg = string.Empty;
            long userId = Convert.ToInt64(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            string userName = Request.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            obj.UserId = userId;
            obj.UserName = userName;
            var result = await _review_repo.Add_New_Review(obj);
            if (result.Success) { msg = "Thanks for your review."; }
            else { msg = "Sorry, Something went wrong."; }
            return PartialView("Info", msg);
        }

        [HttpGet("Get-Shipment-History/{id}")]
        public async Task<PartialViewResult> Get_Shipment_History(string id)
        {
            string msg = "Sorry, We didn't find anything about this shipment.";
            var result = await _fetchr.Get_Shipment_History(id);
            if (result.Success)
            {
                return PartialView("_Shipment_Logs", result.Data);
            }
            return PartialView("Info", msg);
        }

        [AllowAnonymous]
        [HttpGet("Shopping-Cart")]
        public async Task<IActionResult> Index()
        {            
            var pageContent = await _page_repo.Get_By_Url("shopping-cart");
            if (pageContent == null) { return NotFound(); }
            ViewBag.Payments = await _pay_repo.Get();
            decimal _creditAmount = 0;
            if (Request.HttpContext.User.Identity.IsAuthenticated)
            {
                long userId = Convert.ToInt64(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = await _user_repo.GetUserByIdAsync(userId);
                var user = result.Data as User;
                _creditAmount = user.CreditAmount;
            }
            ViewBag.CreditAmount = _creditAmount;
            return View(pageContent);
        }

        [AllowAnonymous]
        [HttpGet("Shopping-Cart/{id}")]
        public async Task<JsonResult> Get_Shopping_Cart(long id)
        {
            var result = await _shop_repo.Shopping_Cart_Tracking_Get_By_UserId(id);
            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet("Selling-Cart")]
        public async Task<IActionResult> Selling_Cart()
        {
            var pageContent = await _page_repo.Get_By_Url("selling-cart");
            if (pageContent == null) { return NotFound(); }
            ViewBag.Payments = await _pay_repo.Get();
            return View(pageContent);
        }

        [AllowAnonymous]
        [HttpGet("Selling-Cart/{id}")]
        public async Task<JsonResult> Get_Selling_Cart(long id)
        {
            var result = await _shop_repo.Selling_Cart_Tracking_Get_By_UserId(id);
            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet("Shopping-Cart/Get-Address/{id}")]
        public async Task<JsonResult> Get_All_Address(long id)
        {
            var result = await _addr_repo.Get_By_UserId(id);
            return Json(result);
        }

        [AllowAnonymous]
        [HttpPost("Shopping-Cart/Add-New-Address")]
        public async Task<JsonResult> Add_New_Address([FromBody] IEnumerable<Address> obj)
        {
            var result = await _addr_repo.Add_Multiple_address(obj);
            return Json(result);
        }

        [AllowAnonymous]
        [HttpPost("Shopping-Cart/Update-Shipping-Address")]
        public async Task<JsonResult> Update_Shipping_Address([FromBody] Cart_DTO obj)
        {
            var result = await _shop_repo.Shopping_Cart_Tracking_Update_Address_Id(obj);
            return Json(result);
        }

        [AllowAnonymous]
        [HttpPost("Shopping-Cart/Update-Total-Price")]
        public async Task<JsonResult> Update_Total_Price([FromBody] Cart_DTO obj)
        {
            var result = await _shop_repo.Shopping_Cart_Tracking_Update_Total_Price_Id(obj);
            if (!string.IsNullOrEmpty(obj.Id))
            {
                var new_list = new List<CartItem>();
                foreach (var x in obj.ShopingCart)
                {
                    if (x.Product_Type == Constant.GAME)
                    {
                        var product = await _game_repo.Get_Details_By_SKU(x.SKU);
                        x.Product_TypeName = product.PlatformName;
                        x.Product_Name = product.Name;
                        x.ImageUrl = product.ImageUrl;
                        x.PageUrl = product.PageUrl;
                    }

                    if (x.Product_Type == Constant.ACCESSORIES || x.Product_Type == Constant.CONSOLE)
                    {
                        var product = await _prod_repo.Get_Details_By_SKU(x.SKU);
                        x.Product_TypeName = product.ProductType;
                        x.Product_Name = product.Name;
                        x.ImageUrl = product.ImageUrl;
                        x.PageUrl = product.PageUrl;
                    }

                    x.Tracking_Id = obj.Id;
                    new_list.Add(x);
                }
                var result2 = await _shop_repo.Add_New_Shopping_Cart_Items(new_list);
            }
            return Json(result);
        }

        [AllowAnonymous]
        [HttpPost("Selling-Cart/Update-Total-Price")]
        public async Task<JsonResult> Update_Total_Price_Sell([FromBody] Cart_DTO obj)
        {
            var result = await _shop_repo.Shopping_Cart_Tracking_Update_Total_Price_Id(obj);
            if (!string.IsNullOrEmpty(obj.Id))
            {
                var new_list = new List<CartItem>();
                foreach (var x in obj.SellingCart)
                {
                    if (x.Product_Type == Constant.GAME)
                    {
                        var product = await _game_repo.Get_Details_By_SKU(x.SKU);
                        x.Product_TypeName = product.PlatformName;
                        x.Product_Name = product.Name;
                        x.ImageUrl = product.ImageUrl;
                        x.PageUrl = product.PageUrl;
                    }

                    if (x.Product_Type == Constant.ACCESSORIES || x.Product_Type == Constant.CONSOLE)
                    {
                        var product = await _prod_repo.Get_Details_By_SKU(x.SKU);
                        x.Product_TypeName = product.ProductType;
                        x.Product_Name = product.Name;
                        x.ImageUrl = product.ImageUrl;
                        x.PageUrl = product.PageUrl;
                    }

                    x.Tracking_Id = obj.Id;                    
                    new_list.Add(x);
                }
                var result2 = await _shop_repo.Add_New_Shopping_Cart_Items(new_list);
            }
            return Json(result);
        }

        [AllowAnonymous]
        [HttpPost("Shopping-Cart/Update")]
        public async Task<JsonResult> Update_Shopping_Cart([FromBody] Cart_DTO obj)
        {
            if (obj != null)
            {
                var new_list = new List<CartItem>();
                foreach (var x in obj.ShopingCart)
                {
                    if (x.Product_Type == Constant.GAME)
                    {
                        var product = await _game_repo.Get_Details_By_SKU(x.SKU);
                        x.Product_TypeName = product.PlatformName;
                        x.Product_Name = product.Name;
                        x.ImageUrl = product.ImageUrl;
                        x.PageUrl = product.PageUrl;                        
                    }

                    if (x.Product_Type == Constant.ACCESSORIES || x.Product_Type == Constant.CONSOLE)
                    {
                        var product = await _prod_repo.Get_Details_By_SKU(x.SKU);
                        x.Product_TypeName = product.ProductType;
                        x.Product_Name = product.Name;
                        x.ImageUrl = product.ImageUrl;
                        x.PageUrl = product.PageUrl;
                    }
                    
                    new_list.Add(x);
                }
                obj.ShopingCart = new_list;
                string tracking_id = string.Empty;
                if (string.IsNullOrEmpty(obj.Id))
                {
                    tracking_id = await _shop_repo.Add_New_Shopping_Cart_Tracking(obj);
                }
                else
                {
                    tracking_id = await _shop_repo.Shopping_Cart_Tracking_Update_By_Id(obj);
                }
                if (!string.IsNullOrEmpty(tracking_id))
                {
                    foreach (var item in new_list)
                    {
                        item.Tracking_Id = tracking_id;
                    }
                    var result = await _shop_repo.Add_New_Shopping_Cart_Items(new_list);
                }
                obj.Id = tracking_id;
            }
            return Json(obj);
        }

        [AllowAnonymous]
        [HttpPost("Selling-Cart/Update")]
        public async Task<JsonResult> Update_Selling_Cart([FromBody] Cart_DTO obj)
        {
            if (obj != null)
            {
                var new_list = new List<CartItem>();
                foreach (var x in obj.SellingCart)
                {
                    if (x.Product_Type == Constant.GAME)
                    {
                        var product = await _game_repo.Get_Details_By_SKU(x.SKU);
                        x.Product_TypeName = product.PlatformName;
                        x.Product_Name = product.Name;
                        x.ImageUrl = product.ImageUrl;
                        x.PageUrl = product.PageUrl;
                    }

                    if (x.Product_Type == Constant.ACCESSORIES || x.Product_Type == Constant.CONSOLE)
                    {
                        var product = await _prod_repo.Get_Details_By_SKU(x.SKU);
                        x.Product_TypeName = product.ProductType;
                        x.Product_Name = product.Name;
                        x.ImageUrl = product.ImageUrl;
                        x.PageUrl = product.PageUrl;
                    }

                    new_list.Add(x);
                }
                obj.SellingCart = new_list;
                string tracking_id = string.Empty;
                if (string.IsNullOrEmpty(obj.Id))
                {
                    tracking_id = await _shop_repo.Add_New_Shopping_Cart_Tracking(obj);
                } 
                else
                {
                    tracking_id = await _shop_repo.Shopping_Cart_Tracking_Update_By_Id(obj);
                }
                if (!string.IsNullOrEmpty(tracking_id))
                {
                    foreach (var item in new_list)
                    {
                        item.Tracking_Id = tracking_id;
                    }
                    var result = await _shop_repo.Add_New_Shopping_Cart_Items(new_list);
                }
                obj.Id = tracking_id;
            }
            return Json(obj);
        }


        [AllowAnonymous]
        [HttpPost("Selling-Cart/sell")]
        public async Task<JsonResult> Selling_Cart_Sell_New([FromBody] Cart_DTO obj)
        {
            var new_list = new List<CartItem>();
            foreach (var x in obj.SellingCart)
            {
                if (x.BoxValue == -1 || x.ManualValue == -1 || x.Condition == -1) { continue; }
                if (x.Product_Type == Constant.GAME)
                {
                    var product = await _game_repo.Get_Details_By_SKU(x.SKU);
                    x.Product_TypeName = product.PlatformName;
                    x.Product_Name = product.Name;
                    x.ImageUrl = product.ImageUrl;
                    x.PageUrl = product.PageUrl;
                }

                if (x.Product_Type == Constant.ACCESSORIES || x.Product_Type == Constant.CONSOLE)
                {
                    var product = await _prod_repo.Get_Details_By_SKU(x.SKU);
                    x.Product_TypeName = product.ProductType;
                    x.Product_Name = product.Name;
                    x.ImageUrl = product.ImageUrl;
                    x.PageUrl = product.PageUrl;
                }

                new_list.Add(x);
            }
            obj.SellingCart = new_list;
            if (string.IsNullOrEmpty(obj.Id))
            {
                obj.Id = await _shop_repo.Add_New_Shopping_Cart_Tracking(obj);
            }
            else
            {
                obj.Id = await _shop_repo.Shopping_Cart_Tracking_Update_By_Id(obj);
            }
            obj.Id = await _shop_repo.Shopping_Cart_Tracking_Update_Total_Price_Id(obj);

            if (!string.IsNullOrEmpty(obj.Id))
            {
                foreach (var item in new_list)
                {
                    item.Tracking_Id = obj.Id;
                }
                var result = await _shop_repo.Add_New_Shopping_Cart_Items(new_list);
            }
            return Json(obj.Id);
        }


        [AllowAnonymous]
        [HttpGet("Check-Out-1/{id}")]
        public async Task<IActionResult> Check_Out_Step_One(string id)
        {
            ViewBag.TrackingId = id;
            ViewBag.Payments = await _pay_repo.Get();
            ViewData["ReturnUrl"] = string.Format("/check-out-1/{0}", id);
            var result = await _shop_repo.Shopping_Cart_Tracking_Get_By_Id(id);
            if (result == null) { return RedirectToAction("Index", "ShopingCart"); }
            ViewBag.CartType = result.CartType;
            if (Request.HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(string.Format("/check-out-2/{0}", id));
            }
            return View();
        }

        
        [HttpGet("Check-Out-2/{id}")]
        public async Task<IActionResult> Check_Out_Step_Two(string id)
        {
            ViewBag.TrackingId = id;
            ViewBag.Payments = await _pay_repo.Get();
            ViewBag.NextStep = string.Format("/check-out-3/{0}", id);
            ViewData["ReturnUrl"] = string.Format("/check-out-2/{0}", id);
            var result = await _shop_repo.Shopping_Cart_Tracking_Get_By_Id(id);
            if (result == null) { return RedirectToAction("Index", "ShopingCart"); }
            ViewBag.CartType = result.CartType;
            long userId = Convert.ToInt64(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            ViewBag.AddressList = await _addr_repo.Get_By_UserId(userId);
            
            return View();
        }

        [HttpGet("Check-Out-3/{id}")]
        public async Task<IActionResult> Check_Out_Step_Three(string id)
        {
            ViewBag.TrackingId = id;
            ViewBag.Payments = await _pay_repo.Get();
            ViewBag.NextStep = string.Format("/check-out-4/{0}", id);
            ViewData["ReturnUrl"] = string.Format("/check-out-3/{0}", id);
            var result = await _shop_repo.Shopping_Cart_Tracking_Get_By_Id(id);
            if (result == null) { return RedirectToAction("Index", "ShopingCart"); }
            if (result.Shipping_Address_Id == 0) { return RedirectToLocal(string.Format("/check-out-2/{0}", id)); }
            ViewBag.CartType = result.CartType;
            ViewBag.TotalAmount = result.Total_Sum;
            return View();
        }

        [HttpGet("Check-Out-4/{id}")]
        public async Task<IActionResult> Check_Out_Step_Four(string id)
        {
            ViewBag.TrackingId = id;
            ViewBag.Payments = await _pay_repo.Get();
            var result = await _shop_repo.Shopping_Cart_Tracking_Get_By_Id(id);
            if (result == null) { return RedirectToAction("Index", "ShopingCart"); }
            if (result.Shipping_Address_Id == 0) { return RedirectToLocal(string.Format("/check-out-2/{0}", id)); }
            ViewBag.CartType = result.CartType;
            var obj = new Cart_Details(result);
            obj.TimeStamp = Utility.GetUnixTime();
            obj.User.Email = Request.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            obj.Billing = await _addr_repo.Get_By_Id(result.Billing_Address_Id);
            obj.Shipping = obj.Billing;            

            obj.CallBackUrl = string.Format("{0}/payment/call-back", Constant.DomainNane);
            obj.SuccessUrl = string.Format("auto:{0}/payment-success/{1}", Constant.DomainNane, id);
            obj.CanceledUrl = string.Format("auto:{0}/payment-cancelled/{1}", Constant.DomainNane, id);
            obj.DecliendUrl = string.Format("auto:{0}/payment-decliend/{1}", Constant.DomainNane, id);
            
            obj.Signature = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:Items:return", Constant.StoreID, obj.Cart.Total_Sum, Constant.Currency, Constant.TelrTestMode, obj.TimeStamp, obj.Cart.Id);
            obj.Signature = Utility.Get_Hash(obj.Signature, Constant.TelrSecretKey).ToLower();
            obj.ReturnSignature = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", obj.CallBackUrl, obj.CallBackUrl, obj.CallBackUrl, obj.SuccessUrl, obj.DecliendUrl, obj.CanceledUrl, obj.Signature);
            obj.ReturnSignature = Utility.Get_Hash(obj.ReturnSignature, Constant.TelrSecretKey).ToLower();

            return View("PaymentRequestPage", obj);
        }

        [HttpPost("Create-Order")]
        public async Task<JsonResult> Create_Order([FromBody] Shipment obj)
        {
            bool order_status = true;
            var cart = await _shop_repo.Shopping_Cart_Tracking_Get_By_Id(obj.Cart_Id);
            var addr = await _addr_repo.Get_By_Id(cart.Shipping_Address_Id);
            addr.Mobile = string.IsNullOrEmpty(addr.Mobile) ? string.Empty : addr.Mobile.StartsWith("+") ? addr.Mobile : string.Format("+{0}{1}", addr.ISDCode, addr.PhoneNumber);
            cart.ShippingAddress = addr;
            if (cart.OrderPlaced) { return Json(order_status); }
            var ord = await _shop_repo.Shopping_Cart_Tracking_Update_Order_Status(obj.Cart_Id, order_status, obj.PaymentType, "Pending");
            var html_template = _emailSender.GetMailTemplate(cart, "order-confirmation.cshtml");
            var mailResult = await _emailSender.SendEmailAsync(cart.Name, cart.Email, "Order Confirmation", html_template);
            await _sms_sender.SendSmsAsync(string.Format("{0}{1}", addr.ISDCode, addr.PhoneNumber), string.Format("Dear {0}, Your order has been successfully placed. Payable amount is {1} .", cart.Name, Math.Round(cart.Total_Sum, 1)));
            return Json(order_status);
        }

        

        [AllowAnonymous]
        [HttpGet("Payment-Success/{id}")]
        public async Task<IActionResult> Check_Out_Step_Five(string id)
        {
            ViewBag.TrackingId = id;
            bool payment_status = false;
            var trans = await TelrApi.Get_Transactions_By_Cart(id);
            var last_trans = trans.FirstOrDefault();
            if (last_trans == null) { return RedirectToAction("Index", "ShopingCart"); }
            string status = (last_trans.auth_status == "A") ? "Success" : "Failed";             
            var result = await _shop_repo.Shopping_Cart_Tracking_Update_Payment_Status_By_Id(id, status, last_trans.auth_status, last_trans.auth_code, last_trans.auth_statustxt, last_trans.auth_message, last_trans.id);
#if DEBUG
            var cart = await _shop_repo.Shopping_Cart_Tracking_Get_By_Id(id);
            var addr = await _addr_repo.Get_By_Id(cart.Shipping_Address_Id);
            var html_template = _emailSender.GetMailTemplate(cart, "payment-confirmation.cshtml");
            var mailResult = await _emailSender.SendEmailAsync(cart.Name, cart.Email, "Payment Confirmation", html_template);
            await _sms_sender.SendSmsAsync(string.Format("{0}{1}", addr.ISDCode, addr.PhoneNumber), string.Format("Dear {0}, Your payment for order #{1} is successfull. Amount paid AED {2} ", cart.Name, cart.Id, Math.Round(cart.Total_Sum, 1)));
#endif

            if (status == "Success") { payment_status = true; }
            ViewBag.Payment_Status = payment_status;
            return View(last_trans);
        }

        [AllowAnonymous]
        [HttpGet("Payment-Cancelled/{id}")]
        public async Task<IActionResult> Check_Out_Step_Five_1(string id)
        {
            ViewBag.TrackingId = id;
            bool payment_status = false;
            var trans = await TelrApi.Get_Transactions_By_Cart(id);
            var last_trans = trans.FirstOrDefault();
            if (last_trans == null) { return RedirectToAction("Index", "ShopingCart"); }
            string status = (last_trans.auth_status == "A") ? "Success" : "Failed";
            var result = await _shop_repo.Shopping_Cart_Tracking_Update_Payment_Status_By_Id(id, status, last_trans.auth_status, last_trans.auth_code, last_trans.auth_statustxt, last_trans.auth_message, last_trans.id);
            if (status == "Success") { payment_status = true; }
            ViewBag.Payment_Status = payment_status;
            return View("Check_Out_Step_Five", last_trans);
        }

        [AllowAnonymous]
        [HttpGet("Payment-Decliend/{id}")]
        public async Task<IActionResult> Check_Out_Step_Five_2(string id)
        {
            ViewBag.TrackingId = id;
            bool payment_status = false;
            var trans = await TelrApi.Get_Transactions_By_Cart(id);
            var last_trans = trans.FirstOrDefault();
            if (last_trans == null) { return RedirectToAction("Index", "ShopingCart"); }
            string status = (last_trans.auth_status == "A") ? "Success" : "Failed";
            var result = await _shop_repo.Shopping_Cart_Tracking_Update_Payment_Status_By_Id(id, status, last_trans.auth_status, last_trans.auth_code, last_trans.auth_statustxt, last_trans.auth_message, last_trans.id);
            if (status == "Success") { payment_status = true; }
            ViewBag.Payment_Status = payment_status;
            return View("Check_Out_Step_Five", last_trans);
        }


        [AllowAnonymous]
        [HttpPost("Payment/Call-Back")]
        public async Task<JsonResult> Payment_Call_Back([FromForm] Transaction obj)
        {
            if (obj != null)
            {
                string status = (obj.auth_status == "A") ? "Success" : "Failed";
                var result = await _shop_repo.Shopping_Cart_Tracking_Update_Payment_Status_By_Id(obj.cart_id, status, obj.auth_status, obj.auth_code, obj.auth_message, obj.auth_message, obj.auth_tranref);
                var cart = await _shop_repo.Shopping_Cart_Tracking_Get_By_Id(obj.cart_id);                
                if (status.Equals("Success"))
                {
                    var addr = await _addr_repo.Get_By_Id(cart.Shipping_Address_Id);
                    var html_template = _emailSender.GetMailTemplate(cart, "payment-confirmation.cshtml");
                    var mailResult = await _emailSender.SendEmailAsync(cart.Name, cart.Email, "Payment Confirmation", html_template);
                    await _sms_sender.SendSmsAsync(string.Format("{0}{1}", addr.ISDCode, addr.PhoneNumber), string.Format("Dear {0}, Your payment for order #{1} is successfull. Amount paid AED {2} ", cart.Name, cart.Id, Math.Round(cart.Total_Sum, 1)));
                }
            }            
            return Json(true);
        }

        [AllowAnonymous]
        [HttpPost("Fetchr/Notification")]
        public async Task<JsonResult> Shipment_Call_Back([FromBody] Shipment_Alert obj)
        {
            var result = await _shop_repo.ShipmentAlerts_Add_New(obj);
            var shipment = new Shipment_Notification();
            shipment.Shipment_Alert = obj;
            shipment.Cart = await _shop_repo.Shopping_Cart_Items_Get_By_Shipment_Id(obj.Tracking_Id);
            var cart = await _shop_repo.Shopping_Cart_Tracking_Get_By_Id(shipment.Cart.Tracking_Id);
            var addr = await _addr_repo.Get_By_Id(cart.Shipping_Address_Id);
            await _sms_sender.SendSmsAsync(string.Format("{0}{1}", addr.ISDCode, addr.PhoneNumber), string.Format("Dear {0}, Your shipment for order #{1} is now {2}. ", cart.Name, cart.Id, obj.Status));
            return Json(result);
        }
    }
}
