using System;
using System.Collections.Generic;

namespace GameStore.Models.Entity
{
    public class GamePlatform
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameUrl { get; set; }
        public string PageTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string PageContent { get; set; }
        public string Banner { get; set; }
        public string LogoUrl { get; set; }
        public bool Active { get; set; }
        public int Position { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Code { get; set; }
        public string Website { get; set; }
        public string Alternative_name { get; set; }
        public int Generation { get; set; }
        public string Games { get; set; }
        public long Igdb_Id { get; set; }

        public Images Logo { get; set; }
        public PaginatedEntity<Game> GameList { get; set; }
        public IList<FeatureLink> FeatureLinks { get; set; }



        public GamePlatform()
        {
            Logo = new Images();
            GameList = new PaginatedEntity<Game>();
            FeatureLinks = new List<FeatureLink>();


        }
    }
    public class GamePlatformExport
    {
        public string Name { get; set; }
        public string PageTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string Active { get; set; }
        public string Position { get; set; }
        public string AddedOn { get; set; }
        public string Code { get; set; }
    }
    


}
