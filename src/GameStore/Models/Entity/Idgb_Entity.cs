using System.Collections.Generic;

namespace GameStore.Models.Entity
{

    public class Idgb_BasicEntity
    {
        public long id { get; set; }

        public string name { get; set; }        
        public string slug { get; set; }
        public string url { get; set; }

        public long created_at { get; set; }
        public long updated_at { get; set; }

        public List<long> games { get; set; }

        public Idgb_BasicEntity()
        {
            games = new List<long>();
        }
    }

    public class Idgb_Rating
    {
        public int rating { get; set; }
        public string synopsis { get; set; }
    }

    public class Idgb_Image_Base
    {
        public string url { get; set; }
        public string cloudinary_id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Idgb_Game : Idgb_BasicEntity
    {        
        public string summary { get; set; }
        public string storyline { get; set; }

        public int hypes { get; set; }
        public double rating { get; set; }
        public double popularity { get; set; }
        public double aggregated_rating { get; set; }
        public int aggregated_rating_count { get; set; }
        public double total_rating { get; set; }
        public int total_rating_count { get; set; }
        public int rating_count { get; set; }
        public int category { get; set; }
        public long collection { get; set; }
        public long first_release_date { get; set; }
        public int status { get; set; }

        

        public List<long> tags { get; set; }        
        public List<long> themes { get; set; }
        public List<long> genres { get; set; }
        public List<long> keywords { get; set; }
        public List<long> developers { get; set; }
        public List<long> publishers { get; set; }
        public List<long> game_modes { get; set; }
        public List<long> game_engines { get; set; }

        public Idgb_PEGI pegi { get; set; }
        public Idgb_ESRB esrb { get; set; }
        public Idgb_Cover cover { get; set; }

        public List<Igdb_Video> videos { get; set; }
        public List<Idgb_Website> websites { get; set; }
        public List<Idgb_Image> screenshots { get; set; }
        public List<Idgb_ReleaseDate> release_dates { get; set; }
        public List<Idgb_Alternative_name> alternative_names { get; set; }

        public Idgb_Game()
        {
            tags = new List<long>();
            themes = new List<long>();
            genres = new List<long>();
            keywords = new List<long>();
            developers = new List<long>();
            publishers = new List<long>();
            game_modes = new List<long>();
            game_engines = new List<long>();

            pegi = new Idgb_PEGI();
            esrb = new Idgb_ESRB();
            cover = new Idgb_Cover();
            videos = new List<Igdb_Video>();
            websites = new List<Idgb_Website>();
            screenshots = new List<Idgb_Image>();            
            release_dates = new List<Idgb_ReleaseDate>();
            alternative_names = new List<Idgb_Alternative_name>();
        }
    }

    public class Idgb_Company : Idgb_BasicEntity
    {
        public Idgb_Image logo { get; set; }
        public string description { get; set; }
        public int country { get; set; }
        public string website { get; set; }

        public long start_date { get; set; }
        public int start_date_category { get; set; }
        public long changed_company_id { get; set; }
        public long change_date { get; set; }
        public int change_date_category { get; set; }

        public string twitter { get; set; }
        public string facebook { get; set; }

        public List<long> published { get; set; }
        public List<long> developed { get; set; }

        public Idgb_Company()
        {
            logo = new Idgb_Image();
            published = new List<long>();
            developed = new List<long>();
        }

    }

    public class Idgb_Game_Engine : Idgb_BasicEntity
    {
        public Idgb_Image logo { get; set; }
        public List<long> platforms { get; set; }
        public List<long> companies { get; set; }

        public Idgb_Game_Engine()
        {
            logo = new Idgb_Image();
            platforms = new List<long>();
            companies = new List<long>();
        }
    }

    public class Id_Obj
    {
        public int id { get; set; }
    }

    public class Idgb_Website
    {
        public string url { get; set; }
        public int category { get; set; }
    }

    public class Igdb_Video
    {
        public string name { get; set; }
        public string video_id { get; set; }
    }

    public class Idgb_Alternative_name
    {
        public string name { get; set; }
        public string comment { get; set; }
    }

    public class Idgb_ReleaseDate
    {
        public long id { get; set; }
        public long game { get; set; }
        public int category { get; set; }
        public long platform { get; set; }
        public string human { get; set; }
        public long created_at { get; set; }
        public long updated_at { get; set; }
        public long date { get; set; }
        public int region { get; set; }
        public int y { get; set; }
        public int m { get; set; }
    }

    public class Idgb_Platform : Idgb_BasicEntity
    {
        public string shortcut { get; set; }
        public string website { get; set; }
        public string summary { get; set; }
        public string alternative_name { get; set; }
        public Idgb_Image logo { get; set; }

        public Idgb_Platform()
        {
            logo = new Idgb_Image();
        }
    }

    public class Idgb_Cover : Idgb_Image_Base
    {
        
    }

    public class Idgb_ESRB : Idgb_Rating
    {
        
    }

    public class Idgb_PEGI : Idgb_Rating
    {
        
    }
    

    public class Idgb_Image : Idgb_Image_Base
    {
        
    }

    public class Idgb_Collection : Idgb_BasicEntity
    {
        
    }

    

    public class Idgb_Genre : Idgb_BasicEntity
    {
        
    }

    public class Idgb_Keyword : Idgb_BasicEntity
    {
        
    }

    

    public class Idgb_Theme : Idgb_BasicEntity
    {
        
    }

    
}
