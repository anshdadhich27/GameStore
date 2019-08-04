using GameStore.Models;
using GameStore.Models.Entity;
using GameStore.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;

namespace GameStore.Controllers
{

    public class ProductsController : BaseController
    {
        private readonly IGamePlatformRepository _gp_repo = null;
        private readonly IGenereRepository _gr_repo = null;
        private readonly IGameRepository _game_repo = null;
        private readonly IImageRepository _img_repo = null;
        private readonly IPageRepository _page_repo = null;
        private readonly IProductRepository _prod_repo = null;


        public ProductsController(
             IGamePlatformRepository gp_repo,
             IGenereRepository gr_repo,
             IImageRepository img_repo,
             IGameRepository game_repo,
             IPageRepository page_repo,
             IProductRepository prod_repo)
        {
            _gp_repo = gp_repo;
            _gr_repo = gr_repo;
            _img_repo = img_repo;
            _game_repo = game_repo;
            _page_repo = page_repo;
            _prod_repo = prod_repo;

        }



        [HttpGet("Game/{url}")]
        public async Task<IActionResult> GameDetails(string url)
        {
            url = Regex.Replace(url, "[^a-zA-Z0-9]", "-");
            var gameDetails = await _game_repo.Get_Details_By_Url(url);
            if (gameDetails.Game == null) { return NotFound(); }

            return RedirectToLocal(gameDetails.Game.PageUrl);
        }

        [HttpGet("Game/{platformUrl}/{url}/{c?}")]
        public async Task<IActionResult> GameDetails(string platformUrl, string url, string c = "new")
        {
            url = Regex.Replace(url, "[^a-zA-Z0-9]", "-");
            platformUrl = Regex.Replace(platformUrl, "[^a-zA-Z0-9]", "-");
            int condition = string.IsNullOrEmpty(c) ? 1 : c.ToUpper().Equals("PRE-OWNED") ? 2 : 1;
            var gameDetails = await _game_repo.Get_Details_By_Url_Platform_n_Condition(url, platformUrl, condition);
            if (gameDetails.Game == null) { return NotFound(); }


            var platform = await _gp_repo.GetByUrlAsync(platformUrl);
            if (platform == null) { return NotFound(); }
            if (!platform.Active) { return NotFound(); }

            gameDetails.Game.Platform = platform;


            string platform_ids = gameDetails.Game.Platforms;
            if (!string.IsNullOrEmpty(platform_ids))
            {
                platform_ids = platform_ids.Contains(",") ? platform_ids.Trim(',') : platform_ids;
                if (platform_ids.Length > 0)
                {
                    gameDetails.Game.SupportedPlatforms = await _gp_repo.Get_Supported_N_Platforms(100, platform_ids);
                }
            }

            string genre_ids = gameDetails.Game.Genres;
            if (!string.IsNullOrEmpty(genre_ids))
            {
                genre_ids = genre_ids.Contains(",") ? genre_ids.Trim(',') : genre_ids;
                if (genre_ids.Length > 0)
                {
                    gameDetails.Game.GenereList = await _gr_repo.Get_Supported_N_Genere(1, genre_ids);
                    gameDetails.Game.Genre = gameDetails.Game.GenereList.FirstOrDefault();
                }

            }

            long platformId = (gameDetails.Game.Platform != null) ? gameDetails.Game.Platform.Id : 0;
            gameDetails.Similar_Games = await _game_repo.GetPaginated_By_PlatformId_ConditionId_From_View(0, 12, platformId, condition);
            foreach (var obj in gameDetails.Similar_Games.PagedSet)
            {
                obj.Platform = platform;
            }

            long genreId = (gameDetails.Game.Genre != null) ? gameDetails.Game.Genre.Id : 0;
            gameDetails.Recommended_Games = await _game_repo.GetPaginated_By_PlatformId_GenreId_ConditionId_From_View(0, 12, platformId, genreId, condition);
            foreach (var obj in gameDetails.Recommended_Games.PagedSet)
            {
                obj.Platform = platform;
            }

            return View(gameDetails);
        }

        [HttpGet("Console")]
        public async Task<IActionResult> ConsolePage(string rd, string r, string c, string t, string order, int page = 1, int min = 0, int max = 0)
        {
            var pageContent = await _page_repo.Get_By_Url("console");
            if (pageContent == null) { return NotFound(); }

            ViewBag.PageIndex = page;
            ViewBag.Release_Date = rd;
            ViewBag.TypeGame = t;
            ViewBag.Condition = c;
            ViewBag.Ratings = r;
            ViewBag.MinPrice = min;
            ViewBag.orderby = order;
            int Offset = (page - 1) * 20;
            string where_clouse = string.Empty;
            string where_withoutprice_clouse = string.Empty;
            string productType = Models.Constant.CONSOLE;

            #region Release Date Filters
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
                        if (index == 0)
                        {
                            where_clouse += string.Format(" AND ( t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                            where_withoutprice_clouse += string.Format(" AND ( t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                        }
                        if (index > 0)
                        {
                            where_clouse += string.Format(" OR t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                            where_withoutprice_clouse += string.Format(" OR t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                        }
                        if (index == rds.Count - 1)
                        {
                            where_clouse += " ) ";
                            where_withoutprice_clouse += " ) ";
                        }
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
                    where_clouse += string.Format(" AND t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                    where_withoutprice_clouse += string.Format(" AND t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                }
            }
            #endregion

            #region Condition Filters
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
                        if (index == 0) { where_clouse += string.Format(" AND ( t1.Condition_Id={0} ", id); where_withoutprice_clouse += string.Format(" AND ( t1.Condition_Id={0} ", id); }
                        if (index > 0) { where_clouse += string.Format(" OR t1.Condition_Id={0} ", id); where_withoutprice_clouse += string.Format(" OR t1.Condition_Id={0} ", id); }
                        if (index == cs.Count - 1) { where_clouse += " ) "; where_withoutprice_clouse += " ) "; }
                    }
                }
                else
                {
                    switch (c)
                    {
                        case "new":
                            where_clouse += string.Format(" AND t1.Condition_Id={0} ", 1);
                            where_withoutprice_clouse += string.Format(" AND t1.Condition_Id={0} ", 1);
                            break;
                        case "pre-owned":
                            where_clouse += string.Format(" AND t1.Condition_Id={0} ", 2);
                            where_withoutprice_clouse += string.Format(" AND t1.Condition_Id={0} ", 2);
                            break;
                    }

                }
            }
            #endregion

            #region Price Filters            
            if (min > 0)
            {
                where_clouse += string.Format(" AND t1.Selling_Price >= {0} ", min);
            }
            if (max > 0)
            {
                where_clouse += string.Format(" AND t1.Selling_Price <= {0} ", max);
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
                        if (index == 0) { where_clouse += string.Format(" AND (( t1.Rating >={0}) ", rate); where_withoutprice_clouse += string.Format(" AND (( t1.Rating >={0}) ", rate); }
                        if (index > 0) { where_clouse += string.Format(" OR ( t1.Rating >={0}) ", rate); where_withoutprice_clouse += string.Format(" OR ( t1.Rating >={0}) ", rate); }
                        if (index == rs.Count - 1) { where_clouse += " ) "; where_withoutprice_clouse += " ) "; }
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
                    where_clouse += string.Format(" AND ( t1.Rating >={0}) ", rate);
                    where_withoutprice_clouse += string.Format(" AND ( t1.Rating >={0}) ", rate);
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

            #region Query Builder
            string query = string.Empty;
            query += " SELECT COUNT(t1.Id) as Total FROM [dbo].[ProductsTbl] t1 ";
            query += " WHERE t1.Id > 0 ";
            query += string.Format(" AND t1.ProductType='{0}' ", productType);
            query += where_clouse;
            query += "; ";


            query += " SELECT t1.*, ";
            query += " (select TOP(1) [Url] from ImagesTbl ";
            query += string.Format(" where Reference_Id=t1.Id AND ImageOf='{0}' ", productType);
            query += " AND ImageType='Thumbnail' AND ImageCategory='Cover') as ImageUrl, ";
            query += " t2.Name as ProductTypeName, ";
            query += " t3.Name as 'Condition', ";
            query += " t4.Name as 'Platform' ";

            query += " FROM [ProductsTbl] t1 ";
            query += " LEFT JOIN [Common_Name_Url_Tbl] t2 on t1.ProductTypeId=t2.Id ";
            query += " LEFT JOIN ConditionTbl t3 on t1.Condition_Id=t3.Id ";
            query += " LEFT JOIN GamePlatformsTbl t4 on t1.Platform_Code=t4.Code ";
            query += " WHERE t1.Id > 0 ";
            query += string.Format(" AND t1.ProductType='{0}' ", productType);
            query += where_clouse;

            if (string.IsNullOrEmpty(order))
            {
                //query += " ORDER by NEWID() ";
            }
            else
                query += " ORDER by " + orderby;


            query += string.Format(" OFFSET {0} ROWS ", Offset);
            query += string.Format(" FETCH NEXT {0} ROWS ONLY ", 30);
            query += "; ";
            query += " select isnull(Max(Selling_price),0) as [MaxPrice] from[ProductsTbl] t1  where ID>0 AND t1.ProductType='" + productType + "' " + where_withoutprice_clouse;
            #endregion


            var productList = await _prod_repo.GetPaginated_By_Custom_QueryAsync(query);
            ViewBag.MaxSelectedPrice = max == 0 ? productList.MaxPrice : max;
            ViewBag.MaxPrice = productList.MaxPrice;
            ViewBag.ProductList = productList;
            return View(pageContent);
        }

        [HttpGet("Console/{url}/{condition?}")]
        public async Task<IActionResult> ConsoleDetails(string url, string condition = "new")
        {
            url = Regex.Replace(url, "[^a-zA-Z0-9]", "-");
            string cns = Models.Constant.ImageCategory.ConsoleImage;
            var result = await _prod_repo.Get_By_Url_Type_Condition(url, cns, condition);
            if (result == null) { return NotFound(); }
            return View(result);
        }


        [HttpGet("Accessories")]
        public async Task<IActionResult> AccessoriesPage(string rd, string r, string c, string t, string order, int page = 1, int min = 0, int max = 0)
        {
            var pageContent = await _page_repo.Get_By_Url("accessories");
            if (pageContent == null) { return NotFound(); }

            ViewBag.PageIndex = page;
            ViewBag.Release_Date = rd;
            ViewBag.TypeGame = t;
            ViewBag.Condition = c;
            ViewBag.Ratings = r;
            ViewBag.MinPrice = min;
            ViewBag.orderby = order;
            int Offset = (page - 1) * 20;
            string where_clouse = string.Empty;
            string where_withoutprice_clouse = string.Empty;
            string productType = Models.Constant.ACCESSORIES;

            #region Release Date Filters
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
                        if (index == 0) { where_clouse += string.Format(" AND ( t1.Added_On > dateadd(day, {0}, getdate()) ", days); where_withoutprice_clouse += string.Format(" AND ( t1.Added_On > dateadd(day, {0}, getdate()) ", days); }
                        if (index > 0) { where_clouse += string.Format(" OR t1.Added_On > dateadd(day, {0}, getdate()) ", days); where_withoutprice_clouse += string.Format(" OR t1.Added_On > dateadd(day, {0}, getdate()) ", days); }
                        if (index == rds.Count - 1) { where_clouse += " ) "; where_withoutprice_clouse += " ) "; }
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
                    where_clouse += string.Format(" AND t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                    where_withoutprice_clouse += string.Format(" AND t1.Added_On > dateadd(day, {0}, getdate()) ", days);
                }
            }
            #endregion

            #region Condition Filters
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
                        if (index == 0) { where_clouse += string.Format(" AND ( t1.Condition_Id={0} ", id); where_withoutprice_clouse += string.Format(" AND ( t1.Condition_Id={0} ", id); }
                        if (index > 0) { where_clouse += string.Format(" OR t1.Condition_Id={0} ", id); where_withoutprice_clouse += string.Format(" OR t1.Condition_Id={0} ", id); }
                        if (index == cs.Count - 1) { where_clouse += " ) "; where_withoutprice_clouse += " ) "; }

                    }
                }
                else
                {
                    switch (c)
                    {
                        case "new":
                            where_clouse += string.Format(" AND t1.Condition_Id={0} ", 1);
                            where_withoutprice_clouse += string.Format(" AND t1.Condition_Id={0} ", 1);
                            break;
                        case "pre-owned":
                            where_clouse += string.Format(" AND t1.Condition_Id={0} ", 2);
                            where_withoutprice_clouse += string.Format(" AND t1.Condition_Id={0} ", 2);
                            break;
                    }
                }
            }
            #endregion

            #region Price Filters            
            if (min > 0)
            {
                where_clouse += string.Format(" AND t1.Selling_Price >= {0} ", min);
            }
            if (max > 0)
            {
                where_clouse += string.Format(" AND t1.Selling_Price <= {0} ", max);
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
                        if (index == 0) { where_clouse += string.Format(" AND (( t1.Rating >={0}) ", rate); where_withoutprice_clouse += string.Format(" AND (( t1.Rating >={0}) ", rate); }
                        if (index > 0) { where_clouse += string.Format(" OR ( t1.Rating >={0}) ", rate); where_withoutprice_clouse += string.Format(" OR ( t1.Rating >={0}) ", rate); }
                        if (index == rs.Count - 1) { where_clouse += " ) "; where_withoutprice_clouse += " ) "; }
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
                    where_clouse += string.Format(" AND ( t1.Rating >={0}) ", rate);
                    where_withoutprice_clouse += string.Format(" AND ( t1.Rating >={0}) ", rate);
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


            #region Query Builder
            string query = string.Empty;
            query += " SELECT COUNT(t1.Id) as Total FROM [dbo].[ProductsTbl] t1 ";
            query += " WHERE t1.Id > 0 ";
            query += string.Format(" AND t1.ProductType='{0}' ", productType);
            query += where_clouse;
            query += "; ";


            query += " SELECT t1.*, ";
            query += " (select TOP(1) [Url] from ImagesTbl ";
            query += string.Format(" where Reference_Id=t1.Id AND ImageOf='{0}' ", productType);
            query += " AND ImageType='Thumbnail' AND ImageCategory='Cover') as ImageUrl, ";
            query += " t2.Name as ProductTypeName, ";
            query += " t3.Name as 'Condition', ";
            query += " t4.Name as 'Platform' ";
            query += " FROM [ProductsTbl] t1 ";
            query += " LEFT JOIN [Common_Name_Url_Tbl] t2 on t1.ProductTypeId=t2.Id ";
            query += " LEFT JOIN ConditionTbl t3 on t1.Condition_Id=t3.Id ";
            query += " LEFT JOIN GamePlatformsTbl t4 on t1.Platform_Code=t4.Code ";
            query += " WHERE t1.Id > 0 ";
            query += string.Format(" AND t1.ProductType='{0}' ", productType);
            query += where_clouse;

            if (!string.IsNullOrEmpty(order))
            {
                //query += " ORDER by NEWID() ";
            }
            else
                query += " ORDER by " + orderby;

            query += string.Format(" OFFSET {0} ROWS ", Offset);
            query += string.Format(" FETCH NEXT {0} ROWS ONLY ", 30);
            query += "; ";
            query += " select isnull(Max(Selling_price),0) as [MaxPrice] from[ProductsTbl] t1  where ID>0 AND ProductType='" + productType + "' " + where_withoutprice_clouse;
            #endregion


            var productList = await _prod_repo.GetPaginated_By_Custom_QueryAsync(query);
            //ViewBag.MaxPrice = productList.PagedSet.OrderByDescending(x => x.Selling_Price).Select(x => x.Selling_Price).FirstOrDefault();
            ViewBag.MaxSelectedPrice = max == 0 ? productList.MaxPrice : max;
            ViewBag.MaxPrice = productList.MaxPrice;
            ViewBag.ProductList = productList;
            return View(pageContent);
        }


        [HttpGet("Accessories/{url}/{condition?}")]
        public async Task<IActionResult> AccessoriesDetails(string url, string condition = "new")
        {
            url = Regex.Replace(url, "[^a-zA-Z0-9]", "-");
            string cns = Models.Constant.ImageCategory.AccessoriesImage;
            var result = await _prod_repo.Get_By_Url_Type_Condition(url, cns, condition);
            if (result == null) { return NotFound(); }
            return View(result);
        }


        /// <summary>
        /// Get Game List by Plateform Url & query
        /// </summary>
        /// <param name="url">Platform Url</param>
        /// <param name="g">Genre</param>
        /// <returns>Game Listing Page</returns>
        [HttpGet("platform/{url}")]
        public async Task<IActionResult> ListingPage1(string url, string g, string rd, string r, string c, string t, string order, int page = 1, int min = 0, int max = 0, int pegi = 0)
        {
            url = Regex.Replace(url, "[^a-zA-Z0-9]", "-");
            var platform = await _gp_repo.GetByUrlAsync(url);
            if (platform == null) { return NotFound(); }
            if (!platform.Active) { return NotFound(); }

            var genreList = await _gr_repo.Get_ALL_ActiveAsync();
            ViewBag.GenreList = genreList;
            ViewBag.GenreUrl = g;
            ViewBag.PageIndex = page;
            ViewBag.Release_Date = rd;
            ViewBag.TypeGame = t;
            ViewBag.Condition = c;
            ViewBag.Ratings = r;
            ViewBag.MinPrice = min;
            ViewBag.orderby = order;

            Game_Filter obj = new Game_Filter { Offset = 0, PageIndex = page, Rows = 20, Platform_Id = platform.Id };
            obj.Offset = (page - 1) * 20;
            string where_clouse = string.Empty;
            string where_withoutprice_clouse = string.Empty;
            if (obj.Platform_Id > 0) { where_clouse += string.Format(" AND t1.PlatformId={0} ", obj.Platform_Id); where_withoutprice_clouse += string.Format(" AND t1.PlatformId={0} ", obj.Platform_Id); }

            #region Genere Filters
            if (!string.IsNullOrEmpty(g))
            {
                string g_url = string.Empty;
                Models.Entity.Genere genre = null;
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
                            if (index == 0) { where_clouse += string.Format(" AND ( t1.Genres LIKE '%,{0},%' ", genre.Id); where_withoutprice_clouse += string.Format(" AND ( t1.Genres LIKE '%,{0},%' ", genre.Id); }
                            if (index > 0) { where_clouse += string.Format(" OR t1.Genres LIKE '%,{0},%' ", genre.Id); where_withoutprice_clouse += string.Format(" OR t1.Genres LIKE '%,{0},%' ", genre.Id); }
                            if (index == grs.Count - 1) { where_clouse += " ) "; where_withoutprice_clouse += " ) "; }
                        }
                    }
                }
                else
                {
                    g_url = Regex.Replace(g, "[^a-zA-Z0-9]", "-");
                    genre = genreList.FirstOrDefault(x => x.NameUrl.Equals(g_url, System.StringComparison.CurrentCultureIgnoreCase));
                    if (genre != null)
                    {
                        where_clouse += string.Format(" AND t1.Genres LIKE '%,{0},%' ", genre.Id);
                        where_withoutprice_clouse += string.Format(" AND t1.Genres LIKE '%,{0},%' ", genre.Id);
                    }
                }
            }
            #endregion

            #region Release Date Filters
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
                        if (index == 0) { where_clouse += string.Format(" AND ( t1.First_release_date > dateadd(day, {0}, getdate()) ", days); where_withoutprice_clouse += string.Format(" AND ( t1.First_release_date > dateadd(day, {0}, getdate()) ", days); }
                        if (index > 0) { where_clouse += string.Format(" OR t1.First_release_date > dateadd(day, {0}, getdate()) ", days); where_withoutprice_clouse += string.Format(" OR t1.First_release_date > dateadd(day, {0}, getdate()) ", days); }
                        if (index == rds.Count - 1) { where_clouse += " ) "; where_withoutprice_clouse += " ) "; }
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
                    where_clouse += string.Format(" AND t1.First_release_date > dateadd(day, {0}, getdate()) ", days);
                    where_withoutprice_clouse += string.Format(" AND t1.First_release_date > dateadd(day, {0}, getdate()) ", days);
                }
            }
            #endregion

            #region Condition Filters
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
                        if (index == 0) { where_clouse += string.Format(" AND ( t1.Condition_Id={0} ", id); where_withoutprice_clouse += string.Format(" AND ( t1.Condition_Id={0} ", id); }
                        if (index > 0) { where_clouse += string.Format(" OR t1.Condition_Id={0} ", id); where_withoutprice_clouse += string.Format(" OR t1.Condition_Id={0} ", id); }
                        if (index == cs.Count - 1) { where_clouse += " ) "; where_withoutprice_clouse += " ) "; }
                    }
                }
                else
                {
                    switch (c)
                    {
                        case "new":
                            where_clouse += string.Format(" AND t1.Condition_Id={0} ", 1);
                            where_withoutprice_clouse += string.Format(" AND t1.Condition_Id={0} ", 1);
                            break;
                        case "pre-owned":
                            where_clouse += string.Format(" AND t1.Condition_Id={0} ", 2);
                            where_withoutprice_clouse += string.Format(" AND t1.Condition_Id={0} ", 2);
                            break;
                    }
                }
            }
            #endregion

            #region Price Filters            
            if (min > 0)
            {
                where_clouse += string.Format(" AND t1.Selling_Price >= {0} ", min);
            }
            if (max > 0)
            {
                where_clouse += string.Format(" AND t1.Selling_Price <= {0} ", max);
            }
            #endregion

            #region PEGI Filters            
            if (pegi > 0)
            {
                where_clouse += string.Format(" AND t1.PEGI_Rating = {0} ", pegi);
                where_withoutprice_clouse += string.Format(" AND t1.PEGI_Rating = {0} ", pegi);
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
                        if (index == 0) { where_clouse += string.Format(" AND ( ( t1.Rating >={0}) ", rate); where_withoutprice_clouse += string.Format(" AND ( ( t1.Rating >={0}) ", rate); }
                        if (index > 0) { where_clouse += string.Format(" OR ( t1.Rating >={0}) ", rate); where_withoutprice_clouse += string.Format(" OR ( t1.Rating >={0}) ", rate); }
                        if (index == rs.Count - 1) { where_clouse += " ) "; where_withoutprice_clouse += " ) "; }
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
                    where_clouse += string.Format(" AND ( t1.Rating >={0} and t1.Rating <({0}+1)) ", rate);
                    where_withoutprice_clouse += string.Format(" AND ( t1.Rating >={0} and t1.Rating <({0}+1)) ", rate);
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

            #region Query Builder
            string query = string.Empty;
            query += " SELECT COUNT(t1.Id) as Total FROM [dbo].[Game_List_View_For_Display] t1 ";
            query += " WHERE t1.Id > 0 ";
            query += where_clouse;
            query += "; ";


            query += " SELECT t1.* ";
            query += " FROM [dbo].[Game_List_View_For_Display] t1 ";
            query += " WHERE t1.Id > 0 ";
            query += where_clouse;
            //query += " ORDER BY t1.First_release_date DESC, t1.Popularity DESC ";
            if (!string.IsNullOrEmpty(order))
                query += " ORDER by " + orderby;
            else
                query += " ORDER BY t1.First_release_date DESC, t1.Popularity DESC ";

            query += string.Format(" OFFSET {0} ROWS ", obj.Offset);
            query += string.Format(" FETCH NEXT {0} ROWS ONLY ", obj.Rows);
            query += "; ";
            query += " SELECT isnull(Max(Selling_price),0) as [MaxPrice] ";
            query += " FROM [dbo].[Game_List_View_For_Display] t1 ";
            query += " WHERE t1.Id > 0 ";
            query += where_withoutprice_clouse;
            #endregion


            platform.GameList = await _game_repo.GetPaginated_By_Custom_QueryAsync(query);
            foreach (var x in platform.GameList.PagedSet)
            {
                x.Platform = platform;
            }
            //ViewBag.MaxPrice = platform.GameList.PagedSet.OrderByDescending(x => x.Selling_Price).Select(x => x.Selling_Price).FirstOrDefault();
            ViewBag.MaxSelectedPrice = max == 0 ? platform.GameList.MaxPrice : max;
            ViewBag.MaxPrice = platform.GameList.MaxPrice;
            return View("ListingPage", platform);
        }

        [HttpGet("platform/{platformUrl}/{genereUrl}")]
        public async Task<IActionResult> ListingPage2(string platformUrl, string genereUrl)
        {
            var platform = await _gp_repo.GetByUrlAsync(platformUrl);
            if (platform == null) { return NotFound(); }
            if (!platform.Active) { return NotFound(); }

            var genere = await _gr_repo.GetByUrlAsync(genereUrl);
            if (genere == null) { return NotFound(); }
            if (!genere.Active) { return NotFound(); }

            platform.GameList = await _game_repo.GetPaginated_By_PlatformId_GenreId(0, 20, platform.Id, genere.Id);
            foreach (var obj in platform.GameList.PagedSet)
            {
                obj.Platform = platform;
            }

            ViewBag.GenreList = await _gr_repo.Get_ALL_ActiveAsync();
            ViewBag.Genre = genere;

            return View("ListingPage", platform);
        }

        [HttpGet("Get-More-Game-List/{game_offset}/{platformUrl}")]
        [HttpGet("Get-More-Game-List/{game_offset}/{platformUrl}/{genereUrl}")]
        public async Task<PartialViewResult> GetMoreGameList(long game_offset, string platformUrl, string genereUrl)
        {
            var platform = await _gp_repo.GetByUrlAsync(platformUrl);
            if (string.IsNullOrEmpty(genereUrl))
            {
                platform.GameList = await _game_repo.GetPaginated_By_PlatformId(game_offset, 20, platform.Id);
            }
            else
            {
                var genere = await _gr_repo.GetByUrlAsync(genereUrl);
                platform.GameList = await _game_repo.GetPaginated_By_PlatformId_GenreId(game_offset, 20, platform.Id, genere.Id);
            }

            foreach (var obj in platform.GameList.PagedSet)
            {
                obj.Platform = platform;
            }
            return PartialView("_Game_List_Partial", platform);
        }

    }
}
