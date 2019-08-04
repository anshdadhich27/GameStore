using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameStore.Models;
using GameStore.Models.Interface;

namespace GameStore.Controllers
{
    [Route("Dashboard")]
    [Authorize(ActiveAuthenticationSchemes = Constant.Admin_Auth_Scheme)]
    public class DashboardController : BaseController
    {
        private readonly IGraphRepository _graph_repo = null;
        public DashboardController(IGraphRepository graph_repo)
        {
            _graph_repo = graph_repo;
        }

        [HttpGet("Get-Selling-Count-By-Product-Type")]
        public async Task<JsonResult> Get_Selling_Count_By_Product_Type()
        {
            var result = await _graph_repo.Get_Selling_Count_By_Product_Type();
            return Json(result);
        }

        [HttpGet("Get-Buying-Count-By-Product-Type")]
        public async Task<JsonResult> Get_Buying_Count_By_Product_Type()
        {
            var result = await _graph_repo.Get_Buying_Count_By_Product_Type();
            return Json(result);
        }

        [HttpGet("Get-Pending-Order-Count")]
        public async Task<JsonResult> Get_Pending_Order_Count()
        {
            var result = await _graph_repo.Get_Pending_Order_Count();
            return Json(result);
        }

        [HttpGet("Get-Todays-Order-Count")]
        public async Task<JsonResult> Get_Todays_Order_Count()
        {
            var result = await _graph_repo.Get_Todays_Order_Count();
            return Json(result);
        }

        [HttpGet("Get-Per-Day-Order-Count/{cartType}/{month?}/{year?}")]
        public JsonResult Get_Order_Count_By_Month(string cartType, int month = 0, int year = 0)
        {
            year = (year == 0) ? DateTime.UtcNow.Year : year;
            month = (month == 0) ? DateTime.UtcNow.Month : month;
            var result = _graph_repo.Get_Order_Count_By_Month(cartType, month, year);
            return Json(result);
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        
    }
}
