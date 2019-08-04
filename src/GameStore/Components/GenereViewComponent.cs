using GameStore.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameStore.Components
{
    [ViewComponent(Name = "GenereMenu")]
    public class GenereViewComponent : ViewComponent
    {
        private readonly IGenereRepository _repo = null;

        public GenereViewComponent(IGenereRepository repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync(int top)
        {
            var genere = await _repo.GetTopNAsync(top);
            return View(genere);
        }
    }
}
