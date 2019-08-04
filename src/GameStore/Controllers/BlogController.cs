using GameStore.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameStore.Controllers
{
    [Route("Blog")]
    public class BlogController : BaseController
    {
        private readonly ICommon_Name_UrlRepository _blog_cat_repo = null;
        private readonly IBlogRepository _blog_repo = null;
        private readonly IPageRepository _page_repo = null;

        public BlogController(IBlogRepository blog_repo,
            ICommon_Name_UrlRepository blog_cat_repo,
            IPageRepository page_repo)
        {
            _blog_repo = blog_repo;
            _blog_cat_repo = blog_cat_repo;
            _page_repo = page_repo;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var pageContent = await _page_repo.Get_By_Url("blog");
            if (pageContent == null) { return NotFound(); }
            ViewBag.TopNews = await _blog_repo.GetTopN_News(5);
            ViewBag.TopEditorsPics = await _blog_repo.GetTopN_EditorsPic(5);
            ViewBag.BlogList = await _blog_repo.GetPaginatedAsync(0, 10);
            return View(pageContent);
        }

        [HttpGet("{link}")]
        public async Task<IActionResult> Details(string link)
        {
            var result = await _blog_repo.Get_By_Link(link);
            if (result == null) { return NotFound(); }
            ViewBag.TopNews = await _blog_repo.GetTopN_News(5);
            ViewBag.TopEditorsPics = await _blog_repo.GetTopN_EditorsPic(5);
            return View(result);
        }


        [HttpGet("Get-More-Blogs/{offset}")]
        public async Task<PartialViewResult> GetMoreBlogs(long offset)
        {
            var result = await _blog_repo.GetPaginatedAsync(offset, 10);
            return PartialView("_Blog_List_Partial", result);
        }
    }
}
