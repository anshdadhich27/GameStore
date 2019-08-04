using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Ads
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string AdScript { get; set; }
        public bool Active { get; set; }
    }
}
