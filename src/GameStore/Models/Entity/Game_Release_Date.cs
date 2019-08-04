using System;

namespace GameStore.Models.Entity
{
    public class Game_Release_Date
    {
        public long Id { get; set; }
        public long Game_Id { get; set; }
        public int Platform_Id { get; set; }
        public int Category { get; set; }
        public int Region { get; set; }
        public long IGdb_Date { get; set; }
        public DateTime? Release_On { get; set; }
    }
}
