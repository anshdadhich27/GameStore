using System;
using System.Collections.Generic;

namespace GameStore.Models.Entity
{
    public class Game : BasicDetails
    {
        public string Summary { get; set; }
        public string Storyline { get; set; }

        public decimal Buying_Price { get; set; }        
        public decimal Selling_Price { get; set; }
        public decimal Original_Price { get; set; }

        public double Popularity { get; set; }
        public DateTime? First_release_date { get; set; }

        public bool IsBestSelling { get; set; }
        public bool Available_To_Buy { get; set; }
        public bool Available_To_Sell { get; set; }

        public bool PreOrder { get; set; }
        public bool Active { get; set; }
        public long Idgb_GameId { get; set; }
        public long Collection { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }

        public int Category { get; set; }
        public int Condition_Id { get; set; }
        public string Condition { get; set; }
        public string Conditions { get; set; }

        public double Rating { get; set; }
        public int RatingCount { get; set; }

        public int Vendor_Id { get; set; }
        public string Vendor_Name { get; set; }
        public string SKU { get; set; }
        public string SimilarGames { get; set; }
        public string Tags { get; set; }
        public string Developers { get; set; }
        public string Publishers { get; set; }
        public string Game_modes { get; set; }
        public string Keywords { get; set; }
        public string Themes { get; set; }
        public string Genres { get; set; }
        public string Platforms { get; set; }

        public string Genre_Names { get; set; }
        public string Platform_Names { get; set; }

        public int PlatformId { get; set; }
        public string PlatformName { get; set; }
        public string PlatformUrl { get; set; }

        public int PEGI_Rating { get; set; }
        public int ESRB_Rating { get; set; }

        public string ESRB_Synopsis { get; set; }
        public string PEGI_Synopsis { get; set; }
                

        public string ImageUrl { get; set; }
        public Genere Genre { get; set; }
        public GamePlatform Platform { get; set; }
        public IEnumerable<Images> ImageList { get; set; }
        public IEnumerable<GamePlatform> SupportedPlatforms { get; set; }
        public IEnumerable<Genere> GenereList { get; set; }

        public string PageUrl { get { return string.Format("/game/{0}/{1}/{2}", PlatformUrl, NameUrl, Condition).ToLower(); } }

        public Game()
        {
            Genre = new Genere();
            ImageList = new List<Images>();
            GenereList = new List<Genere>();
            Platform = new GamePlatform();
            SupportedPlatforms = new List<GamePlatform>();
            
        }
    }

    public class Game_DTO
    {
        public Game Game { get; set; }
        public IEnumerable<Review> Reviews { get; set; }          
        public IEnumerable<Video> VideoList { get; set; }
        public IEnumerable<Images> ImageList { get; set; }
        public PaginatedEntity<Game> Similar_Games { get; set; }
        public PaginatedEntity<Game> Recommended_Games { get; set; }
        public IEnumerable<Game_Release_Date> ReleaseDates { get; set; }
        public IEnumerable<GamePlatformMaping> GamePlatformMapings { get; set; }

        public Game_DTO()
        {
            Game = new Game();
            Reviews = new List<Review>();
            VideoList = new List<Video>();
            ImageList = new List<Images>();
            Similar_Games = new PaginatedEntity<Game>();            
            ReleaseDates = new List<Game_Release_Date>();
            Recommended_Games = new PaginatedEntity<Game>();
            GamePlatformMapings = new List<GamePlatformMaping>();
        }
    }

    public class Game_Filter
    {
        public long Offset { get; set; }
        public int PageIndex { get; set; }
        public int Rows { get; set; }
        public string SearchText { get; set; }
        public int Platform_Id { get; set; }
        public int Genre_Id { get; set; }
    }

    public class Game_Data
    {
        public Game_DTO Game_DTO { get; set; }        
        public IEnumerable<Genere> GenreList { get; set; }
        public IEnumerable<Vendor> VendorList { get; set; }
        public IEnumerable<Keyword> KeywordList { get; set; }        
        public IEnumerable<GameMode> Game_modeList { get; set; }
        public IEnumerable<GamePlatform> PlatformList { get; set; }

        public Game_Data()
        {
            Game_DTO = new Game_DTO();
            GenreList = new List<Genere>();
            VendorList = new List<Vendor>();
            KeywordList = new List<Keyword>();            
            Game_modeList = new List<GameMode>();
            PlatformList = new List<GamePlatform>();
        }
        
    }

    public class Game_Search
    {
        public long Id { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string NameUrl { get; set; }
        public int PlatformId { get; set; }
        public string PlatformName { get; set; }
        public string PlatformUrl { get; set; }
        public string ImageUrl { get; set; }
        public bool PreOrder { get; set; }
        public double Rating { get; set; }
        public int RatingCount { get; set; }
        public decimal Buying_Price { get; set; }
        public decimal Selling_Price { get; set; }
        public decimal Original_Price { get; set; }
        public int Condition_Id { get; set; }
        public string Condition { get; set; }
        public string PageUrl { get { return string.Format("/game/{0}/{1}/{2}", PlatformUrl, NameUrl, Condition).ToLower(); } }
    }

    public class Suggested_Games
    {
        public string Title { get; set; }
        public string TitleStyle { get; set; }
        public IEnumerable<Game> GameList { get; set; }

        public Suggested_Games()
        {
            GameList = new List<Game>();
        }

    }
    public class GameListExport
    {
        public string Name { get; set; }
        public string PlatformName { get; set; }
        public string Condition { get; set; }
        public string First_release_date { get; set; }
        public string Rating { get; set; }
        public string RatingCount { get; set; }
        public string Popularity { get; set; }
        public string Summary { get; set; }
        public string PreOrder { get; set; }
        public string Active { get; set; }
        public string Buying_Price { get; set; }
        public string Selling_Price { get; set; }
        public string Original_Price { get; set; }
        public string IsBestSelling { get; set; }
        public string Available_To_Buy { get; set; }
        public string Available_To_Sell { get; set; }
        public string Quantity { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public string Vendor_Name { get; set; }
    }
}
