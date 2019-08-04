using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameStore.Models.Entity;
using System.Security.Claims;
using GameStore.Models;
using Microsoft.AspNetCore.Http.Authentication;

namespace GameStore.Controllers
{
    public class BaseController : Controller
    {
                
        public async Task SignInAsync(User obj)
        {
            var claims = new List<Claim> {
                         new Claim(ClaimTypes.Email, obj.Email, ClaimValueTypes.String),
                         new Claim(ClaimTypes.Actor, obj.Avatar, ClaimValueTypes.String),
                         new Claim(ClaimTypes.Gender, obj.Gender, ClaimValueTypes.String),
                         new Claim(ClaimTypes.Country, obj.Country, ClaimValueTypes.String),
                         new Claim(ClaimTypes.Surname, obj.LastName, ClaimValueTypes.String),
                         new Claim(ClaimTypes.GivenName, obj.FirstName, ClaimValueTypes.String),
                         new Claim(Constant.REFERRAL_KEY, obj.ReferalKey.ToString(), ClaimValueTypes.Integer),
                         new Claim(ClaimTypes.MobilePhone, string.Format("+{0}-{1}", obj.ISDCode, obj.PhoneNumber)),
                         new Claim(ClaimTypes.NameIdentifier, obj.Id.ToString(), ClaimValueTypes.Integer, Constant.DomainNane),                                                 
                         new Claim(ClaimTypes.Name, string.Format("{0} {1}", obj.FirstName, obj.LastName), ClaimValueTypes.String)
                       };

            var userIdentity = new ClaimsIdentity(claims, "Users");
            var userPrincipal = new ClaimsPrincipal(userIdentity);
            await HttpContext.Authentication.SignInAsync(Constant.Auth_Scheme, userPrincipal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddDays(2),
                    IssuedUtc = DateTime.UtcNow,
                    IsPersistent = true,
                    AllowRefresh = true
                });
        }

        

        public async Task SignInAdminAsync(User obj)
        {
            var claims = new List<Claim> {
                         new Claim(ClaimTypes.Email, obj.Email, ClaimValueTypes.String),
                         new Claim(ClaimTypes.Actor, obj.Avatar, ClaimValueTypes.String),
                         new Claim(ClaimTypes.Gender, obj.Gender, ClaimValueTypes.String),
                         new Claim(ClaimTypes.Country, obj.Country, ClaimValueTypes.String),
                         new Claim(ClaimTypes.Surname, obj.LastName, ClaimValueTypes.String),
                         new Claim(ClaimTypes.GivenName, obj.FirstName, ClaimValueTypes.String),
                         new Claim(ClaimTypes.MobilePhone, string.Format("+{0}-{1}", obj.ISDCode, obj.PhoneNumber)),
                         new Claim(ClaimTypes.NameIdentifier, obj.Id.ToString(), ClaimValueTypes.Integer, Constant.DomainNane),                         
                         new Claim(ClaimTypes.Name, string.Format("{0} {1}", obj.FirstName, obj.LastName), ClaimValueTypes.String)
                       };

            var userIdentity = new ClaimsIdentity(claims, "Administrator");
            var userPrincipal = new ClaimsPrincipal(userIdentity);
            await HttpContext.Authentication.SignInAsync(Constant.Admin_Auth_Scheme, userPrincipal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddDays(5),
                    IssuedUtc = DateTime.UtcNow,
                    IsPersistent = true,
                    AllowRefresh = true
                });
        }


        public async Task SignOutAsync()
        {
            await HttpContext.Authentication.SignOutAsync(Constant.Auth_Scheme);
        }



        public async Task SignOutAdminAsync()
        {
            await HttpContext.Authentication.SignOutAsync(Constant.Admin_Auth_Scheme);
        }


        public IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        
        public virtual AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            AuthenticationProperties authenticationProperties = new AuthenticationProperties()
            {
                RedirectUri = redirectUrl
            };
            authenticationProperties.Items["LoginProvider"] = provider;
            return authenticationProperties;
        }


    }
}
