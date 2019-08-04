using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgdbAPI
{
    public class Total { public long count { get; set; } }

    public class BasicEntity
    {
        public long id { get; set; }

        public string name { get; set; }
        public string slug { get; set; }
        public string url { get; set; }

        public long created_at { get; set; }
        public long updated_at { get; set; }

        public List<long> games { get; set; }

        public BasicEntity()
        {
            games = new List<long>();
        }
    }

    public class Rating
    {
        public int rating { get; set; }
        public string synopsis { get; set; }
    }

    public class Image_Base
    {
        public string url { get; set; }
        public string cloudinary_id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Game : BasicEntity
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

        public PEGI pegi { get; set; }
        public ESRB esrb { get; set; }
        public Cover cover { get; set; }

        public List<Igdb_Video> videos { get; set; }
        public List<Website> websites { get; set; }
        public List<Image> screenshots { get; set; }
        public List<ReleaseDate> release_dates { get; set; }
        public List<Alternative_name> alternative_names { get; set; }

        public Game()
        {
            tags = new List<long>();
            themes = new List<long>();
            genres = new List<long>();
            keywords = new List<long>();
            developers = new List<long>();
            publishers = new List<long>();
            game_modes = new List<long>();
            game_engines = new List<long>();

            pegi = new PEGI();
            esrb = new ESRB();
            cover = new Cover();

            videos = new List<Igdb_Video>();
            websites = new List<Website>();
            screenshots = new List<Image>();
            release_dates = new List<ReleaseDate>();
            alternative_names = new List<Alternative_name>();
        }
    }


    public class GameDetails
    {
        public long id { get; set; }

        public string name { get; set; }
        public string slug { get; set; }
        public string url { get; set; }

        public long created_at { get; set; }
        public long updated_at { get; set; }

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
        public List<Game> games { get; set; }
        public List<Genre> genres { get; set; }
        public List<Keyword> keywords { get; set; }
        public List<People> developers { get; set; }
        public List<People> publishers { get; set; }
        public List<Game_Mode> game_modes { get; set; }
        public List<Game_Engine> game_engines { get; set; }

        public PEGI pegi { get; set; }
        public ESRB esrb { get; set; }
        public Cover cover { get; set; }

        public List<Igdb_Video> videos { get; set; }
        public List<Website> websites { get; set; }
        public List<Image> screenshots { get; set; }
        public List<ReleaseDate> release_dates { get; set; }
        public List<Alternative_name> alternative_names { get; set; }

        public GameDetails()
        {
            tags = new List<long>();
            themes = new List<long>();
            games = new List<Game>();
            genres = new List<Genre>();
            keywords = new List<Keyword>();
            developers = new List<People>();
            publishers = new List<People>();
            game_modes = new List<Game_Mode>();
            game_engines = new List<Game_Engine>();

            pegi = new PEGI();
            esrb = new ESRB();
            cover = new Cover();

            videos = new List<Igdb_Video>();
            websites = new List<Website>();
            screenshots = new List<Image>();
            release_dates = new List<ReleaseDate>();
            alternative_names = new List<Alternative_name>();
        }
    }




    public class Company : BasicEntity
    {
        public Image logo { get; set; }
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

        public Company()
        {
            logo = new Image();
            published = new List<long>();
            developed = new List<long>();
        }

    }

    public class Igdb_Video
    {
        public string name { get; set; }
        public string video_id { get; set; }
    }

    public class Website
    {
        public string url { get; set; }
        public int category { get; set; }
    }

    public class Alternative_name
    {
        public string name { get; set; }
        public string comment { get; set; }
    }

    public class ReleaseDate
    {
        public long id { get; set; }
        public long game { get; set; }
        public int category { get; set; }
        public int platform { get; set; }
        public string human { get; set; }
        public long created_at { get; set; }
        public long updated_at { get; set; }
        public long date { get; set; }
        public int region { get; set; }
        public int y { get; set; }
        public int m { get; set; }
    }

    public class Review
    {
        public long id { get; set; }
        public string username { get; set; }
        public string slug { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public long created_at { get; set; }
        public long updated_at { get; set; }
        public long game { get; set; }
        public int category { get; set; }
        public int likes { get; set; }
        public int views { get; set; }
        public int rating_category { get; set; }
        public long platform { get; set; }
        public string video { get; set; }
        public string introduction { get; set; }
        public string content { get; set; }
        public string conclusion { get; set; }
        public string positive_points { get; set; }
        public string negative_points { get; set; }
    }

    public class Game_Engine : BasicEntity
    {
        public Image logo { get; set; }
        public List<long> platforms { get; set; }
        public List<long> companies { get; set; }

        public Game_Engine()
        {
            logo = new Image();
            platforms = new List<long>();
            companies = new List<long>();
        }
    }

    public class Platform : BasicEntity
    {
        public string shortcut { get; set; }
        public string website { get; set; }
        public string summary { get; set; }
        public string alternative_name { get; set; }
        public Image logo { get; set; }

        public Platform()
        {
            logo = new Image();
        }
    }

    public class People : BasicEntity
    {

    }

    public class Franchise : BasicEntity
    {

    }


    public class Game_Mode : BasicEntity
    {

    }



    public class Cover : Image_Base
    {

    }

    public class ESRB : Rating
    {

    }

    public class PEGI : Rating
    {

    }



    public class Image : Image_Base
    {

    }

    public class Collection : BasicEntity
    {

    }



    public class Genre : BasicEntity
    {

    }

    public class Keyword : BasicEntity
    {

    }



    public class Theme : BasicEntity
    {

    }
}
