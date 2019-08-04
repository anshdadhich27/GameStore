using System;

namespace GameStore.Models.Entity
{
    public class BasicDetails
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long Igdb_Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string NameUrl { get; set; } = string.Empty;
        public string Games { get; set; } = string.Empty;

        public DateTime? Created_at { get; set; } = DateTime.Now;
        public DateTime? Updated_at { get; set; }
    }
}
