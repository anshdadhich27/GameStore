using System;

namespace GameStore.Models.Entity
{
    public class GamePlatformMaping
    {
        public long SNo { get; set; }
        public string SKU { get; set; }
        public long Game_Id { get; set; }
        public int Quantity { get; set; }

        public int Condition_Id { get; set; }
        public int Platform_Id { get; set; }
        public string Platform_Name { get; set; }
        public string Condition { get; set; }

        public bool IsBestSelling { get; set; }
        public bool Available_To_Buy { get; set; }
        public bool Available_To_Sell { get; set; } = true;

        public decimal Buying_Price { get; set; }
        public decimal Selling_Price { get; set; }
        public decimal Original_Price { get; set; }
        
    }
}
