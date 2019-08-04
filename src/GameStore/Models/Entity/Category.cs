using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Platform_Ids { get; set; }
        public string Platform_Names { get; set; }
        public IEnumerable<GamePlatform> PlatformList { get; set; }
        public IEnumerable<Genere> GenereList { get; set; }
        public IEnumerable<Common_Name_Url> AccessoriesTypes { get; set; }

        public Category()
        {
            AccessoriesTypes = new List<Common_Name_Url>();
            PlatformList = new List<GamePlatform>();
            GenereList = new List<Genere>();
        }
    }
}
