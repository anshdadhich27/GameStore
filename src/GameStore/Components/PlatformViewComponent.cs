using GameStore.Models;
using GameStore.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace GameStore.Components
{
    [ViewComponent(Name = "PlatformMenu")]
    public class PlatformViewComponent : ViewComponent
    {
        private readonly IGamePlatformRepository _gp_repo = null;
        private readonly ICommon_Name_UrlRepository _common_repo = null;
        private readonly ICategoryRepository _cat_repo = null;
        private readonly IGenereRepository _gr_repo = null;
        private readonly IFeatureLinkRepository _fea_repo = null;

        public PlatformViewComponent(IGamePlatformRepository gp_repo,
            ICommon_Name_UrlRepository common_repo,
            ICategoryRepository cat_repo,
            IGenereRepository gr_repo,
            IFeatureLinkRepository fea_repo)
        {
            _gp_repo = gp_repo;
            _cat_repo = cat_repo;
            _gr_repo = gr_repo;
            _common_repo = common_repo;
            _fea_repo = fea_repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var generes = await _gr_repo.Get_ALL_ActiveAsync();
            var result = await _cat_repo.Get_All_Async();
            var accessories = await _common_repo.Get_All_By_Type(Constant.ACCESSORIES);
            var featureLinks = await _fea_repo.Get_All();
            if (result != null)
            {
                foreach(var x in result)
                {
                    x.AccessoriesTypes = accessories;        
                    string platform_ids = x.Platform_Ids;
                    if (!string.IsNullOrEmpty(platform_ids))
                    {
                        platform_ids = platform_ids.Contains(",") ? platform_ids.Trim(',') : platform_ids;
                        if (platform_ids.Length > 0)
                        {
                            x.GenereList = generes;
                            x.PlatformList = await _gp_repo.Get_Supported_N_Platforms(100, platform_ids);
                            foreach(var xx in x.PlatformList)
                            {
                                xx.FeatureLinks = featureLinks.Where(o => o.Reference_Id == xx.Id && o.Reference_Name == Constant.ImageCategory.PlatformImage).ToList();
                            }
                        }
                    }
                }                
            }
            return View(result);
        }
    }
}
