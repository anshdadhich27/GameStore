namespace GameStore.Models.Entity
{
    public class GameEngine : BasicDetails
    {        
        public string Companies { get; set; }
        public string Platforms { get; set; }

        public Images Logo { get; set; }

        public GameEngine()
        {
            Logo = new Images();
        }
    }
}
