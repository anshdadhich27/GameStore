using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Common_Name_Url
    {
        public int Id { get; set; }
        public bool Active { get; set; } = true;
        public string Name { get; set; } = string.Empty;
        public string NameUrl { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;        
        public DateTime Added_On { get; set; } = DateTime.Now;
    }
}
