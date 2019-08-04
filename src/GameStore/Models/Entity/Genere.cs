namespace GameStore.Models.Entity
{
    public class Genere : BasicDetails
    {
        public bool Active { get; set; }
    }
    public class Genere_CSV
    {
        public string Name { get; set; }
        public string Created_at { get; set; }
        public string Active { get; set; }
    }
}
