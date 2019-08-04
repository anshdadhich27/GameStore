using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameStore.Models.Interface;
using GameStore.Models;

namespace GameStore.Components
{
    [ViewComponent(Name = "MobileNav")]
    public class MobileNavViewComponent : ViewComponent
    {
        private readonly ICommon_Name_UrlRepository _common_repo = null;
        private readonly IGamePlatformRepository _gp_repo = null;
        private readonly ICategoryRepository _cat_repo = null;
        private readonly IGenereRepository _gr_repo = null;

        public MobileNavViewComponent(IGamePlatformRepository gp_repo,
            ICommon_Name_UrlRepository common_repo,
            ICategoryRepository cat_repo,
            IGenereRepository gr_repo)
        {
            _gp_repo = gp_repo;
            _cat_repo = cat_repo;
            _gr_repo = gr_repo;
            _common_repo = common_repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var accessories = await _common_repo.Get_All_By_Type(Constant.ACCESSORIES);
            var generes = await _gr_repo.Get_ALL_ActiveAsync();
            var result = await _cat_repo.Get_All_Async();
            if (result != null)
            {
                foreach (var x in result)
                {
                    x.AccessoriesTypes = accessories;
                    string platform_ids = x.Platform_Ids;
                    if (!string.IsNullOrEmpty(platform_ids))
                    {
                        platform_ids = platform_ids.Contains(",") ? platform_ids.Trim(',') : platform_ids;
                        if (platform_ids.Length > 0)
                        {
                            x.PlatformList = await _gp_repo.Get_Supported_N_Platforms(100, platform_ids);
                            x.GenereList = generes;
                        }
                    }
                }
            }
            return View(result);
        }
    }
}
