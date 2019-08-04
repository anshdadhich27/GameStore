using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Country
    {
        public int id { get; set; }
        public string iso { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string nicename { get; set; } = string.Empty;
        public string iso3 { get; set; }
        public int numcode { get; set; }
        public int phonecode { get; set; }
        public string ISDCode { get { return string.Format("(+{0}) {1}", phonecode, nicename); } }
    }
}
