using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameStore.Models;
using GameStore.Models.Entity;
using GameStore.Models.Interface;

namespace GameStore.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = Constant.Admin_Auth_Scheme)]
    public class AdminController : BaseController
    {
        private readonly IOperatorRepository _ope_repo = null;

        public AdminController(IOperatorRepository ope_repo)
        {
            _ope_repo = ope_repo;
        }


        public IActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User obj, string returnUrl)
        {
            var result = await _ope_repo.Get_By_Credentials(obj.Email, obj.Password);
            if (result == null) { ViewBag.Message = "Invalid Credentials";  return View(obj); }
            await SignInAdminAsync(result);
            if (!string.IsNullOrEmpty(returnUrl)) { return RedirectToLocal(returnUrl); }
            return RedirectToAction("Index", "Dashboard");
        }


        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await SignOutAdminAsync();
            return RedirectToAction("Login","Admin");
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult SetPassword()
        {
            return View();
        }
    }
}
