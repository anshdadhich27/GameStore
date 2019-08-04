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
    [Route("Operator")]
    [Authorize(ActiveAuthenticationSchemes = Constant.Admin_Auth_Scheme)]
    public class OperatorController : BaseController
    {
        private readonly IOperatorRepository _ope_repo = null;

        public OperatorController(IOperatorRepository ope_repo)
        {
            _ope_repo = ope_repo;
        }


        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Get-All")]
        public async Task<JsonResult> Get_All()
        {
            var result = await _ope_repo.Get_All();
            return Json(result);
        }

        [HttpPost("Add-New")]
        public async Task<JsonResult> Add_New([FromBody] User obj)
        {
            var result = await _ope_repo.Add_New(obj);
            return Json(result);
        }

        [HttpPost("Update")]
        public async Task<JsonResult> Update_By_Id([FromBody] User obj)
        {
            var result = await _ope_repo.Update_By_Id(obj);
            return Json(result);
        }
    }
}
