using HttpHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgdbAPI
{
    public class ApiClient
    {
        Client client = null;

        /// <summary>
        /// Initialize Client with Base Url of API and user-key
        /// </summary>
        /// <param name="api_base_url">API base Url</param>
        public ApiClient(string api_base_url)
        {
            client = new Client(api_base_url);

        }

        /// <summary>
        /// Set API Auth Header if required
        /// </summary>
        /// <param name="key">Header Key</param>
        /// <param name="value">Header Value</param>
        public void SetAuthHeader(string key, string value)
        {
            client.AddHeader(key, value);
        }




        public async Task<string> GetDataAsync(string api)
        {
            return await client.GetJsonAsync(api);
        }

        public async Task<DataResult<string>> Get_Data_Async(string api)
        {
            return await client.GetJson_WithStatusCode_Async(api);
        }

        #region Games
        public async Task<Game> GetGameAsync(long game_id)
        {
            var result = await client.GetDataAsync<List<Game>>(string.Format("games/{0}/?fields=*", game_id));
            return result == null ? null : result.FirstOrDefault();
        }

        public async Task<DataResult<List<Game>>> Get_Game_Async(long game_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Game>>(string.Format("games/{0}/?fields=*", game_id));
        }

        public async Task<List<Game>> SearchGamesAsync(string text)
        {
            return await client.GetDataAsync<List<Game>>(string.Format("games/?fields=*&limit=50&search={0}", text));
        }

        public async Task<DataResult<List<Game>>> Search_Games_Async(string text)
        {
            return await client.GetData_WithStatusCode_Async<List<Game>>(string.Format("games/?fields=*&limit=50&search={0}", text));
        }

        public async Task<List<Game>> GetGamesAsync()
        {
            return await client.GetDataAsync<List<Game>>("games/?fields=*&limit=50");
        }

        public async Task<DataResult<List<Game>>> Get_Games_Async()
        {
            return await client.GetData_WithStatusCode_Async<List<Game>>("games/?fields=*&limit=50");
        }

        public async Task<Total> GetGamesCountAsync()
        {
            return await client.GetDataAsync<Total>("games/count");
        }

        public async Task<DataResult<Total>> Get_Games_Count_Async()
        {
            return await client.GetData_WithStatusCode_Async<Total>("games/count");
        }

        public async Task<List<GameDetails>> GetPaginatedGamesAsync(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetDataAsync<List<GameDetails>>(string.Format("games/?limit={0}&offset={1}&fields=*&order=created_at:{2}&expand=games,developers,publishers,game_engines,game_modes,keywords,genres", limit, offset, created_date_order));
        }

        public async Task<DataResult<List<GameDetails>>> Get_Paginated_Games_Async(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetData_WithStatusCode_Async<List<GameDetails>>(string.Format("games/?limit={0}&offset={1}&fields=*&order=created_at:{2}&expand=games,developers,publishers,game_engines,game_modes,keywords,genres", limit, offset, created_date_order));
        }

        public async Task<List<Game>> GetGamesByPlatformAsync(long platform_id)
        {
            return await client.GetDataAsync<List<Game>>(string.Format("platforms/{0}/games/?fields=*", platform_id));
        }

        public async Task<DataResult<List<Game>>> Get_Games_By_Platform_Async(long platform_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Game>>(string.Format("platforms/{0}/games/?fields=*", platform_id));
        }

        public async Task<List<Game>> GetGamesByCompanyAsync(long company_id)
        {
            return await client.GetDataAsync<List<Game>>(string.Format("companies/{0}/games/?fields=*", company_id));
        }

        public async Task<DataResult<List<Game>>> Get_Games_By_Company_Async(long company_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Game>>(string.Format("companies/{0}/games/?fields=*", company_id));
        }

        public async Task<List<Game>> GetGamesByPeopleAsync(long people_id)
        {
            return await client.GetDataAsync<List<Game>>(string.Format("people/{0}/games/?fields=*", people_id));
        }

        public async Task<DataResult<List<Game>>> Get_Games_By_People_Async(long people_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Game>>(string.Format("people/{0}/games/?fields=*", people_id));
        }

        public async Task<List<Game>> GetGamesByFranchiseAsync(long franchise_id)
        {
            return await client.GetDataAsync<List<Game>>(string.Format("franchises/{0}/games/?fields=*", franchise_id));
        }

        public async Task<DataResult<List<Game>>> Get_Games_By_Franchise_Async(long franchise_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Game>>(string.Format("franchises/{0}/games/?fields=*", franchise_id));
        }

        #endregion


        #region Platforms
        public async Task<List<Platform>> GetPlatformsAsync()
        {
            return await client.GetDataAsync<List<Platform>>("platforms/?fields=*&limit=50");
        }

        public async Task<DataResult<List<Platform>>> Get_Platforms_Async()
        {
            return await client.GetData_WithStatusCode_Async<List<Platform>>("platforms/?fields=*&limit=50");
        }

        public async Task<Total> GetPlatformCountAsync()
        {
            return await client.GetDataAsync<Total>("platforms/count");
        }

        public async Task<DataResult<Total>> Get_PlatformCount_Async()
        {
            return await client.GetData_WithStatusCode_Async<Total>("platforms/count");
        }

        public async Task<List<Platform>> GetPaginatedPlatformsAsync(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetDataAsync<List<Platform>>(string.Format("platforms/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        public async Task<DataResult<List<Platform>>> Get_Paginated_Platforms_Async(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetData_WithStatusCode_Async<List<Platform>>(string.Format("platforms/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        public async Task<Platform> GetPlatformAsync(long platform_id)
        {
            var result = await client.GetDataAsync<List<Platform>>(string.Format("platforms/{0}/?fields=*", platform_id));
            return result == null ? null : result.FirstOrDefault();
        }

        public async Task<DataResult<List<Platform>>> Get_Platform_Async(long platform_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Platform>>(string.Format("platforms/{0}/?fields=*", platform_id));
        }

        #endregion


        #region Keywords
        public async Task<List<Keyword>> GetKeywordsAsync()
        {
            return await client.GetDataAsync<List<Keyword>>("keywords/?fields=*&limit=50");
        }

        public async Task<DataResult<List<Keyword>>> Get_Keywords_Async()
        {
            return await client.GetData_WithStatusCode_Async<List<Keyword>>("keywords/?fields=*&limit=50");
        }

        public async Task<Total> GetKeywordsCountAsync()
        {
            return await client.GetDataAsync<Total>("keywords/count");
        }

        public async Task<DataResult<Total>> Get_Keywords_Count_Async()
        {
            return await client.GetData_WithStatusCode_Async<Total>("keywords/count");
        }

        public async Task<List<Keyword>> GetPaginatedKeywordsAsync(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetDataAsync<List<Keyword>>(string.Format("keywords/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        public async Task<DataResult<List<Keyword>>> Get_Paginated_Keywords_Async(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetData_WithStatusCode_Async<List<Keyword>>(string.Format("keywords/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        public async Task<Keyword> GetKeywordAsync(long keyword_id)
        {
            var result = await client.GetDataAsync<List<Keyword>>(string.Format("keywords/{0}/?fields=*", keyword_id));
            return result == null ? null : result.FirstOrDefault();
        }

        public async Task<DataResult<List<Keyword>>> Get_Keyword_Async(long keyword_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Keyword>>(string.Format("keywords/{0}/?fields=*", keyword_id));
        }

        #endregion


        #region Collections
        public async Task<List<Collection>> GetCollectionsAsync()
        {
            return await client.GetDataAsync<List<Collection>>("collections/?fields=*&limit=50");
        }

        public async Task<DataResult<List<Collection>>> Get_Collections_Async()
        {
            return await client.GetData_WithStatusCode_Async<List<Collection>>("collections/?fields=*&limit=50");
        }

        public async Task<Total> GetCollectionsCountAsync()
        {
            return await client.GetDataAsync<Total>("collections/count");
        }

        public async Task<DataResult<Total>> Get_Collections_Count_Async()
        {
            return await client.GetData_WithStatusCode_Async<Total>("collections/count");
        }

        public async Task<List<Collection>> GetPaginatedCollectionsAsync(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetDataAsync<List<Collection>>(string.Format("collections/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        public async Task<DataResult<List<Collection>>> Get_Paginated_Collections_Async(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetData_WithStatusCode_Async<List<Collection>>(string.Format("collections/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        public async Task<Collection> GetCollectionAsync(long collection_id)
        {
            var result = await client.GetDataAsync<List<Collection>>(string.Format("collections/{0}/?fields=*", collection_id));
            return result == null ? null : result.FirstOrDefault();
        }

        public async Task<DataResult<List<Collection>>> Get_Collection_Async(long collection_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Collection>>(string.Format("collections/{0}/?fields=*", collection_id));
        }
        #endregion


        #region Companies
        public async Task<List<Company>> GetCompaniesAsync()
        {
            return await client.GetDataAsync<List<Company>>("companies/?fields=*&limit=50");
        }

        public async Task<DataResult<List<Company>>> Get_Companies_Async()
        {
            return await client.GetData_WithStatusCode_Async<List<Company>>("companies/?fields=*&limit=50");
        }

        public async Task<Total> GetCompanyCountAsync()
        {
            return await client.GetDataAsync<Total>("companies/count");
        }

        public async Task<DataResult<Total>> Get_Company_Count_Async()
        {
            return await client.GetData_WithStatusCode_Async<Total>("companies/count");
        }

        public async Task<Company> GetCompanyAsync(long company_id)
        {
            var result = await client.GetDataAsync<List<Company>>(string.Format("companies/{0}/?fields=*", company_id));
            return result == null ? null : result.FirstOrDefault();
        }

        public async Task<DataResult<List<Company>>> Get_Company_Async(long company_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Company>>(string.Format("companies/{0}/?fields=*", company_id));
        }

        public async Task<List<Company>> GetPaginatedCompanyAsync(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetDataAsync<List<Company>>(string.Format("companies/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        public async Task<DataResult<List<Company>>> Get_Paginated_Company_Async(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetData_WithStatusCode_Async<List<Company>>(string.Format("companies/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }
        #endregion


        #region Peoples
        public async Task<List<People>> GetPeoplesAsync()
        {
            return await client.GetDataAsync<List<People>>("people/?fields=*&limit=50");
        }

        public async Task<DataResult<List<People>>> Get_Peoples_Async()
        {
            return await client.GetData_WithStatusCode_Async<List<People>>("people/?fields=*&limit=50");
        }

        public async Task<Total> GetPeopleCountAsync()
        {
            return await client.GetDataAsync<Total>("people/count");
        }

        public async Task<DataResult<Total>> Get_People_Count_Async()
        {
            return await client.GetData_WithStatusCode_Async<Total>("people/count");
        }

        public async Task<People> GetPeopleAsync(long people_id)
        {
            var result = await client.GetDataAsync<List<People>>(string.Format("people/{0}/?fields=*", people_id));
            return result == null ? null : result.FirstOrDefault();
        }

        public async Task<DataResult<List<People>>> Get_People_Async(long people_id)
        {
            return await client.GetData_WithStatusCode_Async<List<People>>(string.Format("people/{0}/?fields=*", people_id));
        }

        public async Task<List<People>> GetPaginatedPeopleAsync(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetDataAsync<List<People>>(string.Format("people/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        public async Task<DataResult<List<People>>> Get_Paginated_People_Async(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetData_WithStatusCode_Async<List<People>>(string.Format("people/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        #endregion


        #region Franchises
        public async Task<List<Franchise>> GetFranchisesAsync()
        {
            return await client.GetDataAsync<List<Franchise>>("franchises/?fields=*&limit=50");
        }

        public async Task<DataResult<List<Franchise>>> Get_Franchises_Async()
        {
            return await client.GetData_WithStatusCode_Async<List<Franchise>>("franchises/?fields=*&limit=50");
        }

        public async Task<Total> GetFranchiseCountAsync()
        {
            return await client.GetDataAsync<Total>("franchises/count");
        }

        public async Task<DataResult<Total>> Get_Franchise_Count_Async()
        {
            return await client.GetData_WithStatusCode_Async<Total>("franchises/count");
        }

        public async Task<Franchise> GetFranchiseAsync(long franchise_id)
        {
            var result = await client.GetDataAsync<List<Franchise>>(string.Format("franchises/{0}/?fields=*", franchise_id));
            return result == null ? null : result.FirstOrDefault();
        }

        public async Task<DataResult<List<Franchise>>> Get_Franchise_Async(long franchise_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Franchise>>(string.Format("franchises/{0}/?fields=*", franchise_id));
        }

        public async Task<List<Franchise>> GetPaginatedFranchiseAsync(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetDataAsync<List<Franchise>>(string.Format("franchises/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        public async Task<DataResult<List<Franchise>>> Get_Paginated_Franchise_Async(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetData_WithStatusCode_Async<List<Franchise>>(string.Format("franchises/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }
        #endregion


        #region Reviews
        public async Task<List<Review>> GetReviewsAsync()
        {
            return await client.GetDataAsync<List<Review>>("reviews/?fields=*&limit=50");
        }

        public async Task<DataResult<List<Review>>> Get_Reviews_Async()
        {
            return await client.GetData_WithStatusCode_Async<List<Review>>("reviews/?fields=*&limit=50");
        }

        public async Task<Total> GetReviewCountAsync()
        {
            return await client.GetDataAsync<Total>("reviews/count");
        }

        public async Task<DataResult<Total>> Get_Review_Count_Async()
        {
            return await client.GetData_WithStatusCode_Async<Total>("reviews/count");
        }

        public async Task<Review> GetReviewAsync(long review_id)
        {
            var result = await client.GetDataAsync<List<Review>>(string.Format("reviews/{0}/?fields=*", review_id));
            return result == null ? null : result.FirstOrDefault();
        }

        public async Task<DataResult<List<Review>>> Get_Review_Async(long review_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Review>>(string.Format("reviews/{0}/?fields=*", review_id));
        }

        public async Task<List<Review>> GetReviewsByGameIdAsync(long game_id)
        {
            return await client.GetDataAsync<List<Review>>(string.Format("reviews/?fields=*&limit=50&filter[game][eq]={0}", game_id));
        }

        public async Task<DataResult<List<Review>>> Get_Reviews_By_GameId_Async(long game_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Review>>(string.Format("reviews/?fields=*&limit=50&filter[game][eq]={0}", game_id));
        }

        public async Task<List<Review>> GetPaginatedReviewAsync(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetDataAsync<List<Review>>(string.Format("reviews/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        public async Task<DataResult<List<Review>>> Get_Paginated_Review_Async(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetData_WithStatusCode_Async<List<Review>>(string.Format("reviews/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }
        #endregion


        #region Game Engines
        public async Task<List<Game_Engine>> GetGameEnginesAsync()
        {
            return await client.GetDataAsync<List<Game_Engine>>("game_engines/?fields=*&limit=50");
        }

        public async Task<DataResult<List<Game_Engine>>> Get_Game_Engines_Async()
        {
            return await client.GetData_WithStatusCode_Async<List<Game_Engine>>("game_engines/?fields=*&limit=50");
        }

        public async Task<Total> GetGameEngineCountAsync()
        {
            return await client.GetDataAsync<Total>("game_engines/count");
        }

        public async Task<DataResult<Total>> Get_Game_Engine_Count_Async()
        {
            return await client.GetData_WithStatusCode_Async<Total>("game_engines/count");
        }

        public async Task<Game_Engine> GetGameEngineAsync(long game_engine_id)
        {
            var result = await client.GetDataAsync<List<Game_Engine>>(string.Format("game_engines/{0}/?fields=*", game_engine_id));
            return result == null ? null : result.FirstOrDefault();
        }

        public async Task<DataResult<List<Game_Engine>>> Get_Game_Engine_Async(long game_engine_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Game_Engine>>(string.Format("game_engines/{0}/?fields=*", game_engine_id));
        }

        public async Task<List<Game_Engine>> GetPaginatedGame_EngineAsync(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetDataAsync<List<Game_Engine>>(string.Format("game_engines/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        public async Task<DataResult<List<Game_Engine>>> Get_Paginated_Game_Engine_Async(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetData_WithStatusCode_Async<List<Game_Engine>>(string.Format("game_engines/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }
        #endregion


        #region Game Modes
        public async Task<List<Game_Mode>> GetGameModesAsync()
        {
            return await client.GetDataAsync<List<Game_Mode>>("game_modes/?fields=*&limit=50");
        }

        public async Task<DataResult<List<Game_Mode>>> Get_Game_Modes_Async()
        {
            return await client.GetData_WithStatusCode_Async<List<Game_Mode>>("game_modes/?fields=*&limit=50");
        }

        public async Task<Total> GetGameModeCountAsync()
        {
            return await client.GetDataAsync<Total>("game_modes/count");
        }

        public async Task<DataResult<Total>> Get_Game_Mode_Count_Async()
        {
            return await client.GetData_WithStatusCode_Async<Total>("game_modes/count");
        }

        public async Task<Game_Mode> GetGameModeAsync(long game_mode_id)
        {
            var result = await client.GetDataAsync<List<Game_Mode>>(string.Format("game_modes/{0}/?fields=*", game_mode_id));
            return result == null ? null : result.FirstOrDefault();
        }

        public async Task<DataResult<List<Game_Mode>>> Get_Game_Mode_Async(long game_mode_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Game_Mode>>(string.Format("game_modes/{0}/?fields=*", game_mode_id));
        }

        public async Task<List<Game_Mode>> GetPaginatedGame_ModeAsync(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetDataAsync<List<Game_Mode>>(string.Format("game_modes/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        public async Task<DataResult<List<Game_Mode>>> Get_Paginated_Game_Mode_Async(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetData_WithStatusCode_Async<List<Game_Mode>>(string.Format("game_modes/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }
        #endregion


        #region Genre
        public async Task<List<Genre>> GetGenreAsync()
        {
            return await client.GetDataAsync<List<Genre>>("genres/?fields=*&limit=50");
        }

        public async Task<DataResult<List<Genre>>> Get_Genre_Async()
        {
            return await client.GetData_WithStatusCode_Async<List<Genre>>("genres/?fields=*&limit=50");
        }

        public async Task<Total> GetGenreCountAsync()
        {
            return await client.GetDataAsync<Total>("genres/count");
        }

        public async Task<DataResult<Total>> Get_Genre_Count_Async()
        {
            return await client.GetData_WithStatusCode_Async<Total>("genres/count");
        }

        public async Task<Genre> GetGenreAsync(long genre_id)
        {
            var result = await client.GetDataAsync<List<Genre>>(string.Format("genres/{0}/?fields=*", genre_id));
            return result == null ? null : result.FirstOrDefault();
        }

        public async Task<DataResult<List<Genre>>> Get_Genre_Async(long genre_id)
        {
            return await client.GetData_WithStatusCode_Async<List<Genre>>(string.Format("genres/{0}/?fields=*", genre_id));
        }

        public async Task<List<Genre>> GetPaginatedGenreAsync(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetDataAsync<List<Genre>>(string.Format("genres/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }

        public async Task<DataResult<List<Genre>>> Get_Paginated_Genre_Async(long limit, long offset, string created_date_order = "asc")
        {
            return await client.GetData_WithStatusCode_Async<List<Genre>>(string.Format("genres/?limit={0}&offset={1}&fields=*&order=created_at:{2}", limit, offset, created_date_order));
        }
        #endregion

    }
}
