using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameStore.Models.Interface;

namespace GameStore.Components
{
    [ViewComponent(Name = "AdsView")]
    public class AdsViewComponent : ViewComponent
    {
        private readonly IAdsRepository _repo = null;

        public AdsViewComponent(IAdsRepository repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync(string page)
        {
            var result = await _repo.Get_By_Page_Name(page);
            return View(result);
        }
    }
}
