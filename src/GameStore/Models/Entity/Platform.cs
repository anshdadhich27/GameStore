namespace GameStore.Models.Entity
{
    public class Platform : BasicDetails
    {
        
        public string Website { get; set; }
        public string Summary { get; set; }
        public string Alternative_name { get; set; }
        public int Generation { get; set; }

        public Images Logo { get; set; }

        public Platform()
        {
            Logo = new Images();
        }
    }
}
