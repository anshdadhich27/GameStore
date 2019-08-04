using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameStore.Services;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using GameStore.Models.ViewModel;
using System.Security.Claims;
using GameStore.Models;
using System;
using System.Collections.Generic;


namespace GameStore.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = Constant.Auth_Scheme)]
    public class AccountController : BaseController
    {
        private readonly IUserRepository _userRepository = null;
        private readonly IEmailSender _emailSender = null;
        private readonly IAddressRepository _addr_repo = null;
        private readonly IShopping_Cart_Repository _shop_repo = null;
        private readonly IGuest_User_Repository _guest_repo = null;
        private readonly ICountryRepository _cntry_repo = null;
        private readonly IWishListRepository _wish_repo = null;
        private readonly ICreditRepository _credit_repo = null;
        private readonly IOrder_Repository _order_repo = null;

        public AccountController(
            IUserRepository userRepository,
            IEmailSender emailSender,
            IAddressRepository addr_repo,
            IShopping_Cart_Repository shop_repo,
            IGuest_User_Repository guest_repo,
            ICountryRepository cntry_repo,
            IWishListRepository wish_repo,
            ICreditRepository credit_repo,
            IOrder_Repository order_repo)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            _addr_repo = addr_repo;
            _shop_repo = shop_repo;
            _guest_repo = guest_repo;
            _cntry_repo = cntry_repo;
            _wish_repo = wish_repo;
            _credit_repo = credit_repo;
            _order_repo = order_repo;
        }


        [AllowAnonymous]
        [HttpPost("Guest-Checkout")]
        public async Task<PartialViewResult> Guest_Checkout_Step_One(GuestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepository.FindUserByEmailAsync(model.Email);
                if (result.Success)
                {
                    return PartialView("Info", "This email is already exists. Please login to proceed.");
                }
                else
                {
                    long otp = (new Random()).Next(100000, 999999);
                    otp = await _guest_repo.Add_New_Async(new Guest_User { Cart_Id = model.CartId, Email = model.Email, OTP = otp });

                    dynamic templeteObj = new System.Dynamic.ExpandoObject();
                    templeteObj.otp = otp;

                    var Templete = _emailSender.GetMailTemplate(templeteObj, "Send_OTP.cshtml");

                    var mail_result = await _emailSender.SendEmailAsync("Guest", model.Email, "OTP for guest checkout", Templete);
                    if (mail_result.Success)
                    {
                        return PartialView("_Guest_Checkout_1", model);
                    }
                    else
                    {
                        return PartialView("Info", "Unable to send you an OTP. Please use another email.");
                    }
                }
            }
            else
            {
                return PartialView("Info", "Please enter a valid email.");
            }
        }

        [AllowAnonymous]
        [HttpPost("Guest-Checkout-Check-OTP")]
        public async Task<PartialViewResult> Guest_Checkout_Step_Two(GuestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var g_user = new Guest_User { Cart_Id = model.CartId, Email = model.Email, OTP = model.OTP };
                var result = await _guest_repo.Check_Async(g_user);
                if (result)
                {
                    //await _guest_repo.Delete_Async(g_user);
                    string name = model.Email.Substring(0, model.Email.IndexOf("@"));
                    var user = new User { Email = model.Email, FirstName = name, EmailVerified = true, Password = model.OTP.ToString() };
                    var user_result = await _userRepository.RegisterUserAsync(user);
                    var cart = await _shop_repo.Shopping_Cart_Tracking_Get_By_Id(model.CartId);
                    if (user_result.Success)
                    {
                        user = user_result.Data as User;
                        await SignInAsync(user);
                        cart.UserId = user.Id;
                        user_result = await _userRepository.GenerateEmailConfirmationTokenAsync(user.Id);
                        if (user_result.Success)
                        {
                            user = user_result.Data as User;
                            await _userRepository.Email_ConfirmationAsync(user.UserKey, user.SecurityCode);
                        }
                        await _shop_repo.Shopping_Cart_Tracking_Update_By_Id(cart);
                        return PartialView("_Guest_Checkout_2", model);
                    }
                }
                else
                {
                    return PartialView("Info", "Unable to verify your email.");
                }
            }
            return PartialView("Info", "Something went wrong. Please try again.");
        }

        [HttpGet("My-Wishlist")]
        public async Task<IActionResult> My_Wishlist()
        {
            long userId = Convert.ToInt64(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _wish_repo.Get_All_By_User_Id(userId);
            return View(result);
        }

        [HttpPost("Remove-My-WishList/{id}")]
        public async Task<PartialViewResult> Remove_My_WishList(long id)
        {
            await _wish_repo.Delete_By_Id(id);
            long userId = Convert.ToInt64(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _wish_repo.Get_All_By_User_Id(userId);
            return PartialView("_WishList_Partial", result);
        }


        [HttpGet("User-Account")]
        public async Task<IActionResult> User_Account()
        {
            long userId = Convert.ToInt64(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            //ViewBag.ProfileScore = await _userRepository.Get_Profile_Score(userId);
            ViewBag.CreditLogs = await _credit_repo.Get_logs_By_User_Id(userId);
            return View();
        }

        [AllowAnonymous]
        [HttpGet("Get-Country-List")]
        public async Task<JsonResult> Get_Country_List()
        {
            var result = await _cntry_repo.Get_All();
            return Json(result);
        }
        
        [HttpGet("Delete-Order-By-Id/{id}")]
        public async Task<JsonResult> Delete_Order_By_Id(string id)
        {
            var result = await _shop_repo.Shopping_Cart_Tracking_Delete_By_Id(id);
            return Json(result);
        }

        [HttpPost("Submit-Cancelation-Request")]
        public async Task<JsonResult> Submit_Cancelation_Request([FromBody] Cancelation_Order obj)
        {
            var result = await _order_repo.Add_New_Cancelation_Request(obj);
            return Json(result);
        }

        [HttpGet("My-Orders")]
        public async Task<IActionResult> My_Orders()
        {
            long userId = Convert.ToInt64(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _shop_repo.Get_Order_List_By_UserId(userId);
            return View(result);
        }

        [HttpGet("My-Sold-Items")]
        public async Task<IActionResult> My_Sold_Items()
        {
            long userId = Convert.ToInt64(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _shop_repo.Get_Order_List_By_UserId(userId);
            return View(result);
        }

        [HttpGet("Get-User-Data")]
        public async Task<JsonResult> Get_User_Data()
        {
            long userId = Convert.ToInt64(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _userRepository.GetUserByIdAsync(userId);
            var user = result.Data as User;
            var result1 = await _userRepository.Get_Profile_Score(userId);
            user.ProfileRemainingOptions = result1.ProfileRemainingOptions.Trim().TrimEnd(',');
            user.ProfileScore = result1.SCORE;
            return Json(user);
        }

        [HttpGet("Delete-Address/{id}")]
        public async Task<JsonResult> Delete_Address_By_Id(long id)
        {
            var result = await _addr_repo.Delete_By_Id(id);
            return Json(result);
        }

        [HttpPost("Save-User-Data")]
        public async Task<JsonResult> Save_User_Data([FromBody] User obj)
        {
            long userId = Convert.ToInt64(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            obj.Id = userId;

            var result = await _userRepository.UpdateUserAsync(obj);
            var user = result.Data as User;
            var result1 = await _userRepository.Get_Profile_Score(userId);
            user.ProfileRemainingOptions = result1.ProfileRemainingOptions.Trim().TrimEnd(',');
            user.ProfileScore = result1.SCORE;
            return Json(user);
        }

        [HttpPost("Update-User-Password")]
        public async Task<JsonResult> Update_User_Password([FromBody] User_Credential obj)
        {
            long userId = Convert.ToInt64(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = new User { Id = userId, Password = obj.Password };
            var result = await _userRepository.Change_Password_By_Id(user);

            return Json(result);
        }

        [HttpGet("Get-User-Address")]
        public async Task<JsonResult> Get_User_Address()
        {
            long userId = Convert.ToInt64(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _addr_repo.Get_By_UserId(userId);
            return Json(result);
        }

        [HttpPost("Save-User-Address")]
        public async Task<JsonResult> Save_User_Address([FromBody] IEnumerable<Address> list)
        {
            var result = await _addr_repo.Add_Multiple_address(list);
            return Json(result);
        }


        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel modal)
        {
            bool success = false;
            string message = string.Empty;
            ViewData["Title"] = "Forgot Password?";
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please fill the required fields.");
                return View(modal);
            }
            var result = await _userRepository.FindUserByEmailAsync(modal.Email);
            if (result.Success)
            {
                var user = result.Data as User;
                result = await _userRepository.GenerateEmailConfirmationTokenAsync(user.Id);
                if (result.Success)
                {
                    user = result.Data as User;
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userKey = user.UserKey, code = user.SecurityCode }, protocol: HttpContext.Request.Scheme);

                    dynamic templeteObj = new System.Dynamic.ExpandoObject();
                    templeteObj.callbackUrl = callbackUrl;
                    templeteObj.FirstName = user.FirstName;
                    var Templete = _emailSender.GetMailTemplate(templeteObj, "reset-password.cshtml");
                    result = await _emailSender.SendEmailAsync(user.FirstName, user.Email, "Reset your Password", Templete);

                    //result = await _emailSender.SendEmailAsync(user.FirstName, user.Email, "Reset your Password",
                    //     $"Please confirm your account by clicking this <a href='{callbackUrl}'>link</a>.");
                    if (result.Success) { success = true; message = "We have sent you a password reset link over mail please follow that."; }
                    else { message = result.Message; }
                }
                else { message = result.Message; }
            }
            else { message = result.Message; }
            ViewBag.Success = success;
            ViewBag.Message = message;
            return View(modal);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userKey, string code)
        {
            var result = await _userRepository.Email_ConfirmationAsync(userKey, code);
            if (result.Success)
            {
                var user = result.Data as User;
                return View(new ResetPasswordViewModel(user.UserKey, user.SecurityCode));
            }
            else { return View("Error", result.Message); }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel modal)
        {
            bool success = false;
            string message = string.Empty;
            ViewData["Title"] = "Reset Password";
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please fill the required fields.");
                return View(modal);
            }
            var result = await _userRepository.Email_ConfirmationAsync(modal.UserKey, modal.SecurityCode);
            if (result.Success)
            {
                var user = result.Data as User;
                user.Password = modal.Password;
                result = await _userRepository.Change_PasswordAsync(user);
                if (result.Success)
                {
                    dynamic templeteObj = new System.Dynamic.ExpandoObject();
                    templeteObj.Message = result.Message;

                    var Templete = _emailSender.GetMailTemplate(templeteObj, "Password_Changed_Successfully.cshtml");

                    await _emailSender.SendEmailAsync(user.FirstName, user.Email, result.Message, Templete);
                    success = true; message = result.Message;
                }
                else { message = result.Message; }
            }
            else { message = result.Message; }
            ViewBag.Success = success;
            ViewBag.Message = message;
            return View(modal);
        }



        [HttpPost]
        [AllowAnonymous]
        [Route("CheckEmail")]
        public async Task<JsonResult> CheckEmail(User obj)
        {
            bool result = true;
            try
            {
                if (!string.IsNullOrEmpty(obj.Email))
                {
                    var user = await _userRepository.FindUserByEmailAsync(obj.Email);
                    if (user.Success) { result = false; }
                }
            }
            catch { result = false; }
            return Json(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ConfirmEmail/{userKey}/{code}")]
        public async Task<IActionResult> ConfirmEmail(string userKey, string code)
        {
            var result = await _userRepository.Email_ConfirmationAsync(userKey, code);
            if (!result.Success)
            {
                return View("Error", result.Message);
            }
            var user = result.Data as User;
            return RedirectToAction("Login", "Account", new { email = user.Email, msg = result.Message });
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl, string email, string msg)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (Request.HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }
            var model = new LoginViewModel();
            model.Email = email;
            ViewBag.Message = msg;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modal, string returnUrl)
        {
            ViewData["Title"] = "Login";
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please fill the required fields.");
                return View(modal);
            }
            var obj = await _userRepository.FindUserByEmail_PasswordAsync(modal.Email, modal.Password);
            if (obj.Success)
            {
                await SignInAsync(obj.Data as User);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, obj.Message);
            }
            return View(modal);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (Request.HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterViewModel model, string returnUrl)
        {
            ViewData["Title"] = "SignUp";
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new User { ReferalKey = model.ReferalKey, FirstName = model.FirstName, Email = model.Email, LastName = model.LastName, Password = model.Password };
                var result = await _userRepository.RegisterUserAsync(user);
                if (result.Success)
                {
                    user = result.Data as User;
                    result = await _userRepository.GenerateEmailConfirmationTokenAsync(user.Id);
                    if (result.Success)
                    {
                        user = result.Data as User;
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userKey = user.UserKey, code = user.SecurityCode }, protocol: HttpContext.Request.Scheme);
                        dynamic templeteObj = new System.Dynamic.ExpandoObject();
                        templeteObj.Message = $"Please confirm your account by clicking this <a href='{callbackUrl}'>link</a>.";
                        templeteObj.callbackUrl = callbackUrl;

                        var Templete = _emailSender.GetMailTemplate(templeteObj, "activate-account.cshtml");
                        result = await _emailSender.SendEmailAsync(model.FirstName, model.Email, "Confirm your account", Templete);
                        
                        if (result.Success) { return View(); }
                        else { return PartialView("Error", result.Message); }
                    }
                    else { return PartialView("Error", result.Message); }
                }
            }            
            return PartialView("_Modal_Error", ModelState);
        }

        [AllowAnonymous]
        public async Task<IActionResult> SignOut()
        {
            await SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
            var properties = ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            var obj = await HttpContext.Authentication.GetAuthenticateInfoAsync(Constant.Auth_Scheme);
            if (obj != null)
            {
                string name = obj.Principal.FindFirstValue(ClaimTypes.Name);
                string email = obj.Principal.FindFirstValue(ClaimTypes.Email);
                string first_Name = obj.Principal.FindFirstValue(ClaimTypes.GivenName);
                string last_Name = obj.Principal.FindFirstValue(ClaimTypes.Surname);
                string providerKey = obj.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                string loginProvider = obj.Properties.Items[".AuthScheme"];

                var user = new User { FirstName = first_Name, LastName = last_Name, Email = email, Password = name };
                var login = new UserLogin { LoginProvider = loginProvider, ProviderKey = providerKey, ProviderName = name };
                var result = await _userRepository.Check_User_LoginsAsync(login);
                if (result.Success)
                {
                    await SignInAsync(result.Data as User);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    result = await _userRepository.FindUserByEmailAsync(email);
                    if (result.Success)
                    {
                        user = result.Data as User;
                        login.UserId = user.Id;
                        result = await _userRepository.Create_User_LoginsAsync(login);
                        if (result.Success)
                        {
                            await SignInAsync(result.Data as User);
                            return RedirectToLocal(returnUrl);
                        }
                        else { return View("Error", result.Message); }
                    }
                    else
                    {
                        result = await _userRepository.RegisterUserAsync(user);
                        if (result.Success)
                        {
                            user = result.Data as User;
                            login.UserId = user.Id;
                            result = await _userRepository.Create_User_LoginsAsync(login);
                            if (result.Success)
                            {
                                await SignInAsync(result.Data as User);
                                return RedirectToLocal(returnUrl);
                            }
                            else { return View("Error", result.Message); }
                        }
                    }
                }
            }
            return RedirectToLocal(returnUrl);
        }




    }
}
