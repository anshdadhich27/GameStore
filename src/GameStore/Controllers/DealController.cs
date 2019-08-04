using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameStore.Models;
using GameStore.Models.Interface;
using GameStore.Models.Entity;

namespace GameStore.Controllers
{
    [Route("Deal")]
    [Authorize(ActiveAuthenticationSchemes = Constant.Admin_Auth_Scheme)]
    public class DealController : BaseController
    {
        private readonly IDealRepository _deal_repo = null;

        public DealController(IDealRepository deal_repo)
        {
            _deal_repo = deal_repo;
        }


        [HttpGet("Manage")]
        public IActionResult Manage()
        {
            return View();
        }

        [HttpPost("Add-New")]
        public async Task<JsonResult> Add_New([FromBody] Deal obj)
        {
            var result = await _deal_repo.Add_new(obj);
            return Json(result);
        }

        [HttpPost("Update")]
        public async Task<JsonResult> Update_By_Id([FromBody] Deal obj)
        {
            var result = await _deal_repo.Update_By_Id(obj);
            return Json(result);
        }

        [HttpPost("Search")]
        public async Task<JsonResult> Search_Deals([FromBody] SearchQuery obj)
        {
            var result = await _deal_repo.Search_And_GetPaginated(obj);
            return Json(result);
        }

        [HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete_By_Id(int id)
        {
            var result = await _deal_repo.Delete_By_Id(id);
            return Json(result);
        }

        [HttpGet("Get/{id}")]
        public async Task<JsonResult> Get_By_Id(int id)
        {
            var result = await _deal_repo.Get_By_Id(id);
            return Json(result);
        }

        [HttpGet("Get-Product/{id}")]
        public async Task<JsonResult> Get_Product_By_Id(int id)
        {
            var result = await _deal_repo.Get_Product_By_Id(id);
            return Json(result);
        }


        [HttpGet("Get/{rows}/{offset}")]
        public async Task<JsonResult> GetPaginated(int rows, long offset)
        {
            var result = await _deal_repo.GetPaginated(rows, offset);
            return Json(result);
        }

    }
}
