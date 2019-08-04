using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameStore.Models.Interface;
using GameStore.Models.Entity;

namespace GameStore.Components
{
    [ViewComponent(Name = "AccessoriesView")]
    public class AccessoriesViewComponent : ViewComponent
    {
        private readonly ICommon_Name_UrlRepository _common_repo = null;
        private readonly IProductRepository _prod_repo = null;

        public AccessoriesViewComponent(ICommon_Name_UrlRepository common_repo,
            IProductRepository prod_repo)
        {
            _common_repo = common_repo;
            _prod_repo = prod_repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string cns = Models.Constant.ImageCategory.AccessoriesImage;
            var prodList = await _prod_repo.GetPaginated(new SearchQuery { offset = 0, rows = 12, searchTxt = cns });
            return View(prodList.PagedSet);
        }
    }
}
