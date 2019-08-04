using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Blog
    {
        public long Id { get; set; }        
        public string Title { get; set; }
        public string Link { get; set; }
        public string Author { get; set; }
        
        public string Banner { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string BlogContent { get; set; }
        public string SortDescription { get; set; }

        public bool Active { get; set; }
        public bool IsNews { get; set; }
        public bool EditorsPic { get; set; }


        public string NextLink { get; set; }
        public string PreviousLink { get; set; }


        public int Category_Id { get; set; }
        public string Category_Name { get; set; }
        public string Category_Link { get; set; }
        public string Category_Logo { get; set; }


        public DateTime AddedOn { get; set; } = DateTime.Now;
    }
}
