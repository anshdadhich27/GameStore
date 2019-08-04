using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class FeatureLink
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public long Reference_Id { get; set; }
        public string Reference_Name { get; set; }
        public DateTime AddedOn { get; set; } = DateTime.Now;
    }
}
