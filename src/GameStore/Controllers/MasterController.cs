using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameStore.Models.Interface;
using GameStore.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using GameStore.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Text;
using System.ComponentModel;
using GameStore.Models.ViewModel;
using GameStore.Models.DataProvider;

namespace GameStore.Controllers
{
    [Route("Master")]
    [Authorize(ActiveAuthenticationSchemes = Constant.Admin_Auth_Scheme)]
    public class MasterController : BaseController
    {
        private static IgdbAPI.ApiClient client = null;
        private IHostingEnvironment _HostingEnv;
        private readonly IPlatformRepository _plat_repo = null;
        private readonly ICollectionRepository _coll_repo = null;
        private readonly IGameEngineRepository _ge_repo = null;
        private readonly IGameModeRepository _gm_repo = null;
        private readonly IKeywordRepository _key_repo = null;
        private readonly IImageRepository _img_repo = null;
        private readonly IGamePlatformRepository _gp_repo = null;
        private readonly IGenereRepository _gr_repo = null;
        private readonly IGameRepository _game_repo = null;
        private readonly IReviewRepository _rev_repo = null;
        private readonly IVideoRepository _vid_repo = null;
        private readonly IPageRepository _page_repo = null;
        private readonly INewsLetterRepository _news_repo = null;
        private readonly ICategoryRepository _cat_repo = null;
        private readonly IGamePlatformMapingRepository _gpm_repo = null;
        private readonly ICommon_Name_UrlRepository _common_repo = null;
        private readonly IPaymentSettingRepository _pay_repo = null;
        private readonly IBannerRepository _banner_repo = null;
        private readonly IAdsRepository _ads_repo = null;
        private readonly IFeatureLinkRepository _fea_repo = null;
        private readonly IVendorRepository _vendor_repo = null;
        private readonly IUserRepository _user_repo = null;

        public MasterController(
            IHostingEnvironment env,
            IPlatformRepository plat_repo,
            ICollectionRepository coll_repo,
            IGameEngineRepository ge_repo,
            IGameModeRepository gm_repo,
            IKeywordRepository key_repo,
            IImageRepository img_repo,
            IGamePlatformRepository gp_repo,
            IGenereRepository gr_repo,
            IGameRepository game_repo,
            IReviewRepository rev_repo,
            IVideoRepository vid_repo,
            IPageRepository page_repo,
            INewsLetterRepository news_repo,
            ICategoryRepository cat_repo,
            IGamePlatformMapingRepository gpm_repo,
            ICommon_Name_UrlRepository common_repo,
            IPaymentSettingRepository pay_repo,
            IBannerRepository banner_repo,
            IAdsRepository ads_repo,
            IFeatureLinkRepository fea_repo,
            IVendorRepository vendor_repo,
            IUserRepository user_repo)
        {
            _HostingEnv = env;
            _plat_repo = plat_repo;
            _coll_repo = coll_repo;
            _ge_repo = ge_repo;
            _gm_repo = gm_repo;
            _key_repo = key_repo;
            _img_repo = img_repo;
            _gp_repo = gp_repo;
            _gr_repo = gr_repo;
            _game_repo = game_repo;
            _rev_repo = rev_repo;
            _vid_repo = vid_repo;
            _gpm_repo = gpm_repo;
            _page_repo = page_repo;
            _news_repo = news_repo;
            _cat_repo = cat_repo;
            _common_repo = common_repo;
            _pay_repo = pay_repo;
            _banner_repo = banner_repo;
            _ads_repo = ads_repo;
            _fea_repo = fea_repo;
            _vendor_repo = vendor_repo;
            _user_repo = user_repo;

            client = new IgdbAPI.ApiClient("https://api-2445582011268.apicast.io/");
            client.SetAuthHeader("user-key", Constant.IgdbUserKey);
        }

        #region Users
        [HttpGet("Users")]
        public IActionResult Users()
        {
            return View();
        }
        [HttpPost("Users")]
        public async Task<IActionResult> Users([FromForm] User usr)
        {
            //LoginViewModel modal = new LoginViewModel();
            var obj = await _user_repo.FindUserByEmailAsync(usr.Email);
            string Password = EncryptionHelper.Decrypt(obj.Data.Password);
            string Email = usr.Email;
            string returnUrl = string.Empty;
            return RedirectToAction("Login", new { Email = Email, Password = Password });
        }
        [HttpGet("Login/{Email}/{Password}")]
        public async Task<IActionResult> Login(string Email, string Password, string returnUrl)
        {
            ViewData["Title"] = "Login";
            var obj = await _user_repo.FindUserByEmail_PasswordAsync(Email, Password);
            if (obj.Success)
            {
                await SignInAsync(obj.Data as User);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, obj.Message);
            }
            return View(null);
        }
        [HttpGet("Get-Paginated-Users/{offset}/{rows}")]
        public async Task<JsonResult> GetPaginated_Users(long offset, int rows)
        {
            var result = await _user_repo.GetPaginated(offset, rows);
            return Json(result);
        }

        [HttpPost("Get-Paginated-Users")]
        public async Task<JsonResult> GetPaginated_Users_By_Search([FromBody] SearchQuery obj)
        {
            var result = await _user_repo.GetPaginated_By_Search(obj);
            return Json(result);
        }
        #endregion

        #region Ads Page
        [HttpGet("Ads")]
        public IActionResult AdPage()
        {
            return View();
        }

        [HttpPost("Add-New-ads")]
        public async Task<JsonResult> Add_New_Ads([FromBody] Ads obj)
        {
            var result = await _ads_repo.Add_New(obj);
            return Json(result);
        }

        [HttpPost("Update-Ads")]
        public async Task<JsonResult> Update_Ads([FromBody] Ads obj)
        {
            var result = await _ads_repo.Update_By_Id(obj);
            return Json(result);
        }

        [HttpGet("Get-All-Ads")]
        public async Task<JsonResult> Get_All_Ads()
        {
            var result = await _ads_repo.Get_All();
            return Json(result);
        }

        [HttpGet("Delete-Ads/{id}")]
        public async Task<JsonResult> Delete_Ads_By_Id(int id)
        {
            var result = await _ads_repo.Delete_By_Id(id);
            return Json(result);
        }

        #endregion

        #region Banners
        [HttpGet("Banners")]
        public IActionResult Banners()
        {
            return View();
        }

        [HttpPost("Add-New-Banner")]
        public async Task<JsonResult> Add_New_Banner([FromBody] Banner obj)
        {
            var result = await _banner_repo.Add_new(obj);
            return Json(result);
        }

        [HttpPost("Update-Banner")]
        public async Task<JsonResult> Update_Banner([FromBody] Banner obj)
        {
            var result = await _banner_repo.Update_By_Id(obj);
            return Json(result);
        }

        [HttpGet("Get-All-Banner")]
        public async Task<JsonResult> Get_All_Banner()
        {
            var result = await _banner_repo.Get_All();
            return Json(result);
        }

        [HttpGet("Delete-Banner/{id}")]
        public async Task<JsonResult> Delete_Banner_By_Id(int id)
        {
            var result = await _banner_repo.Delete_By_Id(id);
            return Json(result);
        }

        #endregion

        #region Payment Settings
        [HttpGet("Payment-Settings")]
        public IActionResult PaymentSettingPage()
        {
            return View();
        }

        [HttpPost("Update-Payment-Setting")]
        public async Task<JsonResult> Update_Payment_Setting([FromBody] PaymentSetting obj)
        {
            var result = await _pay_repo.Update(obj);
            return Json(result);
        }

        [HttpGet("Get-Payment-Setting")]
        public async Task<JsonResult> Get_Payment_Setting()
        {
            var result = await _pay_repo.Get();
            return Json(result);
        }
        #endregion

        #region Common Methods
        [HttpPost("Add-New-Common-Name-Url")]
        public async Task<JsonResult> AddNew_Common_Name_Url([FromBody] Common_Name_Url obj)
        {
            var result = await _common_repo.Add_New(obj);
            return Json(result);
        }

        [HttpPost("Update-Common-Name-Url")]
        public async Task<JsonResult> Update_Common_Name_Url([FromBody] Common_Name_Url obj)
        {
            var result = await _common_repo.Update_By_Id(obj);
            return Json(result);
        }

        [HttpGet("Delete-Common-Name-Url/{id}")]
        public async Task<JsonResult> Delete_Common_Name_Url(int id)
        {
            var result = await _common_repo.Delete_By_Id(id);
            return Json(result);
        }

        [HttpGet("Get-All-Common-Name-Url")]
        public async Task<JsonResult> Get_All_Common_Name_Url()
        {
            var result = await _common_repo.Get_All_Async();
            return Json(result);
        }
        #endregion

        #region Category Pages
        [HttpGet("Category")]
        public IActionResult CategoryPages()
        {
            return View();
        }

        [HttpPost("Add-New-Category")]
        public async Task<JsonResult> AddNew_Category([FromBody] Category obj)
        {
            var result = await _cat_repo.Add_New_Async(obj);
            return Json(result);
        }

        [HttpPost("Update-Category")]
        public async Task<JsonResult> Update_Category([FromBody] Category obj)
        {
            var result = await _cat_repo.Update_By_Id(obj);
            return Json(result);
        }

        [HttpGet("Delete-Category/{id}")]
        public async Task<JsonResult> Delete_Category(int id)
        {
            var result = await _cat_repo.Delete_By_Id(id);
            return Json(result);
        }

        [HttpGet("Get-All-Category")]
        public async Task<JsonResult> Get_All_Category()
        {
            var result = await _cat_repo.Get_All_Async();
            return Json(result);
        }

        #endregion

        #region NewsLetter
        [HttpGet("News-Letter-Subscriber")]
        public IActionResult NewsLetter_Subscriber()
        {
            return View();
        }

        [HttpGet("Delete-News-Letter/{id}")]
        public async Task<JsonResult> Delete_NewsLetter(int id)
        {
            var result = await _news_repo.Delete_By_Id(id);
            return Json(result);
        }

        [HttpGet("GetPaginated-News-Letter/{offset}/{rows}")]
        public async Task<JsonResult> GetPaginated_NewsLetter(long offset, int rows)
        {
            var result = await _news_repo.GetPaginatedAsync(offset, rows);
            return Json(result);
        }

        [HttpPost("Search-News-Letter")]
        public async Task<JsonResult> Search_NewsLetter([FromBody] SearchQuery obj)
        {
            var result = await _news_repo.Search_And_GetPaginatedAsync(obj);
            return Json(result);
        }
        #endregion

        #region Pages
        [HttpGet("Page-Contents")]
        public IActionResult Page_Contents()
        {
            return View();
        }

        [HttpPost("Add-New-Page-Content")]
        public async Task<JsonResult> AddNew_Page_Content([FromBody] PageContent obj)
        {
            var result = await _page_repo.Add_New(obj);
            return Json(result);
        }

        [HttpPost("Update-Page-Content")]
        public async Task<JsonResult> Update_Page_Content([FromBody] PageContent obj)
        {
            var result = await _page_repo.Update_By_Id(obj);
            return Json(result);
        }

        [HttpGet("Delete-Page-Content/{id}")]
        public async Task<JsonResult> Delete_Page_Content(int id)
        {
            var result = await _page_repo.Delete_By_Id(id);
            return Json(result);
        }

        [HttpGet("GetPaginated-Page-Content/{offset}/{rows}")]
        public async Task<JsonResult> GetPaginated_Page_Content_Async(long offset, int rows)
        {
            var result = await _page_repo.GetPaginatedAsync(offset, rows);
            return Json(result);
        }

        [HttpPost("Search-Page-Content")]
        public async Task<JsonResult> Search_Page_Content([FromBody] SearchQuery obj)
        {
            var result = await _page_repo.Search_And_GetPaginatedAsync(obj);
            return Json(result);
        }

        #endregion

        #region Game Modes

        [HttpGet("GameMode")]
        public IActionResult GameModePage()
        {
            return View();
        }

        [HttpGet("GetGameMode_Last_IgdbId")]
        public async Task<JsonResult> GetGameMode_Last_IgdbId()
        {
            var result = await _gm_repo.Get_Last_IdgbId();
            return Json(result);
        }

        [HttpGet("GetGameMode_Count_Igdb")]
        public async Task<JsonResult> GetGameMode_Count_Igdb()
        {
            var result = await client.Get_Game_Mode_Count_Async();
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetGameMode_Count_Igdb")); }
            return Json(result.Data?.count);
        }

        [HttpGet("GetPaginatedGameMode_From_Igdb/{offset}")]
        public async Task<JsonResult> GetPaginatedGameMode_From_Igdb(long offset)
        {
            var result = await client.Get_Paginated_Game_Mode_Async(50, offset);
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetPaginatedGameMode_From_Igdb")); }
            return Json(result.Data);
        }

        [HttpPost("Save_Igdb_GameMode")]
        public async Task<JsonResult> Save_Igdb_GameMode([FromBody] IgdbAPI.Game_Mode obj)
        {
            string games = ",";
            foreach (var x in obj.games) { games += string.Format("{0},", x); }
            var newObj = new GameMode { Games = games, Igdb_Id = obj.id, Name = obj.name, NameUrl = obj.name };
            var result = await _gm_repo.Add_New_IdgbGame_ModeAsync(newObj);
            return Json(result);
        }

        [HttpGet("GetPaginatedGameModeAsync/{offset}/{rows}")]
        public async Task<JsonResult> GetPaginatedGameModeAsync(long offset, int rows)
        {
            var result = await _gm_repo.GetPaginatedAsync(offset, rows);
            return Json(result);
        }

        [HttpPost("SearchGameMode")]
        public async Task<JsonResult> SearchGameMode([FromBody] SearchQuery obj)
        {
            var result = await _gm_repo.Search_And_GetPaginatedAsync(obj);
            return Json(result);
        }


        [HttpPost("AddNewGameMode")]
        public async Task<JsonResult> AddNewGameMode([FromBody] GameMode obj)
        {
            var result = await _gm_repo.Add_NewAsync(obj);
            return Json(result);
        }

        [HttpPost("UpdateGameMode")]
        public async Task<JsonResult> UpdateGameMode([FromBody] GameMode obj)
        {
            var result = await _gm_repo.UpdateAsync(obj);
            return Json(result);
        }

        [HttpGet("DeleteGameMode/{id}")]
        public async Task<JsonResult> DeleteGameMode(long id)
        {
            var result = await _gm_repo.DeleteByIdAsync(id);
            return Json(result);
        }

        #endregion

        #region Game Engine
        [HttpGet("GameEngine")]
        public IActionResult GameEnginePage()
        {
            return View();
        }

        [HttpGet("GetGameEngine_Last_IgdbId")]
        public async Task<JsonResult> GetGameEngine_Last_IgdbId()
        {
            var result = await _ge_repo.Get_Last_IdgbId();
            return Json(result);
        }

        [HttpGet("GetGameEngine_Count_Igdb")]
        public async Task<JsonResult> GetGameEngine_Count_Igdb()
        {
            var result = await client.Get_Game_Engine_Count_Async();
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetGameEngine_Count_Igdb")); }
            return Json(result.Data?.count);
        }

        [HttpGet("GetPaginatedGameEngine_From_Igdb/{offset}")]
        public async Task<JsonResult> GetPaginatedGameEngine_From_Igdb(long offset)
        {
            var result = await client.Get_Paginated_Game_Engine_Async(50, offset);
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetPaginatedGameEngine_From_Igdb")); }
            return Json(result.Data);
        }

        [HttpPost("Save_Igdb_GameEngine")]
        public async Task<JsonResult> Save_Igdb_GameEngine([FromBody] IgdbAPI.Game_Engine obj)
        {
            string games = ",";
            foreach (var x in obj.games) { games += string.Format("{0},", x); }
            string comp = ",";
            foreach (var xx in obj.companies) { comp += string.Format("{0},", xx); }
            string plt = ",";
            foreach (var xx in obj.platforms) { plt += string.Format("{0},", xx); }
            var newObj = new GameEngine { Games = games, Companies = comp, Platforms = plt, Igdb_Id = obj.id, Name = obj.name, NameUrl = obj.name };
            var result = await _ge_repo.Add_New_IdgbGame_EngineAsync(newObj);
            if (result.Success)
            {
                newObj = result.Data as GameEngine;
                if (obj.logo != null)
                {
                    var logo = obj.logo;
                    if (!string.IsNullOrEmpty(logo.cloudinary_id))
                    {
                        var img = new Images { Height = logo.height, Width = logo.width, Reference_Id = newObj.Id };
                        img.ImageOf = Constant.ImageCategory.GameEngineImage;
                        img.ImageCategory = Constant.ImageCategory.ImageType.Logo;
                        var imgResult = await Utility.SaveLogoAndGetUrl(_HostingEnv, _img_repo, img, logo.cloudinary_id);
                    }
                }
            }
            return Json(result);
        }

        [HttpGet("GetPaginatedGameEngineAsync/{offset}/{rows}")]
        public async Task<JsonResult> GetPaginatedGameEngineAsync(long offset, int rows)
        {
            var result = await _ge_repo.GetPaginatedAsync(offset, rows);
            return Json(result);
        }

        [HttpPost("SearchGameEngine")]
        public async Task<JsonResult> SearchGameEngine([FromBody] SearchQuery obj)
        {
            var result = await _ge_repo.Search_And_GetPaginatedAsync(obj);
            return Json(result);
        }


        [HttpPost("AddNewGameEngine")]
        public async Task<JsonResult> AddNewGameEngine([FromBody] GameEngine obj)
        {
            var result = await _ge_repo.Add_NewAsync(obj);
            return Json(result);
        }

        [HttpPost("UpdateGameEngine")]
        public async Task<JsonResult> UpdateGameEngine([FromBody] GameEngine obj)
        {
            var result = await _ge_repo.UpdateAsync(obj);
            return Json(result);
        }

        [HttpGet("DeleteGameEngine/{id}")]
        public async Task<JsonResult> DeleteGameEngine(long id)
        {
            var result = await _ge_repo.DeleteByIdAsync(id);
            return Json(result);
        }
        #endregion

        #region Game Collection
        [HttpGet("GameCollection")]
        public IActionResult GameCollection()
        {
            return View();
        }

        [HttpGet("GetCollection_Last_IgdbId")]
        public async Task<JsonResult> GetCollection_Last_IgdbId()
        {
            var result = await _coll_repo.Get_Last_IdgbId();
            return Json(result);
        }

        [HttpGet("GetCollection_Count_Igdb")]
        public async Task<JsonResult> GetCollection_Count_Igdb()
        {
            var result = await client.Get_Collections_Count_Async();
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetCollection_Count_Igdb")); }
            return Json(result.Data?.count);
        }

        [HttpGet("GetPaginatedCollection_From_Igdb/{offset}")]
        public async Task<JsonResult> GetPaginatedCollection_From_Igdb(long offset)
        {
            var result = await client.Get_Paginated_Collections_Async(50, offset);
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetPaginatedCollection_From_Igdb")); }
            return Json(result);
        }

        [HttpPost("Save_Igdb_Collection")]
        public async Task<JsonResult> Save_Igdb_Collection([FromBody] IgdbAPI.Collection obj)
        {
            string games = ",";
            foreach (var x in obj.games) { games += string.Format("{0},", x); }
            var newObj = new Collection { Games = games, Igdb_Id = obj.id, Name = obj.name, NameUrl = obj.name };
            var result = await _coll_repo.Add_New_IgdbCollectionAsync(newObj);
            return Json(result);
        }

        [HttpGet("GetPaginatedGameCollectionAsync/{offset}/{rows}")]
        public async Task<JsonResult> GetPaginatedGameCollectionAsync(long offset, int rows)
        {
            var result = await _coll_repo.GetPaginatedAsync(offset, rows);
            return Json(result);
        }

        [HttpPost("SearchGameCollection")]
        public async Task<JsonResult> SearchGameCollection([FromBody] SearchQuery obj)
        {
            var result = await _coll_repo.Search_And_GetPaginatedAsync(obj);
            return Json(result);
        }


        [HttpPost("AddNewGameCollection")]
        public async Task<JsonResult> AddNewGameCollection([FromBody] Collection obj)
        {
            var result = await _coll_repo.Add_NewAsync(obj);
            return Json(result);
        }

        [HttpPost("UpdateGameCollection")]
        public async Task<JsonResult> UpdateGameCollection([FromBody] Collection obj)
        {
            var result = await _coll_repo.UpdateAsync(obj);
            return Json(result);
        }

        [HttpGet("DeleteGameCollection/{id}")]
        public async Task<JsonResult> DeleteGameCollection(long id)
        {
            var result = await _coll_repo.DeleteByIdAsync(id);
            return Json(result);
        }
        #endregion

        #region Keywords
        [HttpGet("Keywords")]
        public IActionResult Keywords()
        {
            return View();
        }

        [HttpGet("GetKeyword_Last_IgdbId")]
        public async Task<JsonResult> GetKeyword_Last_IgdbId()
        {
            var result = await _key_repo.Get_Last_IdgbId();
            return Json(result);
        }

        [HttpGet("GetKeyword_Count_Igdb")]
        public async Task<JsonResult> GetKeyword_Count_Igdb()
        {
            var result = await client.Get_Keywords_Count_Async();
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetKeyword_Count_Igdb")); }
            return Json(result.Data?.count);
        }

        [HttpGet("GetPaginatedKeyword_From_Igdb/{offset}")]
        public async Task<JsonResult> GetPaginatedKeyword_From_Igdb(long offset)
        {
            var result = await client.Get_Paginated_Keywords_Async(50, offset);
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetPaginatedKeyword_From_Igdb")); }
            return Json(result.Data);
        }

        [HttpPost("Save_Igdb_Keyword")]
        public async Task<JsonResult> Save_Igdb_Keyword([FromBody] IgdbAPI.Keyword obj)
        {
            string games = ",";
            foreach (var x in obj.games) { games += string.Format("{0},", x); }
            var newObj = new Keyword { Games = games, Igdb_Id = obj.id, Name = obj.name, NameUrl = obj.name };
            var result = await _key_repo.Add_New_IgdbKeywordAsync(newObj);
            return Json(result);
        }

        [HttpGet("GetPaginatedKeywordsAsync/{offset}/{rows}")]
        public async Task<JsonResult> GetPaginatedKeywordsAsync(long offset, int rows)
        {
            var result = await _key_repo.GetPaginatedAsync(offset, rows);
            return Json(result);
        }

        [HttpPost("SearchKeywords")]
        public async Task<JsonResult> SearchKeywords([FromBody] SearchQuery obj)
        {
            var result = await _key_repo.Search_And_GetPaginatedAsync(obj);
            return Json(result);
        }


        [HttpPost("AddNewKeywords")]
        public async Task<JsonResult> AddNewKeywords([FromBody] Keyword obj)
        {
            var result = await _key_repo.Add_NewAsync(obj);
            return Json(result);
        }

        [HttpPost("UpdateKeywords")]
        public async Task<JsonResult> UpdateKeywords([FromBody] Keyword obj)
        {
            var result = await _key_repo.UpdateAsync(obj);
            return Json(result);
        }

        [HttpGet("DeleteKeywords/{id}")]
        public async Task<JsonResult> DeleteKeywords(long id)
        {
            var result = await _key_repo.DeleteByIdAsync(id);
            return Json(result);
        }
        #endregion

        #region Platforms
        [HttpGet("Platforms")]
        private IActionResult Platforms()
        {
            return View();
        }

        [HttpGet("GetPlatform_Last_IgdbId")]
        public async Task<JsonResult> GetPlatform_Last_IgdbId()
        {
            var result = await _plat_repo.Get_Last_IdgbId();
            return Json(result);
        }

        [HttpGet("GetPlatform_Count_Igdb")]
        public async Task<JsonResult> GetPlatform_Count_Igdb()
        {
            var result = await client.Get_PlatformCount_Async();
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetPlatform_Count_Igdb")); }
            return Json(result.Data?.count);
        }

        [HttpGet("GetPaginatedPlatform_From_Igdb/{offset}")]
        public async Task<JsonResult> GetPaginatedPlatform_From_Igdb(long offset)
        {
            var result = await client.Get_Paginated_Platforms_Async(50, offset);
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetPaginatedPlatform_From_Igdb")); }
            return Json(result.Data);
        }

        [HttpPost("Save_Igdb_Platform")]
        public async Task<JsonResult> Save_Igdb_Platform([FromBody] IgdbAPI.Platform platform)
        {
            string games = ",";
            foreach (var x in platform.games) { games += string.Format("{0},", x); }
            var newPlatform = new Platform { Games = games, Alternative_name = platform.alternative_name, Igdb_Id = platform.id, Name = platform.name, NameUrl = platform.name, Summary = platform.summary, Website = platform.website };
            var result = await _plat_repo.Add_New_IdgbPlatformAsync(newPlatform);
            if (result.Success)
            {
                newPlatform = result.Data as Platform;
                if (platform.logo != null)
                {
                    var logo = platform.logo;
                    if (!string.IsNullOrEmpty(logo.cloudinary_id))
                    {
                        var img = new Images { Height = logo.height, Width = logo.width, Reference_Id = newPlatform.Id };
                        img.ImageOf = Constant.ImageCategory.PlatformImage;
                        img.ImageCategory = Constant.ImageCategory.ImageType.Logo;
                        logo.url = await Utility.SaveLogoAndGetUrl(_HostingEnv, _img_repo, img, logo.cloudinary_id);
                    }
                }
            }
            return Json(result);
        }

        [HttpGet("GetPaginatedPlatformsAsync/{offset}/{rows}")]
        public async Task<JsonResult> GetPaginatedPlatformsAsync(long offset, int rows)
        {
            var result = await _plat_repo.GetPaginatedAsync(offset, rows);
            return Json(result);
        }

        [HttpPost("SearchPlatforms")]
        public async Task<JsonResult> SearchPlatforms([FromBody] SearchQuery obj)
        {
            var result = await _plat_repo.Search_And_GetPaginatedAsync(obj);
            return Json(result);
        }


        [HttpPost("AddNewPlatforms")]
        public async Task<JsonResult> AddNewPlatforms([FromBody] Platform obj)
        {
            var result = await _plat_repo.Add_NewAsync(obj);
            return Json(result);
        }

        [HttpPost("UpdatePlatforms")]
        public async Task<JsonResult> UpdatePlatforms([FromBody] Platform obj)
        {
            var result = await _plat_repo.UpdateAsync(obj);
            return Json(result);
        }

        [HttpGet("DeletePlatforms/{id}")]
        public async Task<JsonResult> DeletePlatforms(long id)
        {
            var result = await _plat_repo.DeleteByIdAsync(id);
            return Json(result);
        }
        #endregion

        #region Genere
        [HttpGet("Genere")]
        public IActionResult GenerePage()
        {
            return View();
        }

        [HttpGet("GetGenere_Last_IgdbId")]
        public async Task<JsonResult> GetGenere_Last_IgdbId()
        {
            var result = await _gr_repo.Get_Last_IdgbId();
            return Json(result);
        }

        [HttpGet("GetGenere_Count_Igdb")]
        public async Task<JsonResult> GetGenere_Count_Igdb()
        {
            var result = await client.Get_Genre_Count_Async();
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetGenere_Count_Igdb")); }
            return Json(result.Data?.count);
        }

        [HttpGet("GetPaginatedGenere_From_Igdb/{offset}")]
        public async Task<JsonResult> GetPaginatedGenere_From_Igdb(long offset)
        {
            var result = await client.Get_Paginated_Genre_Async(50, offset);
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetPaginatedGenere_From_Igdb")); }
            return Json(result.Data);
        }

        [HttpPost("Save_Igdb_Genere")]
        public async Task<JsonResult> Save_Igdb_Genere([FromBody] IgdbAPI.Genre obj)
        {
            string games = ",";
            foreach (var x in obj.games) { games += string.Format("{0},", x); }
            var newObj = new Genere { Name = obj.name, NameUrl = obj.name, Games = games, Igdb_Id = obj.id };
            var result = await _gr_repo.Add_New_From_IdgbAsync(newObj);
            return Json(result);
        }

        [HttpGet("GetPaginatedGenereAsync/{offset}/{rows}")]
        public async Task<JsonResult> GetPaginatedGenereAsync(long offset, int rows)
        {
            var result = await _gr_repo.GetPaginatedAsync(offset, rows);
            return Json(result);
        }

        [HttpPost("SearchGenere")]
        public async Task<JsonResult> SearchGenere([FromBody] SearchQuery obj)
        {
            var result = await _gr_repo.Search_And_GetPaginatedAsync(obj);
            return Json(result);
        }


        [HttpPost("AddNewGenere")]
        public async Task<JsonResult> AddNewGenere([FromBody] Genere obj)
        {
            var result = await _gr_repo.Add_NewAsync(obj);
            return Json(result);
        }

        [HttpPost("UpdateGenere")]
        public async Task<JsonResult> UpdateGenere([FromBody] Genere obj)
        {
            var result = await _gr_repo.UpdateAsync(obj);
            return Json(result);
        }

        [HttpGet("DeleteGenere/{id}")]
        public async Task<JsonResult> DeleteGenere(int id)
        {
            var result = await _gr_repo.DeleteByIdAsync(id);
            return Json(result);
        }
        #endregion

        #region Game Platform
        [HttpGet("Game-Platform")]
        public IActionResult GamePlatformPage()
        {
            return View();
        }

        [HttpGet("Get_Platform_List")]
        public async Task<JsonResult> Get_Platform_List()
        {
            var result = await _gp_repo.Get_ALL_ActiveAsync();
            return Json(result);
        }

        [HttpGet("GetGamePlatform_Last_IgdbId")]
        public async Task<JsonResult> GetGamePlatform_Last_IgdbId()
        {
            var result = await _gp_repo.Get_Last_IdgbId();
            return Json(result);
        }

        [HttpGet("GetGamePlatform_Count_Igdb")]
        public async Task<JsonResult> GetGamePlatform_Count_Igdb()
        {
            var result = await client.Get_PlatformCount_Async();
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetGamePlatform_Count_Igdb")); }
            return Json(result.Data?.count);
        }

        [HttpGet("GetPaginated_GamePlatform_From_Igdb/{offset}")]
        public async Task<JsonResult> GetPaginatedGamePlatform_From_Igdb(long offset)
        {
            var result = await client.Get_Paginated_Platforms_Async(50, offset);
            if (!result.Success) { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(new Exception(result.Content), "GetPaginatedGamePlatform_From_Igdb")); }
            return Json(result.Data);
        }

        [HttpPost("Save_Igdb_GamePlatform")]
        public async Task<JsonResult> Save_Igdb_GamePlatform([FromBody] IgdbAPI.Platform obj)
        {
            string games = ",";
            foreach (var x in obj.games) { games += string.Format("{0},", x); }
            var newPlatform = new GamePlatform { Games = games, Alternative_name = obj.alternative_name, Igdb_Id = obj.id, Name = obj.name, NameUrl = obj.name, PageContent = obj.summary, Website = obj.website };
            var result = await _gp_repo.Add_New_From_IdgbAsync(newPlatform);
            if (result.Success)
            {
                newPlatform = result.Data as GamePlatform;
                if (obj.logo != null)
                {
                    var logo = obj.logo;
                    if (!string.IsNullOrEmpty(logo.cloudinary_id))
                    {
                        var img = new Images { Height = logo.height, Width = logo.width, Url = logo.url, Reference_Id = newPlatform.Id };
                        img.ImageOf = Constant.ImageCategory.PlatformImage;
                        img.ImageCategory = Constant.ImageCategory.ImageType.Logo;
                        logo.url = await Utility.SaveLogoAndGetUrl(_HostingEnv, _img_repo, img, logo.cloudinary_id);
                    }
                }
            }
            return Json(result);
        }

        [HttpGet("GetPaginatedGame-PlatformAsync/{offset}/{rows}")]
        public async Task<JsonResult> GetPaginatedGamePlatformAsync(long offset, int rows)
        {
            var result = await _gp_repo.GetPaginatedAsync(offset, rows);
            return Json(result);
        }

        [HttpPost("SearchGame-Platform")]
        public async Task<JsonResult> SearchGamePlatforms([FromBody] SearchQuery obj)
        {
            var result = await _gp_repo.Search_And_GetPaginatedAsync(obj);
            return Json(result);
        }


        [HttpPost("AddNewGame-Platform")]
        public async Task<JsonResult> AddNewGamePlatform([FromBody] GamePlatform obj)
        {
            var result = await _gp_repo.Add_NewAsync(obj);
            if (result.Success)
            {
                if (obj.FeatureLinks.Count() > 0)
                {
                    var p = result.Data as GamePlatform;
                    IList<FeatureLink> list = new List<FeatureLink>();
                    foreach (var x in obj.FeatureLinks)
                    {
                        var feature = new FeatureLink { AddedOn = x.AddedOn, Id = x.Id, Link = x.Link, Name = x.Name };
                        feature.Reference_Id = p.Id;
                        feature.Reference_Name = Constant.ImageCategory.PlatformImage;
                        list.Add(feature);
                    }
                    var f_result = await _fea_repo.Add_New(list);
                }
            }
            return Json(result);
        }

        [HttpPost("UpdateGame-Platform")]
        public async Task<JsonResult> UpdateGamePlatform([FromBody] GamePlatform obj)
        {
            var result = await _gp_repo.UpdateAsync(obj);
            if (result.Success)
            {
                if (obj.FeatureLinks.Count() > 0)
                {
                    var p = result.Data as GamePlatform;
                    IList<FeatureLink> list = new List<FeatureLink>();
                    foreach (var x in obj.FeatureLinks)
                    {
                        var feature = new FeatureLink { AddedOn = x.AddedOn, Id = x.Id, Link = x.Link, Name = x.Name };
                        feature.Reference_Id = p.Id;
                        feature.Reference_Name = Constant.ImageCategory.PlatformImage;
                        list.Add(feature);
                    }
                    var f_result = await _fea_repo.Add_New(list);
                }
            }
            return Json(result);
        }

        [HttpGet("DeleteGame-Platform/{id}")]
        public async Task<JsonResult> DeleteGamePlatform(int id)
        {
            await _fea_repo.Delete_By_Reference(id, Constant.ImageCategory.PlatformImage);
            var result = await _gp_repo.DeleteByIdAsync(id);
            return Json(result);
        }

        [HttpGet("Get-All-Feature-Links")]
        public async Task<JsonResult> Get_All_Feature_Links()
        {
            var result = await _fea_repo.Get_All();
            return Json(result);
        }
        #endregion

        #region Games
        [HttpGet("Games")]
        public IActionResult GamePage()
        {
            return View();
        }

        [HttpGet("New-Game")]
        public IActionResult Add_New_Game()
        {
            return View();
        }

        [HttpPost("Add-New-Game")]
        public async Task<JsonResult> Add_New_Game_Details([FromBody] Game obj)
        {
            var result = await _game_repo.Add_NewGame_ManuallyAsync(obj);
            return Json(result);
        }

        [HttpPost("Save-Game-Details")]
        public async Task<JsonResult> Save_Game_Details([FromBody] Game_DTO obj)
        {
            var maping_result = await _gpm_repo.Add_New_Mapings(obj.GamePlatformMapings);
            var vid_result = await _vid_repo.Add_or_Update_Multiple(obj.VideoList);
            var img_result = await _img_repo.Add_New_ImagesAsync(obj.ImageList);
            var result = await _game_repo.Update_Game_DetailsAsync(obj.Game);
            return Json(result);
        }

        [HttpGet("Update-Game/{id}")]
        public async Task<IActionResult> Update_Game_Details(long id)
        {
            var data = new Game_Data();
            data.Game_DTO = await _game_repo.Get_Details_By_Id(id);
            if (data.Game_DTO.Game == null) { return RedirectToAction("Add_New_Game", "Master"); }
            data.Game_modeList = await _gm_repo.Get_ALL_ActiveAsync();
            data.GenreList = await _gr_repo.Get_ALL_ActiveAsync();
            data.KeywordList = await _key_repo.Get_ALL_ActiveAsync();
            data.PlatformList = await _gp_repo.Get_ALL_ActiveAsync();
            data.VendorList = await _vendor_repo.Get_All();
            ViewData["GameData"] = data;
            ViewBag.GameId = id;
            return View();
        }

        [HttpGet("Update-Game-Mappings")]
        public async Task<JsonResult> Update_Game_Mappings()
        {
            var platformList = await _gp_repo.Get_ALL_ActiveAsync();
            foreach (var platform in platformList)
            {
                long total_records = 0;
                long item_processed = 0;
                do
                {
                    var games = await _game_repo.GetPaginated_By_PlatformId(item_processed, 50, platform.Id);
                    total_records = games.TotalCount;
                    item_processed += games.NumResult;
                    foreach (var game in games.PagedSet)
                    {
                        var data = new Game_Data();
                        data.Game_DTO = await _game_repo.Get_Details_By_Id(game.Id);
                        if (data.Game_DTO.GamePlatformMapings.Count() == 0)
                        {
                            var platform_str = string.IsNullOrEmpty(game.Platforms) ? string.Empty : game.Platforms;
                            if (platform_str.Contains(','))
                            {
                                var platforms = platform_str.Split(',');
                                foreach (var p in platforms)
                                {
                                    if (string.IsNullOrEmpty(p)) { continue; }
                                    var pid = Convert.ToInt32(p);
                                    if (platformList.Any(x => x.Id == pid))
                                    {
                                        string pname = platformList.Where(x => x.Id == pid).Select(x => x.Name).FirstOrDefault();
                                        var result = await _gpm_repo.Add_New_Maping(new GamePlatformMaping { Game_Id = game.Id, Platform_Id = pid, Condition_Id = 1 });
                                    }
                                }
                            }
                        }
                    }

                } while (total_records > item_processed);
            }

            return Json(true);
        }

        [HttpPost("Delete-Images")]
        public async Task<JsonResult> Delete_Images([FromBody] Images obj)
        {
            if (obj == null) { return Json(false); }
            if (string.IsNullOrEmpty(obj.Url)) { return Json(false); }
            var fileInfo = _HostingEnv.WebRootFileProvider.GetFileInfo(obj.Url);
            if (fileInfo.Exists)
            {
                System.IO.File.Delete(fileInfo.PhysicalPath);
                var result = await _img_repo.Remove_By_Url_Async(obj.Url);
                return Json(result);
            }
            return Json(false);
        }

        [HttpPost("Delete-Game")]
        public async Task<JsonResult> Delete_Games([FromBody] Game obj)
        {
            if (obj == null) { return Json(false); }
            var img_list = await _img_repo.Get_Image_By_ReferenceId_ImageOfAsync(obj.Id, Constant.ImageCategory.GameImage);
            if (img_list.Success)
            {
                var images = img_list.Data as IEnumerable<Images>;
                foreach (var img in images)
                {
                    var fileInfo = _HostingEnv.WebRootFileProvider.GetFileInfo(img.Url);
                    if (fileInfo.Exists) { System.IO.File.Delete(fileInfo.PhysicalPath); }
                }
            }
            var result = await _game_repo.DeleteByIdAsync(obj.Id);
            return Json(result.Success);
        }

        [HttpGet("Remove-Maping/{game_Id}/{platform_Id}/{condition_Id}")]
        public async Task<JsonResult> Remove_Game_Platform_Maping_Async(long game_Id, int platform_Id, int condition_Id)
        {
            var result = await _gpm_repo.Remove_Maping_Async(game_Id, platform_Id, condition_Id);
            return Json(result);
        }

        [HttpGet("Remove-Maping/{sku}")]
        public async Task<JsonResult> Remove_Maping_By_Sku(string sku)
        {
            var result = await _gpm_repo.Remove_Maping_By_Sku(sku);
            return Json(result);
        }

        [HttpGet("Remove-Video/{id}")]
        public async Task<JsonResult> Remove_Video_By_Id_Async(long id)
        {
            var result = await _vid_repo.Remove_By_Id_Async(id);
            return Json(result);
        }

        [HttpGet("Get-ALL-Active-Genre")]
        public async Task<JsonResult> Get_ALL_Active_Genre()
        {
            var result = await _gr_repo.Get_ALL_ActiveAsync();
            return Json(result);
        }

        [HttpGet("Get-ALL-Active-Platform")]
        public async Task<JsonResult> Get_ALL_Active_Platform()
        {
            var result = await _gp_repo.Get_ALL_ActiveAsync();
            return Json(result);
        }

        [HttpGet("Get-ALL-Active-Game-Mode")]
        public async Task<JsonResult> Get_ALL_Active_Game_Mode()
        {
            var result = await _gm_repo.Get_ALL_ActiveAsync();
            return Json(result);
        }

        [HttpGet("Get-ALL-Active-Keyword")]
        public async Task<JsonResult> Get_ALL_Active_Keyword()
        {
            var result = await _key_repo.Get_ALL_ActiveAsync();
            return Json(result);
        }


        [HttpPost("download-csv-games")]
        public async Task<IActionResult> Get_CSV_Of_Games([FromForm] Game_Filter obj)
        {
            string where_clouse = string.Empty;
            if (obj.Platform_Id > 0) { where_clouse += string.Format(" AND t1.Platforms LIKE '%,{0},%' ", obj.Platform_Id); }
            if (obj.Genre_Id > 0) { where_clouse += string.Format(" AND t1.Genres LIKE '%,{0},%' ", obj.Genre_Id); }
            if (!string.IsNullOrEmpty(obj.SearchText)) { where_clouse += string.Format(" AND t1.Name LIKE '%{0}%' ", obj.SearchText); }


            string query = string.Empty;



            query += "SELECT Name,PlatformName,Condition,First_release_date,Rating,RatingCount,Popularity,Summary,PreOrder,case when Active=1 then 'Yes' else 'No' End as Active,	Buying_Price,Selling_Price,Original_Price, ";
            query += " case when IsBestSelling = 1 then 'Yes' else 'No' end IsBestSelling,case when Available_To_Buy = 1 then 'Yes' else 'No' end Available_To_Buy, case when Available_To_Sell = 1 then 'Yes' else 'No' End Available_To_Sell, Quantity, SKU, Barcode, Vendor_Name ";
            query += " FROM[Game_List_View_For_Display] t1 where Name  <> '' ";
            query += where_clouse;
            query += " ORDER by First_release_date DESC ";

            var result = await _game_repo.GetGame_By_Custom_QueryAsync(query);
            var data = Utility.ExportExcel(result.PagedSet.ToList(), "Games");
            return File(data, "text/xlsx", $"Games.xlsx");
        }

        [HttpPost("download-csv-plaform")]
        public async Task<IActionResult> Get_CSV_Of_plaform([FromForm] SearchQuery obj)
        {
            string where_clouse = string.Empty;
            if (!string.IsNullOrEmpty(obj.searchTxt)) { where_clouse += string.Format(" AND Name LIKE '%{0}%' ", obj.searchTxt); }


            string query = string.Empty;



            query += "select Name,PageTitle,MetaKeyword,MetaDescription,case when Active=1 then 'Yes' else 'No' End Active,Position,CONVERT(char(10), AddedOn,126) as AddedOn,Code ";
            query += " from GamePlatformsTbl where Deleted = 0   " + where_clouse + " order by Active desc, Position asc";

            var result = await _gp_repo.GetGamePlatform_By_Custom_QueryAsync(query);
            var data = Utility.ExportExcel(result.PagedSet.ToList(), "Plaform");
            return File(data, "text/xlsx", $"GamePlaform.xlsx");
        }
        [HttpPost("download-csv-genere")]
        public async Task<IActionResult> Get_CSV_Of_genere([FromForm] SearchQuery obj)
        {
            string where_clouse = string.Empty;
            if (!string.IsNullOrEmpty(obj.searchTxt)) { where_clouse += string.Format(" where Name LIKE '%{0}%' ", obj.searchTxt); }


            string query = string.Empty;



            query += "select Name,Created_at,case when Active=1 then 'Yes' else 'No' End Active from GenereTbl " + where_clouse + " order by Id desc";

            var result = await _gr_repo.GetGenere_By_Custom_QueryAsync(query);
            var data = Utility.ExportExcel(result.PagedSet.ToList(), "Genere_CSV");
            return File(data, "text/xlsx", $"Genere.xlsx");
        }



        [HttpPost("Get-Paginated-Games")]
        public async Task<JsonResult> GetPaginatedGames([FromBody] Game_Filter obj)
        {
            string where_clouse = string.Empty;
            if (obj.Platform_Id > 0) { where_clouse += string.Format(" AND t1.Platforms LIKE '%,{0},%' ", obj.Platform_Id); }
            if (obj.Genre_Id > 0) { where_clouse += string.Format(" AND t1.Genres LIKE '%,{0},%' ", obj.Genre_Id); }
            if (!string.IsNullOrEmpty(obj.SearchText)) { where_clouse += string.Format(" AND t1.Name LIKE '%{0}%' ", obj.SearchText); }


            string query = string.Empty;
            query += " SELECT COUNT(t1.Id) as Total FROM GamesTbl t1 ";
            query += " WHERE t1.Id > 0 ";
            query += where_clouse;
            query += "; ";


            query += " SELECT t1.*, t2.Name as 'Vendor_Name', ";
            query += " Genre_Names=dbo.fn_GetGenereNamesByIds(t1.Genres), ";
            query += " Platform_Names=dbo.fn_GetPlatformNamesByIds(t1.Platforms), ";
            query += " (select TOP(1) [Url] from ImagesTbl where Reference_Id=t1.Id AND ImageOf='Game' AND ImageType='Thumbnail' AND ImageCategory='Cover') as ImageUrl ";
            query += " FROM GamesTbl t1 ";
            query += " LEFT JOIN VendorTbl t2 on t1.Vendor_Id=t2.Id ";
            query += " WHERE t1.Id > 0 ";
            query += where_clouse;
            query += " ORDER by t1.Id DESC ";
            query += string.Format(" OFFSET {0} ROWS ", obj.Offset);
            query += string.Format(" FETCH NEXT {0} ROWS ONLY ", obj.Rows);
            query += "; ";


            var result = await _game_repo.GetPaginated_By_Custom_master_QueryAsync(query);
            return Json(result);
        }

        [HttpGet("Save_IGdb_50_Games_Per_Platforms/{offset?}")]
        public async Task<JsonResult> Save_IGdb_50_Games_Per_Platforms(long offset = 0)
        {
            var totalPlatform = await _gp_repo.GetTopNAsync(50);
            foreach (var p in totalPlatform)
            {
                try
                {
                    var gameObj = await client.Get_Data_Async(string.Format("games/?fields=*&limit=50&offset={0}&filter[release_dates.platform][eq]={1}&order=created_at:asc", offset, p.Igdb_Id));
                    if (!gameObj.Success) { throw new Exception(gameObj.Content); }
                    var gameList = JsonConvert.DeserializeObject<List<IgdbAPI.Game>>(gameObj.Data);
                    if (gameList != null)
                    {
                        foreach (var x in gameList)
                        {
                            try
                            {
                                if (x != null)
                                {
                                    var _result = await _game_repo.IsIdgbGame_ExistsAsync(x.id);
                                    //var _result = await Task.FromResult(new Models.DataProvider.DbResult { Success = true });
                                    if (!_result.Success)
                                    {
                                        string gms = ",";
                                        foreach (var xx in x.games) { gms += string.Format("{0},", xx); }

                                        string dev = ",";
                                        foreach (var xx in x.developers) { dev += string.Format("{0},", xx); }

                                        string pub = ",";
                                        foreach (var xx in x.publishers) { pub += string.Format("{0},", xx); }

                                        string tags = ",";
                                        foreach (var xx in x.tags) { tags += string.Format("{0},", xx); }

                                        string keywords = ",";
                                        foreach (var xx in x.keywords) { keywords += string.Format("{0},", xx); }

                                        string theme = ",";
                                        foreach (var xx in x.themes) { theme += string.Format("{0},", xx); }

                                        string genres = ",";
                                        foreach (var xx in x.genres)
                                        {
                                            var genr = await _gr_repo.IsIdgb_Genere_ExistsAsync(xx);
                                            if (genr.Success)
                                            {
                                                var newGen = genr.Data as Genere;
                                                genres += string.Format("{0},", newGen.Id);
                                            }
                                            else { genres += string.Format("{0},", xx); }

                                        }

                                        string gm = ",";
                                        foreach (var xx in x.game_modes)
                                        {
                                            var gmod = await _gm_repo.IsIdgb_Game_Mode_ExistsAsync(xx);
                                            if (gmod.Success)
                                            {
                                                var newGmod = gmod.Data as GameMode;
                                                gm += string.Format("{0},", newGmod.Id);
                                            }
                                            else { gm += string.Format("{0},", xx); }
                                        }

                                        string ge = ",";
                                        foreach (var xx in x.game_engines)
                                        {
                                            var gmen = await _ge_repo.IsIdgb_GameEngine_ExistsAsync(xx);
                                            if (gmen.Success)
                                            {
                                                var newGme = gmen.Data as GameEngine;
                                                ge += string.Format("{0},", newGme.Id);
                                            }
                                            else { ge += string.Format("{0},", xx); }
                                        }

                                        string pf = ",";
                                        foreach (var xx in x.release_dates)
                                        {
                                            var pltf = await _gp_repo.IsIdgbPlatform_ExistsAsync(xx.platform);
                                            if (pltf.Success)
                                            {
                                                var newPlat = pltf.Data as GamePlatform;
                                                pf += string.Format("{0},", newPlat.Id);
                                            }
                                            else { pf += string.Format("{0},", xx.platform); }
                                        }


                                        var newGame = new Game { Game_modes = gm, Publishers = pub, Developers = dev, SimilarGames = gms, Collection = x.collection, Rating = x.rating, RatingCount = x.rating_count, Name = x.name, NameUrl = x.name, Storyline = x.storyline, Summary = x.summary };
                                        newGame.Tags = tags; newGame.Keywords = keywords; newGame.Themes = theme; newGame.Genres = genres; newGame.Category = x.category;
                                        newGame.Idgb_GameId = x.id; newGame.Igdb_Id = x.id; newGame.First_release_date = Utility.GetDateTime(x.first_release_date);
                                        newGame.Popularity = x.popularity; newGame.Platforms = pf;

                                        if (x.pegi != null)
                                        {
                                            newGame.PEGI_Rating = x.pegi.rating;
                                            newGame.PEGI_Synopsis = x.pegi.synopsis;
                                        }

                                        if (x.esrb != null)
                                        {
                                            newGame.ESRB_Rating = x.esrb.rating;
                                            newGame.ESRB_Synopsis = x.esrb.synopsis;
                                        }


                                        _result = await _game_repo.Add_NewGame_From_Idgb_ApiAsync(newGame);
                                        if (_result.Success)
                                        {
                                            newGame = _result.Data as Game;
                                            var img = new Images();
                                            if (x.screenshots != null)
                                            {
                                                foreach (var sc in x.screenshots)
                                                {
                                                    img = new Images { Height = sc.height, Width = sc.width, Url = sc.url, Reference_Id = newGame.Id };
                                                    img.ImageOf = Constant.ImageCategory.GameImage;
                                                    img.ImageCategory = Constant.ImageCategory.ImageType.ScreenShot;
                                                    sc.url = await Utility.SaveScreenshotAndGetUrl(_HostingEnv, _img_repo, img, sc.cloudinary_id);
                                                }
                                            }
                                            if (x.cover != null)
                                            {
                                                img = new Images { Height = x.cover.height, Width = x.cover.width, Url = x.cover.url, Reference_Id = newGame.Id };
                                                img.ImageOf = Constant.ImageCategory.GameImage;
                                                img.ImageCategory = Constant.ImageCategory.ImageType.Cover;
                                                x.cover.url = await Utility.SaveCoverImageAndGetUrl(_HostingEnv, _img_repo, img, x.cover.cloudinary_id);
                                            }

                                            var video = new Video();
                                            if (x.videos != null)
                                            {
                                                foreach (var sc in x.videos)
                                                {
                                                    video = new Video { Reference_Id = newGame.Id, Name = sc.name, Video_Id = sc.video_id };
                                                    video.Video_of = Constant.ImageCategory.GameImage;
                                                    video.Snapshot = string.Format("https://img.youtube.com/vi/{0}/0.jpg", sc.video_id);
                                                    await _vid_repo.Add_NewAsync(video);
                                                }
                                            }

                                            if (x.release_dates != null)
                                            {
                                                foreach (var xx in x.release_dates)
                                                {
                                                    var releaseDate = new Game_Release_Date { Category = xx.category, Game_Id = newGame.Id, IGdb_Date = xx.date, Region = xx.region };
                                                    releaseDate.Release_On = Utility.GetDateTime(xx.date);
                                                    var pltf = await _gp_repo.IsIdgbPlatform_ExistsAsync(xx.platform);
                                                    if (pltf.Success)
                                                    {
                                                        var newPlat = pltf.Data as GamePlatform;
                                                        releaseDate.Platform_Id = newPlat.Id;
                                                    }
                                                    else { releaseDate.Platform_Id = xx.platform; }
                                                    await _game_repo.Add_New_Release_Date_From_IGDbAsync(releaseDate);
                                                    await _gpm_repo.Add_New_Maping(new GamePlatformMaping { Game_Id = newGame.Id, Platform_Id = releaseDate.Platform_Id, Condition_Id = 1 });
                                                }
                                            }

                                            var reviews = await client.GetReviewsByGameIdAsync(x.id);
                                            if (reviews != null)
                                            {
                                                foreach (var rev in reviews)
                                                {
                                                    _result = await _rev_repo.IsIgdbReview_ExistsAsync(rev.id);
                                                    if (!_result.Success)
                                                    {
                                                        var newReview = new Review { Category = rev.category, Conclusion = rev.conclusion, Content = rev.content, Reference_Id = newGame.Id, Igdb_Id = rev.id, Introduction = rev.introduction, Likes = rev.likes, Link = rev.title, Negative_Points = rev.negative_points, Positive_Points = rev.positive_points, Rating_Category = rev.rating_category, Title = rev.title, UserName = rev.username, Video = rev.video, Views = rev.views, ReviewFor = Constant.ImageCategory.GameImage };
                                                        var pltf = await _gp_repo.IsIdgbPlatform_ExistsAsync(rev.platform);
                                                        if (pltf.Success)
                                                        {
                                                            var newPlat = pltf.Data as GamePlatform;
                                                            newReview.Platform_Id = newPlat.Id;
                                                        }
                                                        else { newReview.Platform_Id = rev.platform; }
                                                        _result = await _rev_repo.Add_New_IgdbReviewAsync(newReview);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex) { ExceptionGateway.AddException(new ExceptionLog(ex, "Save_IGdb_50_Games_Per_Platforms -> Games")); continue; }
                        }
                    }
                }
                catch (Exception ex) { ExceptionGateway.AddException(new ExceptionLog(ex, "Save_IGdb_50_Games_Per_Platforms -> Platforms")); continue; }
            }
            bool result = await Task.FromResult(true);
            return Json(result);
        }

        [HttpGet("Update-Images-Of-Games")]
        public async Task<JsonResult> UpdateImagesOfGames()
        {
            var games = await _game_repo.Get_ALL_Witch_Has_No_Cover_Image();
            foreach (var game in games)
            {
                try
                {
                    var result = await client.Get_Game_Async(game.Igdb_Id);
                    if (result.Success)
                    {
                        var x = result.Data.FirstOrDefault();
                        var img = new Images();
                        if (x.screenshots != null)
                        {
                            foreach (var sc in x.screenshots)
                            {
                                img = new Images { Height = sc.height, Width = sc.width, Url = sc.url, Reference_Id = game.Id };
                                img.ImageOf = Constant.ImageCategory.GameImage;
                                img.ImageCategory = Constant.ImageCategory.ImageType.ScreenShot;
                                sc.url = await Utility.SaveScreenshotAndGetUrl(_HostingEnv, _img_repo, img, sc.cloudinary_id);
                            }
                        }
                        if (x.cover != null)
                        {
                            img = new Images { Height = x.cover.height, Width = x.cover.width, Url = x.cover.url, Reference_Id = game.Id };
                            img.ImageOf = Constant.ImageCategory.GameImage;
                            img.ImageCategory = Constant.ImageCategory.ImageType.Cover;
                            x.cover.url = await Utility.SaveCoverImageAndGetUrl(_HostingEnv, _img_repo, img, x.cover.cloudinary_id);
                        }
                    }
                    else { throw new Exception(result.Content); }
                }
                catch (Exception ex) { ExceptionGateway.AddException(new ExceptionLog(ex, "UpdateImagesOfGames -> Games")); continue; }
            }
            return Json(true);
        }

        [HttpGet("Update-Videos-Of-Games")]
        public async Task<JsonResult> UpdateVideosOfGames()
        {
            var games = await _game_repo.Get_ALL_Which_Has_No_VideosAsync();
            foreach (var game in games)
            {
                try
                {
                    var result = await client.Get_Game_Async(game.Igdb_Id);
                    if (result.Success)
                    {
                        var x = result.Data.FirstOrDefault();
                        var video = new Video();
                        if (x.videos != null)
                        {
                            foreach (var sc in x.videos)
                            {
                                video = new Video { Reference_Id = game.Id, Name = sc.name, Video_Id = sc.video_id };
                                video.Video_of = Constant.ImageCategory.GameImage;
                                video.Snapshot = string.Format("https://img.youtube.com/vi/{0}/0.jpg", sc.video_id);
                                await _vid_repo.Add_NewAsync(video);
                            }
                        }
                    }
                    else { throw new Exception(result.Content); }
                }
                catch (Exception ex) { ExceptionGateway.AddException(new ExceptionLog(ex, "UpdateVideosOfGames -> Games")); continue; }
            }
            return Json(true);
        }

        #endregion
    }
}
