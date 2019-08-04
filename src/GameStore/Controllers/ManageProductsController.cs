using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameStore.Models;
using GameStore.Models.Interface;
using GameStore.Models.Entity;
using Microsoft.AspNetCore.Hosting;

namespace GameStore.Controllers
{
    [Route("Manage-Products")]
    [Authorize(ActiveAuthenticationSchemes = Constant.Admin_Auth_Scheme)]
    public class ManageProductsController : BaseController
    {
        private IHostingEnvironment _HostingEnv;
        private readonly IImageRepository _img_repo = null;
        private readonly IProductRepository _product_repo = null;
        private readonly ICommon_Name_UrlRepository _common_repo = null;


        public ManageProductsController(IHostingEnvironment env,
            IProductRepository product_repo,
            ICommon_Name_UrlRepository common_repo,
            IImageRepository img_repo)
        {
            _HostingEnv = env;
            _common_repo = common_repo;
            _product_repo = product_repo;
            _img_repo = img_repo;
        }


        #region Products Page

        [HttpGet("Accessories-Type")]
        public IActionResult AccessoriesTypePage()
        {
            return View();
        }

        [HttpGet("Console-Type")]
        public IActionResult ConsoleType()
        {
            return View();
        }

        [HttpGet("Accessories")]
        public IActionResult AccessoriesPage()
        {
            return View();
        }

        [HttpGet("Console")]
        public IActionResult ConsolePage(string btnExport)
        {

            return View();
        }
        [HttpPost("download-csv-console")]
        public async Task<IActionResult> Get_CSV_Of_Console([FromForm] SearchQuery obj)
        {
            string where_clouse = string.Empty;
            if (!string.IsNullOrEmpty(obj.searchTxt)) { where_clouse += string.Format(" where t1.ProductType LIKE '%{0}%' ", obj.searchTxt); }


            string query = string.Empty;



            query += "select t1.Name,t2.Name as PlatformName,Rating,RatingCount,Popularity,PreOrder,	Buying_Price,Selling_Price,Original_Price, ";
            query += " case when IsBestSelling = 1 then 'Yes' else 'No' end IsBestSelling,case when Available_To_Buy = 1 then 'Yes' else 'No' end Available_To_Buy, case when Available_To_Sell = 1 then 'Yes' else 'No' End Available_To_Sell, Quantity, SKU, Barcode ";
            query += " FROM ProductsTbl t1 ";
            query += " LEFT JOIN Common_Name_Url_Tbl t2 on t1.ProductTypeId = t2.Id ";
            query += " LEFT JOIN ConditionTbl t3 on t1.Condition_Id = t3.Id ";
            query += " LEFT JOIN GamePlatformsTbl t4 on t1.Platform_Code = t4.Code ";
            query += where_clouse;
            query += " order by t1.IsBestSelling desc, t1.Id desc ";

            var result = await _img_repo.GetConsole_By_Custom_QueryAsync(query);
            var data = Utility.ExportExcel(result.PagedSet.ToList(), obj.searchTxt);
            return File(data, "text/xlsx", $""+ obj.searchTxt + ".xlsx");
        }
        [HttpPost("Add-New-Product")]
        public async Task<JsonResult> AddNew_Product([FromBody] Product obj)
        {
            var result = await _product_repo.Add_New(obj);
            if (result > 0)
            {
                foreach (var img in obj.ImageList)
                {
                    img.Reference_Id = result;
                }
                await _img_repo.Add_New_ImagesAsync(obj.ImageList);
            }
            return Json(result);
        }

        [HttpPost("Update-Product")]
        public async Task<JsonResult> Update_Product([FromBody] Product obj)
        {
            var result = await _product_repo.Update_By_Id(obj);
            foreach (var img in obj.ImageList)
            {
                img.Reference_Id = obj.Id;
            }
            await _img_repo.Add_New_ImagesAsync(obj.ImageList);
            return Json(result);
        }

        [HttpPost("Delete-Product")]
        public async Task<JsonResult> Delete_Product([FromBody] Product obj)
        {
            if (obj == null) { return Json(false); }
            foreach (var img in obj.ImageList)
            {
                var fileInfo = _HostingEnv.WebRootFileProvider.GetFileInfo(img.Url);
                if (fileInfo.Exists) { System.IO.File.Delete(fileInfo.PhysicalPath); }
            }
            var result = await _product_repo.Delete_By_Id(obj.Id);
            return Json(result);
        }


        [HttpPost("Get-Paginated")]
        public async Task<JsonResult> GetPaginated_Product([FromBody] SearchQuery obj)
        {
            var result = await _product_repo.GetPaginated(obj);
            foreach (var x in result.PagedSet)
            {
                var img_result = await _img_repo.Get_Image_By_ReferenceId_ImageOfAsync(x.Id, obj.searchTxt);
                if (img_result.Success)
                {
                    x.ImageList = img_result.Data as IEnumerable<Images>;
                }
            }
            return Json(result);
        }

        #endregion
    }
}
