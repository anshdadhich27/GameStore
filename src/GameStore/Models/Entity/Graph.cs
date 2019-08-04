using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Total_Count
    {
        public int Count { get; set; }
        public string Name { get; set; }        
    }

    public class DateWiseGraph
    {
        public List<string> labels { get; set; }
        public List<Data_Item> datasets { get; set; }

        public DateWiseGraph()
        {
            labels = new List<string>();
            datasets = new List<Data_Item>();
        }
    }

    

    public class Data_Item
    {
        public string label { get; set; }
        public string borderColor { get; set; }
        public List<int> data { get; set; }

        public Data_Item()
        {
            data = new List<int>();
            borderColor = Get_Dynamic_Color();
        }


        private string Get_Dynamic_Color()
        {
            var r = RandomNumber.GenerateLockedRandom(0, 255);
            var g = RandomNumber.GenerateLockedRandom(0, 255);
            var b = RandomNumber.GenerateLockedRandom(0, 255);
            return string.Format("rgb({0},{1},{2})", r, g, b);
        }
    }
}
