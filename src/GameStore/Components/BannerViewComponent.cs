using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameStore.Models.Interface;

namespace GameStore.Components
{
    [ViewComponent(Name = "BannerView")]
    public class BannerViewComponent : ViewComponent
    {
        private readonly IBannerRepository  _repo = null;

        public BannerViewComponent(IBannerRepository repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _repo.Get_All_Active();
            return View(result);
        }
    }
}
