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
    [Route("Manage-Review")]
    [Authorize(ActiveAuthenticationSchemes = Constant.Admin_Auth_Scheme)]
    public class ManageReviewController : BaseController
    {
        private readonly IReviewRepository _rev_repo = null;

        public ManageReviewController(IReviewRepository rev_repo)
        {
            _rev_repo = rev_repo;
        }


        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Update")]
        public async Task<JsonResult> Update_By_Id([FromBody] Review obj)
        {
            var result = await _rev_repo.Update_By_Id(obj);
            return Json(result);
        }

        [HttpGet("Delete/{id}")]
        public async Task<JsonResult> Delete_By_Id(long id)
        {
            var result = await _rev_repo.Delete_By_Id(id);
            return Json(result);
        }

        [HttpGet("Get-Paginated/{rows}/{offset}")]
        public async Task<JsonResult> Get_ReviewList(int rows, long offset)
        {
            var result = await _rev_repo.GetPaginated(rows, offset);
            return Json(result);
        }
    }
}
