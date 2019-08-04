using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameStore.Models.Interface;
using GameStore.Models.Entity;

namespace GameStore.Components
{
    [ViewComponent(Name = "ConsoleView")]
    public class ConsoleViewComponent : ViewComponent
    {
        private readonly ICommon_Name_UrlRepository _common_repo = null;
        private readonly IProductRepository _prod_repo = null;

        public ConsoleViewComponent(ICommon_Name_UrlRepository common_repo,
            IProductRepository prod_repo)
        {
            _common_repo = common_repo;
            _prod_repo = prod_repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = new List<Console_DTO>();
            string cns = Models.Constant.ImageCategory.ConsoleImage;
            var consoleTypes = await _common_repo.Get_All_By_Type(cns);
            foreach(var x in consoleTypes)
            {
                var console = new Console_DTO { Name = x.Name, NameUrl = x.NameUrl };
                var prodList = await _prod_repo.GetPaginated_By_TypeId(new SearchQuery { offset = 0, rows = 12, searchTxt = cns, TypeId = x.Id });
                console.ProductList = prodList.PagedSet;
                list.Add(console);
            }            
            return View(list);
        }
    }
}
