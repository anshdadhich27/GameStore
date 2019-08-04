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
    [Route("Manage-Blogs")]
    [Authorize(ActiveAuthenticationSchemes = Constant.Admin_Auth_Scheme)]
    public class ManageBlogsController : BaseController
    {
        private readonly ICommon_Name_UrlRepository _common_repo = null;
        private readonly IBlogRepository _blog_repo = null;

        public ManageBlogsController(IBlogRepository blog_repo,
            ICommon_Name_UrlRepository common_repo)
        {
            _common_repo = common_repo;
            _blog_repo = blog_repo;
        }


        #region Blogs

        [HttpGet("Blog-Category")]
        public IActionResult Blog_CategoryPages()
        {
            return View();
        }


        [HttpGet("Blog")]
        public IActionResult BlogPage()
        {
            return View();
        }

        [HttpPost("Add-New-Blog")]
        public async Task<JsonResult> AddNew_Blog([FromBody] Blog obj)
        {
            var result = await _blog_repo.Add_New(obj);
            return Json(result);
        }

        [HttpPost("Update-Blog")]
        public async Task<JsonResult> Update_Blog([FromBody] Blog obj)
        {
            var result = await _blog_repo.Update_By_Id(obj);
            return Json(result);
        }

        [HttpGet("Delete-Blog/{id}")]
        public async Task<JsonResult> Delete_Blog(int id)
        {
            var result = await _blog_repo.Delete_By_Id(id);
            return Json(result);
        }

        [HttpGet("GetPaginated-Blog/{offset}/{rows}")]
        public async Task<JsonResult> GetPaginated_Blog_Async(long offset, int rows)
        {
            var result = await _blog_repo.GetPaginatedAsync(offset, rows);
            return Json(result);
        }

        [HttpPost("Search-Blog")]
        public async Task<JsonResult> Search_Blog([FromBody] SearchQuery obj)
        {
            var result = await _blog_repo.Search_And_GetPaginatedAsync(obj);
            return Json(result);
        }

        #endregion
        
    }
}
