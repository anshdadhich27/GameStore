using System;

namespace GameStore.Models.Entity
{
    public class Review
    {
        public long Id { get; set; }
        public long Igdb_Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string ReviewFor { get; set; }
        public string SKU { get; set; }

        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }

        public long Reference_Id { get; set; }
        public int Category { get; set; }
        public int Likes { get; set; }
        public int Views { get; set; }
        public int Rating_Category { get; set; }
        public long Platform_Id { get; set; }
        public int Rating { get; set; }
        public bool Active { get; set; }

        public string Video { get; set; }
        public string Introduction { get; set; }
        public string Content { get; set; }
        public string Conclusion { get; set; }
        public string Positive_Points { get; set; }
        public string Negative_Points { get; set; }

        public string Reference_Name { get; set; }
        public string Reference_Link { get; set; }
        public string Reference_Type_Name { get; set; }
        public string Reference_Type_Link { get; set; }
        
    }
}
