using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Video
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Video_Id { get; set; }
        public string Snapshot { get; set; }
        public long Reference_Id { get; set; }
        public string Video_of { get; set; }
    }
}
