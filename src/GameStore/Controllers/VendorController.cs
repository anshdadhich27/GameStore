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
    [Route("Vendor")]
    [Authorize(ActiveAuthenticationSchemes = Constant.Admin_Auth_Scheme)]
    public class VendorController : BaseController
    {
        private readonly IVendorRepository _vendor_repo = null;

        public VendorController(IVendorRepository vendor_repo)
        {
            _vendor_repo = vendor_repo;
        }



        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
                
        [HttpGet("Get-All")]
        public async Task<JsonResult> Get_all()
        {
            var result = await _vendor_repo.Get_All();
            return Json(result);
        }

        [HttpGet("Get/{id}")]
        public async Task<JsonResult> Get(int id)
        {
            var result = await _vendor_repo.Get_By_Id(id);
            return Json(result);
        }

        [HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete_By_Id(int id)
        {
            var result = await _vendor_repo.Delete_By_Id(id);
            return Json(result);
        }


        [HttpPost("Add-New")]
        public async Task<JsonResult> Add_New([FromBody] Vendor obj)
        {
            var result = await _vendor_repo.Add_New(obj);
            return Json(result);
        }

        [HttpPost("Update")]
        public async Task<JsonResult> Update_By_Id([FromBody] Vendor obj)
        {
            var result = await _vendor_repo.Update_By_Id(obj);
            return Json(result);
        }

        [HttpGet("Get-Paginated/{rows}/{offset}")]
        public async Task<JsonResult> Get_Paginated(int rows, long offset)
        {
            var result = await _vendor_repo.GetPaginated(offset, rows);
            return Json(result);
        }

        [HttpPost("Get-Search-Result")]
        public async Task<JsonResult> Get_Paginated_Search(SearchQuery obj)
        {
            var result = await _vendor_repo.Search_And_GetPaginated(obj);
            return Json(result);
        }
    }
}
