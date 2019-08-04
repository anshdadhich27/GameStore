using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using Microsoft.AspNetCore.Cors;
using GameStore.Models;
using GameStore.Services;
using System.IO;

namespace GameStore.Controllers
{
    public class NewsLetterController : BaseController
    {
        private readonly INewsLetterRepository _news_repo = null;
        private readonly IEmailSender _emailSender = null;

        public NewsLetterController(INewsLetterRepository news_repo, IEmailSender emailSender)
        {
            _news_repo = news_repo;
            _emailSender = emailSender;
        }

        [HttpGet("News-Letter/Verify-Email/{key}")]
        public async Task<IActionResult> Verify_Email(string key)
        {
            var result = await _news_repo.NewsLetter_Subscriber_Verify(key);
            ViewBag.Message = result.Message;
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<PartialViewResult> Index(NewsLetter obj)
        {
            if (string.IsNullOrEmpty(obj.Email))
            {
                return PartialView("Info", "Please fill the input field.");
            }

            if (string.IsNullOrEmpty(obj.FirstName))
            {
                obj.FirstName = obj.Email.Substring(0, obj.Email.IndexOf('@'));
                obj.FirstName = Utility.GetInTitleCase(obj.FirstName);
            }
            obj.Security_Key = Guid.NewGuid().ToString();
            var result = await _news_repo.NewsLetter_Subscriber_Add_New(obj);
            // Send EMail
            if(result.Success)
            {
                dynamic templeteObj = new System.Dynamic.ExpandoObject();
                templeteObj.Message = $"Your subscription has been confirmed. You've been added to our list and will hear from us soon.";

                var Templete =  _emailSender.GetMailTemplate(templeteObj, "subscribe.cshtml");
                var mailResult = await _emailSender.SendEmailAsync(obj.FirstName, obj.Email, "Thank you for subscribe newsletter", Templete);
            }

            return PartialView("Info", result.Message);
        }
    }
}
