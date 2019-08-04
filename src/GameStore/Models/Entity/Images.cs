using System;

namespace GameStore.Models.Entity
{
    public class Images
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ImageOf { get; set; }
        public string ImageType { get; set; }
        public long Reference_Id { get; set; }
        public string ImageCategory { get; set; }
        public DateTime Added_On { get; set; } = DateTime.Now;
    }
}
