using GameStore.Models.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using GameStore.Models.Entity;
using System;
using GameStore.Models;
using GameStore.Services;

namespace GameStore.Controllers
{

    public class HomeController : BaseController
    {
        private IHostingEnvironment _HostingEnv;
        private readonly IGamePlatformRepository _gp_repo = null;
        private readonly IGameRepository _game_repo = null;
        private readonly ISearchRepository _search_repo = null;
        private readonly IPageRepository _page_repo = null;
        private readonly IContactRepository _cont_repo = null;
        private readonly ICareerRepository _car_repo = null;
        private readonly IGenereRepository _gr_repo = null;
        private readonly IBannerRepository _banner_repo = null;
        private readonly IHotDealsRepository _hotdeals_repo = null;
        private readonly IEmailSender _emailSender = null;
        public HomeController(IHostingEnvironment env,
            IGamePlatformRepository gp_repo,

            IGameRepository game_repo,
            IPageRepository page_repo,
            ISearchRepository search_repo,
            IContactRepository cont_repo,
            ICareerRepository car_repo,
            IGenereRepository gr_repo,
            IBannerRepository banner_repo,
            IHotDealsRepository hotdeals_repo,
            IEmailSender emailSender)
        {
            _HostingEnv = env;
            _gp_repo = gp_repo;
            _game_repo = game_repo;
            _page_repo = page_repo;
            _search_repo = search_repo;
            _cont_repo = cont_repo;
            _car_repo = car_repo;
            _gr_repo = gr_repo;
            _banner_repo = banner_repo;
            _hotdeals_repo = hotdeals_repo;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var gamesByPlatform = await _gp_repo.GetTopNAsync(6);
            foreach (var platform in gamesByPlatform)
            {
                platform.GameList = await _game_repo.GetPaginated_By_PlatformId_From_View(0, 12, platform.Id);
                foreach (var obj in platform.GameList.PagedSet)
                {
                    obj.Platform = platform;
                }
            }
            ViewBag.GamesByPlatform = gamesByPlatform;


            var pageContent = await _page_repo.Get_By_Url("home");
            if (pageContent == null) { return NotFound(); }
            return View(pageContent);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string q, string p, string g, string rd, string r, string c, string t, string order, int page = 1, int min = 0, int max = 0, int pegi = 0, bool bs = false)
        {
            q = string.IsNullOrEmpty(q) ? string.Empty : q;
            ViewBag.Search_Query = q;
            ViewBag.PageTitle = string.Format("Search results for: {0}", q);
            string query = Regex.Replace(q, "[^a-zA-Z0-9 ]", "");

            string query_for_game = string.Empty;
            string query_for_console = string.Empty;
            string query_for_accessories = string.Empty;

            string where_clouse_for_game = string.Empty;
            string where_clouse_for_game_maxprice = string.Empty;
            string where_clouse_for_console = string.Empty;
            string where_clouse_for_console_maxprice = string.Empty;
            string where_clouse_for_accessories = string.Empty;
            string where_clouse_for_accessories_maxprice = string.Empty;
            string best_selling_query = string.Empty;


            var plarformList = await _gp_repo.Get_ALL_ActiveAsync();
            var genreList = await _gr_repo.Get_ALL_ActiveAsync();
            ViewBag.GenreList = genreList;
            ViewBag.PlarformList = plarformList;
            ViewBag.GenreUrl = g;
            ViewBag.PlarformUrl = p;
            ViewBag.PageIndex = page;
            ViewBag.Release_Date = rd;
            ViewBag.TypeGame = t;
            ViewBag.Condition = c;
            ViewBag.Ratings = r;
            ViewBag.MinPrice = min;
            ViewBag.orderby = order;

            #region Genere Filters
            if (!string.IsNullOrEmpty(g))
            {
                string g_url = string.Empty;
                Genere genre = null;
                if (g.Contains(","))
                {
                    var grs = g.Split(',').ToList();
                    grs = grs.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < grs.Count; index++)
                    {
                        if (string.IsNullOrEmpty(grs[index])) { continue; }
                        g_url = Regex.Replace(grs[index], "[^a-zA-Z0-9]", "-");
                        genre = genreList.FirstOrDefault(x => x.NameUrl.Equals(g_url, System.StringComparison.CurrentCultureIgnoreCase));
                        if (genre != null)
                        {
                            if (index == 0) { where_clouse_for_game += string.Format(" AND ( t1.Genres LIKE '%,{0},%' ", genre.Id); where_clouse_for_game_maxprice += string.Format(" AND ( t1.Genres LIKE '%,{0},%' ", genre.Id); }
                            if (index > 0) { where_clouse_for_game += string.Format(" OR t1.Genres LIKE '%,{0},%' ", genre.Id); where_clouse_for_game_maxprice += string.Format(" OR t1.Genres LIKE '%,{0},%' ", genre.Id); }
                            if (index == grs.Count - 1) { where_clouse_for_game += " ) "; where_clouse_for_game_maxprice += " ) "; }
                        }
                    }
                }
                else
                {
                    g_url = Regex.Replace(g, "[^a-zA-Z0-9]", "-");
                    genre = genreList.FirstOrDefault(x => x.NameUrl.Equals(g_url, System.StringComparison.CurrentCultureIgnoreCase));
                    if (genre != null)
                    {
                        where_clouse_for_game += string.Format(" AND t1.Genres LIKE '%,{0},%' ", genre.Id);
                        where_clouse_for_game_maxprice += string.Format(" AND t1.Genres LIKE '%,{0},%' ", genre.Id);
                    }
                }
            }
            #endregion

            #region Platform Filters
            if (!string.IsNullOrEmpty(p))
            {
                string p_url = string.Empty;
                GamePlatform platform = null;
                if (p.Contains(","))
                {
                    var pls = p.Split(',').ToList();
                    pls = pls.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < pls.Count; index++)
                    {
                        if (string.IsNullOrEmpty(pls[index])) { continue; }
                        p_url = Regex.Replace(pls[index], "[^a-zA-Z0-9]", "-");
                        platform = plarformList.FirstOrDefault(x => x.NameUrl.Equals(p_url, System.StringComparison.CurrentCultureIgnoreCase));
                        if (platform != null)
                        {
                            if (index == 0) { where_clouse_for_game += string.Format(" AND ( t1.PlatformId={0} ", platform.Id); where_clouse_for_game_maxprice += string.Format(" AND ( t1.PlatformId={0} ", platform.Id); }
                            if (index > 0) { where_clouse_for_game += string.Format(" OR t1.PlatformId={0} ", platform.Id); where_clouse_for_game_maxprice += string.Format(" OR t1.PlatformId={0} ", platform.Id); }
                            if (index == pls.Count - 1) { where_clouse_for_game += " ) "; where_clouse_for_game_maxprice += " ) "; }
                        }
                    }
                }
                else
                {
                    p_url = Regex.Replace(p, "[^a-zA-Z0-9]", "-");
                    platform = plarformList.FirstOrDefault(x => x.NameUrl.Equals(p_url, System.StringComparison.CurrentCultureIgnoreCase));
                    if (platform != null)
                    {
                        where_clouse_for_game += string.Format(" AND t1.PlatformId={0} ", platform.Id);
                        where_clouse_for_game_maxprice += string.Format(" AND t1.PlatformId={0} ", platform.Id);
                    }
                }
            }
            #endregion

            #region Release Date Filters For Game
            if (!string.IsNullOrEmpty(rd))
            {
                if (rd.Contains(","))
                {
                    var rds = rd.Split(',').ToList();
                    rds = rds.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < rds.Count; index++)
                    {
                        if (string.IsNullOrEmpty(rds[index])) { continue; }
                        int days = 0;
                        switch (rds[index])
                        {
                            case "new-releases":
                                days = -180;
                                break;
                            case "last-30-days":
                                days = -30;
                                break;
                            case "last-90-days":
                                days = -90;
                                break;
                            case "best-of-2017":
                                days = -360;
                                break;
                        }
                        if (index == 0) { where_clouse_for_game += string.Format(" AND ( t1.First_release_date > dateadd(day, {0}, getdate()) ", days); where_clouse_for_game_maxprice += string.Format(" AND ( t1.First_release_date > dateadd(day, {0}, getdate()) ", days); }
                        if (index > 0) { where_clouse_for_game += string.Format(" OR t1.First_release_date > dateadd(day, {0}, getdate()) ", days); where_clouse_for_game_maxprice += string.Format(" OR t1.First_release_date > dateadd(day, {0}, getdate()) ", days); }
                        if (index == rds.Count - 1) { where_clouse_for_game += " ) "; where_clouse_for_game_maxprice += " ) "; }
                    }
                }
                else
                {
                    int days = 0;
                    switch (rd)
                    {
                        case "new-releases":
                            days = -180;
                            break;
                        case "last-30-days":
                            days = -30;
                            break;
                        case "last-90-days":
                            days = -90;
                            break;
                        case "best-of-2017":
                            days = -360;
                            break;
                    }
                    where_clouse_for_game += string.Format(" AND t1.First_release_date > dateadd(day, {0}, getdate()) ", days);
                    where_clouse_for_game_maxprice += string.Format(" AND t1.First_release_date > dateadd(day, {0}, getdate()) ", days);
                }
            }
            #endregion

            #region Release Date Filters For Accessories
            if (!string.IsNullOrEmpty(rd))
            {
                if (rd.Contains(","))
                {
                    var rds = rd.Split(',').ToList();
                    rds = rds.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < rds.Count; index++)
                    {
                        if (string.IsNullOrEmpty(rds[index])) { continue; }
                        int days = 0;
                        switch (rds[index])
                        {
                            case "new-releases":
                                days = -180;
                                break;
                            case "last-30-days":
                                days = -30;
                                break;
                            case "last-90-days":
                                days = -90;
                                break;
                            case "best-of-2017":
                                days = -360;
                                break;
                        }
                        if (index == 0) { where_clouse_for_accessories += string.Format(" AND ( t1.Added_On > dateadd(day, {0}, getdate()) ", days); where_clouse_for_accessories_maxprice += string.Format(" AND ( t1.Added_On > dateadd(day, {0}, getdate()) ", days); }
                        if (index > 0) { where_clouse_for_accessories += string.Format(" OR t1.Added_On > dateadd(day, {0}, getdate()) ", days); where_clouse_for_accessories_maxprice += string.Format(" OR t1.Added_On > dateadd(day, {0}, getdate()) ", days); }
                        if (index == rds.Count - 1) { where_clouse_for_accessories += " ) "; where_clouse_for_accessories_maxprice += " ) "; }
                    }
                }
                else
                {
                    int days = 0;
                    switch (rd)
                    {
                        case "new-releases":
                            days = -180;
                            break;
                        case "last-30-days":
                            days = -30;
                            break;
                        case "last-90-days":
                            days = -90;
                            break;
                        case "best-of-2017":
                            days = -360;
                            break;
                    }
                    where_clouse_for_accessories += string.Format(" AND t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                    where_clouse_for_accessories_maxprice += string.Format(" AND t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                }
            }
            #endregion

            #region Release Date Filters For Console
            if (!string.IsNullOrEmpty(rd))
            {
                if (rd.Contains(","))
                {
                    var rds = rd.Split(',').ToList();
                    rds = rds.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < rds.Count; index++)
                    {
                        if (string.IsNullOrEmpty(rds[index])) { continue; }
                        int days = 0;
                        switch (rds[index])
                        {
                            case "new-releases":
                                days = -180;
                                break;
                            case "last-30-days":
                                days = -30;
                                break;
                            case "last-90-days":
                                days = -90;
                                break;
                            case "best-of-2017":
                                days = -360;
                                break;
                        }
                        if (index == 0) { where_clouse_for_console += string.Format(" AND ( t1.Added_On > dateadd(day, {0}, getdate()) ", days); where_clouse_for_console_maxprice += string.Format(" AND ( t1.Added_On > dateadd(day, {0}, getdate()) ", days); }
                        if (index > 0) { where_clouse_for_console += string.Format(" OR t1.Added_On > dateadd(day, {0}, getdate()) ", days); where_clouse_for_console_maxprice += string.Format(" OR t1.Added_On > dateadd(day, {0}, getdate()) ", days); }
                        if (index == rds.Count - 1) { where_clouse_for_console += " ) "; where_clouse_for_console_maxprice += " ) "; }
                    }
                }
                else
                {
                    int days = 0;
                    switch (rd)
                    {
                        case "new-releases":
                            days = -180;
                            break;
                        case "last-30-days":
                            days = -30;
                            break;
                        case "last-90-days":
                            days = -90;
                            break;
                        case "best-of-2017":
                            days = -360;
                            break;
                    }
                    where_clouse_for_console += string.Format(" AND t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                    where_clouse_for_console_maxprice += string.Format(" AND t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                }
            }
            #endregion

            #region Condition Filters For Game
            if (!string.IsNullOrEmpty(c))
            {
                if (c.Contains(","))
                {
                    var cs = c.Split(',').ToList();
                    cs = cs.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < cs.Count; index++)
                    {
                        int id = 0;
                        if (string.IsNullOrEmpty(cs[index])) { continue; }
                        switch (cs[index])
                        {
                            case "new":
                                id = 1;
                                break;
                            case "pre-owned":
                                id = 2;
                                break;
                        }
                        if (index == 0) { where_clouse_for_game += string.Format(" AND ( t1.Condition_Id={0} ", id); where_clouse_for_game_maxprice += string.Format(" AND ( t1.Condition_Id={0} ", id); }
                        if (index > 0) { where_clouse_for_game += string.Format(" OR t1.Condition_Id={0} ", id); where_clouse_for_game_maxprice += string.Format(" OR t1.Condition_Id={0} ", id); }
                        if (index == cs.Count - 1) { where_clouse_for_game += " ) "; where_clouse_for_game_maxprice += " ) "; }
                    }
                }
                else
                {
                    switch (c)
                    {
                        case "new":
                            where_clouse_for_game += string.Format(" AND t1.Condition_Id={0} ", 1);
                            where_clouse_for_game_maxprice += string.Format(" AND t1.Condition_Id={0} ", 1);
                            break;
                        case "pre-owned":
                            where_clouse_for_game += string.Format(" AND t1.Condition_Id={0} ", 2);
                            where_clouse_for_game_maxprice += string.Format(" AND t1.Condition_Id={0} ", 2);
                            break;
                    }
                }
            }
            #endregion

            #region Condition Filters For Accessories
            if (!string.IsNullOrEmpty(c))
            {
                if (c.Contains(","))
                {
                    var cs = c.Split(',').ToList();
                    cs = cs.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < cs.Count; index++)
                    {
                        int id = 0;
                        if (string.IsNullOrEmpty(cs[index])) { continue; }
                        switch (cs[index])
                        {
                            case "new":
                                id = 1;
                                break;
                            case "pre-owned":
                                id = 2;
                                break;
                        }
                        if (index == 0) { where_clouse_for_accessories += string.Format(" AND ( t1.Condition_Id={0} ", id); where_clouse_for_accessories_maxprice += string.Format(" AND ( t1.Condition_Id={0} ", id); }
                        if (index > 0) { where_clouse_for_accessories += string.Format(" OR t1.Condition_Id={0} ", id); where_clouse_for_accessories_maxprice += string.Format(" OR t1.Condition_Id={0} ", id); }
                        if (index == cs.Count - 1) { where_clouse_for_accessories += " ) "; where_clouse_for_accessories_maxprice += " ) "; }

                    }
                }
                else
                {
                    switch (c)
                    {
                        case "new":
                            where_clouse_for_accessories += string.Format(" AND t1.Condition_Id={0} ", 1);
                            where_clouse_for_accessories_maxprice += string.Format(" AND t1.Condition_Id={0} ", 1);
                            break;
                        case "pre-owned":
                            where_clouse_for_accessories += string.Format(" AND t1.Condition_Id={0} ", 2);
                            where_clouse_for_accessories_maxprice += string.Format(" AND t1.Condition_Id={0} ", 2);
                            break;
                    }
                }
            }
            #endregion

            #region Condition Filters For Console
            if (!string.IsNullOrEmpty(c))
            {
                if (c.Contains(","))
                {
                    var cs = c.Split(',').ToList();
                    cs = cs.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < cs.Count; index++)
                    {
                        int id = 0;
                        if (string.IsNullOrEmpty(cs[index])) { continue; }
                        switch (cs[index])
                        {
                            case "new":
                                id = 1;
                                break;
                            case "pre-owned":
                                id = 2;
                                break;
                        }
                        if (index == 0) { where_clouse_for_console += string.Format(" AND ( t1.Condition_Id={0} ", id); where_clouse_for_console_maxprice += string.Format(" AND ( t1.Condition_Id={0} ", id); }
                        if (index > 0) { where_clouse_for_console += string.Format(" OR t1.Condition_Id={0} ", id); where_clouse_for_console_maxprice += string.Format(" OR t1.Condition_Id={0} ", id); }
                        if (index == cs.Count - 1) { where_clouse_for_console += " ) "; where_clouse_for_console_maxprice += " ) "; }
                    }
                }
                else
                {
                    switch (c)
                    {
                        case "new":
                            where_clouse_for_console += string.Format(" AND t1.Condition_Id={0} ", 1);
                            where_clouse_for_console_maxprice += string.Format(" AND t1.Condition_Id={0} ", 1);
                            break;
                        case "pre-owned":
                            where_clouse_for_console += string.Format(" AND t1.Condition_Id={0} ", 2);
                            where_clouse_for_console_maxprice += string.Format(" AND t1.Condition_Id={0} ", 2);
                            break;
                    }

                }
            }
            #endregion

            #region Price Filters  For Game        
            if (min > 0)
            {

                where_clouse_for_game += string.Format(" AND t1.Selling_Price >= {0} ", min);
                where_clouse_for_console += string.Format(" AND t1.Selling_Price >= {0} ", min);
                where_clouse_for_accessories += string.Format(" AND t1.Selling_Price >= {0} ", min);
            }
            if (max > 0)
            {
                where_clouse_for_game += string.Format(" AND t1.Selling_Price <= {0} ", max);
                where_clouse_for_console += string.Format(" AND t1.Selling_Price <= {0} ", max);
                where_clouse_for_accessories += string.Format(" AND t1.Selling_Price <= {0} ", max);
            }
            #endregion

            #region Rating Filters
            if (!string.IsNullOrEmpty(r))
            {
                if (r.Contains(","))
                {
                    var rs = r.Split(',').ToList();
                    rs = rs.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < rs.Count; index++)
                    {
                        if (string.IsNullOrEmpty(rs[index])) { continue; }
                        int rate = 0;
                        switch (rs[index])
                        {
                            case "5-star":
                                rate = 5;
                                break;
                            case "4-star":
                                rate = 4;
                                break;
                            case "3-star":
                                rate = 3;
                                break;
                            case "2-star":
                                rate = 2;
                                break;
                            case "1-star":
                                rate = 1;
                                break;
                        }
                        if (index == 0)
                        {
                            where_clouse_for_game += string.Format(" AND ( t1.Rating>={0} ", rate);
                            where_clouse_for_game_maxprice += string.Format(" AND ( t1.Rating>={0} ", rate);
                            where_clouse_for_console += string.Format(" AND ( t1.Rating>={0} ", rate);
                            where_clouse_for_console_maxprice += string.Format(" AND ( t1.Rating>={0} ", rate);
                            where_clouse_for_accessories += string.Format(" AND ( t1.Rating>={0} ", rate);
                            where_clouse_for_accessories_maxprice += string.Format(" AND ( t1.Rating>={0} ", rate);
                        }
                        if (index > 0)
                        {
                            where_clouse_for_game += string.Format(" OR t1.Rating>={0} ", rate);
                            where_clouse_for_game_maxprice += string.Format(" OR t1.Rating>={0} ", rate);
                            where_clouse_for_console += string.Format(" OR t1.Rating>={0} ", rate);
                            where_clouse_for_console_maxprice += string.Format(" OR t1.Rating>={0} ", rate);
                            where_clouse_for_accessories += string.Format(" OR t1.Rating>={0} ", rate);
                            where_clouse_for_accessories_maxprice += string.Format(" OR t1.Rating>={0} ", rate);
                        }
                        if (index == rs.Count - 1)
                        {
                            where_clouse_for_game += " ) ";
                            where_clouse_for_game_maxprice += " ) ";
                            where_clouse_for_console += " ) ";
                            where_clouse_for_console_maxprice += " ) ";
                            where_clouse_for_accessories += " ) ";
                            where_clouse_for_accessories_maxprice += " ) ";
                        }
                    }
                }
                else
                {
                    int rate = 0;
                    switch (r)
                    {
                        case "5-star":
                            rate = 5;
                            break;
                        case "4-star":
                            rate = 4;
                            break;
                        case "3-star":
                            rate = 3;
                            break;
                        case "2-star":
                            rate = 2;
                            break;
                        case "1-star":
                            rate = 1;
                            break;
                    }
                    where_clouse_for_game += string.Format(" AND t1.Rating>={0} ", rate);
                    where_clouse_for_game_maxprice += string.Format(" AND t1.Rating>={0} ", rate);
                    where_clouse_for_console += string.Format(" AND t1.Rating>={0} ", rate);
                    where_clouse_for_console_maxprice += string.Format(" AND t1.Rating>={0} ", rate);
                    where_clouse_for_accessories += string.Format(" AND t1.Rating>={0} ", rate);
                    where_clouse_for_accessories_maxprice += string.Format(" AND t1.Rating>={0} ", rate);
                }
            }
            #endregion

            #region OrderBy
            string orderby = string.Empty;
            switch (order)
            {
                case "Price low to high":
                    orderby = " t1.Selling_Price asc ";
                    break;
                case "Price high to low":
                    orderby = " t1.Selling_Price desc ";
                    break;
                case "Popularity":
                    orderby = " t1.Popularity desc ";
                    break;
                case "Best Sellers":
                    orderby = " t1.IsBestSelling desc ";
                    break;
            }
            #endregion

            #region Best Selling Titles
            if (bs)
            {
                best_selling_query += " AND (t1.[IsBestSelling]=1 OR t1.[Id] in ";
                best_selling_query += " (SELECT Product_Id FROM(SELECT [Product_Id], COUNT([Product_Id]) as Total ";
                best_selling_query += " FROM [Shopping_Cart_Items_Tbl] tt1 ";
                best_selling_query += " LEFT JOIN Shopping_Cart_Tracking_Tbl tt2 on tt1.Tracking_id=tt2.Id ";
                best_selling_query += " WHERE CAST(tt2.OrderDate as date)>CAST(DATEADD(day, -60, getdate()) as date) ";
                best_selling_query += " GROUP BY tt1.Product_Id) t WHERE Total>0) ) ";

                where_clouse_for_game += best_selling_query;
                where_clouse_for_game_maxprice += best_selling_query;
                where_clouse_for_console += best_selling_query;
                where_clouse_for_console_maxprice += best_selling_query;
                where_clouse_for_accessories += best_selling_query;
                where_clouse_for_accessories_maxprice += best_selling_query;
            }
            #endregion

            #region Query Builder For Game            
            query_for_game = " SELECT TOP(35) t1.* ";
            query_for_game += " FROM [dbo].[Game_List_View_For_Display] t1 ";
            query_for_game += " WHERE t1.Id > 0 ";
            query_for_game += " AND t1.Active=1 AND t1.[Quantity]>0 ";
            query_for_game += string.IsNullOrEmpty(query) ? "" : string.Format(" AND t1.Name LIKE '%{0}%' ", query);
            query_for_game += where_clouse_for_game;
            //query_for_game += " ORDER by t1.First_release_date DESC, t1.Popularity DESC ";
            if (string.IsNullOrEmpty(order))
                query_for_game += " ORDER by t1.First_release_date DESC, t1.Popularity DESC ";
            else
                query_for_game += " ORDER by " + orderby;
            query_for_game += "; ";
            #endregion

            #region Query Builder For Accessories            
            query_for_accessories = " SELECT TOP(10) t1.*, ";
            query_for_accessories += " (select TOP(1) [Url] from ImagesTbl ";
            query_for_accessories += " where Reference_Id=t1.Id AND ImageOf='Accessories' ";
            query_for_accessories += " AND ImageType='Thumbnail' AND ImageCategory='Cover') as ImageUrl, ";
            query_for_accessories += " t2.Name as ProductTypeName, t3.Name as 'Condition', t4.Name as 'Platform' ";
            query_for_accessories += " FROM [ProductsTbl] t1 ";
            query_for_accessories += " LEFT JOIN Common_Name_Url_Tbl t2 on t1.ProductTypeId=t2.Id ";
            query_for_accessories += " LEFT JOIN ConditionTbl t3 on t1.Condition_Id=t3.Id ";
            query_for_accessories += " LEFT JOIN GamePlatformsTbl t4 on t1.Platform_Code=t4.Code ";
            query_for_accessories += " WHERE t1.Id > 0 ";
            query_for_accessories += " AND t1.ProductType='Accessories' ";
            query_for_accessories += " AND t1.Quantity>0 ";
            query_for_accessories += string.IsNullOrEmpty(query) ? "" : string.Format(" AND t1.Name LIKE '%{0}%' ", query);
            query_for_accessories += where_clouse_for_accessories;
            //query_for_accessories += " ORDER by t1.Popularity DESC ";
            if (string.IsNullOrEmpty(order))
                query_for_accessories += " ORDER by t1.Popularity DESC ";
            else
                query_for_accessories += " ORDER by " + orderby;
            query_for_accessories += "; ";
            #endregion

            #region Query Builder For Console            
            query_for_console = " SELECT TOP(5) t1.*, ";
            query_for_console += " (select TOP(1) [Url] from ImagesTbl ";
            query_for_console += " where Reference_Id=t1.Id AND ImageOf='Console' ";
            query_for_console += " AND ImageType='Thumbnail' AND ImageCategory='Cover') as ImageUrl, ";
            query_for_console += " t2.Name as ProductTypeName, t3.Name as 'Condition', t4.Name as 'Platform'  ";
            query_for_console += " FROM [ProductsTbl] t1 ";
            query_for_console += " LEFT JOIN Common_Name_Url_Tbl t2 on t1.ProductTypeId=t2.Id ";
            query_for_console += " LEFT JOIN ConditionTbl t3 on t1.Condition_Id=t3.Id ";
            query_for_console += " LEFT JOIN GamePlatformsTbl t4 on t1.Platform_Code=t4.Code ";
            query_for_console += " WHERE t1.Id > 0 ";
            query_for_console += " AND t1.ProductType='Console' ";
            query_for_console += " AND t1.Quantity>0 ";
            query_for_console += string.IsNullOrEmpty(query) ? "" : string.Format(" AND t1.Name LIKE '%{0}%' ", query);
            query_for_console += where_clouse_for_console;
            //query_for_console += " ORDER by t1.Popularity DESC ";
            if (string.IsNullOrEmpty(order))
                query_for_console += " ORDER by t1.Popularity DESC ";
            else
                query_for_console += " ORDER by " + orderby;
            query_for_console += "; ";
            #endregion

            #region Query Builder For Max Price
            string MaxPriceQuery = string.Empty;


            MaxPriceQuery += " select Max ( [Selling_Price]) from(SELECT isnull(Max(t1.Selling_Price), 0) as[Selling_Price] ";
            MaxPriceQuery += " FROM[dbo].[Game_List_View_For_Display] t1 ";
            MaxPriceQuery += " WHERE t1.Id > 0 ";
            MaxPriceQuery += " AND t1.Active = 1 AND t1.[Quantity] > 0 ";
            MaxPriceQuery += " AND t1.Original_Price <> t1.Selling_Price AND t1.Selling_Price > 0 ";
            MaxPriceQuery += where_clouse_for_game_maxprice;
            MaxPriceQuery += " Union ";
            MaxPriceQuery += " SELECT  isnull(Max(t1.Selling_Price), 0) as[Selling_Price] ";
            MaxPriceQuery += " FROM[ProductsTbl] t1 ";
            MaxPriceQuery += " WHERE t1.Id > 0 ";
            MaxPriceQuery += " AND t1.ProductType = 'Accessories' ";
            MaxPriceQuery += " AND t1.Quantity > 0 ";
            MaxPriceQuery += " AND t1.Original_Price <> t1.Selling_Price AND t1.Selling_Price > 0 ";
            MaxPriceQuery += where_clouse_for_accessories_maxprice;
            MaxPriceQuery += " Union ";
            MaxPriceQuery += " SELECT  isnull(Max(t1.Selling_Price), 0) as[Selling_Price] ";
            MaxPriceQuery += " FROM[ProductsTbl] t1 ";
            MaxPriceQuery += " WHERE t1.Id > 0 ";
            MaxPriceQuery += " AND t1.ProductType = 'Console' ";
            MaxPriceQuery += " AND t1.Quantity > 0 ";
            MaxPriceQuery += " AND t1.Original_Price <> t1.Selling_Price AND t1.Selling_Price > 0 ";
            MaxPriceQuery += where_clouse_for_console_maxprice;
            MaxPriceQuery += ") as[a] ";






            //MaxPriceQuery += "SELECT isnull(Max(t1.Selling_Price),0) ";
            //MaxPriceQuery += " FROM[dbo].[Game_List_View_For_Display] t1 ";
            //MaxPriceQuery += " WHERE t1.Id > 0 ";
            //MaxPriceQuery += " AND t1.Active = 1 AND t1.[Quantity] > 0 ";
            //MaxPriceQuery += where_clouse_for_game;
            #endregion
            var result = await _search_repo.Search_With_Custom_query(query_for_game, query_for_accessories, query_for_console, MaxPriceQuery);
            //ViewBag.MaxPrice = result.GameList.OrderByDescending(x => x.Selling_Price).Select(x => x.Selling_Price).FirstOrDefault();
            ViewBag.MaxSelectedPrice = max == 0 ? result.MaxPrice : max;
            ViewBag.MaxPrice = result.MaxPrice;
            return View(result);
        }

        [HttpPost("Search")]
        public async Task<PartialViewResult> Search([FromBody] Search_Query obj)
        {
            string query = Regex.Replace(obj.Query, "[^a-zA-Z0-9 ]", "");
            var result = await _search_repo.Search_By_query(query);
            return PartialView("_Search_Result_Partial", result.Data);
        }

        [HttpGet("hotdeals")]
        public async Task<IActionResult> HotDeals(string q, string p, string g, string rd, string r, string c, string t, string order, int page = 1, int min = 0, int max = 0, int pegi = 0)
        {
            q = string.IsNullOrEmpty(q) ? string.Empty : q;
            ViewBag.Search_Query = q;
            ViewBag.PageTitle = "HotDeals";// string.Format("HotDeals results for: {0}", q);

            string query = Regex.Replace(q, "[^a-zA-Z0-9 ]", "");

            string query_for_game = string.Empty;
            string query_for_console = string.Empty;
            string query_for_accessories = string.Empty;

            string where_clouse_for_game = string.Empty;
            string where_clouse_for_game_maxprice = string.Empty;
            string where_clouse_for_console = string.Empty;
            string where_clouse_for_console_maxprice = string.Empty;
            string where_clouse_for_accessories = string.Empty;
            string where_clouse_for_accessories_maxprice = string.Empty;

            var plarformList = await _gp_repo.Get_ALL_ActiveAsync();
            var genreList = await _gr_repo.Get_ALL_ActiveAsync();
            ViewBag.GenreList = genreList;
            ViewBag.PlarformList = plarformList;
            ViewBag.GenreUrl = g;
            ViewBag.PlarformUrl = p;
            ViewBag.PageIndex = page;
            ViewBag.Release_Date = rd;
            ViewBag.TypeGame = t;
            ViewBag.Condition = c;
            ViewBag.Ratings = r;
            ViewBag.MinPrice = min;
            ViewBag.orderby = order;

            Game_Filter obj = new Game_Filter { Offset = 0, PageIndex = page, Rows = 20, Platform_Id = 0 };
            obj.Offset = (page - 1) * 20;

            #region Genere Filters
            if (!string.IsNullOrEmpty(g))
            {
                string g_url = string.Empty;
                Genere genre = null;
                if (g.Contains(","))
                {
                    var grs = g.Split(',').ToList();
                    grs = grs.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < grs.Count; index++)
                    {
                        if (string.IsNullOrEmpty(grs[index])) { continue; }
                        g_url = Regex.Replace(grs[index], "[^a-zA-Z0-9]", "-");
                        genre = genreList.FirstOrDefault(x => x.NameUrl.Equals(g_url, System.StringComparison.CurrentCultureIgnoreCase));
                        if (genre != null)
                        {
                            if (index == 0) { where_clouse_for_game += string.Format(" AND ( t1.Genres LIKE '%,{0},%' ", genre.Id); where_clouse_for_game_maxprice += string.Format(" AND ( t1.Genres LIKE '%,{0},%' ", genre.Id); }
                            if (index > 0) { where_clouse_for_game += string.Format(" OR t1.Genres LIKE '%,{0},%' ", genre.Id); where_clouse_for_game_maxprice += string.Format(" OR t1.Genres LIKE '%,{0},%' ", genre.Id); }
                            if (index == grs.Count - 1) { where_clouse_for_game += " ) "; where_clouse_for_game_maxprice += " ) "; }
                        }
                    }
                }
                else
                {
                    g_url = Regex.Replace(g, "[^a-zA-Z0-9]", "-");
                    genre = genreList.FirstOrDefault(x => x.NameUrl.Equals(g_url, System.StringComparison.CurrentCultureIgnoreCase));
                    if (genre != null)
                    {
                        where_clouse_for_game += string.Format(" AND t1.Genres LIKE '%,{0},%' ", genre.Id);
                        where_clouse_for_game_maxprice += string.Format(" AND t1.Genres LIKE '%,{0},%' ", genre.Id);
                    }
                }
            }
            #endregion

            #region Platform Filters
            if (!string.IsNullOrEmpty(p))
            {
                string p_url = string.Empty;
                GamePlatform platform = null;
                if (p.Contains(","))
                {
                    var pls = p.Split(',').ToList();
                    pls = pls.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < pls.Count; index++)
                    {
                        if (string.IsNullOrEmpty(pls[index])) { continue; }
                        p_url = Regex.Replace(pls[index], "[^a-zA-Z0-9]", "-");
                        platform = plarformList.FirstOrDefault(x => x.NameUrl.Equals(p_url, System.StringComparison.CurrentCultureIgnoreCase));
                        if (platform != null)
                        {
                            if (index == 0) { where_clouse_for_game += string.Format(" AND ( t1.PlatformId={0} ", platform.Id); where_clouse_for_game_maxprice += string.Format(" AND ( t1.PlatformId={0} ", platform.Id); }
                            if (index > 0) { where_clouse_for_game += string.Format(" OR t1.PlatformId={0} ", platform.Id); where_clouse_for_game_maxprice += string.Format(" OR t1.PlatformId={0} ", platform.Id); }
                            if (index == pls.Count - 1) { where_clouse_for_game += " ) "; where_clouse_for_game_maxprice += " ) "; }
                        }
                    }
                }
                else
                {
                    p_url = Regex.Replace(p, "[^a-zA-Z0-9]", "-");
                    platform = plarformList.FirstOrDefault(x => x.NameUrl.Equals(p_url, System.StringComparison.CurrentCultureIgnoreCase));
                    if (platform != null)
                    {
                        where_clouse_for_game += string.Format(" AND t1.PlatformId={0} ", platform.Id);
                        where_clouse_for_game_maxprice += string.Format(" AND t1.PlatformId={0} ", platform.Id);
                    }
                }
            }
            #endregion

            #region Release Date Filters For Game
            if (!string.IsNullOrEmpty(rd))
            {
                if (rd.Contains(","))
                {
                    var rds = rd.Split(',').ToList();
                    rds = rds.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < rds.Count; index++)
                    {
                        if (string.IsNullOrEmpty(rds[index])) { continue; }
                        int days = 0;
                        switch (rds[index])
                        {
                            case "new-releases":
                                days = -180;
                                break;
                            case "last-30-days":
                                days = -30;
                                break;
                            case "last-90-days":
                                days = -90;
                                break;
                            case "best-of-2017":
                                days = -360;
                                break;
                        }
                        if (index == 0) { where_clouse_for_game += string.Format(" AND ( t1.First_release_date > dateadd(day, {0}, getdate()) ", days); where_clouse_for_game_maxprice += string.Format(" AND ( t1.First_release_date > dateadd(day, {0}, getdate()) ", days); }
                        if (index > 0) { where_clouse_for_game += string.Format(" OR t1.First_release_date > dateadd(day, {0}, getdate()) ", days); where_clouse_for_game_maxprice += string.Format(" OR t1.First_release_date > dateadd(day, {0}, getdate()) ", days); }
                        if (index == rds.Count - 1) { where_clouse_for_game += " ) "; where_clouse_for_game_maxprice += " ) "; }
                    }
                }
                else
                {
                    int days = 0;
                    switch (rd)
                    {
                        case "new-releases":
                            days = -180;
                            break;
                        case "last-30-days":
                            days = -30;
                            break;
                        case "last-90-days":
                            days = -90;
                            break;
                        case "best-of-2017":
                            days = -360;
                            break;
                    }
                    where_clouse_for_game += string.Format(" AND t1.First_release_date > dateadd(day, {0}, getdate()) ", days);
                    where_clouse_for_game_maxprice += string.Format(" AND t1.First_release_date > dateadd(day, {0}, getdate()) ", days);
                }
            }
            #endregion

            #region Release Date Filters For Accessories
            if (!string.IsNullOrEmpty(rd))
            {
                if (rd.Contains(","))
                {
                    var rds = rd.Split(',').ToList();
                    rds = rds.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < rds.Count; index++)
                    {
                        if (string.IsNullOrEmpty(rds[index])) { continue; }
                        int days = 0;
                        switch (rds[index])
                        {
                            case "new-releases":
                                days = -180;
                                break;
                            case "last-30-days":
                                days = -30;
                                break;
                            case "last-90-days":
                                days = -90;
                                break;
                            case "best-of-2017":
                                days = -360;
                                break;
                        }
                        if (index == 0) { where_clouse_for_accessories += string.Format(" AND ( t1.Added_On > dateadd(day, {0}, getdate()) ", days); where_clouse_for_accessories_maxprice += string.Format(" AND ( t1.Added_On > dateadd(day, {0}, getdate()) ", days); }
                        if (index > 0) { where_clouse_for_accessories += string.Format(" OR t1.Added_On > dateadd(day, {0}, getdate()) ", days); where_clouse_for_accessories_maxprice += string.Format(" OR t1.Added_On > dateadd(day, {0}, getdate()) ", days); }
                        if (index == rds.Count - 1) { where_clouse_for_accessories += " ) "; where_clouse_for_accessories_maxprice += " ) "; }
                    }
                }
                else
                {
                    int days = 0;
                    switch (rd)
                    {
                        case "new-releases":
                            days = -180;
                            break;
                        case "last-30-days":
                            days = -30;
                            break;
                        case "last-90-days":
                            days = -90;
                            break;
                        case "best-of-2017":
                            days = -360;
                            break;
                    }
                    where_clouse_for_accessories += string.Format(" AND t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                    where_clouse_for_accessories_maxprice += string.Format(" AND t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                }
            }
            #endregion

            #region Release Date Filters For Console
            if (!string.IsNullOrEmpty(rd))
            {
                if (rd.Contains(","))
                {
                    var rds = rd.Split(',').ToList();
                    rds = rds.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < rds.Count; index++)
                    {
                        if (string.IsNullOrEmpty(rds[index])) { continue; }
                        int days = 0;
                        switch (rds[index])
                        {
                            case "new-releases":
                                days = -180;
                                break;
                            case "last-30-days":
                                days = -30;
                                break;
                            case "last-90-days":
                                days = -90;
                                break;
                            case "best-of-2017":
                                days = -360;
                                break;
                        }
                        if (index == 0) { where_clouse_for_console += string.Format(" AND ( t1.Added_On > dateadd(day, {0}, getdate()) ", days); where_clouse_for_console_maxprice += string.Format(" AND ( t1.Added_On > dateadd(day, {0}, getdate()) ", days); }
                        if (index > 0) { where_clouse_for_console += string.Format(" OR t1.Added_On > dateadd(day, {0}, getdate()) ", days); where_clouse_for_console_maxprice += string.Format(" OR t1.Added_On > dateadd(day, {0}, getdate()) ", days); }
                        if (index == rds.Count - 1) { where_clouse_for_console += " ) "; where_clouse_for_console_maxprice += " ) "; }
                    }
                }
                else
                {
                    int days = 0;
                    switch (rd)
                    {
                        case "new-releases":
                            days = -180;
                            break;
                        case "last-30-days":
                            days = -30;
                            break;
                        case "last-90-days":
                            days = -90;
                            break;
                        case "best-of-2017":
                            days = -360;
                            break;
                    }
                    where_clouse_for_console += string.Format(" AND t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                    where_clouse_for_console_maxprice += string.Format(" AND t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                }
            }
            #endregion

            #region Condition Filters For Game
            if (!string.IsNullOrEmpty(c))
            {
                if (c.Contains(","))
                {
                    var cs = c.Split(',').ToList();
                    cs = cs.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < cs.Count; index++)
                    {
                        int id = 0;
                        if (string.IsNullOrEmpty(cs[index])) { continue; }
                        switch (cs[index])
                        {
                            case "new":
                                id = 1;
                                break;
                            case "pre-owned":
                                id = 2;
                                break;
                        }
                        if (index == 0) { where_clouse_for_game += string.Format(" AND ( t1.Condition_Id={0} ", id); where_clouse_for_game_maxprice += string.Format(" AND ( t1.Condition_Id={0} ", id); }
                        if (index > 0) { where_clouse_for_game += string.Format(" OR t1.Condition_Id={0} ", id); where_clouse_for_game_maxprice += string.Format(" OR t1.Condition_Id={0} ", id); }
                        if (index == cs.Count - 1) { where_clouse_for_game += " ) "; where_clouse_for_game_maxprice += " ) "; }
                    }
                }
                else
                {
                    switch (c)
                    {
                        case "new":
                            where_clouse_for_game += string.Format(" AND t1.Condition_Id={0} ", 1);
                            where_clouse_for_game_maxprice += string.Format(" AND t1.Condition_Id={0} ", 1);
                            break;
                        case "pre-owned":
                            where_clouse_for_game += string.Format(" AND t1.Condition_Id={0} ", 2);
                            where_clouse_for_game_maxprice += string.Format(" AND t1.Condition_Id={0} ", 2);
                            break;
                    }
                }
            }
            #endregion

            #region Condition Filters For Accessories
            if (!string.IsNullOrEmpty(c))
            {
                if (c.Contains(","))
                {
                    var cs = c.Split(',').ToList();
                    cs = cs.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < cs.Count; index++)
                    {
                        int id = 0;
                        if (string.IsNullOrEmpty(cs[index])) { continue; }
                        switch (cs[index])
                        {
                            case "new":
                                id = 1;
                                break;
                            case "pre-owned":
                                id = 2;
                                break;
                        }
                        if (index == 0) { where_clouse_for_accessories += string.Format(" AND ( t1.Condition_Id={0} ", id); where_clouse_for_accessories_maxprice += string.Format(" AND ( t1.Condition_Id={0} ", id); }
                        if (index > 0) { where_clouse_for_accessories += string.Format(" OR t1.Condition_Id={0} ", id); where_clouse_for_accessories_maxprice += string.Format(" OR t1.Condition_Id={0} ", id); }
                        if (index == cs.Count - 1) { where_clouse_for_accessories += " ) "; where_clouse_for_accessories_maxprice += " ) "; }

                    }
                }
                else
                {
                    switch (c)
                    {
                        case "new":
                            where_clouse_for_accessories += string.Format(" AND t1.Condition_Id={0} ", 1);
                            where_clouse_for_accessories_maxprice += string.Format(" AND t1.Condition_Id={0} ", 1);
                            break;
                        case "pre-owned":
                            where_clouse_for_accessories += string.Format(" AND t1.Condition_Id={0} ", 2);
                            where_clouse_for_accessories_maxprice += string.Format(" AND t1.Condition_Id={0} ", 2);
                            break;
                    }
                }
            }
            #endregion

            #region Condition Filters For Console
            if (!string.IsNullOrEmpty(c))
            {
                if (c.Contains(","))
                {
                    var cs = c.Split(',').ToList();
                    cs = cs.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < cs.Count; index++)
                    {
                        int id = 0;
                        if (string.IsNullOrEmpty(cs[index])) { continue; }
                        switch (cs[index])
                        {
                            case "new":
                                id = 1;
                                break;
                            case "pre-owned":
                                id = 2;
                                break;
                        }
                        if (index == 0) { where_clouse_for_console += string.Format(" AND ( t1.Condition_Id={0} ", id); where_clouse_for_console_maxprice += string.Format(" AND ( t1.Condition_Id={0} ", id); }
                        if (index > 0) { where_clouse_for_console += string.Format(" OR t1.Condition_Id={0} ", id); where_clouse_for_console_maxprice += string.Format(" OR t1.Condition_Id={0} ", id); }
                        if (index == cs.Count - 1) { where_clouse_for_console += " ) "; where_clouse_for_console_maxprice += " ) "; }
                    }
                }
                else
                {
                    switch (c)
                    {
                        case "new":
                            where_clouse_for_console += string.Format(" AND t1.Condition_Id={0} ", 1);
                            where_clouse_for_console_maxprice += string.Format(" AND t1.Condition_Id={0} ", 1);
                            break;
                        case "pre-owned":
                            where_clouse_for_console += string.Format(" AND t1.Condition_Id={0} ", 2);
                            where_clouse_for_console_maxprice += string.Format(" AND t1.Condition_Id={0} ", 2);
                            break;
                    }

                }
            }
            #endregion

            #region Price Filters  For Game        
            if (min > 0)
            {
                where_clouse_for_game += string.Format(" AND t1.Selling_Price >= {0} ", min);
                where_clouse_for_console += string.Format(" AND t1.Selling_Price >= {0} ", min);
                where_clouse_for_accessories += string.Format(" AND t1.Selling_Price >= {0} ", min);
            }
            if (max > 0)
            {
                where_clouse_for_game += string.Format(" AND t1.Selling_Price <= {0} ", max);
                where_clouse_for_console += string.Format(" AND t1.Selling_Price <= {0} ", max);
                where_clouse_for_accessories += string.Format(" AND t1.Selling_Price <= {0} ", max);
            }
            #endregion

            #region Rating Filters
            if (!string.IsNullOrEmpty(r))
            {
                if (r.Contains(","))
                {
                    var rs = r.Split(',').ToList();
                    rs = rs.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    for (int index = 0; index < rs.Count; index++)
                    {
                        if (string.IsNullOrEmpty(rs[index])) { continue; }
                        int rate = 0;
                        switch (rs[index])
                        {
                            case "5-star":
                                rate = 5;
                                break;
                            case "4-star":
                                rate = 4;
                                break;
                            case "3-star":
                                rate = 3;
                                break;
                            case "2-star":
                                rate = 2;
                                break;
                            case "1-star":
                                rate = 1;
                                break;
                        }
                        if (index == 0)
                        {
                            where_clouse_for_game += string.Format(" AND ( t1.Rating>={0} ", rate);
                            where_clouse_for_game_maxprice += string.Format(" AND ( t1.Rating>={0} ", rate);
                            where_clouse_for_console += string.Format(" AND ( t1.Rating>={0} ", rate);
                            where_clouse_for_console_maxprice += string.Format(" AND ( t1.Rating>={0} ", rate);
                            where_clouse_for_accessories += string.Format(" AND ( t1.Rating>={0} ", rate);
                            where_clouse_for_accessories_maxprice += string.Format(" AND ( t1.Rating>={0} ", rate);
                        }
                        if (index > 0)
                        {
                            where_clouse_for_game += string.Format(" OR t1.Rating>={0} ", rate);
                            where_clouse_for_game_maxprice += string.Format(" OR t1.Rating>={0} ", rate);
                            where_clouse_for_console += string.Format(" OR t1.Rating>={0} ", rate);
                            where_clouse_for_console_maxprice += string.Format(" OR t1.Rating>={0} ", rate);
                            where_clouse_for_accessories += string.Format(" OR t1.Rating>={0} ", rate);
                            where_clouse_for_accessories_maxprice += string.Format(" OR t1.Rating>={0} ", rate);
                        }
                        if (index == rs.Count - 1)
                        {
                            where_clouse_for_game += " ) ";
                            where_clouse_for_game_maxprice += " ) ";
                            where_clouse_for_console += " ) ";
                            where_clouse_for_console_maxprice += " ) ";
                            where_clouse_for_accessories += " ) ";
                            where_clouse_for_accessories_maxprice += " ) ";
                        }
                    }
                }
                else
                {
                    int rate = 0;
                    switch (r)
                    {
                        case "5-star":
                            rate = 5;
                            break;
                        case "4-star":
                            rate = 4;
                            break;
                        case "3-star":
                            rate = 3;
                            break;
                        case "2-star":
                            rate = 2;
                            break;
                        case "1-star":
                            rate = 1;
                            break;
                    }
                    where_clouse_for_game += string.Format(" AND t1.Rating>={0} ", rate);
                    where_clouse_for_game_maxprice += string.Format(" AND t1.Rating>={0} ", rate);
                    where_clouse_for_console += string.Format(" AND t1.Rating>={0} ", rate);
                    where_clouse_for_console_maxprice += string.Format(" AND t1.Rating>={0} ", rate);
                    where_clouse_for_accessories += string.Format(" AND t1.Rating>={0} ", rate);
                    where_clouse_for_accessories_maxprice += string.Format(" AND t1.Rating>={0} ", rate);
                }
            }
            #endregion

            #region OrderBy
            string orderby = string.Empty;
            switch (order)
            {
                case "Price low to high":
                    orderby = " t1.Selling_Price asc ";
                    break;
                case "Price high to low":
                    orderby = " t1.Selling_Price desc ";
                    break;
                case "Popularity":
                    orderby = " t1.Popularity desc ";
                    break;
                case "Best Sellers":
                    orderby = " t1.IsBestSelling desc ";
                    break;
            }
            #endregion

            #region Query Builder For Game   
            //Get Total Count
            query_for_game = " SELECT  Count (*) TotalCount ";
            query_for_game += " FROM [dbo].[Game_List_View_For_Display] t1 ";
            query_for_game += " WHERE t1.Id > 0 ";
            query_for_game += " AND t1.Active=1 AND t1.[Quantity]>0 ";
            query_for_game += " AND t1.Original_Price <> t1.Selling_Price AND t1.Selling_Price > 0";
            query_for_game += where_clouse_for_game;
            query_for_game += "; ";
            //END
            query_for_game += " SELECT   t1.* ";
            query_for_game += " FROM [dbo].[Game_List_View_For_Display] t1 ";
            query_for_game += " WHERE t1.Id > 0 ";
            query_for_game += " AND t1.Active=1 AND t1.[Quantity]>0 ";
            query_for_game += " AND t1.Original_Price <> t1.Selling_Price AND t1.Selling_Price > 0";
            query_for_game += where_clouse_for_game;
            //query_for_game += " ORDER by t1.First_release_date DESC, t1.Popularity DESC ";
            if (string.IsNullOrEmpty(order))
                query_for_game += " ORDER by t1.First_release_date DESC, t1.Popularity DESC ";
            else
                query_for_game += " ORDER by " + orderby;
            query_for_game += string.Format(" OFFSET {0} ROWS ", obj.Offset);
            query_for_game += string.Format(" FETCH NEXT {0} ROWS ONLY ", obj.Rows);
            query_for_game += "; ";
            #endregion

            #region Query Builder For Accessories  
            //Get Total Count   
            query_for_accessories = " SELECT  Count (*) TotalCount ";

            query_for_accessories += " FROM [ProductsTbl] t1 ";
            query_for_accessories += " WHERE t1.Id > 0 ";
            query_for_accessories += " AND t1.ProductType='Accessories' ";
            query_for_accessories += " AND t1.Quantity>0 ";
            query_for_accessories += " AND t1.Original_Price <> t1.Selling_Price AND t1.Selling_Price > 0";
            query_for_accessories += where_clouse_for_accessories;
            query_for_accessories += "; ";
            //END
            query_for_accessories += " SELECT  t1.*, ";
            query_for_accessories += " (select TOP(1) [Url] from ImagesTbl ";
            query_for_accessories += " where Reference_Id=t1.Id AND ImageOf='Accessories' ";
            query_for_accessories += " AND ImageType='Thumbnail' AND ImageCategory='Cover') as ImageUrl, ";
            query_for_accessories += " t2.Name as ProductTypeName, t3.Name as 'Condition', t4.Name as 'Platform' ";
            query_for_accessories += " FROM [ProductsTbl] t1 ";
            query_for_accessories += " LEFT JOIN Common_Name_Url_Tbl t2 on t1.ProductTypeId=t2.Id ";
            query_for_accessories += " LEFT JOIN ConditionTbl t3 on t1.Condition_Id=t3.Id ";
            query_for_accessories += " LEFT JOIN GamePlatformsTbl t4 on t1.Platform_Code=t4.Code ";
            query_for_accessories += " WHERE t1.Id > 0 ";
            query_for_accessories += " AND t1.ProductType='Accessories' ";
            query_for_accessories += " AND t1.Quantity>0 ";
            query_for_accessories += " AND t1.Original_Price <> t1.Selling_Price AND t1.Selling_Price > 0";
            query_for_accessories += string.IsNullOrEmpty(query) ? "" : string.Format(" AND t1.Name LIKE '%{0}%' ", query);
            query_for_accessories += where_clouse_for_accessories;
            //query_for_accessories += " ORDER by t1.Popularity DESC ";
            if (string.IsNullOrEmpty(order))
                query_for_accessories += " ORDER by t1.Popularity DESC ";
            else
                query_for_accessories += " ORDER by " + orderby;
            query_for_accessories += string.Format(" OFFSET {0} ROWS ", obj.Offset);
            query_for_accessories += string.Format(" FETCH NEXT {0} ROWS ONLY ", obj.Rows);
            query_for_accessories += "; ";
            #endregion

            #region Query Builder For Console    

            // Get Total Count 
            query_for_console = " SELECT  Count (*) TotalCount ";
            query_for_console += " FROM [ProductsTbl] t1 ";
            query_for_console += " WHERE t1.Id > 0 ";
            query_for_console += " AND t1.ProductType='Console' ";
            query_for_console += " AND t1.Quantity>0 ";
            query_for_console += " AND t1.Original_Price <> t1.Selling_Price AND t1.Selling_Price > 0";
            query_for_console += where_clouse_for_console;
            query_for_accessories += "; ";
            // ENd        
            query_for_console += " SELECT  t1.*, ";
            query_for_console += " (select TOP(1) [Url] from ImagesTbl ";
            query_for_console += " where Reference_Id=t1.Id AND ImageOf='Console' ";
            query_for_console += " AND ImageType='Thumbnail' AND ImageCategory='Cover') as ImageUrl, ";
            query_for_console += " t2.Name as ProductTypeName, t3.Name as 'Condition', t4.Name as 'Platform'  ";
            query_for_console += " FROM [ProductsTbl] t1 ";
            query_for_console += " LEFT JOIN Common_Name_Url_Tbl t2 on t1.ProductTypeId=t2.Id ";
            query_for_console += " LEFT JOIN ConditionTbl t3 on t1.Condition_Id=t3.Id ";
            query_for_console += " LEFT JOIN GamePlatformsTbl t4 on t1.Platform_Code=t4.Code ";
            query_for_console += " WHERE t1.Id > 0 ";
            query_for_console += " AND t1.ProductType='Console' ";
            query_for_console += " AND t1.Quantity>0 ";
            query_for_console += " AND t1.Original_Price <> t1.Selling_Price AND t1.Selling_Price > 0";
            query_for_console += where_clouse_for_console;
            //query_for_console += " ORDER by t1.Popularity DESC ";
            if (string.IsNullOrEmpty(order))
                query_for_console += " ORDER by t1.Popularity DESC ";
            else
                query_for_console += " ORDER by " + orderby;
            query_for_console += string.Format(" OFFSET {0} ROWS ", obj.Offset);
            query_for_console += string.Format(" FETCH NEXT {0} ROWS ONLY ", obj.Rows);
            query_for_console += "; ";
            #endregion

            #region Query Builder For Max Price
            string MaxPriceQuery = string.Empty;
            MaxPriceQuery += " select Max ( [Selling_Price]) from(SELECT isnull(Max(t1.Selling_Price), 0) as[Selling_Price] ";
            MaxPriceQuery += " FROM[dbo].[Game_List_View_For_Display] t1 ";
            MaxPriceQuery += " WHERE t1.Id > 0 ";
            MaxPriceQuery += " AND t1.Active = 1 AND t1.[Quantity] > 0 ";
            MaxPriceQuery += " AND t1.Original_Price <> t1.Selling_Price AND t1.Selling_Price > 0 ";
            MaxPriceQuery += where_clouse_for_game_maxprice;
            MaxPriceQuery += " Union ";
            MaxPriceQuery += " SELECT  isnull(Max(t1.Selling_Price), 0) as[Selling_Price] ";
            MaxPriceQuery += " FROM[ProductsTbl] t1 ";
            MaxPriceQuery += " WHERE t1.Id > 0 ";
            MaxPriceQuery += " AND t1.ProductType = 'Accessories' ";
            MaxPriceQuery += " AND t1.Quantity > 0 ";
            MaxPriceQuery += " AND t1.Original_Price <> t1.Selling_Price AND t1.Selling_Price > 0 ";
            MaxPriceQuery += where_clouse_for_accessories_maxprice;
            MaxPriceQuery += " Union ";
            MaxPriceQuery += " SELECT  isnull(Max(t1.Selling_Price), 0) as[Selling_Price] ";
            MaxPriceQuery += " FROM[ProductsTbl] t1 ";
            MaxPriceQuery += " WHERE t1.Id > 0 ";
            MaxPriceQuery += " AND t1.ProductType = 'Console' ";
            MaxPriceQuery += " AND t1.Quantity > 0 ";
            MaxPriceQuery += " AND t1.Original_Price <> t1.Selling_Price AND t1.Selling_Price > 0 ";
            MaxPriceQuery += where_clouse_for_console_maxprice;
            MaxPriceQuery += ") as[a] ";

            //MaxPriceQuery += "SELECT isnull(Max(t1.Selling_Price),0) ";
            //MaxPriceQuery += " FROM[dbo].[Game_List_View_For_Display] t1 ";
            //MaxPriceQuery += " WHERE t1.Id > 0 ";
            //MaxPriceQuery += " AND t1.Active = 1 AND t1.[Quantity] > 0 ";
            //MaxPriceQuery += " AND t1.Original_Price<> t1.Selling_Price AND t1.Selling_Price > 0 ";
            //MaxPriceQuery += where_clouse_for_game_maxprice;
            #endregion

            var result = await _hotdeals_repo.Search_With_Custom_query(query_for_game, query_for_accessories, query_for_console, MaxPriceQuery);
            //ViewBag.MaxPrice = result.GameList.OrderByDescending(x => x.Selling_Price).Select(x => x.Selling_Price).FirstOrDefault();
            ViewBag.MaxSelectedPrice = max == 0 ? result.MaxPrice : max;
            ViewBag.MaxPrice = result.MaxPrice;
            return View(result);
        }

        [HttpPost("hotdeals")]
        public async Task<PartialViewResult> HotDeals([FromBody] Search_Query obj)
        {
            string query = Regex.Replace(obj.Query, "[^a-zA-Z0-9 ]", "");
            var result = await _search_repo.Search_By_query(query);
            return PartialView("_HotDeals_List_Partial", result.Data);
        }

        [HttpGet("Pages/{url}")]
        public async Task<IActionResult> DynamicPages(string url)
        {
            url = string.IsNullOrEmpty(url) ? string.Empty : url;
            url = Regex.Replace(url.Trim(), "[^a-zA-Z0-9]", "-");
            var pageContent = await _page_repo.Get_By_Url(url);
            if (pageContent == null) { return NotFound(); }
            return View(pageContent);
        }

        [HttpGet("About-us")]
        public async Task<IActionResult> About()
        {
            var pageContent = await _page_repo.Get_By_Url("about-us");
            if (pageContent == null) { return NotFound(); }
            return View(pageContent);
        }

        [HttpGet("Contact-us")]
        public async Task<IActionResult> Contact()
        {
            var pageContent = await _page_repo.Get_By_Url("contact-us");
            if (pageContent == null) { return NotFound(); }
            return View(pageContent);
        }

        [HttpGet("Terms-Of-Use")]
        public async Task<IActionResult> TermsOfUse()
        {
            var pageContent = await _page_repo.Get_By_Url("term-of-use");
            if (pageContent == null) { return NotFound(); }
            return View(pageContent);
        }

        [HttpGet("Privacy-Policy")]
        public async Task<IActionResult> PrivacyPolicy()
        {
            var pageContent = await _page_repo.Get_By_Url("privacy-policy");
            if (pageContent == null) { return NotFound(); }
            return View(pageContent);
        }

        [HttpGet("Pre-Owned-Guarantee")]
        public async Task<IActionResult> PreOwnedGameGarantee()
        {
            var pageContent = await _page_repo.Get_By_Url("pre-owned-guarantee");
            if (pageContent == null) { return NotFound(); }
            return View(pageContent);
        }

        [HttpGet("Return-And-Refund-Policy")]
        public async Task<IActionResult> ReturnAndRefundPolicy()
        {
            var pageContent = await _page_repo.Get_By_Url("return-and-refund-policy");
            if (pageContent == null) { return NotFound(); }
            return View(pageContent);
        }

        [HttpGet("Shipping-Policy")]
        public async Task<IActionResult> ShippingPolicy()
        {
            var pageContent = await _page_repo.Get_By_Url("shipping-policy");
            if (pageContent == null) { return NotFound(); }
            return View(pageContent);
        }

        [HttpGet("Refer-and-Earn")]
        public async Task<IActionResult> ReferAndEarn()
        {
            var pageContent = await _page_repo.Get_By_Url("refer-and-earn");
            if (pageContent == null) { return NotFound(); }
            return View(pageContent);
        }

        [HttpGet("FAQ")]
        public async Task<IActionResult> FAQ()
        {
            var pageContent = await _page_repo.Get_By_Url("faq");
            if (pageContent == null) { return NotFound(); }
            return View(pageContent);
        }

        [HttpGet("Career")]
        public async Task<IActionResult> Career()
        {
            var pageContent = await _page_repo.Get_By_Url("career");
            if (pageContent == null) { return NotFound(); }
            return View(pageContent);
        }

        [Route("/Error/{ErrorCode?}")]
        public IActionResult Error(int ErrorCode = 500)
        {
            return View(ErrorCode);

        }

        [HttpPost("Add-New-Contact")]
        public async Task<PartialViewResult> Add_New_Contact(Contact obj)
        {
            int otp = (new Random()).Next(10000, 99999);
            obj.OTP = otp;
            var result = await _cont_repo.Add_New(obj);
            if (result.Id > 0)
            {
                dynamic templeteObj = new System.Dynamic.ExpandoObject();
                templeteObj.Name = obj.Name;
                templeteObj.Email = obj.Email;
                templeteObj.Contact_For = obj.Contact_For;
                templeteObj.Message = obj.Message;
                templeteObj.Phone_No = obj.Phone_No;


                var Templete = _emailSender.GetMailTemplate(templeteObj, "enquiry.cshtml");
                var mailResult = await _emailSender.SendEmailAsync(obj.Name, "support@gamestoreme.com", "We Got New Enquiry", Templete);
                //var mailResult = await _emailSender.SendEmailAsync(obj.Name, "neeraj@vervelogic.com", "We Got New Enquiry", Templete);
            }
            string msg = "Thank you for contacting us. We will get back to you sortly.";
            return PartialView("Info", msg);
        }

        [HttpPost("Career/Apply-Now")]
        public async Task<PartialViewResult> Add_New_Career(Career obj)
        {
            int otp = (new Random()).Next(10000, 99999);
            obj.OTP = otp;
            var result = await _car_repo.Add_New(obj);
            string msg = "Thank you. We will get back to you sortly.";
            return PartialView("Info", msg);
        }

        [HttpPost("UploadFile")]
        public JsonResult UploadFile()
        {
            string fileName = System.Guid.NewGuid().ToString();
            foreach (var file in Request.Form.Files)
            {
                fileName += file.FileName.Substring(file.FileName.LastIndexOf("."));
                string folderPath = string.Format(@"{0}\resources\common", _HostingEnv.WebRootPath);
                if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
                string filePath = string.Format(@"{0}\{1}", folderPath, fileName);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            return Json($"/resources/common/{fileName}");
        }
    }
}
